using MockForge.Core.Abstractions;
using MockForge.StaticData;

namespace MockForge.Providers
{
    public sealed class CardProvider(IRandomizer r) : IProvider
    {
        public string Name => "Card";

        public string PokerCardRank() => r.Pick(CardDataStore.PokerCardsRanks);

        public string PokerCardSuit() => r.Pick(CardDataStore.PokerCardsSuits);

        public string PokerCard() => $"{PokerCardRank()} of {PokerCardSuit()}";

        public string SpanishCardRank() => r.Pick(CardDataStore.SpanishCardsRanks);

        public string SpanishCardSuit() => r.Pick(CardDataStore.SpanishCardsSuits);

        public string SpanishCard() => $"{SpanishCardRank()} de {SpanishCardSuit()}";

        public string UnoColor() => r.Pick(CardDataStore.UnoCardsColors);

        public string UnoNumberCard() => r.Pick(CardDataStore.UnoCardsNumberCards);

        public string UnoActionCard() => r.Pick(CardDataStore.UnoCardsActionCards);

        public string UnoWildCard() => r.Pick(CardDataStore.UnoCardsWildCards);

        public string UnoCard()
        {
            var cardType = r.Next(0, 4);
            return cardType switch
            {
                0 => $"{UnoColor()} {UnoNumberCard()}",
                1 => $"{UnoColor()} {UnoActionCard()}",
                2 => UnoWildCard(),
                _ => $"{UnoColor()} {UnoNumberCard()}"
            };
        }

        public string TarotMajorArcana() => r.Pick(CardDataStore.TarotCardsMajorArcana);

        public string TarotArcanoMayor() => r.Pick(CardDataStore.TarotCardsArcanosMayores);
    }
}