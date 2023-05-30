using System;

public enum Rank
{
    Joker = 0,
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}

public class Card
{
    public readonly Suit Suit;
    public readonly Rank Rank;

    public Card(Rank rank, Suit suit)
    {
        this.Suit = suit;
        this.Rank = rank;
    }

    public string ValueText()
    {
        switch (this.Rank) {
            case Rank.Ace:
                return "A";
            case Rank.Jack:
                return "J";
            case Rank.Queen:
                return "Q";
            case Rank.King:
                return "K";
            case Rank.Joker:
                return "Jo";
            default:
                return ((int)this.Rank).ToString();
        } 
    }
    
    public string SuitSymbol()
    {
        switch (this.Suit) {
            case Suit.Hearts:
                return "♥";
            case Suit.Diamonds:
                return "♦";
            case Suit.Clubs:
                return "♣";
            case Suit.Spades:
                return "♠";
            case Suit.Wild:
                return "★";
            default:
                return base.ToString();
        }
    }

    public string Printable()
    {
        return $"{ValueText()}{this.SuitSymbol()}";
    }

    public override string ToString()
    {
        return $"{Printable()}";
    }

    public override bool Equals(object obj)
    {
        if (obj is not Card objCard)
        {
            return false;
        }
        return objCard.Rank == Rank && objCard.Suit == Suit;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Rank, Suit);
    }
}