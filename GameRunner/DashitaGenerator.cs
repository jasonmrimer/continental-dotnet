using System;
using System.Collections.Generic;

public class DashitaGenerator
{
    public static HashSet<Dashita> GenerateOptions(CardList hand)
    {
        List<Run> runOptions = RunFinder.FindPossibleRuns(hand);
        HashSet<Dashita> dashitaOptions = new(new DashitaEqualityComparer());

        // Iterate through each possible combination Runs
        foreach (Run initialRun in runOptions)
        {
            CardList remainingCardsAfterInitial = new(hand);
            RemoveCardsFromList(remainingCardsAfterInitial, initialRun);

            List<Run> additionalRuns = RunFinder.FindPossibleRuns(remainingCardsAfterInitial);

            // Iterate through each possible combination of 2 Runs
            foreach (Run additionalRun in additionalRuns)
            {
                CardList remainingCardsAfterAdditional = new(remainingCardsAfterInitial);
                RemoveCardsFromList(remainingCardsAfterAdditional, additionalRun);

                List<Atama> atamasAvailable = AtamaFinder.FindAtama(remainingCardsAfterAdditional);

                foreach (Atama atama in atamasAvailable)
                {
                    dashitaOptions.Add(new Dashita(
                        new List<Run>() { initialRun, additionalRun },
                        atama
                    ));
                }
            }
        }

        return dashitaOptions;
    }

    private static void RemoveCardsFromList(List<Card> cards, IEnumerable<Card> cardsToRemove)
    {
        foreach (Card card in cardsToRemove)
        {
            cards.Remove(card);
        }
    }
}