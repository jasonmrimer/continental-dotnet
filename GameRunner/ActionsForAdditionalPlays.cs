namespace GameRunner;

public static class ActionsForAdditionalPlays
{
    public static CardList AvailablePlays(
        List<CardList> playZone,
        CardList cards
    )
    {
        CardList availablePlays = new();
        
        availablePlays.AddRange(
            from card in cards
            from cardList in playZone
            where cardList.GetType() == typeof(Run)
            let appendToEnd = new Run(cardList) { card }
            where RunFinder.IsRun(appendToEnd)
            select card
        );

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
        
        return availablePlays;
    }
}