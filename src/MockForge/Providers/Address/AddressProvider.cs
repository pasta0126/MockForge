using MockForge.Core.Abstractions;

namespace MockForge.Providers.Address;

public sealed class AddressProvider(IRandomizer r) : IProvider
{
    public string Name => "Address";

    public ValueTask<string> CityAsync() => ValueTask.FromResult(r.Pick(DataStore.Cities));
}
