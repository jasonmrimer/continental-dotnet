namespace GameRunner;

public static class PlayFinder
{
    public static List<PlayAction> FindPlays(
        List<CardList> playZone,
        CardList cards
    )
    {
        List<PlayAction> availablePlays = new();

        AddCardsToRun(playZone, cards, availablePlays);

        AddCardsToAtama(playZone, cards, availablePlays);

        return availablePlays;
    }

    private static void AddCardsToAtama(
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

    private static void AddCardsToRun(
        List<CardList> playZone,
        CardList cards,
        List<PlayAction> availablePlays)
    {
        List<Run> availableRuns = SelectRunsFromPlayZone(playZone);

        foreach (Run run in availableRuns)
        {
            availablePlays.AddRange(
                RecursiveRunAdder(
                    new List<PlayAction>(),
                    new PlayAction(run, new CardList()),
                    cards
                ));
        }
    }

    private static List<Run> SelectRunsFromPlayZone(List<CardList> playZone)
    {
        return playZone.Where(runOrAtama =>
            runOrAtama.GetType() == typeof(Run)).Cast<Run>().ToList();
    }

    private static List<PlayAction> RecursiveRunAdder(
        List<PlayAction> playOptions,
        PlayAction currentPlay,
        CardList additionalCards
    )
    {
        Run baseRun = (Run)currentPlay.RunOrAtama;

        if (additionalCards.Count == 0)
        {
            return playOptions;
        }

        foreach (Card card in additionalCards)
        {
            CardList remainingCards = new CardList(additionalCards);
            Run currentRun = CreateCurrentRunFromDiscoveredAdditions(currentPlay);

            Run potentialRun = RunFinder.AddCardToRun(currentRun, card);

            if (cardNotAddedToRun(potentialRun, currentRun)) continue;

            CardList playableCards = new CardList(currentPlay.CardsToAdd) { card };

            PlayAction newPlay = new PlayAction(baseRun, playableCards);
            playOptions.Add(newPlay);
            remainingCards.Remove(card);
            return RecursiveRunAdder(playOptions, newPlay, remainingCards);
        }

        return playOptions;
    }

    private static bool cardNotAddedToRun(Run potentialRun, Run currentRun)
    {
        return potentialRun.Equals(currentRun);
    }

    private static Run CreateCurrentRunFromDiscoveredAdditions(PlayAction currentPlay)
    {
        return RunFinder.AddCardsToRun(
            (Run)currentPlay.RunOrAtama,
            currentPlay.CardsToAdd
        );
    }
}