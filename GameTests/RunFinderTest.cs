using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class RunFinderTest
{
    private Card _cardAcC;
    private Card _card06C;
    private Card _card08C;
    private Card _card09C;
    private Card _card10C;
    private Card _cardJaC;
    private Card _cardQuC;
    private Card _cardKiC;

    private Card _card03H;
    private Card _card04H;
    private Card _card05H;
    private Card _card06H;
    private Card _card08H;
    private Card _card09H;
    private Card _card10H;
    private Card _cardJaH;
    private Card _cardQuH;

    private List<Card> _expectedRun3Cto6C;
    private List<Card> _expectedRun2Cto6C;
    private List<Card> _expectedRun8CtoJaC;
    private List<Card> _expectedRun9CtoQuC;
    private List<Card> _expectedRun8CtoQuC;

    private List<Card> _expectedRun3Hto6H;
    private List<Card> _expectedRun8HtoJaH;
    private List<Card> _expectedRun9HtoQuH;
    private List<Card> _expectedRun8HtoQuH;
    private List<Card> _expectedRunAcCto04C;
    private List<Card> _expectedRunAcCto05C;
    private List<Card> _expectedRunJaCtoAcC;

    [SetUp]
    public void SetUp()
    {
        _cardAcC = new Card(Rank.Ace, Suit.Clubs);
        _card06C = new Card(Rank.Six, Suit.Clubs);
        _card08C = new Card(Rank.Eight, Suit.Clubs);
        _card09C = new Card(Rank.Nine, Suit.Clubs);
        _card10C = new Card(Rank.Ten, Suit.Clubs);
        _cardJaC = new Card(Rank.Jack, Suit.Clubs);
        _cardQuC = new Card(Rank.Queen, Suit.Clubs);
        _cardKiC = new Card(Rank.King, Suit.Clubs);

        _card03H = new Card(Rank.Three, Suit.Hearts);
        _card04H = new Card(Rank.Four, Suit.Hearts);
        _card05H = new Card(Rank.Five, Suit.Hearts);
        _card06H = new Card(Rank.Six, Suit.Hearts);
        _card08H = new Card(Rank.Eight, Suit.Hearts);
        _card09H = new Card(Rank.Nine, Suit.Hearts);
        _card10H = new Card(Rank.Ten, Suit.Hearts);
        _cardJaH = new Card(Rank.Jack, Suit.Hearts);
        _cardQuH = new Card(Rank.Queen, Suit.Hearts);


        _expectedRun3Cto6C = new List<Card>()
        {
            TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, _card06C
        };

        _expectedRun2Cto6C = new List<Card>()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, _card06C
        };

        _expectedRun8CtoJaC = new List<Card>()
        {
            _card08C, _card09C, _card10C, _cardJaC,
        };

        _expectedRun9CtoQuC = new List<Card>()
        {
            _card09C, _card10C, _cardJaC, _cardQuC
        };

        _expectedRun8CtoQuC = new List<Card>()
        {
            _card08C, _card09C, _card10C, _cardJaC, _cardQuC
        };


        _expectedRunAcCto04C = new List<Card>()
        {
            _cardAcC, TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C
        };


        _expectedRunAcCto05C = new List<Card>()
        {
            _cardAcC, TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C
        };


        _expectedRunJaCtoAcC = new List<Card>()
        {
            _cardJaC, _cardQuC, _cardKiC, _cardAcC
        };

        _expectedRun3Hto6H = new List<Card>()
        {
            _card03H, _card04H, _card05H, _card06H
        };

        _expectedRun8HtoJaH = new List<Card>()
        {
            _card08H, _card09H, _card10H, _cardJaH,
        };

        _expectedRun9HtoQuH = new List<Card>()
        {
            _card09H, _card10H, _cardJaH, _cardQuH
        };

        _expectedRun8HtoQuH = new List<Card>()
        {
            _card08H, _card09H, _card10H, _cardJaH, _cardQuH
        };
    }

    [Test]
    public void FindsSimpleRun()
    {
        CardList cards = new CardList()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, 
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        Assert.AreEqual(1, actualRuns.Count);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
    }
    
    [Test]
    public void Finds3AvailableRuns()
    {
        CardList cards = new CardList()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, _card06C
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        Assert.AreEqual(3, actualRuns.Count);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
        Assert.Contains(_expectedRun3Cto6C, actualRuns);
        Assert.Contains(_expectedRun2Cto6C, actualRuns);
    }

    [Test]
    public void Finds3AvailableRunsWithDistractors()
    {
        CardList cards = new()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, _card06C,
            new Card(Rank.Two, Suit.Hearts), new Card(Rank.Queen, Suit.Diamonds),
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        Assert.AreEqual(3, actualRuns.Count);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
        Assert.Contains(_expectedRun3Cto6C, actualRuns);
        Assert.Contains(_expectedRun2Cto6C, actualRuns);
    }

    [Test]
    public void Finds4AvailableRunsWithGap()
    {
        Card card02H = new Card(Rank.Two, Suit.Hearts);
        Card cardQuD = new Card(Rank.Queen, Suit.Diamonds);
        CardList cards = new()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, _card06C,
            card02H, cardQuD,
            _card08C, _card09C, _card10C, _cardJaC,
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        Assert.AreEqual(4, actualRuns.Count);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
        Assert.Contains(_expectedRun3Cto6C, actualRuns);
        Assert.Contains(_expectedRun2Cto6C, actualRuns);
        Assert.Contains(_expectedRun8CtoJaC, actualRuns);
    }

    [Test]
    public void Finds6AvailableRunsWithGapAndExtension()
    {
        CardList cards = new()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, _card06C,
            new Card(Rank.Two, Suit.Hearts), new Card(Rank.Queen, Suit.Diamonds),
            _card08C, _card09C, _card10C, _cardJaC, _cardQuC
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        Assert.AreEqual(6, actualRuns.Count);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
        Assert.Contains(_expectedRun3Cto6C, actualRuns);
        Assert.Contains(_expectedRun2Cto6C, actualRuns);
        Assert.Contains(_expectedRun8CtoJaC, actualRuns);
        Assert.Contains(_expectedRun9CtoQuC, actualRuns);
        Assert.Contains(_expectedRun8CtoQuC, actualRuns);
    }

    [Test]
    public void Finds6AvailableRunsWithMulitpleSuits()
    {
        CardList cards = new()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, _card06C,
            new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Queen, Suit.Diamonds),
            _card08C, _card09C, _card10C, _cardJaC, _cardQuC,
            _card03H, _card04H, _card05H, _card06H,
            _card08H, _card09H, _card10H, _cardJaH, _cardQuH,
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        Assert.AreEqual(10, actualRuns.Count);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
        Assert.Contains(_expectedRun3Cto6C, actualRuns);
        Assert.Contains(_expectedRun2Cto6C, actualRuns);
        Assert.Contains(_expectedRun8CtoJaC, actualRuns);
        Assert.Contains(_expectedRun9CtoQuC, actualRuns);
        Assert.Contains(_expectedRun8CtoQuC, actualRuns);

        Assert.Contains(_expectedRun3Hto6H, actualRuns);
        Assert.Contains(_expectedRun8HtoJaH, actualRuns);
        Assert.Contains(_expectedRun9HtoQuH, actualRuns);
        Assert.Contains(_expectedRun8HtoQuH, actualRuns);
    }


    [Test]
    public void FindsAvailableRunsWithAcesOnEachSide()
    {
        CardList cards = new()
        {
            _cardAcC, TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C,
            _cardJaC, _cardQuC,_cardKiC,
            new Card(Rank.Two, Suit.Hearts), new Card(Rank.Queen, Suit.Diamonds),
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        // Assert.AreEqual(4, actualRuns.Count);
        Assert.Contains(_expectedRunAcCto04C, actualRuns);
        Assert.Contains(_expectedRunAcCto05C, actualRuns);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
        Assert.Contains(_expectedRunJaCtoAcC, actualRuns);
    }
    
    [Test]
    public void MultipleInSuit()
    {
        CardList cards = new()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C,
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C,
        };


        List<Run> actualRuns = RunFinder.FindPossibleRuns(cards);

        Assert.AreEqual(1, actualRuns.Count);
        Assert.Contains(TestHelper.Run02CTo05C, actualRuns);
    }
}