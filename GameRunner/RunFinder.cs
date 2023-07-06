using System;
using System.Collections.Generic;
using System.Linq;

public enum RunLocation
{
    START,
    END,
    START_OR_END,
    NONE,
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
                    Card nextCardInRun = new Card(nextRank, runStartCard.Suit);

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

        if (!CardsAreSameRank(cards))
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

    private static bool CardsAreSameRank(CardList cards)
    {
        Suit firstSuit = cards[0].Suit;
        return cards.All(card => card.Suit == firstSuit);
    }


    public static bool CanAddToRun(Run run, Card card)
    {
        return FindAdditionLocation(run, card) != RunLocation.NONE;
    }

    public static RunLocation FindAdditionLocation(Run run, Card card)
    {
        Run runAddToEnd = new Run(run) { card };
        Run runAddToStart = new Run(run);
        runAddToStart.Insert(0, card);

        if (IsRun(runAddToStart) && IsRun(runAddToEnd))
        {
            return RunLocation.START_OR_END;
        }

        if (IsRun(runAddToStart))
        {
            return RunLocation.START;
        }

        if (IsRun(runAddToEnd))
        {
            return RunLocation.END;
        }

        return RunLocation.NONE;
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
                    case RunLocation.START:
                        endingRun.Insert(0, card);
                        cardsToRemove.Add(card);
                        break;
                    case RunLocation.END:
                        endingRun.Add(card);
                        cardsToRemove.Add(card);
                        break;
                    case RunLocation.START_OR_END:
                        endingRun.Insert(0, card);
                        cardsToRemove.Add(card);
                        break;
                    case RunLocation.NONE:
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