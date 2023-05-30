using GameRunner;

namespace GameTests;

[TestFixture]
public class ActionsForAdditionalPlaysTest
{
    [Test]
    public void PlaysAdditionalCardAfterDashita_AboveRun()
    {
        CardList hand = new CardList() { TestHelper.Card06C };

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        CardList availablePlays = ActionsForAdditionalPlays.AvailablePlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06C));
    }

    [Test]
    [Ignore("should not work but does")]
    public void PlaysAdditionalCardAfterDashita_BelowRun()
    {
        CardList hand = new CardList() { TestHelper.Card06D };

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);

        CardList availablePlays = ActionsForAdditionalPlays.AvailablePlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(1));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06D));
    }
    
    [Test]
    public void PlaysAdditionalCardAfterDashita_AboveAndBelowRun()
    {
        CardList hand = new CardList()
        {
            TestHelper.Card06C, TestHelper.Card06D
        };

        Dealer dealer = new Dealer();
        dealer.ReceiveDashita(TestHelper.Dashita02CTo05CAnd07DTo10DAndJacks);
        
        CardList availablePlays = ActionsForAdditionalPlays.AvailablePlays(dealer.PlayZone, hand);
        Assert.That(availablePlays, Has.Count.EqualTo(2));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06C));
        Assert.That(availablePlays, Does.Contain(TestHelper.Card06D));
    }
}