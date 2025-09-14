namespace MockForge.Core.Abstractions;
public interface ILocaleStore
{
    ValueTask<IReadOnlyList<string>> GetListAsync(string locale, string key, CancellationToken ct = default);
}
