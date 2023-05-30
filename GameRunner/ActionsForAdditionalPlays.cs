namespace GameRunner;

public static class ActionsForAdditionalPlays
{
    public static CardList AvailablePlays(
        List<CardList> playZone,
        CardList cards
    )
    {
        CardList availablePlays = new();

        AddCardsToTopOfRuns(playZone, cards, availablePlays);

        AddCardsToBottomOfRuns(playZone, cards, availablePlays);

        AddCardsToAtama(playZone, cards, availablePlays);

        return availablePlays;
    }

    private static void AddCardsToAtama(List<CardList> playZone, CardList cards, CardList availablePlays)
    {
        foreach (Card card in cards)
        {
            foreach (CardList playedList in playZone)
            {
                if (playedList.GetType() != typeof(Atama)) continue;
                Atama addToAtama = new Atama(playedList) { card };
                if (AtamaFinder.IsAtama(addToAtama))
                {
                    availablePlays.Add(card);
                }
            }
        }
    }

    private static void AddCardsToBottomOfRuns(List<CardList> playZone, CardList cards, CardList availablePlays)
    {
        foreach (Card card in cards)
        {
            foreach (CardList playedList in playZone)
            {
                if (playedList.GetType() != typeof(Run)) continue;
                Run insertAtStart = new Run(playedList);
                insertAtStart.Insert(0, card);
                if (RunFinder.IsRun(insertAtStart))
                {
                    availablePlays.Add(card);
                }
            }
        }
    }

    private static void AddCardsToTopOfRuns(List<CardList> playZone, CardList cards, CardList availablePlays)
    {
        availablePlays.AddRange(
            from card in cards
            from cardList in playZone
            where cardList.GetType() == typeof(Run)
            let appendToEnd = new Run(cardList) { card }
            where RunFinder.IsRun(appendToEnd)
            select card
        );
    }
}