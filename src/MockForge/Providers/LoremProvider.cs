using MockForge.Core;
using MockForge.Data;
using MockForge.Locales;
using MockForge.Templates;

namespace MockForge.Providers;

/// <summary>
/// Interface for Lorem ipsum text generation.
/// </summary>
public interface ILoremProvider : IDataProvider
{
    /// <summary>
    /// Generates a random word.
    /// </summary>
    Task<string> WordAsync();

    /// <summary>
    /// Generates multiple words.
    /// </summary>
    Task<string> WordsAsync(int count = 3);

    /// <summary>
    /// Generates a sentence.
    /// </summary>
    Task<string> SentenceAsync();

    /// <summary>
    /// Generates multiple sentences.
    /// </summary>
    Task<string> SentencesAsync(int count = 3);

    /// <summary>
    /// Generates a paragraph.
    /// </summary>
    Task<string> ParagraphAsync();

    /// <summary>
    /// Generates multiple paragraphs.
    /// </summary>
    Task<string> ParagraphsAsync(int count = 3);

    /// <summary>
    /// Generates text of specific length (approximate).
    /// </summary>
    Task<string> TextAsync(int length = 100);
}

/// <summary>
/// Provider for generating Lorem ipsum text.
/// </summary>
public class LoremProvider : BaseDataProvider, ILoremProvider
{
    private LoremData? _loremData;

    public LoremProvider(
        IRandomGenerator random,
        ILocaleDataProvider localeDataProvider,
        ITemplateEngine templateEngine,
        string locale = "en")
        : base(random, localeDataProvider, templateEngine, locale)
    {
    }

    /// <summary>
    /// Generates a random word.
    /// </summary>
    public async Task<string> WordAsync()
    {
        var loremData = await GetLoremDataAsync();
        return GetRandomElement(loremData.Words);
    }

    /// <summary>
    /// Generates multiple words.
    /// </summary>
    public async Task<string> WordsAsync(int count = 3)
    {
        var words = new string[count];
        for (int i = 0; i < count; i++)
        {
            words[i] = await WordAsync();
        }
        return string.Join(" ", words);
    }

    /// <summary>
    /// Generates a sentence.
    /// </summary>
    public async Task<string> SentenceAsync()
    {
        var loremData = await GetLoremDataAsync();
        if (loremData.Sentences.Length > 0)
        {
            return GetRandomElement(loremData.Sentences);
        }

        // Generate sentence from words if no pre-made sentences available
        var wordCount = Random.Next(4, 18);
        var words = await WordsAsync(wordCount);
        return char.ToUpper(words[0]) + words.Substring(1) + ".";
    }

    /// <summary>
    /// Generates multiple sentences.
    /// </summary>
    public async Task<string> SentencesAsync(int count = 3)
    {
        var sentences = new string[count];
        for (int i = 0; i < count; i++)
        {
            sentences[i] = await SentenceAsync();
        }
        return string.Join(" ", sentences);
    }

    /// <summary>
    /// Generates a paragraph.
    /// </summary>
    public async Task<string> ParagraphAsync()
    {
        var sentenceCount = Random.Next(3, 7);
        return await SentencesAsync(sentenceCount);
    }

    /// <summary>
    /// Generates multiple paragraphs.
    /// </summary>
    public async Task<string> ParagraphsAsync(int count = 3)
    {
        var paragraphs = new string[count];
        for (int i = 0; i < count; i++)
        {
            paragraphs[i] = await ParagraphAsync();
        }
        return string.Join("\n\n", paragraphs);
    }

    /// <summary>
    /// Generates text of specific length (approximate).
    /// </summary>
    public async Task<string> TextAsync(int length = 100)
    {
        var text = "";
        while (text.Length < length)
        {
            var sentence = await SentenceAsync();
            if (text.Length + sentence.Length + 1 > length)
            {
                // Trim to exact length
                var remaining = length - text.Length;
                if (remaining > 0)
                {
                    text += sentence.Substring(0, Math.Min(remaining, sentence.Length));
                }
                break;
            }
            
            text += (text.Length > 0 ? " " : "") + sentence;
        }
        return text;
    }

    private async Task<LoremData> GetLoremDataAsync()
    {
        if (_loremData == null)
        {
            _loremData = await LocaleDataProvider.GetDataAsync<LoremData>(Locale, "lorem");
            
            // Fallback to default lorem words if no data available
            if (_loremData?.Words == null || _loremData.Words.Length == 0)
            {
                _loremData = new LoremData
                {
                    Words = new[]
                    {
                        "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
                        "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
                        "magna", "aliqua", "enim", "ad", "minim", "veniam", "quis", "nostrud",
                        "exercitation", "ullamco", "laboris", "nisi", "aliquip", "ex", "ea", "commodo",
                        "consequat", "duis", "aute", "irure", "in", "reprehenderit", "voluptate",
                        "velit", "esse", "cillum", "fugiat", "nulla", "pariatur", "excepteur", "sint",
                        "occaecat", "cupidatat", "non", "proident", "sunt", "culpa", "qui", "officia",
                        "deserunt", "mollit", "anim", "id", "est", "laborum"
                    }
                };
            }
        }

        return _loremData;
    }
}