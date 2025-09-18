using MockForge.Core.Abstractions;

namespace MockForge.Core.Localization
{
    public class SimpleLocaleStore : ILocaleStore
    {
        public Task<IReadOnlyList<string>> GetListAsync(string locale, string key)
        {
            // Implementación simple que devuelve lista vacía por ahora
            return Task.FromResult<IReadOnlyList<string>>(Array.Empty<string>());
        }
    }
}