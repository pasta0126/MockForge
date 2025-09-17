using MockForge.Core.Abstractions;

namespace MockForge.Providers
{
    public sealed class AnswerProvider(IRandomizer r) : IProvider
    {
        public string Name => "Answer";

        public ValueTask<string> CityAsync() => ValueTask.FromResult(r.Pick(DataStore.Cities));


    }
}
