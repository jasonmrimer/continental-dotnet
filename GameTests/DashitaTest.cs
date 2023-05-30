using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class DashitaTest
{
    [Test]
    public void Equals()
    {
        Dashita dashita1 = new Dashita(
            new List<Run> { TestHelper.Run02CTo05C, TestHelper.Run07DTo10D },
            TestHelper.AtamaJacksHHS
        );

        Dashita dashita2 = new Dashita(
            TestHelper.Run02CTo05C, 
            TestHelper.Run07DTo10D,
            TestHelper.AtamaJacksHHS
        );
        Assert.AreEqual(dashita1, dashita2);

        Assert.IsTrue(dashita1.Equals(dashita2));
    }

    [Test]
    public void NotEquals()
    {
        Run run02Cto06C = new(TestHelper.Run02CTo05C);
        run02Cto06C.Add(new Card(Rank.Six, Suit.Clubs));

        Dashita dashita1 = new Dashita(
            new List<Run> { run02Cto06C, TestHelper.Run07DTo10D },
            TestHelper.AtamaJacksHHS
        );

        Dashita dashita2 = new Dashita(
            new List<Run> { TestHelper.Run02CTo05C, TestHelper.Run07DTo10D },
            TestHelper.AtamaJacksHHS
        );

        Assert.AreNotEqual(dashita1, dashita2);

        Assert.IsFalse(dashita1.Equals(dashita2));
    }

    [Test]
    public void EqualsWithRunOrderChange()
    {
        Dashita dashita1 = new Dashita(
            new List<Run> { TestHelper.Run07DTo10D, TestHelper.Run02CTo05C },
            TestHelper.AtamaJacksHHS
        );

        Dashita dashita2 = new Dashita(
            new List<Run> { TestHelper.Run02CTo05C, TestHelper.Run07DTo10D },
            TestHelper.AtamaJacksHHS
        );

        Assert.AreEqual(dashita1, dashita2);

        Assert.IsTrue(dashita1.Equals(dashita2));
    }

    [Test]
    public void EqualsWithAtamaOrderChange()
    {
        Card cardJaH1 = new Card(Rank.Jack, Suit.Hearts);
        Card cardJaH2 = new Card(Rank.Jack, Suit.Hearts);
        Card cardJaS = new Card(Rank.Jack, Suit.Spades);

        Atama atamaJacksMix = new()
        {
            cardJaH2, cardJaS, cardJaH1
        };

        Dashita dashita1 = new Dashita(
            new List<Run> { TestHelper.Run02CTo05C, TestHelper.Run07DTo10D },
            TestHelper.AtamaJacksHHS
        );

        Dashita dashita2 = new Dashita(
            new List<Run> { TestHelper.Run02CTo05C, TestHelper.Run07DTo10D },
            atamaJacksMix
        );
        Assert.AreEqual(dashita1, dashita2);

        Assert.IsTrue(dashita1.Equals(dashita2));
    }
}