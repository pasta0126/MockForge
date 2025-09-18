using MockForge.Core.Abstractions;

namespace MockForge.Providers
{
    public sealed class ArrayProvider(IRandomizer r) : IProvider
    {
        public string Name => "Array";

        public T? Pick<T>(T[]? array)
        {
            if (array == null || array.Length == 0)
                return default;
            
            return r.Pick(array);
        }
    }
}