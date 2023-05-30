using System;
using System.Collections.Generic;
using System.Linq;

public abstract class RunFinder
{
    public static List<Run> FindPossibleRuns(CardList cards)
    {
        List<Run> runOptions = new();
        IEnumerable<IGrouping<Suit,Card>> suitGroups = cards.GroupBy(card => card.Suit);

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

    public static bool IsRun(CardList suitedAndSortedCards)
    {
        if (suitedAndSortedCards.Count < 4)
        {
            return false;
        }

        // Check if the cards have consecutive ranks, accounting for Aces as both 1 and 14
        for (int i = 1; i < suitedAndSortedCards.Count; i++)
        {
            Rank currentRank = suitedAndSortedCards[i].Rank;
            Rank previousRank = suitedAndSortedCards[i - 1].Rank;

            if (previousRank == Rank.Ace)
            {
                if (currentRank != Rank.Two)
                {
                    return false;
                }
            }
            else if (previousRank == Rank.King)
            {
                // Handle Ace as both 1 and 14
                if (currentRank != Rank.Ace)
                {
                    return false;
                }
            }
            else if (currentRank != previousRank + 1)
            {
                return false;
            }
        }

        return true;
    }
}