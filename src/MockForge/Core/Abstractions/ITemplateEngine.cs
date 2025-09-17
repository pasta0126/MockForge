namespace MockForge.Core.Abstractions;
public interface ITemplateEngine
{
    string Render(string template, IReadOnlyDictionary<string, Func<string>> map);
}
