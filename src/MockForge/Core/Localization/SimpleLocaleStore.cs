using MockForge.Core.Abstractions;

namespace MockForge.Core.Localization
{
    public class SimpleLocaleStore : ILocaleStore
    {
        public Task<IReadOnlyList<string>> GetListAsync(string locale, string key)
        {
            // Implementaci�n simple que devuelve lista vac�a por ahora
            return Task.FromResult<IReadOnlyList<string>>(Array.Empty<string>());
        }
    }
}