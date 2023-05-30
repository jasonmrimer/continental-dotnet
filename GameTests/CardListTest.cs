using NUnit.Framework;

[TestFixture]
public class CardListTest
{
    [Test]
    public void RemovesOneCopyOfCard()
    {
        CardList hand = new(TestHelper.Run02CTo05C);

        hand.AddRange(new[]
        {
            TestHelper.Card02C,
            TestHelper.Card03C,
            TestHelper.Card04C,
            TestHelper.Card05C
        });

        Assert.AreEqual(8, hand.Count);

        hand.RemoveRange(TestHelper.Run02CTo05C);

        Assert.AreEqual(4, hand.Count);
        Assert.AreEqual(TestHelper.Run02CTo05C, hand);
    }
}