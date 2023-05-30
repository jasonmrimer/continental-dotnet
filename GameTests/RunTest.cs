using NUnit.Framework;

[TestFixture]
public class RunTest
{
    [Test]
    public void EqualsWhenSameObjects()
    {
        Run run01 = new() { TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C };
        Run run02 = new() { TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C };
        
        Assert.AreEqual(run01, run02);
    }

    [Test]
    public void EqualsWhenSameCardsButDifferentObjects()
    {
        Card card02Cv2 = new Card(Rank.Two, Suit.Clubs);
        Card card03Cv2 = new Card(Rank.Three, Suit.Clubs);
        Card card04Cv2 = new Card(Rank.Four, Suit.Clubs);
        Card card05Cv2 = new Card(Rank.Five, Suit.Clubs);

        Run run01 = new() { TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C };
        Run run02 = new() { card02Cv2, card03Cv2, card04Cv2, card05Cv2 };

        Assert.AreEqual(run01, run02);
    }

    [Test]
    public void NotEqualWhenDifferentOrder()
    {
        Run run01 = new() { TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C };
        Run run02 = new() { TestHelper.Card03C, TestHelper.Card02C, TestHelper.Card04C, TestHelper.Card05C };

        Assert.AreNotEqual(run01, run02);
    }

    [Test]
    public void NotEqualWhenDifferentSuit()
    {
        Card card02H = new Card(Rank.Two, Suit.Hearts);
        Card card03H = new Card(Rank.Three, Suit.Hearts);
        Card card04H = new Card(Rank.Four, Suit.Hearts);
        Card card05H = new Card(Rank.Five, Suit.Hearts);

        Run run01 = new() { TestHelper.Card02C, TestHelper.Card03C, TestHelper.Card04C, TestHelper.Card05C };
        Run run02 = new() { card02H, card03H, card04H, card05H };

        Assert.AreNotEqual(run01, run02);
    }
}