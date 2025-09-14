using Microsoft.Extensions.DependencyInjection;
using MockForge.Core;
using MockForge.Core.Abstractions;
namespace MockForge;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMockForge(this IServiceCollection services, Action<MockForgeOptions>? setup = null)
    {
        var opts = new MockForgeOptions(); setup?.Invoke(opts);
        services.AddSingleton(opts);
        services.AddSingleton<IMockForge, MockForgeImpl>();
        return services;
    }
}
