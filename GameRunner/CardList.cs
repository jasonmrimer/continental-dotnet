using System.Collections.Generic;
using System.Linq;

public class CardList : List<Card>
{
    public CardList(IOrderedEnumerable<Card> orderBy)
    {
        AddRange(orderBy);
    }

    public CardList()
    {
    }

    public CardList(List<Card> cards)
    {
        AddRange(cards);
    }

    public void RemoveRange(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            RemoveFirstMatch(card);
        }
    }

    private void RemoveFirstMatch(Card card)
    {
        for (int i = 0; i < this.Count; i++)
        {
            if (this[i].Suit == card.Suit && this[i].Rank == card.Rank)
            {
                this.RemoveAt(i);
                break;
            }
        }
    }

    private void RemoveLastMatch(Card card)
    {
        for (int i = this.Count - 1; i > -1; i--)
        {
            if (this[i].Suit == card.Suit && this[i].Rank == card.Rank)
            {
                this.RemoveAt(i);
                break;
            }
        }
    }

    public override string ToString()
    {
        if (this.Count == 0)
        {
            return "empty card list";
        }
        string aggregate = this.Aggregate("", (current, card) => current + $"{card}, ");
        return aggregate.Remove(aggregate.Length - 2, 2);
    }

    public override bool Equals(object? obj)
    {
        CardList other = (CardList)obj;
        if (other == null)
        {
            return false;
        }

        if (other == this)
        {
            return true;
        }
        
        other.OrderBy(c => c.Suit);
        this.OrderBy(c => c.Suit);

        return this.OrderBy(c => c.Rank).SequenceEqual(other.OrderBy(c => c.Rank));
    }
}