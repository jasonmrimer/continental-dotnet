namespace GameRunner;

public class Dealer
{
    private readonly List<Player> _players;
    private readonly Deck _deck;
    private readonly CardList _discardPile;
    private Card _topDiscard;

    public Dealer()
    {
        _deck = new Deck();
        _players = PlayerStub.CreatePlayers();
        _topDiscard = null;
        _discardPile = new CardList();
        PlayZone = new List<CardList>();
    }
        
    public Dealer(Deck deck, List<Player> players) : this()
    {
        _deck = deck;
        _players = players;
    }

    public Card TopDiscard => _topDiscard;
    public List<CardList> PlayZone { get; private set; }

    public void RecyclePileIntoDeck()
    {
        if (DeckCardCount() == 0)
        {
            Card topOfPile = _discardPile.Last();
            _discardPile.Remove(topOfPile);
            _deck.AddCards(_discardPile);
            _discardPile.Clear();
            _discardPile.Add(topOfPile);
            Shuffle();
        }
    }

    public void Deal()
    {
        Shuffle();
        _players.ForEach(DealStartingHand);
        _topDiscard = _deck.DrawCard();
    }

    private void DealStartingHand(Player player)
    {
        for (int i = 0; i < 11; i++)
        {
            Card card = _deck.DrawCard();
            player.AddToHand(card);
        }
    }

    public void ReceiveDiscardFromPlayer(Card discard)
    {
        TransferCurrentTopDiscardToPile();
        _topDiscard = discard;
    }

    private void TransferCurrentTopDiscardToPile()
    {
        if (_topDiscard != null)
        {
            AddToPile(_topDiscard);
        }

        _topDiscard = null;
    }

    private void AddToPile(Card discard)
    {
        _discardPile.Add(discard);
    }

    public int PileCardCount()
    {
        return _discardPile.Count;
    }

    public int DeckCardCount()
    {
        return _deck.CardCount();
    }

    private Card DrawFromDeck()
    {
        return _deck.DrawCard();
    }

    private void Shuffle()
    {
        // Shuffle the deck using Fisher-Yates algorithm
        Random random = new Random();
        int n = _deck.CardCount();
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (_deck.Cards[k], _deck.Cards[n]) = (_deck.Cards[n], _deck.Cards[k]);
        }
    }

    public Card GiveCardFrom(DrawSource drawSource)
    {
        Card cardToGive;

        if (drawSource == DrawSource.Deck)
        {
            TransferCurrentTopDiscardToPile();
            cardToGive = DrawFromDeck();
        }
        else
        {
            cardToGive = _topDiscard;
            _topDiscard = null;
        }

        return cardToGive;
    }

    public void ReceiveDashita(Dashita? dashita)
    {
        foreach (Run run in dashita.Runs)
        {
            PlayZone.Add(run);
        }
        
        PlayZone.Add(dashita.Atama);
    }
}