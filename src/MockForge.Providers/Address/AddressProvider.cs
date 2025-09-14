using MockForge.Core.Abstractions;
namespace MockForge.Providers.Address;
public sealed class AddressProvider(IRandomizer r, ILocaleStore s, string locale) : IProvider
{
    public string Name => "Address";

    public async ValueTask<string> CityAsync() => r.Pick(await s.GetListAsync(locale, "address.city"));
}
