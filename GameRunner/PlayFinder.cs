namespace GameRunner;

public static class PlayFinder
{
    public static List<PlayAction> AvailablePlaysV2(
        List<CardList> playZone,
        CardList cards
    )
    {
        List<PlayAction> availablePlays = new();

        AddCardsToRun(playZone, cards, availablePlays);

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
        foreach (CardList runOrAtama in playZone)
        {
            if (runOrAtama.GetType() == typeof(Run))
            {
                availablePlays.AddRange(
                    RecursiveRunAdderBottom(
                        new List<PlayAction>(),
                        new PlayAction(runOrAtama, new CardList()),
                        (Run)runOrAtama,
                        cards
                    ));
            }
        }
        // foreach (Card card in cards)
        // {
        //     foreach (CardList playedList in playZone)
        //     {
        //         if (playedList.GetType() != typeof(Run)) continue;
        //         Run insertAtStart = new Run(playedList);
        //         insertAtStart.Insert(0, card);
        //         if (RunFinder.IsRun(insertAtStart))
        //         {
        //             availablePlays.Add(new PlayAction(
        //                 playedList,
        //                 card
        //             ));
        //         }
        //     }
        // }
    }

    private static void AddCardsToTopOfRunsV2(
        List<CardList> playZone,
        CardList cards,
        List<PlayAction> availablePlays)
    {
        foreach (CardList runOrAtama in playZone)
        {
            if (runOrAtama.GetType() == typeof(Run))
            {
                availablePlays.AddRange(
                    RecursiveRunAdderTop(
                        new List<PlayAction>(),
                        new PlayAction(runOrAtama, new CardList()),
                        (Run)runOrAtama,
                        cards
                    ));
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
                    run,
                    cards
                ));
        }
    }

    private static List<Run> SelectRunsFromPlayZone(List<CardList> playZone)
    {
        return playZone.Where(runOrAtama =>
            runOrAtama.GetType() == typeof(Run)).Cast<Run>().ToList();
    }

    private static List<PlayAction> RecursiveRunAdderTop(
        List<PlayAction> playOptions,
        PlayAction currentPlay,
        Run baseRun,
        CardList additionalCards
    )
    {
        if (additionalCards.Count == 0)
        {
            return playOptions;
        }

        foreach (Card card in additionalCards)
        {
            CardList remainingCards = new CardList(additionalCards);

            Run potentialRun = AppendDiscoveredAndNewCardsToEndOfRun(currentPlay, baseRun, card);

            if (!RunFinder.IsRun(potentialRun)) continue;
            CardList playableCards = new CardList(currentPlay.CardsToAdd) { card };

            currentPlay = new PlayAction(baseRun, playableCards);
            playOptions.Add(currentPlay);
            remainingCards.Remove(card);
            return RecursiveRunAdderTop(playOptions, currentPlay, baseRun, remainingCards);
        }

        return playOptions;
    }

    private static List<PlayAction> RecursiveRunAdder(
        List<PlayAction> playOptions,
        PlayAction currentPlay,
        Run baseRun,
        CardList additionalCards
    )
    {
        if (additionalCards.Count == 0)
        {
            return playOptions;
        }

        foreach (Card card in additionalCards)
        {
            CardList remainingCards = new CardList(additionalCards);
            Run currentRun = CreateCurrentRunFromDiscoveredAdditions(currentPlay);
            
            // add already discovered cards to make current run could be top and bottom
            // then try to add to top or bottom
            Run potentialRun = AppendDiscoveredAndNewCardsToEndOfRun(currentPlay, baseRun, card);

            if (!RunFinder.IsRun(potentialRun)) continue;
            CardList playableCards = new CardList(currentPlay.CardsToAdd) { card };

            currentPlay = new PlayAction(baseRun, playableCards);
            playOptions.Add(currentPlay);
            remainingCards.Remove(card);
            return RecursiveRunAdderTop(playOptions, currentPlay, baseRun, remainingCards);
        }

        return playOptions;
    }

    private static Run CreateCurrentRunFromDiscoveredAdditions(PlayAction currentPlay)
    {
        throw new NotImplementedException();
    }

    private static List<PlayAction> RecursiveRunAdderBottom(
        List<PlayAction> playOptions,
        PlayAction currentPlay,
        Run baseRun,
        CardList additionalCards
    )
    {
        if (additionalCards.Count == 0)
        {
            return playOptions;
        }

        foreach (Card card in additionalCards)
        {
            CardList remainingCards = new CardList(additionalCards);

            Run potentialRun = AppendDiscoveredAndNewCardsToStartOfRun(currentPlay, baseRun, card);

            if (!RunFinder.IsRun(potentialRun)) continue;
            CardList playableCards = new CardList(currentPlay.CardsToAdd) { card };

            currentPlay = new PlayAction(baseRun, playableCards);
            playOptions.Add(currentPlay);
            remainingCards.Remove(card);
            return RecursiveRunAdderBottom(playOptions, currentPlay, baseRun, remainingCards);
        }

        return playOptions;
    }

    private static Run AppendDiscoveredAndNewCardsToEndOfRun(PlayAction currentPlay, Run baseRun, Card card)
    {
        Run appendToEnd = new Run(baseRun);

        foreach (Card discoveredCard in currentPlay.CardsToAdd)
        {
            appendToEnd.Add(discoveredCard);
        }

        appendToEnd.Add(card);
        return appendToEnd;
    }

    private static Run AppendDiscoveredAndNewCardsToStartOfRun(PlayAction currentPlay, Run baseRun, Card card)
    {
        Run appendToStart = new Run(baseRun);

        foreach (Card discoveredCard in currentPlay.CardsToAdd)
        {
            // appendToStart.Add(discoveredCard);
            appendToStart.Insert(0, discoveredCard);
        }

        return appendToStart;
    }
}