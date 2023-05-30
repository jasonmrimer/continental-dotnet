using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class PlayerTest
{
    private Player _player;

    [SetUp]
    public void SetUp()
    {
        _player = new Player("Alice");
    }

    [Test]
    public void PrintHand()
    {
        GivePlayerSmallDisconnectedHand();
        Assert.AreEqual(
            "2♣ | 7♦ | Q♥ | A♠ | Jo★",
            _player.FormatHandForPrint()
        );
    }

    private void GivePlayerSmallDisconnectedHand()
    {
        _player.AddToHand(new Card(Rank.Two, Suit.Clubs));
        _player.AddToHand(new Card(Rank.Seven, Suit.Diamonds));
        _player.AddToHand(new Card(Rank.Queen, Suit.Hearts));
        _player.AddToHand(new Card(Rank.Ace, Suit.Spades));
        _player.AddToHand(new Card(Rank.Joker, Suit.Wild));
    }

    [Test]
    public void Discard()
    {
        GivePlayerSmallDisconnectedHand();
        Assert.AreEqual(5, _player.CardCount());
        _player.DiscardFromHand();
        Assert.AreEqual(4, _player.CardCount());
    }

    [Test]
    public void ChooseDrawSourceAtRandom()
    {
        _player.AddToHand(new Card(Rank.Two, Suit.Clubs));
        _player.AddToHand(new Card(Rank.Seven, Suit.Diamonds));
        _player.AddToHand(new Card(Rank.Queen, Suit.Hearts));
        _player.AddToHand(new Card(Rank.Ace, Suit.Spades));
        _player.AddToHand(new Card(Rank.Joker, Suit.Wild));

        // Card topOfPile = new Card(CardValue.Eight, Suit.Spades);
        HashSet<DrawSource> chosenSources = new HashSet<DrawSource>();
        int choiceCount = 1;

        while (chosenSources.Count < 2 && choiceCount < 100)
        {
            chosenSources.Add(Player.ChooseDrawSource(pileIsAvailable: true));
            choiceCount++;
        }

        Assert.Less(
            choiceCount,
            100,
            "Player did not randomly select both within 100 tries--unlikely result indicative of failure"
        );
    }

    [Test]
    public void DiscardRemovesFromHand()
    {
        GivePlayerSmallDisconnectedHand();
        _player.DiscardFromHand();
        Assert.AreEqual(4, _player.CardCount());
    }

    [Test]
    public void PlayerWillDashitaRandomlyGivenSingleDashitaHand()
    {
        foreach (Card card in TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks)
        {
            _player.AddToHand(card);
        }

        Dashita expectedDashita = new(
            TestHelper.Run02CTo05C,
            TestHelper.Run07DTo10D,
            TestHelper.AtamaJacksHHS
        );


        int choiceCount = 0;
        Dashita playedDashita = null;
        while (playedDashita == null && choiceCount < 100)
        {
            playedDashita = _player.PlayDecision();
            choiceCount++;
        }

        Assert.IsTrue(_player.HasPlayedDashita);
        Assert.AreEqual(
            expectedDashita,
            playedDashita
        );

        Assert.IsEmpty(_player.Hand());
    }

    [Test]
    public void PlaysAdditionalCardAfterDashita()
    {
        foreach (Card card in TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks)
        {
            _player.AddToHand(card);
        }

        int choiceCount = 0;
        Dashita playedDashita = null;
        while (playedDashita == null && choiceCount < 100)
        {
            playedDashita = _player.PlayDecision();
            choiceCount++;
        }

        _player.AddToHand(TestHelper.Card06C);

        Dealer dealer = new Dealer(new Deck(), new List<Player> { _player });
        dealer.ReceiveDashita(playedDashita);

        Assert.IsTrue(_player.CanPlay(dealer.PlayZone));
        
        CardList availablePlays = _player.AvailablePlays(dealer.PlayZone);
        Assert.AreEqual(1, availablePlays.Count);
        Assert.Contains(TestHelper.Card06C, availablePlays);
    }

    [Test]
    [Ignore("do the dealer acceptance first")]
    public void PlayerWillDashitaRandomlyGivenMultiDashitaHand()
    {
        CardList hand = new(TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks);
        hand.AddRange(TestHelper.Run08HtoJaH);
        foreach (Card card in hand)
        {
            _player.AddToHand(card);
        }

        Dashita expectedDashita = new(
            TestHelper.Run02CTo05C,
            TestHelper.Run07DTo10D,
            TestHelper.AtamaJacksHHS
        );


        int choiceCount = 0;
        Dashita playedDashita = null;
        while (playedDashita == null && choiceCount < 100)
        {
            playedDashita = _player.PlayDecision();
            choiceCount++;
        }

        Assert.IsTrue(_player.HasPlayedDashita);
        Assert.AreEqual(
            expectedDashita,
            playedDashita
        );
    }
}