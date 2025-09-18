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
    }
}