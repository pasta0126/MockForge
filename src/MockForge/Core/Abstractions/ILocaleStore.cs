namespace MockForge.Core.Abstractions;

public interface ILocaleStore
{
    Task<IReadOnlyList<string>> GetListAsync(string locale, string key);
}