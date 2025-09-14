namespace MockForge.Core.Abstractions;
public interface IMockForge
{
    string Locale { get; }
    T Get<T>() where T : IProvider;
}
