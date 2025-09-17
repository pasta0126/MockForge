using MockForge.Core.Abstractions;
namespace MockForge.Providers.Name;
public sealed class NameProvider(IRandomizer r) : IProvider
{
    public string Name => "Name";

    public ValueTask<string> FirstAsync() => ValueTask.FromResult(r.Pick(DataStore.FirstNames));
    public ValueTask<string> LastAsync() => ValueTask.FromResult(r.Pick(DataStore.LastNames));
    public async ValueTask<string> FullAsync() => $"{await FirstAsync()} {await LastAsync()}";
}
