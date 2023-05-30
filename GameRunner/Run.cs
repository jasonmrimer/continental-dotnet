using System.Linq;

public class Run : CardList
{
    public Run(Run run)
    {
        AddRange(run);
    }

    public Run()
    {
    }

    public Run(CardList cardList)
    {
        AddRange(cardList);
    }
        

    public override bool Equals(object obj)
    {
        if (obj is not Run other)
        {
            return false;
        }

        return (other.Count == this.Count) && (other.SequenceEqual(this));
    }

    public override string ToString()
    {
        return this.Aggregate("Run: ", (current, card) => current + $"{card}, ");
    }
}