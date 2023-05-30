using GameRunner;

[TestFixture]
public class PlayFinderTest
{
    [Test]
    public void SinglePlayToRunAtEnd()
    {
        CardList hand = new CardList() { TestHelper.Card06C };

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        PlayAction expectedAction = new PlayAction(TestHelper.Run02CTo05C, TestHelper.Card06C);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Does.Contain(expectedAction));
        Assert.That(availablePlays, Has.Count.EqualTo(1));
    }


    [Test]
    public void MultiplePlaysToRunAtEndAndToAtama()
    {
        CardList hand = new CardList
        {
            TestHelper.CardJaD, TestHelper.CardQuD, TestHelper.CardKiD
        };

        PlayAction playActionAtamaJack = new PlayAction(
            TestHelper.AtamaJacksHHS, TestHelper.CardJaD
        );

        PlayAction playActionRunJack = new PlayAction(
            TestHelper.Run07DTo10D, TestHelper.CardJaD
        );

        PlayAction playActionRunJackQueen = new PlayAction(
            TestHelper.Run07DTo10D,
            new CardList { TestHelper.CardJaD, TestHelper.CardQuD }
        );

        PlayAction playActionRunJackQueenKing = new PlayAction(
            TestHelper.Run07DTo10D,
            new CardList { TestHelper.CardJaD, TestHelper.CardQuD, TestHelper.CardKiD }
        );


        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);

        Assert.That(availablePlays, Does.Contain(playActionAtamaJack));
        Assert.That(availablePlays, Does.Contain(playActionRunJack));
        Assert.That(availablePlays, Does.Contain(playActionRunJackQueen));
        Assert.That(availablePlays, Does.Contain(playActionRunJackQueenKing));
        Assert.That(availablePlays, Has.Count.EqualTo(4));
    }

    [Test]
    public void MultiplePlaysToRunAtEnd()
    {
        CardList hand = new CardList
        {
            TestHelper.Card06C, TestHelper.Card07C, TestHelper.Card08C
        };
        
        PlayAction playActionRunPlusSix = new PlayAction(
            TestHelper.Run02CTo05C, TestHelper.Card06C
        );

        PlayAction playActionRunPlusSixSeven = new PlayAction(
            TestHelper.Run02CTo05C,
            new CardList { TestHelper.Card06C, TestHelper.Card07C }
        );

        PlayAction playActionRunPlusSixSevenEight = new PlayAction(
            TestHelper.Run02CTo05C,
            new CardList { TestHelper.Card06C, TestHelper.Card07C, TestHelper.Card08C }
        );


        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);

        Assert.That(availablePlays, Does.Contain(playActionRunPlusSix));
        Assert.That(availablePlays, Does.Contain(playActionRunPlusSixSeven));
        Assert.That(availablePlays, Does.Contain(playActionRunPlusSixSevenEight));
        Assert.That(availablePlays, Has.Count.EqualTo(3));
    }

    [Test]
    public void SinglePlayToRunAtStart()
    {
        CardList hand = new CardList() { TestHelper.Card06D };
        PlayAction expectedAction = new PlayAction(TestHelper.Run07DTo10D, TestHelper.Card06D);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);
        
        Assert.That(availablePlays, Does.Contain(expectedAction));
        Assert.That(availablePlays, Has.Count.EqualTo(1));
    }

    [Test]
    public void SinglePlayToRunAtEndWithAce()
    {
        CardList hand = new CardList { TestHelper.CardAcD };
        PlayAction expectedAction = new PlayAction(TestHelper.Run10DToKiD, TestHelper.CardAcD);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd10DToKiDAndJacks);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Does.Contain(expectedAction));
        Assert.That(availablePlays, Has.Count.EqualTo(1));
    }

    [Test]
    public void SinglePlayToRunAtStartWithAce()
    {
        CardList hand = new CardList { TestHelper.CardAcC };
        PlayAction expectedAction = new PlayAction(TestHelper.Run02CTo05C, TestHelper.CardAcC);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Does.Contain(expectedAction));
        Assert.That(availablePlays, Has.Count.EqualTo(1));
    }

    [Test]
    public void SeparatePlaysToRunAtStartAndToRunAtEnd()
    {
        CardList hand = new CardList
        {
            TestHelper.Card06C, TestHelper.Card06D
        };
        PlayAction expectedAction01 = new PlayAction(TestHelper.Run02CTo05C, TestHelper.Card06C);
        PlayAction expectedAction02 = new PlayAction(TestHelper.Run07DTo10D, TestHelper.Card06D);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Does.Contain(expectedAction01));
        Assert.That(availablePlays, Does.Contain(expectedAction02));
        Assert.That(availablePlays, Has.Count.EqualTo(2));
    }

    [Test]
    public void SinglePlayToAtama()
    {
        CardList hand = new CardList { TestHelper.CardJaD };
        PlayAction expectedAction = new PlayAction(TestHelper.AtamaJacksHHS, TestHelper.CardJaD);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd10DToKiDAndJacks);

        List<PlayAction> availablePlays = PlayFinder.FindPlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Does.Contain(expectedAction));
        Assert.That(availablePlays, Has.Count.EqualTo(1));
    }
}