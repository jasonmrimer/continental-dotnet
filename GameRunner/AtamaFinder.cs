using System.Collections.Generic;
using System.Linq;

public class AtamaFinder
{
    public static List<Atama> FindAtama(List<Card> cards)
    {
        List<Atama> atamaOptions = new();
        Dictionary<Rank, CardList> rankGroups = GroupCardsByRank(cards);

        foreach (var rankGroup in rankGroups)
        {
            CardList cardsOfCurrentRank = rankGroup.Value;
            if (cardsOfCurrentRank.Count >= 3)
            {
                GenerateAtamaCombinations(
                    cardsOfCurrentRank,
                    currentCombination: new CardList(),
                    atamaOptions
                );
            }
        }

        return atamaOptions;
    }

    private static void GenerateAtamaCombinations(
        CardList cards,
        CardList currentCombination,
        List<Atama> combinations
    )
    {
        if (currentCombination.Count >= 3)
        {
            Atama atamaSortedToPreventDupes = new(currentCombination.OrderBy(card => card.Suit));
            combinations.Add(atamaSortedToPreventDupes);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            currentCombination.Add(card);

            // Recursive call with remaining cards to generate combinations
            CardList range = new CardList(cards.GetRange(i + 1, cards.Count - (i + 1)));
            GenerateAtamaCombinations(
                range,
                currentCombination,
                combinations
            );

            currentCombination.Remove(card);
        }
    }

    private static Dictionary<Rank, CardList> GroupCardsByRank(List<Card> cards)
    {
        Dictionary<Rank, CardList> rankGroups = new Dictionary<Rank, CardList>();

        foreach (Card card in cards)
        {
            if (!rankGroups.ContainsKey(card.Rank))
            {
                rankGroups[card.Rank] = new CardList();
            }

            rankGroups[card.Rank].Add(card);
        }

        return rankGroups;
    }
}