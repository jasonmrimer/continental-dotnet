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

        return availablePlays;
    }
}