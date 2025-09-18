using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers.Name
{
    public sealed class NameProvider : IProvider
    {
        private readonly IRandomizer _rnd;
        private readonly ILocaleStore _store;
        private readonly string _locale;

        public string Name => nameof(NameProvider);

        public NameProvider(IRandomizer rnd, ILocaleStore store, string locale)
        {
            _rnd = rnd;
            _store = store;
            _locale = locale;
        }

        public ValueTask<string> FirstAsync()
        {
            var names = _rnd.Next(0, 2) == 0 
                ? IdentityDataStore.MaleNameData 
                : IdentityDataStore.FemaleNameData;
            return ValueTask.FromResult(_rnd.Pick(names));
        }

        public ValueTask<string> LastAsync()
        {
            return ValueTask.FromResult(_rnd.Pick(IdentityDataStore.LastNameData));
        }

        public async ValueTask<string> FullAsync()
        {
            var firstName = await FirstAsync();
            var lastName = await LastAsync();
            return $"{firstName} {lastName}";
        }
    }
}