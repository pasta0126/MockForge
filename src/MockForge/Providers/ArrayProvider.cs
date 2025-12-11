using MockForge.Core.Abstractions;

namespace MockForge.Providers
{
    public sealed class ArrayProvider(IRandomizer r) : IProvider
    {
        public string Name => "Array";

        public T Pick<T>(T[] array)
        {
            return r.Pick(array);
        }

        public T[] Shuffle<T>(T[]? array)
        {
            return r.Shuffle(array);
        }
    }
}