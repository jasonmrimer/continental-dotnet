using GameRunner;

namespace GameTests;

[TestFixture]
public class DealerTest
{
    private List<Player> _players;
    private Deck _deck;
    private Dealer _dealer;

    [SetUp]
    public void SetUp()
    {
        _players = PlayerStub.CreatePlayers();
        _deck = new Deck();
        _dealer = new Dealer(_deck, _players);

        _dealer.Deal();
    }

    [Test]
    public void DealElevenCardsToFourPlayers()
    {
        // Todo fix all the equals overrides to allow unique detection again
        // _players.ForEach(AllCardsAndUnique);
        Assert.That(
            _deck.CardCount(), Is.EqualTo(63),
            "There should be 63 cards: 108 - 44 dealt - 1 discard pile."
        );
    }

    [Test]
    public void FacilitatesADiscardPileFromDeal()
    {
        Assert.That(
            _dealer.PileCardCount(), Is.EqualTo(0),
            "After dealing, the unavailable discard pile should be empty"
        );
        Assert.NotNull(
            _dealer.TopDiscard,
            "After dealing, the top discard should be available"
        );
    }

    [Test]
    public void RecyclesPileIntoDeck()
    {
    }

    [Test]
    public void GiveCardFromDeckTransfersTopDiscardToPile()
    {
        // in continental, once passed on the top discard cannot be accessed
        int startingDeckCount = 63;
        int startingPileCount = 0;
        Assert.NotNull(_dealer.TopDiscard);

        Card drawnCard = _dealer.GiveCardFrom(DrawSource.Deck);

        Assert.NotNull(drawnCard);
        Assert.Null(_dealer.TopDiscard);
        Assert.That(_dealer.DeckCardCount(), Is.EqualTo(startingDeckCount - 1));
        Assert.That(_dealer.PileCardCount(), Is.EqualTo(startingPileCount + 1));
    }

    [Test]
    public void GiveCardFromPileRemovesTopDiscard()
    {
        // in continental, once picked up the cards beneath the top card are still inaccessible
        int startingDeckCount = _dealer.DeckCardCount();
        int startingPileCount = _dealer.PileCardCount();
        Card topDiscard = _dealer.TopDiscard;

        Card drawnCard = _dealer.GiveCardFrom(DrawSource.Pile);

        Assert.That(drawnCard, Is.SameAs(topDiscard));
        Assert.Null(_dealer.TopDiscard);
        Assert.That(_dealer.DeckCardCount(), Is.EqualTo(startingDeckCount));
        Assert.That(_dealer.PileCardCount(), Is.EqualTo(startingPileCount));
    }

    [Test]
    public void TakeDiscardAddsFormerToPileAndNewToTop()
    {
        Card startingTopCard = _dealer.TopDiscard;
        Card discard = new Card(Rank.Ace, Suit.Clubs);
        int startingPileCount = _dealer.PileCardCount();

        Assert.NotNull(_dealer.TopDiscard);

        _dealer.ReceiveDiscardFromPlayer(discard);

        Assert.That(
            _dealer.PileCardCount(), Is.EqualTo(startingPileCount + 1),
            "Did not add to discard pile"
        );

        Assert.That(
            _dealer.TopDiscard, Is.Not.SameAs(startingTopCard),
            "Did not switch top cards"
        );

        Assert.That(
            _dealer.TopDiscard, Is.SameAs(discard),
            "Did not place new discard on top"
        );
    }

    [Test]
    public void ReceivesDashitaFromPlayerAndAddsToPlayZone()
    {
        _dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        Assert.Contains(TestHelper.Run02CTo05C, _dealer.PlayZone);
        Assert.Contains(TestHelper.Run07DTo10D, _dealer.PlayZone);
        Assert.Contains(TestHelper.AtamaJacksHHS, _dealer.PlayZone);
    }
}