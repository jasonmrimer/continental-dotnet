using System;
using System.Collections.Generic;
using System.Linq;

public class Deck
{
    private CardList _cards;

    public Deck()
    {
        _cards = new CardList();
        const int numberOfDecks = 2;
        
        for (int i = 0; i < numberOfDecks; i++)
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                if (IsJoker(suit))
                {
                    CreateJokers(suit);
                }
                else
                {
                    CreateAllNonJokers(suit);
                }
            }
        }
    }

    private void CreateAllNonJokers(Suit suit)
    {
        for (int j = 1; j <= 13; j++)
        {
            Rank rank = (Rank)j;
            Card card = new Card(rank, suit);
            _cards.Add(card);
        }
    }

    private void CreateJokers(Suit suit)
    {
        Card joker1 = new Card(Rank.Joker, suit);
        Card joker2 = new Card(Rank.Joker, suit);
        _cards.Add(joker1);
        _cards.Add(joker2);
    }

    private static bool IsJoker(Suit suit)
    {
        return suit == Suit.Wild;
    }

    public int CardCount()
    {
        return _cards.Count;
    }

    public Card DrawCard()
    {
        if (CardCount() == 0)
        {
            return null;
        }
        Card drawnCard = _cards.Last();
        _cards.RemoveAt(_cards.Count - 1);
        return drawnCard;
    }

    public void AddCards(List<Card> discardPile)
    {
        _cards.AddRange(discardPile);
    }

    public List<Card> Cards => _cards;
}