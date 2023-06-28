namespace GameRunner;

public static class PlayFinder
{
    public static List<PlayAction> AvailablePlaysV2(
        List<CardList> playZone,
        CardList cards
    )
    {
        List<PlayAction> availablePlays = new();

        AddCardsToTopOfRunsV2(playZone, cards, availablePlays);

        AddCardsToBottomOfRunsV2(playZone, cards, availablePlays);

        AddCardsToAtamaV2(playZone, cards, availablePlays);

        return availablePlays;
    }

    private static void AddCardsToAtamaV2(
        List<CardList> playZone,
        CardList cards,
        List<PlayAction> availablePlays)
    {
        foreach (Card card in cards)
        {
            foreach (CardList playedList in playZone)
            {
                if (playedList.GetType() != typeof(Atama)) continue;
                Atama addToAtama = new Atama(playedList) { card };
                if (AtamaFinder.IsAtama(addToAtama))
                {
                    availablePlays.Add(
                        new PlayAction(
                            playedList,
                            card
                        )
                    );
                }
            }
        }
    }

    private static void AddCardsToBottomOfRunsV2(List<CardList> playZone, CardList cards,
        List<PlayAction> availablePlays)
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
                    availablePlays.Add(new PlayAction(
                        playedList,
                        card
                    ));
                }
            }
        }
    }

    private static void AddCardsToTopOfRunsV2(
        List<CardList> playZone,
        CardList cards,
        List<PlayAction> availablePlays)
    {
        // todo group and sort cards by suit/rank
        cards.OrderBy(c => c.Rank);

        availablePlays.AddRange(
            from card in cards
            from cardList in playZone
            where cardList.GetType() == typeof(Run)
            let appendToEnd = new Run(cardList) { card }
            where RunFinder.IsRun(appendToEnd)
            select new PlayAction(cardList, card)
        );
    }
}