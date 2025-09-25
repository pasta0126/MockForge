namespace MockForge.Utilities
{
    public static class Converters
    {
        public static string ToBase60(int value, bool keepSeparator)
        {
            var digits = new List<int>();
            while (value > 0)
            {
                digits.Insert(0, value % 60);
                value /= 60;
            }
            if (digits.Count == 0)
                digits.Add(0);

            var parts = digits.ConvertAll(d => d.ToString("00"));
            return keepSeparator ? string.Join(":", parts) : string.Join("", parts);
        }
    }
}
