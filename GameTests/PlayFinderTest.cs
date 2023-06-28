using GameRunner;

[TestFixture]
public class PlayFinderTest
{
    [Test]
    public void FindsAdditionalPlayAfterDashita_AboveRun()
    {
        CardList hand = new CardList() { TestHelper.Card06C };

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        PlayAction expectedAction = new PlayAction(TestHelper.Run02CTo05C, TestHelper.Card06C);

        List<PlayAction> availablePlays = PlayFinder.AvailablePlaysV2(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(expectedAction));
    }


    [Test]
    public void FindsAdditionalPlaysAfterDashita_MultiCardAboveRunAndAtama()
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

        List<PlayAction> playActionsWithAtamaOnly = new List<PlayAction>()
        {
            playActionAtamaJack
        };


        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        List<PlayAction> availablePlays = PlayFinder.AvailablePlaysV2(dealer.PlayZone, hand);
        /*
         * jack to atama only
         * jack to run
         * jack queen to run
         * jack king to run
         */

        Assert.That(availablePlays, Has.Count.EqualTo(4));
        Assert.That(availablePlays, Does.Contain(playActionAtamaJack));
        Assert.That(availablePlays, Does.Contain(playActionRunJack));
        Assert.That(availablePlays, Does.Contain(playActionRunJackQueen));
        Assert.That(availablePlays, Does.Contain(playActionRunJackQueenKing));
    }

    [Test]
    public void FindsAdditionalPlayAfterDashita_BelowRun()
    {
        CardList hand = new CardList() { TestHelper.Card06D };
        PlayAction expectedAction = new PlayAction(TestHelper.Run07DTo10D, TestHelper.Card06D);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        // CardList availablePlays = PlayFinder.AvailablePlays(dealer.PlayZone, hand);
        // Assert.That(availablePlays, Has.Count.EqualTo(1));
        // Assert.That(availablePlays, Does.Contain(TestHelper.Card06D));
        
        
        
        List<PlayAction> availablePlays = PlayFinder.AvailablePlaysV2(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(expectedAction));
    }

    [Test]
    public void FindsAdditionalPlayAfterDashita_TopOfRun()
    {
        CardList hand = new CardList { TestHelper.CardAcD };
        PlayAction expectedAction = new PlayAction(TestHelper.Run10DToKiD, TestHelper.CardAcD);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd10DToKiDAndJacks);

        List<PlayAction> availablePlays = PlayFinder.AvailablePlaysV2(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(expectedAction));
    }

    [Test]
    public void FindsAdditionalPlayAfterDashita_BottomOfRun()
    {
        CardList hand = new CardList { TestHelper.CardAcC };
        PlayAction expectedAction = new PlayAction(TestHelper.Run02CTo05C, TestHelper.CardAcC);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        List<PlayAction> availablePlays = PlayFinder.AvailablePlaysV2(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(expectedAction));
    }

    [Test]
    public void FindsAdditionalPlayAfterDashita_AboveAndBelowRun()
    {
        CardList hand = new CardList
        {
            TestHelper.Card06C, TestHelper.Card06D
        };
        PlayAction expectedAction01 = new PlayAction(TestHelper.Run02CTo05C, TestHelper.Card06C);
        PlayAction expectedAction02 = new PlayAction(TestHelper.Run07DTo10D, TestHelper.Card06D);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);
        
        List<PlayAction> availablePlays = PlayFinder.AvailablePlaysV2(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(2));
        Assert.That(availablePlays, Does.Contain(expectedAction01));
        Assert.That(availablePlays, Does.Contain(expectedAction02));
    }

    [Test]
    public void FindsAdditionalPlayAfterDashita_AddToAtama()
    {
        CardList hand = new CardList { TestHelper.CardJaD };
        PlayAction expectedAction = new PlayAction(TestHelper.AtamaJacksHHS, TestHelper.CardJaD);

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd10DToKiDAndJacks);
        
        List<PlayAction> availablePlays = PlayFinder.AvailablePlaysV2(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(expectedAction));
    }
}