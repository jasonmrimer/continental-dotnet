namespace GameTests;

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
        Assert.That(
            _player.FormatHandForPrint(),
            Is.EqualTo("2♣ | 7♦ | Q♥ | A♠ | Jo★"));
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
        Assert.That(_player.CardCount(), Is.EqualTo(5));
        _player.DiscardFromHand();
        Assert.That(_player.CardCount(), Is.EqualTo(4));
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

        Assert.That(
            choiceCount, Is.LessThan(100),
            "Player did not randomly select both within 100 tries--unlikely result indicative of failure"
        );
    }

    [Test]
    public void DiscardRemovesFromHand()
    {
        GivePlayerSmallDisconnectedHand();
        _player.DiscardFromHand();
        Assert.That(_player.CardCount(), Is.EqualTo(4));
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

        Dashita? playedDashita = IterateUntilPlayedDashita();
        Assert.Multiple(() =>
        {
            Assert.That(_player.HasPlayedDashita);
            Assert.That(playedDashita, Is.EqualTo(expectedDashita));
            Assert.That(_player.Hand(), Is.Empty);
        });
    }

    [Test]
    public void PlaysAdditionalCardAfterDashita_AboveRun()
    {
        foreach (Card card in TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks)
        {
            _player.AddToHand(card);
        }

        Dashita? playedDashita = IterateUntilPlayedDashita();

        _player.AddToHand(TestHelper.Card06C);

        Dealer dealer = new Dealer(new Deck(), new List<Player> { _player });
        dealer.ReceiveDashita(playedDashita);

        Assert.That(_player.CanPlay(dealer.PlayZone), Is.True);

        CardList availablePlays = _player.AvailablePlays(dealer.PlayZone);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06C));
    }

    [Test]
    public void PlaysAdditionalCardAfterDashita_BelowRun()
    {
        foreach (Card card in TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks)
        {
            _player.AddToHand(card);
        }

        Dashita? playedDashita = IterateUntilPlayedDashita();

        _player.AddToHand(TestHelper.Card06D);

        Dealer dealer = new Dealer(new Deck(), new List<Player> { _player });
        dealer.ReceiveDashita(playedDashita);

        Assert.That(_player.CanPlay(dealer.PlayZone));

        CardList availablePlays = _player.AvailablePlays(dealer.PlayZone);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06D));
    }

    [Test]
    public void PlaysAdditionalCardAfterDashita_AboveAndBelowRun()
    {
        foreach (Card card in TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks)
        {
            _player.AddToHand(card);
        }

        Dashita? playedDashita = IterateUntilPlayedDashita();

        _player.AddToHand(TestHelper.Card06C);
        _player.AddToHand(TestHelper.Card06D);

        Dealer dealer = new Dealer(new Deck(), new List<Player> { _player });
        dealer.ReceiveDashita(playedDashita);

        Assert.That(_player.CanPlay(dealer.PlayZone), Is.True);

        CardList availablePlays = _player.AvailablePlays(dealer.PlayZone);
        Assert.That(availablePlays, Has.Count.EqualTo(2));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06C));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06D));
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

        Dashita? playedDashita = IterateUntilPlayedDashita();

        Assert.IsTrue(_player.HasPlayedDashita);
        Assert.That(playedDashita, Is.EqualTo(expectedDashita));
    }

    private Dashita? IterateUntilPlayedDashita()
    {
        int choiceCount = 0;
        Dashita? playedDashita = null;
        while (playedDashita == null && choiceCount < 100)
        {
            playedDashita = _player.PlayDecision();
            choiceCount++;
        }

        return playedDashita;
    }
}