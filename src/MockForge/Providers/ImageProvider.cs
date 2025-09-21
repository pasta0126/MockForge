using System;
using System.IO;
using System.IO.Compression;
using MockForge.Core.Abstractions;

namespace MockForge.Providers
{
    public sealed class ImageProvider(IRandomizer r) : IProvider
    {
        public string Name => "Image";

        private static int Clamp(int v, int lo, int hi) => v < lo ? lo : v > hi ? hi : v;

        // Generates a 24-bit BMP image with random color tiles
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

        // Generates a PNG with RGBA pixels, where RGB changes smoothly per tile
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

        // Generates a PNG with RGBA pixels, hue varies per tile in HSV space
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
