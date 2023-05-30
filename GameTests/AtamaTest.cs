using NUnit.Framework;

[TestFixture]
public class AtamaTest
{
    private Card _cardJaD;
    private Card _cardJaH1;
    private Card _cardJaH2;
    private Card _cardJaS;

    [SetUp]
    public void SetUp()
    {
        _cardJaD = new Card(Rank.Jack, Suit.Diamonds);
        _cardJaH1 = new Card(Rank.Jack, Suit.Hearts);
        _cardJaH2 = new Card(Rank.Jack, Suit.Hearts);
        _cardJaS = new Card(Rank.Jack, Suit.Spades);
    }

    [Test]
    public void EqualsWhenSameOrder()
    {
        Atama atama01 = new() { _cardJaD, _cardJaH1, _cardJaS };
        Atama atama02 = new() { _cardJaD, _cardJaH1, _cardJaS };

        Assert.IsTrue(atama01.Equals(atama02));
    }

    [Test]
    public void EqualsAsDifferentObjectsButSameCards()
    {
        Atama atama01 = new() { _cardJaD, _cardJaH1, _cardJaS };
        Atama atama02 = new() { _cardJaD, _cardJaH2, _cardJaS };

        Assert.IsTrue(atama01.Equals(atama02));
    }

    [Test]
    public void EqualsWhenDifferentOrder()
    {
        Atama atama01 = new() { _cardJaD, _cardJaH1, _cardJaS };
        Atama atama02 = new() { _cardJaS, _cardJaD, _cardJaH1 };

        Assert.IsTrue(atama01.Equals(atama02));
    }

    [Test]
    public void NotEqualsWithDifferentCards()
    {
        Atama atama01 = new() { _cardJaD, _cardJaH1, _cardJaH2 };
        Atama atama02 = new() { _cardJaD, _cardJaH1, _cardJaS };

        Assert.IsFalse(atama01.Equals(atama02));
    }
}