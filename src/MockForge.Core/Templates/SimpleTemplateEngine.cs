using System.Text.RegularExpressions;
using MockForge.Core.Abstractions;
namespace MockForge.Core.Templates;
public sealed class SimpleTemplateEngine : ITemplateEngine
{
    static readonly Regex Rx = new(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
    public string Render(string template, IReadOnlyDictionary<string, Func<string>> map)
        => Rx.Replace(template, m => map.TryGetValue(m.Groups[1].Value, out var f) ? f() : m.Value);
}
