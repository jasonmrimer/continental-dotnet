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
        cards.OrderBy(c => c.Rank);


        //     availablePlays.AddRange(
        //         from card in cards
        //         from cardList in playZone
        //         where cardList.GetType() == typeof(Run)
        //         let appendToEnd = new Run(cardList) { card }
        //         where RunFinder.IsRun(appendToEnd)
        //         select new PlayAction(cardList, card)
        //     );
        // }

        foreach (CardList runOrAtama in playZone)
        {
            if (runOrAtama.GetType() == typeof(Run))
            {
                availablePlays.AddRange(RecursiveRunAdder(
                    new List<PlayAction>(),
                    null,
                    (Run)runOrAtama,
                    cards
                ));
            }
        }

        // availablePlays.AddRange(
        //     from cardList in playZone
        //     where cardList.GetType() == typeof(Run)
        //     from card in cards
        //     let appendToEnd = new Run(cardList) { card }
        //     where RunFinder.IsRun(appendToEnd)
        //     select new PlayAction(cardList, card)
        // );
    }

    private static List<PlayAction> RecursiveRunAdder(
        List<PlayAction> playOptions,
        PlayAction currentPlay,
        Run baseRun,
        CardList additionalCards
    )
    {
        if (currentPlay == null)
        {
            currentPlay = new PlayAction(baseRun, new CardList());
        }

        if (additionalCards.Count == 0)
        {
            return playOptions;
        }

        foreach (Card card in additionalCards)
        {
            CardList remainingCards = new CardList(additionalCards);
            Run appendToEnd = new Run(baseRun);

            foreach (Card discoveredCard in currentPlay.CardsToAdd)
            {
                appendToEnd.Add(discoveredCard);
            }

            appendToEnd.Add(card);

            if (RunFinder.IsRun(appendToEnd))
            {
                CardList playableCards = new CardList(currentPlay.CardsToAdd) { card };

                currentPlay = new PlayAction(baseRun, playableCards);
                playOptions.Add(currentPlay);
                remainingCards.Remove(card);
                return RecursiveRunAdder(playOptions, currentPlay, baseRun, remainingCards);
            }
        }

        return playOptions;
    }
}