using MockForge.Core.Abstractions;

namespace MockForge.Providers
{
    public sealed class NumberProvider(IRandomizer r) : IProvider
    {
        public string Name => "Number";

        public decimal RandomDecimal(decimal? min = 0, decimal? max = 1, int? decimals = 2)
        {
            var effectiveMin = min ?? 0;
            var effectiveMax = max ?? 1;
            var effectiveDecimals = decimals ?? 2;

            if (effectiveMin > effectiveMax)
                (effectiveMin, effectiveMax) = (effectiveMax, effectiveMin);

            var range = effectiveMax - effectiveMin;
            var randomValue = effectiveMin + (decimal)r.NextDouble() * range;

            return Math.Round(randomValue, effectiveDecimals);
        }

        public T RandomNumber<T>(T? min = default, T? max = default) where T : struct, IComparable<T>, IConvertible
        {
            var effectiveMin = min ?? default(T);
            var effectiveMax = max ?? default(T);

            if (effectiveMin.CompareTo(effectiveMax) > 0)
                (effectiveMin, effectiveMax) = (effectiveMax, effectiveMin);

            var minDouble = effectiveMin.ToDouble(null);
            var maxDouble = effectiveMax.ToDouble(null);
            var randomDouble = minDouble + r.NextDouble() * (maxDouble - minDouble);

            return (T)Convert.ChangeType(randomDouble, typeof(T));
        }

        public string RandomPaddedNumber(int length, int maxInclusive)
        {
            if (length <= 0)
                return string.Empty;

            if (maxInclusive < 0)
                maxInclusive = 0;

            int value;

            if (maxInclusive == int.MaxValue)
            {
                value = r.Next(0, int.MaxValue);
            }
            else
            {
                var exclusiveMax = maxInclusive + 1;
                if (exclusiveMax <= 0) 
                    exclusiveMax = int.MaxValue;

                value = r.Next(0, exclusiveMax);
            }

            var s = value.ToString();
            if (s.Length >= length)
                return s; 

            return s.PadLeft(length, '0');
        }
    }
}