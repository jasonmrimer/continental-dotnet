using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class AtamaFinderTest
{
    Card _card02C = new(Rank.Two, Suit.Clubs);
    Card _card02D = new(Rank.Two, Suit.Diamonds);
    Card _card02H = new(Rank.Two, Suit.Hearts);

    Card _card05C = new(Rank.Five, Suit.Clubs);
    Card _card06C = new(Rank.Six, Suit.Clubs);

    Card _card08C = new(Rank.Eight, Suit.Clubs);
    Card _card08D = new(Rank.Eight, Suit.Diamonds);
    Card _card08H = new(Rank.Eight, Suit.Hearts);
    Card _card08S = new(Rank.Eight, Suit.Spades);

    Card _cardQuC = new(Rank.Queen, Suit.Clubs);
    Card _cardQuD = new(Rank.Queen, Suit.Diamonds);
    Card _cardQuH1 = new(Rank.Queen, Suit.Hearts);
    Card _cardQuH2 = new(Rank.Queen, Suit.Hearts);

    private List<Card> _expectedAtama02s;
    private List<Card> _expectedAtama08s1;
    private List<Card> _expectedAtama08s2;
    private List<Card> _expectedAtama08s3;
    private List<Card> _expectedAtama08s4;
    private List<Card> _expectedAtama08s5;
    private List<Card> _expectedAtamaQus1;
    private List<Card> _expectedAtamaQus2;
    private List<Card> _expectedAtamaQus3;
    private List<Card> _expectedAtamaQus4;
    private List<Card> _expectedAtamaQus5;

    [SetUp]
    public void SetUp()
    {
        _expectedAtama02s = new List<Card>()
        {
            _card02C, _card02D, _card02H
        };

        _expectedAtama08s1 = new List<Card>()
        {
            _card08C, _card08D, _card08H, _card08S,
        };
        
        _expectedAtama08s2 = new List<Card>()
        {
            _card08C, _card08D, _card08H,
        };
        
        _expectedAtama08s3 = new List<Card>()
        {
            _card08D, _card08H, _card08S,
        };
        
        _expectedAtama08s4 = new List<Card>()
        {
            _card08C, _card08D,  _card08S,
        };
        
        _expectedAtama08s5 = new List<Card>()
        {
            _card08C,  _card08H, _card08S,
        };

        _expectedAtamaQus1 = new List<Card>()
        {
            _cardQuC, _cardQuD, _cardQuH1, 
        };
        
        _expectedAtamaQus2 = new List<Card>()
        {
            _cardQuC, _cardQuD, _cardQuH2,
        };
        
        _expectedAtamaQus3 = new List<Card>()
        {
            _cardQuC, _cardQuH1, _cardQuH2,
        };
        
        _expectedAtamaQus4 = new List<Card>()
        { 
            _cardQuD, _cardQuH1, _cardQuH2,
        };
        
        _expectedAtamaQus5 = new List<Card>()
        {
            _cardQuC, _cardQuD, _cardQuH1, _cardQuH2,
        };
    }

    [Test]
    public void FindsSimpleAtama()
    {
        List<Card> hand = new()
        {
            _card02C, _card02D, _card02H
        };

        List<Atama> atamaFound = AtamaFinder.FindAtama(hand);

        Assert.AreEqual(1, atamaFound.Count);
        Assert.Contains(_expectedAtama02s, atamaFound);
    }

    [Test]
    public void Finds5UniqueAtamaFrom4Cards()
    {
        List<Card> hand = new()
        {
            _card08C, _card08D, _card08H, _card08S
        };

        List<Atama> atamaFound = AtamaFinder.FindAtama(hand);

        Assert.AreEqual(5, atamaFound.Count);
        AssertFindsAtamasOf8s(atamaFound);
    }

    private void AssertFindsAtamasOf8s(List<Atama> atamaFound)
    {
        Assert.Contains(_expectedAtama08s1, atamaFound);
        Assert.Contains(_expectedAtama08s2, atamaFound);
        Assert.Contains(_expectedAtama08s3, atamaFound);
        Assert.Contains(_expectedAtama08s4, atamaFound);
        Assert.Contains(_expectedAtama08s5, atamaFound);
    }

    [Test]
    public void FindsSimpleAtamaWithDistractions()
    {
        List<Card> hand = new()
        {
            _card02C, _card02D, _card02H, _card08C,
        };

        List<Atama> atamaFound = AtamaFinder.FindAtama(hand);

        Assert.AreEqual(1, atamaFound.Count);
        Assert.Contains(_expectedAtama02s, atamaFound);
    }

    [Test]
    public void FindsAtamaFromMultipleRanks()
    { 
        List<Card> hand = new()
        {
            _card02C, _card02D, _card02H, 
            _card08C, _card08D, _card08H, _card08S,
        };

        List<Atama> atamaFound = AtamaFinder.FindAtama(hand);

        Assert.AreEqual(6, atamaFound.Count);
        Assert.Contains(_expectedAtama02s, atamaFound);
        AssertFindsAtamasOf8s(atamaFound);
    }
    
    [Test]
    public void FindsAtamaWithSameSuitSameRank()
    {
        List<Card> hand = new()
        {
            _cardQuC, _cardQuD, _cardQuH1, _cardQuH2,
        };

        List<Atama> atamaFound = AtamaFinder.FindAtama(hand);

        Assert.AreEqual(5, atamaFound.Count);
        Assert.Contains(_expectedAtamaQus1, atamaFound);
        Assert.Contains(_expectedAtamaQus2, atamaFound);
        Assert.Contains(_expectedAtamaQus3, atamaFound);
        Assert.Contains(_expectedAtamaQus4, atamaFound);
        Assert.Contains(_expectedAtamaQus5, atamaFound);
    }
        
}