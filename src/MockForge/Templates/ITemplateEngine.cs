namespace MockForge.Templates;

/// <summary>
/// Interface for template processing.
/// </summary>
public interface ITemplateEngine
{
    /// <summary>
    /// Processes a template string, replacing tokens with provided values.
    /// </summary>
    /// <param name="template">The template string containing tokens in {{token}} format.</param>
    /// <param name="tokens">Dictionary of token values.</param>
    /// <returns>The processed template string.</returns>
    string Process(string template, Dictionary<string, string> tokens);
}