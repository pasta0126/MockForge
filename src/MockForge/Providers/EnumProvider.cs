using MockForge.Core.Abstractions;

namespace MockForge.Providers
{
    public sealed class EnumProvider(IRandomizer r) : IProvider
    {
        public string Name => "Enum";

        public T GetRandomValue<T>() where T : struct, Enum
        {
            return r.Pick(EnumCache<T>.Values);
        }

        public bool IsValid<T>(string? value) where T : struct, Enum
        {
            return !string.IsNullOrWhiteSpace(value) && EnumCache<T>.Names.Contains(value!);
        }

        private static class EnumCache<T> where T : struct, Enum
        {
            public static readonly T[] Values = Enum.GetValues<T>();
            public static readonly HashSet<string> Names = new(Enum.GetNames(typeof(T)), StringComparer.OrdinalIgnoreCase);
        }
    }
}
