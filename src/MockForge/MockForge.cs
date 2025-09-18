using MockForge.Core;

namespace MockForge;

public static class MF
{
    static readonly MockForgeImpl _default = new(new MockForgeOptions());
}
