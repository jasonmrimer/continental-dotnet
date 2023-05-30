using System;
using System.Collections.Generic;
using System.Linq;

public class GameWriter
{
    public static void PrintDeckAndPileStatus(Dealer dealer, int turnCount, List<Player> players)
    {
        string topDiscard = dealer.TopDiscard != null ? dealer.TopDiscard.Printable() : "none";
        Console.WriteLine(
            $"> Turn {turnCount} <  Cards left in Deck: " +
            $"{dealer.DeckCardCount()} & Pile: {dealer.PileCardCount()} | " +
            $"Top Card: {topDiscard}"
        );
        Console.WriteLine($"Total cards in play: {SumCards(dealer, players)}");
    }

    private static int SumCards(Dealer dealer, List<Player> players)
    {
        return dealer.PileCardCount() + dealer.DeckCardCount() + players.Sum(player => player.CardCount());
    }

    public static void PrintDiscardAction(Player player, Card discard)
    {
        Console.WriteLine($"{player.Name} discards {discard.Printable()}");
    }

    public static void PrintDrawAction(Player player, DrawSource drawSource, Card drawnCard)
    {
        Console.WriteLine($"{player.Name} draws from _{drawSource}_: {drawnCard.Printable()}");
    }

    public static void PrintTurnStart(Player player, int turnCount)
    {
        Console.WriteLine($"{player.Name} begins turn {turnCount} with: {player.FormatHandForPrint()}");
    }

    public static void PrintPenaltyAction(Player player, Card drawnCard, Card penalty)
    {
        string penaltyMessage = penalty != null ? $"with penalty of {penalty.Printable()}" : "without penalty";
        Console.WriteLine($"{player.Name} takes {drawnCard.Printable()} from _Pile_ {penaltyMessage}");
    }

    public static void PrintPlayerDrawsTopCard(Player player)
    {
        string message = player != null ? $"{player.Name} drew top discard" : "nobody drew top discard";
        Console.WriteLine(message);
    }
}