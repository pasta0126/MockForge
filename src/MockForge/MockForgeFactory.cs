using MockForge.Core;
using MockForge.Core.Abstractions;

namespace MockForge;

public static class MockForgeFactory
{
    public static IMockForge Create(MockForgeOptions? options = null)
        => new MockForgeImpl(options ?? new MockForgeOptions());
}
