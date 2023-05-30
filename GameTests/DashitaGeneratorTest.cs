using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

[TestFixture]
public class DashitaGeneratorTest
{
    private Player _player;
    private Card _cardJaD;
    private Card _cardKiC;
    private Card _cardKiD;
    private Card _cardKiS;
    private Atama _atamaKings;
    private Run _run07DtoJaD;
    private Run _run08DtoJaD;
    private Atama _atamaJacksDHH;
    private Atama _atamaJacksDHS;
    private Atama _atamaJacksDHHS;

    [SetUp]
    public void SetUp()
    {
        _cardJaD = new Card(Rank.Jack, Suit.Diamonds);

        _cardKiC = new Card(Rank.King, Suit.Clubs);
        _cardKiD = new Card(Rank.King, Suit.Diamonds);
        _cardKiS = new Card(Rank.King, Suit.Spades);


        _run07DtoJaD = new Run(TestHelper.Run07DTo10D) { _cardJaD };
        _run08DtoJaD = new Run
        {
            TestHelper.Card08D, TestHelper.Card09D, TestHelper.Card10D, _cardJaD
        };

        _atamaJacksDHH = new Atama
        {
            _cardJaD, TestHelper.CardJaH1, TestHelper.CardJaH2,
        };

        _atamaJacksDHS = new Atama
        {
            _cardJaD, TestHelper.CardJaH2, TestHelper.CardJaS,
        };

        _atamaJacksDHHS = new Atama
        {
            _cardJaD, TestHelper.CardJaH1, TestHelper.CardJaH2, TestHelper.CardJaS,
        };

        _atamaKings = new Atama() { _cardKiC, _cardKiD, _cardKiS };
    }

    [Test]
    public void GeneratesSimplestHand()
    {
        CardList hand = new(TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks);

        Dashita expectedDashita = new Dashita(
            TestHelper.Run02CTo05C,
            TestHelper.Run07DTo10D,
            TestHelper.AtamaJacksHHS
        );


        HashSet<Dashita> dashitaOptions = DashitaGenerator.GenerateOptions(hand);


        Assert.AreEqual(1, dashitaOptions.Count);
        Assert.IsTrue(dashitaOptions.Contains(expectedDashita));
    }

    [Test]
    public void TestDashitaFromMultipleRunsAvailableAndSameAtama()
    {
        Card card06C = new Card(Rank.Six, Suit.Clubs);

        CardList hand = new(TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks)
        {
            card06C,
        };

        Run run3Cto6C = new Run()
        {
            TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C, card06C
        };

        Run run2Cto6C = new Run(TestHelper.Run02CTo05C)
        {
            card06C
        };

        Dashita dashitaWith2Cto5C = new Dashita(
            TestHelper.Run02CTo05C,
            TestHelper.Run07DTo10D,
            TestHelper.AtamaJacksHHS
        );
        Dashita dashitaWith3Cto6C = new Dashita(
            new List<Run>() { run3Cto6C, TestHelper.Run07DTo10D },
            TestHelper.AtamaJacksHHS
        );

        Dashita dashitaWith2Cto6C = new Dashita(
            new List<Run>() { run2Cto6C, TestHelper.Run07DTo10D },
            TestHelper.AtamaJacksHHS
        );

        HashSet<Dashita> dashitaOptions = DashitaGenerator.GenerateOptions(hand);

        Assert.AreEqual(
            3,
            dashitaOptions.Count,
            "Did not find correct number of dashita options"
        );

        List<Dashita> dashitaList = new(dashitaOptions);
        Assert.Contains(dashitaWith2Cto5C, dashitaList);
        Assert.Contains(dashitaWith3Cto6C, dashitaList);
        Assert.Contains(dashitaWith2Cto6C, dashitaList);
    }

    [Test]
    public void TwoRunsAndTwoAtamaChoices()
    {
        CardList hand = new(TestHelper.HandForDashita2CTo5CAnd7DTo10DAndJacks)
        {
            _cardKiC, _cardKiD, _cardKiS
        };

        Dashita expectedDashitaWithJacks = new Dashita(
            TestHelper.Run02CTo05C,
            TestHelper.Run07DTo10D,
            TestHelper.AtamaJacksHHS
        );
        Dashita expectedDashitaWithKings = new Dashita(
            TestHelper.Run02CTo05C,
            TestHelper.Run07DTo10D,
            _atamaKings
        );

        HashSet<Dashita> dashitaOptions = DashitaGenerator.GenerateOptions(hand);

        Assert.AreEqual(
            2,
            dashitaOptions.Count,
            "Did not find correct number of dashita options"
        );

        List<Dashita> dashitaList = new(dashitaOptions);
        Assert.Contains(expectedDashitaWithJacks, dashitaList);
        Assert.Contains(expectedDashitaWithKings, dashitaList);
    }

    [Test]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public void RunsThatTouchAtama()
    {
        CardList hand = new()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C,
            TestHelper.Card07D, TestHelper.Card08D, TestHelper.Card09D, TestHelper.Card10D,
            _cardJaD,
            TestHelper.CardJaH1, TestHelper.CardJaH2, TestHelper.CardJaS,
        };

        Dashita expectedDashita07Dto10DJacksDHH = new Dashita(
            TestHelper.Run02CTo05C, TestHelper.Run07DTo10D, _atamaJacksDHH
        );
        Dashita expectedDashita07Dto10DJacksDHS = new Dashita(
            TestHelper.Run02CTo05C, TestHelper.Run07DTo10D, _atamaJacksDHS
        );
        Dashita expectedDashita07Dto10DJacksHHS = new Dashita(
            TestHelper.Run02CTo05C, TestHelper.Run07DTo10D, TestHelper.AtamaJacksHHS
        );
        Dashita expectedDashita07Dto10DJacksDHHS =
            new Dashita(TestHelper.Run02CTo05C, TestHelper.Run07DTo10D, _atamaJacksDHHS
            );
        Dashita expectedDashita07DtoJaDJacksHHS =
            new Dashita(TestHelper.Run02CTo05C, _run07DtoJaD, TestHelper.AtamaJacksHHS);
        Dashita expectedDashita08DtoJaDJacksHHS =
            new Dashita(TestHelper.Run02CTo05C, _run08DtoJaD, TestHelper.AtamaJacksHHS);

        HashSet<Dashita> dashitaOptions = DashitaGenerator.GenerateOptions(hand);

        Assert.AreEqual(
            6,
            dashitaOptions.Count,
            "Did not find correct number of dashita options"
        );

        List<Dashita> dashitaList = new(dashitaOptions);
        Assert.Contains(expectedDashita07Dto10DJacksDHH, dashitaList);
        Assert.Contains(expectedDashita07Dto10DJacksDHS, dashitaList);
        Assert.Contains(expectedDashita07Dto10DJacksHHS, dashitaList);
        Assert.Contains(expectedDashita07Dto10DJacksDHHS, dashitaList);
        Assert.Contains(expectedDashita07DtoJaDJacksHHS, dashitaList);
        Assert.Contains(expectedDashita08DtoJaDJacksHHS, dashitaList);
    }

    [Test]
    public void SameRuns()
    {
        CardList hand = new()
        {
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C,
            TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C,
            TestHelper.CardJaH1, TestHelper.CardJaH2, TestHelper.CardJaS,
        };

        Dashita expectedDashita = new Dashita(
            TestHelper.Run02CTo05C, TestHelper.Run02CTo05C, TestHelper.AtamaJacksHHS
        );


        HashSet<Dashita> dashitaOptions = DashitaGenerator.GenerateOptions(hand);


        Assert.AreEqual(1, dashitaOptions.Count);
        Assert.IsTrue(dashitaOptions.Contains(expectedDashita));
    }
}