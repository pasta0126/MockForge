using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers
{
    public sealed class AnswerProvider(IRandomizer r) : IProvider
    {
        public string Name => "Answer";

        public string YesNo() => r.Pick(AnswerDataStore.YesNoAnswer);

        public string Magic8Ball() => r.Pick(AnswerDataStore.Magic8BallAnswers);

        public bool TrueFalse() => r.Pick(AnswerDataStore.TrueFalseAnswer) == "True";
    }
}
