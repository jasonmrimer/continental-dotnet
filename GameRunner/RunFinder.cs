public enum RunLocation
{
    Start,
    End,
    StartOrEnd,
    None,
}

public abstract class RunFinder
{
    public static List<Run> FindPossibleRuns(CardList cards)
    {
        List<Run> runOptions = new();
        IEnumerable<IGrouping<Suit, Card>> suitGroups = cards.GroupBy(card => card.Suit);

        foreach (IGrouping<Suit, Card> suitGroup in suitGroups)
        {
            HashSet<Card> singleCopySuitedCards = new(suitGroup);

            foreach (Card runStartCard in singleCopySuitedCards)
            {
                Run run = new() { runStartCard };

                for (int nextRankInt = (int)(runStartCard.Rank + 1); nextRankInt <= 14; nextRankInt++)
                {
                    Rank nextRank = nextRankInt == 14 ? Rank.Ace : (Rank)nextRankInt;
                    Card nextCardInRun = new(nextRank, runStartCard.Suit);

                    if (singleCopySuitedCards.TryGetValue(nextCardInRun, out nextCardInRun))
                    {
                        run.Add(nextCardInRun);
                    }
                    else
                    {
                        break;
                    }

                    if (IsRun(run))
                    {
                        runOptions.Add(new Run(run));
                    }
                }
            }
        }

        return runOptions;
    }

    public static bool IsRun(CardList cards)
    {
        if (cards.Count < 4)
        {
            return false;
        }

        if (!CardsAreSameSuit(cards))
        {
            return false;
        }


        // Check if the cards have consecutive ranks, accounting for Aces as both 1 and 14
        for (int i = 1; i < cards.Count; i++)
        {
            Rank currentRank = cards[i].Rank;
            Rank previousRank = cards[i - 1].Rank;

            switch (previousRank)
            {
                case Rank.Ace:
                {
                    if (currentRank != Rank.Two)
                    {
                        return false;
                    }

                    break;
                }
                case Rank.King:
                {
                    // Handle Ace as both 1 and 14
                    if (currentRank != Rank.Ace)
                    {
                        return false;
                    }

                    break;
                }
                default:
                {
                    if (currentRank != previousRank + 1)
                    {
                        return false;
                    }

                    break;
                }
            }
        }

        return true;
    }

    private static bool CardsAreSameSuit(CardList cards)
    {
        Suit firstSuit = cards[0].Suit;
        return cards.All(card => card.Suit == firstSuit);
    }


    public static bool CanAddToRun(Run run, Card card)
    {
        return FindAdditionLocation(run, card) != RunLocation.None;
    }

    public static RunLocation FindAdditionLocation(Run run, Card card)
    {
        Run runAddToEnd = new Run(run) { card };
        Run runAddToStart = new Run(run);
        runAddToStart.Insert(0, card);

        if (IsRun(runAddToStart) && IsRun(runAddToEnd))
        {
            return RunLocation.StartOrEnd;
        }

        if (IsRun(runAddToStart))
        {
            return RunLocation.Start;
        }

        if (IsRun(runAddToEnd))
        {
            return RunLocation.End;
        }

        return RunLocation.None;
    }

    public static Run AddCardsToRun(Run startingRun, CardList cardsToAdd)
    {
        Run endingRun = new Run(startingRun);
        CardList remainingCards = new CardList(cardsToAdd);

        while (remainingCards.Count > 0)
        {
            CardList cardsToRemove = new();

            foreach (Card card in remainingCards)
            {
                RunLocation location = FindAdditionLocation(endingRun, card);

                switch (location)
                {
                    case RunLocation.Start:
                        endingRun.Insert(0, card);
                        cardsToRemove.Add(card);
                        break;
                    case RunLocation.End:
                        endingRun.Add(card);
                        cardsToRemove.Add(card);
                        break;
                    case RunLocation.StartOrEnd:
                        endingRun.Insert(0, card);
                        cardsToRemove.Add(card);
                        break;
                    case RunLocation.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (CannotAddMoreCardsToRun())
            {
                break;
            }

            remainingCards.RemoveRange(cardsToRemove);

            bool CannotAddMoreCardsToRun()
            {
                return cardsToRemove.Count == 0;
            }
        }

        return endingRun;
    }

    public static Run AddCardToRun(Run run, Card card)
    {
        return AddCardsToRun(run, new CardList { card });
    }
}