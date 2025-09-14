using MockForge.Core.Abstractions;
namespace MockForge.Providers.Name;
public sealed class NameProvider(IRandomizer r, ILocaleStore s, string locale) : IProvider
{
    public string Name => "Name";

    public async ValueTask<string> FirstAsync() => r.Pick(await s.GetListAsync(locale, "name.first"));
    public async ValueTask<string> LastAsync() => r.Pick(await s.GetListAsync(locale, "name.last"));
    public async ValueTask<string> FullAsync() => $"{await FirstAsync()} {await LastAsync()}";
}
