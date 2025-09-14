using MockForge.Core;
using MockForge.Core.Abstractions;
using MockForge.Providers.Name;
namespace MockForge;
public static class MF
{
    static readonly IMockForge _default = new MockForgeImpl(new MockForgeOptions { Locale = "en" });
    public static ValueTask<string> FirstName() => _default.Get<NameProvider>().FirstAsync();
    public static ValueTask<string> LastName() => _default.Get<NameProvider>().LastAsync();
}
