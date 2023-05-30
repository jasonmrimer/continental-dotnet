using System;
using NUnit.Framework;

[TestFixture]
public class DeckTest
{
    [Test]
    public void CreatesTwoStandardDecksWithJokers()
    {
        Deck deck = new Deck();

        Assert.AreEqual(108, deck.CardCount());
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Ace).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Two).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Three).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Four).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Five).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Six).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Seven).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Eight).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Nine).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Ten).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Jack).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.Queen).Count);
        Assert.AreEqual(8, deck.Cards.FindAll(card => card.Rank == Rank.King).Count);
        Assert.AreEqual(4, deck.Cards.FindAll(card => card.Rank == Rank.Joker).Count);

        Assert.AreEqual(26, deck.Cards.FindAll(card => card.Suit == Suit.Clubs).Count);
        Assert.AreEqual(26, deck.Cards.FindAll(card => card.Suit == Suit.Diamonds).Count);
        Assert.AreEqual(26, deck.Cards.FindAll(card => card.Suit == Suit.Hearts).Count);
        Assert.AreEqual(26, deck.Cards.FindAll(card => card.Suit == Suit.Spades).Count);
        Assert.AreEqual(4, deck.Cards.FindAll(card => card.Suit == Suit.Wild).Count);
    }

    [Test]
    public void DrawsCard()
    {
        Deck deck = new Deck();
        Card drawnCard = deck.DrawCard();
        if (drawnCard.Rank == Rank.Joker)
        {
            Assert.AreEqual(3, deck.Cards.FindAll(card => Equals(card, drawnCard)).Count);
        }
        else
        {
            Assert.AreEqual(1, deck.Cards.FindAll(card => Equals(card, drawnCard)).Count);
        }
    }

    [Test]
    public void DoesNotDrawCardIfEmpty()
    {
        Deck deck = new Deck();
        for (int i = 0; i < 108; i++)
        {
            deck.DrawCard();
            Console.WriteLine(i);
        }

        Card drawnCard = deck.DrawCard();
        Assert.IsNull(drawnCard);
    }

    [Test]
    public void AddCards()
    {
        Deck deck = new Deck();
        deck.AddCards(new CardList()
        {
            new(Rank.Ace, Suit.Clubs),
            new(Rank.Ace, Suit.Diamonds),
            new(Rank.Ace, Suit.Hearts),
            new(Rank.Ace, Suit.Spades),
        });

        Assert.AreEqual(112, deck.CardCount());
        Assert.AreEqual(12, deck.Cards.FindAll(card => card.Rank == Rank.Ace).Count);

        Assert.AreEqual(27, deck.Cards.FindAll(card => card.Suit == Suit.Clubs).Count);
        Assert.AreEqual(27, deck.Cards.FindAll(card => card.Suit == Suit.Diamonds).Count);
        Assert.AreEqual(27, deck.Cards.FindAll(card => card.Suit == Suit.Hearts).Count);
        Assert.AreEqual(27, deck.Cards.FindAll(card => card.Suit == Suit.Spades).Count);
    }
}