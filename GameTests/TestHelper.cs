using System.Collections.Generic;

public static class TestHelper
{
    public static Card CardAcC = new(Rank.Ace, Suit.Clubs);
    public static Card Card02C = new(Rank.Two, Suit.Clubs);
    public static Card Card03C = new(Rank.Three, Suit.Clubs);
    public static Card Card04C = new(Rank.Four, Suit.Clubs);
    public static Card Card05C = new(Rank.Five, Suit.Clubs);
    public static Card Card06C = new(Rank.Six, Suit.Clubs);
    public static Card Card07C = new(Rank.Seven, Suit.Clubs);
    public static Card Card08C = new(Rank.Eight, Suit.Clubs);
    public static Card CardAcD = new(Rank.Ace, Suit.Diamonds);
    public static Card Card06D = new(Rank.Six, Suit.Diamonds);
    public static Card Card07D = new(Rank.Seven, Suit.Diamonds);
    public static Card Card08D = new(Rank.Eight, Suit.Diamonds);
    public static Card Card09D = new(Rank.Nine, Suit.Diamonds);
    public static Card Card10D = new(Rank.Ten, Suit.Diamonds);
    public static Card CardJaD = new(Rank.Jack, Suit.Diamonds);
    public static Card CardQuD = new(Rank.Queen, Suit.Diamonds);
    public static Card CardKiD = new(Rank.King, Suit.Diamonds);
    public static Card Card08H = new(Rank.Eight, Suit.Hearts);
    public static Card Card09H = new(Rank.Nine, Suit.Hearts);
    public static Card Card10H = new(Rank.Ten, Suit.Hearts);
    public static Card CardJaH1 = new(Rank.Jack, Suit.Hearts);
    public static Card CardJaH2 = new(Rank.Jack, Suit.Hearts);
    public static Card CardJaS = new(Rank.Jack, Suit.Spades);

    public static List<Player> FourPlayers => PlayerStub.CreatePlayers();

    public static CardList HandForDashita2CTo5CAnd7DTo10DAndJacks => new CardList()
    {
        Card02C, Card03C, Card04C, Card05C,
        Card07D, Card08D, Card09D, Card10D,
        CardJaH1, CardJaH2, CardJaS,
    };

    public static Run Run02CTo05C => new() { Card02C, Card03C, Card04C, Card05C };
    public static Run Run07DTo10D => new() { Card07D, Card08D, Card09D, Card10D, };
    public static Run Run10DToKiD => new() { Card10D, CardJaD, CardQuD, CardKiD };
    public static Run Run08HtoJaH => new() { Card08H, Card09H, Card10H, CardJaH1 };
    public static Atama AtamaJacksHHS => new() { CardJaH1, CardJaH2, CardJaS, };
    public static Dashita Dashita02CTo05CAnd07DTo10DAndJacks => new(Run02CTo05C, Run07DTo10D, AtamaJacksHHS);
    public static Dashita Dashita02CTo05CAnd10DToKiDAndJacks => new(Run02CTo05C, Run10DToKiD, AtamaJacksHHS);
}