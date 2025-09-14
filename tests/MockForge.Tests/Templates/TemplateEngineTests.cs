using MockForge.Templates;

namespace MockForge.Tests.Templates;

public class TemplateEngineTests
{
    [Fact]
    public void Process_WithValidTokens_ReplacesTokensCorrectly()
    {
        // Arrange
        var engine = new TemplateEngine();
        var template = "Hello {{name}}, you are {{age}} years old.";
        var tokens = new Dictionary<string, string>
        {
            { "name", "John" },
            { "age", "25" }
        };

        // Act
        var result = engine.Process(template, tokens);

        // Assert
        Assert.Equal("Hello John, you are 25 years old.", result);
    }

    [Fact]
    public void Process_WithMissingTokens_LeavesTokensUnchanged()
    {
        // Arrange
        var engine = new TemplateEngine();
        var template = "Hello {{name}}, you are {{age}} years old.";
        var tokens = new Dictionary<string, string>
        {
            { "name", "John" }
        };

        // Act
        var result = engine.Process(template, tokens);

        // Assert
        Assert.Equal("Hello John, you are {{age}} years old.", result);
    }

    [Fact]
    public void Process_WithEmptyTemplate_ReturnsEmptyString()
    {
        // Arrange
        var engine = new TemplateEngine();
        var template = "";
        var tokens = new Dictionary<string, string>();

        // Act
        var result = engine.Process(template, tokens);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void Process_WithNullTemplate_ReturnsNull()
    {
        // Arrange
        var engine = new TemplateEngine();
        string template = null!;
        var tokens = new Dictionary<string, string>();

        // Act
        var result = engine.Process(template, tokens);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Process_WithNullTokens_ReturnsOriginalTemplate()
    {
        // Arrange
        var engine = new TemplateEngine();
        var template = "Hello {{name}}";
        Dictionary<string, string> tokens = null!;

        // Act
        var result = engine.Process(template, tokens);

        // Assert
        Assert.Equal("Hello {{name}}", result);
    }

    [Fact]
    public void Process_WithEmptyTokens_ReturnsOriginalTemplate()
    {
        // Arrange
        var engine = new TemplateEngine();
        var template = "Hello {{name}}";
        var tokens = new Dictionary<string, string>();

        // Act
        var result = engine.Process(template, tokens);

        // Assert
        Assert.Equal("Hello {{name}}", result);
    }

    [Fact]
    public void Process_WithMultipleOccurrencesOfSameToken_ReplacesAll()
    {
        // Arrange
        var engine = new TemplateEngine();
        var template = "{{word}} {{word}} {{word}}";
        var tokens = new Dictionary<string, string>
        {
            { "word", "test" }
        };

        // Act
        var result = engine.Process(template, tokens);

        // Assert
        Assert.Equal("test test test", result);
    }
}