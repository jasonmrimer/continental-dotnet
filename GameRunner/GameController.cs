using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;

public class GameController
{
    private List<Player> _players;
    private bool _gameIsOver;
    private int _turnCount;
    private int _currentPlayerIndex;
    private readonly int _turnLimit;
    private MessageProducer _messageProducer;
    private readonly Dealer _dealer;

    public Dealer Dealer => _dealer;

    public GameController(
        Dealer dealer,
        List<Player> players,
        int turnLimit
    ) : this()
    {
        _dealer = dealer;
        _players = players;
        _turnLimit = turnLimit;
    }

    public GameController(int turnLimit = 1000)
    {
        _players = PlayerStub.CreatePlayers();
        _dealer = new(new Deck(), _players);
        _turnCount = 1;
        _currentPlayerIndex = 0;
        _gameIsOver = false;
        _turnLimit = turnLimit;
        _messageProducer = new MessageProducer(
            bootstrapServers: "localhost:9092",
            topic: "GameAndPlayerStateBeforeAction"
        );
    }

    public void Deal()
    {
        Dealer.Deal();
    }

    public async Task Play()
    {
        while (ShouldContinuePlaying())
        {
            // _messageProducer.ProduceMessageAsync($"{_turnCount}");
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };


            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                Console.WriteLine("hereiam 1");
                try
                {
                    Console.WriteLine("hereiam 2");
                    var deliveryReport = await producer.ProduceAsync("GameAndPlayerStateBeforeAction",
                        new Message<Null, string> { Value = $"turn {_turnCount}" });
                    Console.WriteLine("hereiam 3");
                    Console.WriteLine($"Message delivered to '{deliveryReport.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> ex)
                {
                    Console.WriteLine("hereiam 4");
                    Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
                }
            }

            Player currentPlayer = _players[_currentPlayerIndex];

            GameWriter.PrintDeckAndPileStatus(Dealer, _turnCount, _players);
            GameWriter.PrintTurnStart(currentPlayer, _turnCount);

            if (OutOfCards())
            {
                _gameIsOver = true;
                break;
            }

            Dealer.RecyclePileIntoDeck();

            Player playerWhoDrew = null;
            PlayersVieForTopDiscard(_currentPlayerIndex, currentPlayer, ref playerWhoDrew);
            PlayerDraws(currentPlayer, playerWhoDrew);
            PlayerDiscards(currentPlayer);

            _currentPlayerIndex = RotateToNextPlayer(_currentPlayerIndex);
        }
    }

    private void PlayersVieForTopDiscard(
        int currentPlayerIndex,
        Player currentPlayer,
        ref Player playerWhoDrew
    )
    {
        int playerCount = _players.Count;

        // Normalize the startIndex within the range of the list size
        int startIndex = (currentPlayerIndex % playerCount + playerCount) % playerCount;

        for (int i = startIndex; i < startIndex + playerCount; i++)
        {
            int index = i % playerCount;
            Player vyingPlayer = _players[index];
            bool playerDecision = Player.DecideWhetherToTakePenalty();
            if (playerDecision)
            {
                Dealer.RecyclePileIntoDeck();

                playerWhoDrew = vyingPlayer;
                Card drawnCard = Dealer.GiveCardFrom(DrawSource.Pile);
                Card penalty = null;
                vyingPlayer.AddToHand(drawnCard);

                if (vyingPlayer != currentPlayer)
                {
                    // take penalty
                    penalty = Dealer.GiveCardFrom(DrawSource.Deck);
                    vyingPlayer.AddToHand(penalty);
                }

                GameWriter.PrintPenaltyAction(vyingPlayer, drawnCard, penalty);

                return;
            }

            Console.WriteLine($"{vyingPlayer.Name} passes on pile");
        }
    }

    private bool OutOfCards()
    {
        return (Dealer.DeckCardCount() + Dealer.PileCardCount() == 1);
    }

    private bool ShouldContinuePlaying()
    {
        if (_turnCount == _turnLimit)
        {
            _gameIsOver = true;
        }

        return !_gameIsOver;
    }

    private int RotateToNextPlayer(int currentPlayerIndex)
    {
        _turnCount++;
        currentPlayerIndex = (currentPlayerIndex == _players.Count - 1) ? 0 : (currentPlayerIndex + 1);
        return currentPlayerIndex;
    }

    private void PlayerDraws(Player player, Player playerWhoDrew)
    {
        // GameWriter.PrintPlayerDrawsTopCard(playerWhoDrew);

        if (playerWhoDrew != player)
        {
            Dealer.RecyclePileIntoDeck();
            Card drawnCard = Dealer.GiveCardFrom(DrawSource.Deck);
            player.AddToHand(drawnCard);
            GameWriter.PrintDrawAction(player, DrawSource.Deck, drawnCard);
        }
    }

    private void PlayerDiscards(Player player)
    {
        Card discard = player.DiscardFromHand();
        Dealer.ReceiveDiscardFromPlayer(discard);
    }

    public bool IsFinished()
    {
        return _gameIsOver;
    }

    public int TurnCount()
    {
        return _turnCount;
    }
}