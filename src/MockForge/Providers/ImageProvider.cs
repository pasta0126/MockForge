using MockForge.Core.Abstractions;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace MockForge.Providers
{
    public sealed class ImageProvider(IRandomizer r) : IProvider
    {
        public string Name => "Image";

        private static int Clamp(int v, int lo, int hi) => v < lo ? lo : v > hi ? hi : v;

        public byte[] GenerateRandomBitmap(int width, int height, int tileSize = 32)
        {
            const int bytesPerPixel = 3; // B, G, R
            var buffer = new (byte B, byte G, byte R)[height, width];

            for (int y = 0; y < height;)
            {
                int tileH = Math.Min(tileSize, height - y);

                for (int x = 0; x < width;)
                {
                    int tileW = Math.Min(tileSize, width - x);

                    byte rr = (byte)r.Next(0, 256);
                    byte gg = (byte)r.Next(0, 256);
                    byte bb = (byte)r.Next(0, 256);

                    for (int yy = y; yy < y + tileH; yy++)
                    {
                        for (int xx = x; xx < x + tileW; xx++)
                        {
                            buffer[yy, xx] = (bb, gg, rr);
                        }
                    }

                    x += tileW;
                }

                y += tileH;
            }

            int stride = ((width * bytesPerPixel + 3) / 4) * 4;
            int pixelDataSize = stride * height;
            const int fileHeaderSize = 14;
            const int infoHeaderSize = 40;
            int fileSize = fileHeaderSize + infoHeaderSize + pixelDataSize;

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);

            // BITMAPFILEHEADER
            bw.Write((ushort)0x4D42); // 'BM'
            bw.Write(fileSize);
            bw.Write((ushort)0);
            bw.Write((ushort)0);
            bw.Write(fileHeaderSize + infoHeaderSize);

            // BITMAPINFOHEADER
            bw.Write(infoHeaderSize);
            bw.Write(width);
            bw.Write(height);
            bw.Write((ushort)1);
            bw.Write((ushort)(bytesPerPixel * 8));
            bw.Write(0);
            bw.Write(pixelDataSize);
            bw.Write(0);
            bw.Write(0);
            bw.Write(0);
            bw.Write(0);

            byte[] padding = new byte[stride - width * bytesPerPixel];

            for (int row = height - 1; row >= 0; row--)
            {
                for (int col = 0; col < width; col++)
                {
                    var (bb, gg, rr) = buffer[row, col];
                    bw.Write(bb);
                    bw.Write(gg);
                    bw.Write(rr);
                }
                if (padding.Length > 0)
                    bw.Write(padding);
            }

            bw.Flush();
            return ms.ToArray();
        }

        public byte[] GeneratePngRGBNative(int width, int height, int tileSize = 32, int delta = 30)
        {
            var buffer = new (byte B, byte G, byte R)[height, width];

            int prevR = r.Next(0, 256), prevG = r.Next(0, 256), prevB = r.Next(0, 256);

            for (int y = 0; y < height;)
            {
                int h = Math.Min(tileSize, height - y);

                for (int x = 0; x < width;)
                {
                    prevR = Clamp(prevR + (int)Math.Round((r.NextDouble() * 2 - 1) * delta), 0, 255);
                    prevG = Clamp(prevG + (int)Math.Round((r.NextDouble() * 2 - 1) * delta), 0, 255);
                    prevB = Clamp(prevB + (int)Math.Round((r.NextDouble() * 2 - 1) * delta), 0, 255);

                    int w = Math.Min(tileSize, width - x);

                    for (int yy = y; yy < y + h; yy++)
                    {
                        for (int xx = x; xx < x + w; xx++)
                        {
                            buffer[yy, xx] = ((byte)prevB, (byte)prevG, (byte)prevR);
                        }
                    }
                    x += tileSize;
                }

                y += tileSize;
            }

            byte[] pixelData = new byte[width * height * 4];
            int idx = 0;

            for (int yy = 0; yy < height; yy++)
            {
                for (int xx = 0; xx < width; xx++)
                {
                    var (b, g, rcomp) = buffer[yy, xx];

                    pixelData[idx++] = rcomp;
                    pixelData[idx++] = g;
                    pixelData[idx++] = b;
                    pixelData[idx++] = 255; // A
                }
            }

            return PngEncoder.SavePng(pixelData, width, height);
        }

        public byte[] GeneratePngHSVNative(int width, int height, int tileSize = 32, float maxHueStep = 15f)
        {
            var buffer = new (byte B, byte G, byte R)[height, width];
            float hue = (float)(r.NextDouble() * 360f);
            const float sat = 0.6f, val = 0.9f;

            for (int y = 0; y < height;)
            {
                int h = Math.Min(tileSize, height - y);

                for (int x = 0; x < width;)
                {
                    hue = (hue + (float)(r.NextDouble() * 2 - 1) * maxHueStep + 360f) % 360f;
                    var (r8, g8, b8) = HsvToRgb(hue, sat, val);

                    int w = Math.Min(tileSize, width - x);

                    for (int yy = y; yy < y + h; yy++)
                    {
                        for (int xx = x; xx < x + w; xx++)
                        {
                            buffer[yy, xx] = (b8, g8, r8);
                        }
                    }

                    x += tileSize;
                }

                y += tileSize;
            }

            byte[] pixelData = new byte[width * height * 4];
            int idx = 0;

            for (int yy = 0; yy < height; yy++)
            {
                for (int xx = 0; xx < width; xx++)
                {
                    var (b, g, rcomp) = buffer[yy, xx];

                    pixelData[idx++] = rcomp;
                    pixelData[idx++] = g;
                    pixelData[idx++] = b;
                    pixelData[idx++] = 255; // A
                }
            }

            return PngEncoder.SavePng(pixelData, width, height);
        }

        private static (byte r, byte g, byte b) HsvToRgb(float h, float s, float v)
        {
            float c = v * s;
            float x = c * (1 - MathF.Abs((h / 60f) % 2f - 1));
            float m = v - c;
            float rp = 0, gp = 0, bp = 0;

            if (h < 60) { rp = c; gp = x; }
            else if (h < 120) { rp = x; gp = c; }
            else if (h < 180) { gp = c; bp = x; }
            else if (h < 240) { gp = x; bp = c; }
            else if (h < 300) { rp = x; bp = c; }
            else { rp = c; bp = x; }

            return (
                r: (byte)MathF.Round((rp + m) * 255f),
                g: (byte)MathF.Round((gp + m) * 255f),
                b: (byte)MathF.Round((bp + m) * 255f)
            );
        }

        public byte[] GenerateAvatarPng(string seed, int logicalSize = 8, int scale = 8)
        {
            var hash = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(seed));
            int size = logicalSize;
            int half = size / 2;
            var region = new byte[size, half];
            int[] leftBound = new int[size];
            int[] rightBound = new int[size];

            for (int y = 0; y < size; y++)
            {
                leftBound[y] = half;
                rightBound[y] = -1;
            }

            int idx = 0;

            byte NextByte()
            {
                if (idx >= hash.Length) idx = 0;
                return hash[idx++];
            }

            int NextInt(int max) => max <= 0 ? 0 : NextByte() % max;

            double NextUniform()
            {
                int v = (NextByte() << 8) | NextByte();
                return v / 65535.0;
            }

            double NextGaussian(double mean, double std)
            {
                double u1 = NextUniform();
                if (u1 <= 0.0) u1 = 1e-6;
                double u2 = NextUniform();
                double r = Math.Sqrt(-2.0 * Math.Log(u1));
                double theta = 2.0 * Math.PI * u2;
                double z = r * Math.Cos(theta);
                return mean + std * z;
            }

            float baseHue = (hash[0] / 255f) * 360f;
            float secondaryHue = (baseHue + ((hash[1] & 1) == 0 ? -25f : 25f) + 360f) % 360f;
            float accentHue = (baseHue + 180f) % 360f;

            var (pr, pg, pb) = HsvToRgb(baseHue, 0.8f, 0.9f);
            var (sr, sg, sb) = HsvToRgb(secondaryHue, 0.75f, 0.85f);
            var (ar, ag, ab) = HsvToRgb(accentHue, 0.9f, 0.95f);

            var primary = (B: pb, G: pg, R: pr);
            var secondary = (B: sb, G: sg, R: sr);
            var accent = (B: ab, G: ag, R: ar);

            int remaining = size;
            int headH = Clamp((int)Math.Round(NextGaussian(size * 0.3, size * 0.08)), 2, size - 4);
            remaining -= headH;
            int bodyH = Clamp((int)Math.Round(NextGaussian(size * 0.4, size * 0.10)), 2, remaining - 2);
            remaining -= bodyH;
            int legsH = remaining;
            if (legsH < 1) legsH = 1;

            int bodyStart = headH;
            int legsStart = headH + bodyH;

            int centerCol = half - 1;

            float headWidthBase = half * (float)Math.Clamp(NextGaussian(0.5, 0.15), 0.2, 0.9);
            float bodyWidthBase = half * (float)Math.Clamp(NextGaussian(0.8, 0.15), 0.4, 1.1);
            float legsWidthBase = half * (float)Math.Clamp(NextGaussian(0.45, 0.12), 0.2, 0.8);

            int legsMode = NextInt(3); // 0 = dos piernas, 1 = falda/bloque, 2 = flotante

            void Mark(int y, int x, byte val)
            {
                if (x < 0 || x >= half || y < 0 || y >= size) return;
                region[y, x] = val;
                if (x < leftBound[y]) leftBound[y] = x;
                if (x > rightBound[y]) rightBound[y] = x;
            }

            float width = headWidthBase;

            for (int y = 0; y < headH; y++)
            {
                float t = headH > 1 ? (float)y / (headH - 1) : 0f;
                float bulge = 1f - MathF.Abs(2f * t - 1f);

                width = width * (0.8f + 0.3f * bulge);
                width += (float)NextGaussian(0, 0.3f);

                if (width < 0.5f) width = 0.5f;
                if (width > half - 0.2f) width = half - 0.2f;

                int span = Clamp((int)MathF.Round(width), 1, half);
                int left = centerCol - span / 2;

                for (int x = 0; x < span; x++)
                    Mark(y, left + x, 1);
            }

            int shouldersRows = Math.Max(1, bodyH / 3);
            float bodyWidth = bodyWidthBase;

            for (int i = 0; i < shouldersRows; i++)
            {
                int row = bodyStart + i;
                float stretch = 1.1f + (float)NextGaussian(0, 0.1f);
                float rowWidth = bodyWidth * stretch;

                if (rowWidth < 1f) rowWidth = 1f;
                if (rowWidth > half - 0.2f) rowWidth = half - 0.2f;

                int span = Clamp((int)MathF.Round(rowWidth), 2, half);
                int left = centerCol - span / 2;

                for (int x = 0; x < span; x++)
                    Mark(row, left + x, 1);

                int armLen = 1 + NextInt(2);

                for (int d = 1; d <= armLen; d++)
                {
                    Mark(row, left - d, 1);
                    Mark(row, left + span - 1 + d, 1);
                }
            }

            int bodyCoreStart = bodyStart + shouldersRows;

            for (int y = bodyCoreStart; y < bodyStart + bodyH; y++)
            {
                float t = bodyH > 1 ? (float)(y - bodyStart) / (bodyH - 1) : 0f;
                float waistFactor = 1f - 0.4f * MathF.Abs(2f * t - 1f);
                float targetWidth = bodyWidthBase * waistFactor;

                bodyWidth = bodyWidth * 0.6f + targetWidth * 0.4f;
                bodyWidth += (float)NextGaussian(0, 0.25f);

                if (bodyWidth < 1f) bodyWidth = 1f;
                if (bodyWidth > half - 0.2f) bodyWidth = half - 0.2f;

                int span = Clamp((int)MathF.Round(bodyWidth), 2, half);
                int left = centerCol - span / 2;

                for (int x = 0; x < span; x++)
                    Mark(y, left + x, 1);
            }

            if (legsMode == 0)
            {
                int legWidth = 1 + NextInt(2);
                int legGap = 1 + NextInt(2);
                int totalSpan = legWidth * 2 + legGap;
                int legLeft = centerCol - totalSpan / 2;

                if (legLeft < 0) legLeft = 0;
                if (legLeft + totalSpan > half) legLeft = half - totalSpan;

                int legRightStart = legLeft + legWidth + legGap;

                for (int y = 0; y < legsH; y++)
                {
                    int row = legsStart + y;

                    if (NextInt(5) == 0) continue;

                    for (int dx = 0; dx < legWidth; dx++)
                    {
                        Mark(row, legLeft + dx, 1);
                        Mark(row, legRightStart + dx, 1);
                    }
                }
            }
            else if (legsMode == 1)
            {
                float legWidth = legsWidthBase;

                for (int y = 0; y < legsH; y++)
                {
                    int row = legsStart + y;
                    float t = legsH > 1 ? (float)y / (legsH - 1) : 0f;

                    legWidth = legWidth * 0.6f + legsWidthBase * (0.9f - 0.4f * t) * 0.4f;
                    legWidth += (float)NextGaussian(0, 0.2f);

                    if (legWidth < 1f) legWidth = 1f;
                    if (legWidth > half - 0.2f) legWidth = half - 0.2f;

                    int span = Clamp((int)MathF.Round(legWidth), 1, half);
                    int left = centerCol - span / 2;

                    if (NextInt(6) == 0) continue;

                    for (int x = 0; x < span; x++)
                        Mark(row, left + x, 1);
                }
            }

            for (int y = 0; y < size; y++)
            {
                if (leftBound[y] > rightBound[y]) continue;

                for (int x = leftBound[y]; x <= rightBound[y]; x++)
                {
                    if (region[y, x] == 0) continue;

                    if (NextInt(12) == 0)
                        region[y, x] = 0;
                }
            }

            for (int y = bodyStart; y < bodyStart + bodyH; y++)
            {
                if (leftBound[y] > rightBound[y]) continue;

                int mid = centerCol;

                if (mid >= leftBound[y] && mid <= rightBound[y] && region[y, mid] == 1)
                    region[y, mid] = 2;
            }

            if (headH > 1)
            {
                int eyesRow = Clamp(1 + NextInt(headH - 1), 1, headH - 1);
                int eyeOffset = 1 + NextInt(Math.Max(1, half - 2));
                int leftEyeX = centerCol - eyeOffset;
                int rightEyeX = centerCol + eyeOffset >= half ? half - 1 : centerCol + eyeOffset;

                if (leftEyeX >= 0 && leftEyeX < half && region[eyesRow, leftEyeX] != 0)
                    region[eyesRow, leftEyeX] = 3;

                if (rightEyeX >= 0 && rightEyeX < half && region[eyesRow, rightEyeX] != 0)
                    region[eyesRow, rightEyeX] = 3;
            }

            int beltRow = bodyStart + bodyH / 2;

            if (beltRow >= bodyStart && beltRow < bodyStart + bodyH)
            {
                for (int x = 0; x < half; x++)
                {
                    if (region[beltRow, x] != 0 && NextInt(3) == 0)
                        region[beltRow, x] = 3;
                }
            }

            for (int row = size - 1; row >= legsStart; row--)
            {
                bool any = false;

                for (int x = 0; x < half; x++)
                {
                    if (region[row, x] == 1)
                    {
                        any = true;
                        region[row, x] = 3;
                    }
                }

                if (any) break;
            }

            var buffer = new (byte B, byte G, byte R)[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < half; x++)
                {
                    (byte B, byte G, byte R) c = region[y, x] switch
                    {
                        1 => primary,
                        2 => secondary,
                        3 => accent,
                        _ => (0, 0, 0)
                    };

                    buffer[y, x] = c;
                    buffer[y, size - 1 - x] = c;
                }
            }

            int widthPx = size * scale;
            int heightPx = size * scale;
            byte[] pixelData = new byte[widthPx * heightPx * 4];
            int p = 0;

            for (int yy = 0; yy < heightPx; yy++)
            {
                int sy = yy / scale;

                for (int xx = 0; xx < widthPx; xx++)
                {
                    int sx = xx / scale;
                    var (B, G, R) = buffer[sy, sx];

                    pixelData[p++] = R;
                    pixelData[p++] = G;
                    pixelData[p++] = B;
                    pixelData[p++] = 255;
                }
            }

            return PngEncoder.SavePng(pixelData, widthPx, heightPx);
        }

        private static class PngEncoder
        {
            public static byte[] SavePng(byte[] pixelData, int width, int height)
            {
                using var ms = new MemoryStream();
                WriteSignature(ms);
                WriteIHDR(ms, width, height);
                WriteIDAT(ms, pixelData, width, height);
                WriteIEND(ms);
                return ms.ToArray();
            }

            static void WriteSignature(Stream s)
            {
                s.Write(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });
            }

            static void WriteIHDR(Stream s, int width, int height)
            {
                Span<byte> ihdrData = stackalloc byte[13];

                WriteBigEndian(ihdrData, 0, width);
                WriteBigEndian(ihdrData, 4, height);

                ihdrData[8] = 8;  // bit depth
                ihdrData[9] = 6;  // color type RGBA
                ihdrData[10] = 0; // compression
                ihdrData[11] = 0; // filter
                ihdrData[12] = 0; // interlace

                WriteChunk(s, "IHDR", ihdrData);
            }

            static void WriteIDAT(Stream s, byte[] pixels, int width, int height)
            {
                int rowLength = width * 4; // RGBA
                int stride = rowLength + 1; // + filter byte
                byte[] raw = new byte[height * stride];

                for (int y = 0; y < height; y++)
                {
                    int destRow = y * stride;
                    raw[destRow] = 0; // filter type 0
                    Array.Copy(pixels, y * rowLength, raw, destRow + 1, rowLength);
                }

                using var msCompressed = new MemoryStream();
                using (var zs = new ZLibStream(msCompressed, CompressionLevel.Fastest, leaveOpen: true))
                {
                    zs.Write(raw, 0, raw.Length);
                }

                byte[] compressed = msCompressed.ToArray();

                WriteChunk(s, "IDAT", compressed);
            }

            static void WriteIEND(Stream s)
            {
                WriteChunk(s, "IEND", Array.Empty<byte>());
            }

            static void WriteChunk(Stream s, string chunkType, ReadOnlySpan<byte> data)
            {
                byte[] typeBytes = System.Text.Encoding.ASCII.GetBytes(chunkType);
                int length = data.Length;

                WriteBigEndian(s, length);

                s.Write(typeBytes, 0, 4);
                s.Write(data);

                uint crc = ComputeCrc(typeBytes, data);

                WriteBigEndian(s, unchecked((int)crc));
            }

            static uint ComputeCrc(ReadOnlySpan<byte> typeBytes, ReadOnlySpan<byte> data)
            {
                var crc32 = new Crc32();

                crc32.Append(typeBytes);
                crc32.Append(data);

                return crc32.Hash;
            }

            static void WriteBigEndian(Stream s, int value)
            {
                Span<byte> buffer = stackalloc byte[4];

                WriteBigEndian(buffer, 0, value);

                s.Write(buffer);
            }

            static void WriteBigEndian(Span<byte> buffer, int offset, int value)
            {
                buffer[offset + 0] = (byte)((value >> 24) & 0xFF);
                buffer[offset + 1] = (byte)((value >> 16) & 0xFF);
                buffer[offset + 2] = (byte)((value >> 8) & 0xFF);
                buffer[offset + 3] = (byte)(value & 0xFF);
            }

            private sealed class Crc32
            {
                const uint Polynomial = 0xEDB88320u;
                readonly uint[] table;
                uint current;

                public Crc32()
                {
                    table = new uint[256];

                    for (uint i = 0; i < 256; i++)
                    {
                        uint crc = i;

                        for (int j = 0; j < 8; j++)
                        {
                            if ((crc & 1) != 0)
                                crc = (crc >> 1) ^ Polynomial;
                            else
                                crc >>= 1;
                        }

                        table[i] = crc;
                    }

                    current = 0xFFFFFFFFu;
                }

                public void Append(ReadOnlySpan<byte> data)
                {
                    foreach (byte b in data)
                    {
                        uint temp = (current ^ b) & 0xFF;
                        current = (current >> 8) ^ table[temp];
                    }
                }

                public uint Hash => ~current;
            }
        }
    }
}
