using System.Collections;

namespace GameRunner;

public static class PlayFinder
{
    public static List<PlayAction> FindPlays(
        List<CardList> playZone,
        CardList cards
    )
    {
        List<PlayAction> availablePlays = new();

        AddCardsToExistingRuns(playZone, cards, availablePlays);

        AddCardsToExistingAtamas(playZone, cards, availablePlays);

        return availablePlays;
    }

    private static void AddCardsToExistingAtamas(
        IReadOnlyCollection<CardList> playZone,
        CardList cards,
        List<PlayAction> availablePlays)
    {
        availablePlays.AddRange(
            from card in cards
            from playedList in playZone
            where playedList.GetType() == typeof(Atama)
            let addToAtama = new Atama(playedList) { card }
            where AtamaFinder.IsAtama(addToAtama)
            select new PlayAction(playedList, card)
        );
    }

    private static void AddCardsToExistingRuns(
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

    private static List<Run> SelectRunsFromPlayZone(IEnumerable<CardList> playZone)
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

            if (CardNotAddedToRun(potentialRun, currentRun)) continue;

            CardList playableCards = new CardList(currentPlay.CardsToAdd) { card };

            PlayAction newPlay = new PlayAction(baseRun, playableCards);
            playOptions.Add(newPlay);
            remainingCards.Remove(card);
            return RecursiveRunAdder(playOptions, newPlay, remainingCards);
        }

        return playOptions;
    }

    private static bool CardNotAddedToRun(IEnumerable potentialRun, IEnumerable currentRun)
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