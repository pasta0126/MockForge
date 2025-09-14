using System.Text.RegularExpressions;

namespace MockForge.Templates;

/// <summary>
/// Simple template engine that processes {{token}} replacements.
/// </summary>
public class TemplateEngine : ITemplateEngine
{
    private static readonly Regex TokenRegex = new(@"\{\{(\w+)\}\}", RegexOptions.Compiled);

    /// <summary>
    /// Processes a template string, replacing tokens with provided values.
    /// </summary>
    /// <param name="template">The template string containing tokens in {{token}} format.</param>
    /// <param name="tokens">Dictionary of token values.</param>
    /// <returns>The processed template string.</returns>
    public string Process(string template, Dictionary<string, string> tokens)
    {
        if (string.IsNullOrEmpty(template))
            return template;

        if (tokens == null || tokens.Count == 0)
            return template;

        return TokenRegex.Replace(template, match =>
        {
            var token = match.Groups[1].Value;
            return tokens.TryGetValue(token, out var value) ? value : match.Value;
        });
    }
}