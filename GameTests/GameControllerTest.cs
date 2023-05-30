using System;
using System.Collections.Generic;
using NUnit.Framework;

public class GameControllerTest
{
    [Test]
    public void Play1000TurnsOrUntilDiscardPileEmpty()
    {
        List<Player> players = PlayerStub.CreatePlayers();
        Dealer dealer = new(new Deck(), players);
        
        _gameController = new GameController(
            dealer,
            players,
            turnLimit: 1000
            );
        _gameController.Deal();
        _gameController.Play();

        Assert.IsTrue(
            _gameController.IsFinished(),
            "Should conclude after 1000 turns or out of cards."
        );
        
        if (AreCardsLeftToDraw())
        {
            Assert.AreEqual(
                _gameController.TurnCount(),
                1000,
                "Expected game to end at 1000 turns."
            ); 
            
        }
        else
        {
            Assert.LessOrEqual(
                _gameController.TurnCount(),
                1000,
                "Expected to end after 1000 turns even with cards left to draw."
            ); 
        }
       
    }

    private bool AreCardsLeftToDraw()
    {
        return (_gameController.Dealer.PileCardCount() + _gameController.Dealer.DeckCardCount()) > 1;
    }

    private GameController _gameController;

    // players take random action

    // until game is over

    // game can end by player discarding or playing final card

    // game can end by only 1 remaining discard pile and all players get one turn

    // or 1000 turns without discard pile empty


    [Test]
    [Ignore("")]
    public void PlayWithDrawChoiceUntilTurnOneHundred()
    {
        _gameController = new GameController(turnLimit: 100);
        _gameController.Play();
        Assert.IsTrue(
            _gameController.IsFinished(),
            "Should conclude after 100 turns."
        );
        Assert.AreEqual(
            100,
            _gameController.TurnCount(),
            "Random-choice deck/pile set to 100 turns."
        );
    }
}