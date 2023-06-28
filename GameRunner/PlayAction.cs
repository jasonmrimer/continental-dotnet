public class PlayAction
{
    public CardList RunOrAtama { get; }
    public Card CardToAdd { get; }

    public PlayAction(CardList runOrAtama, Card cardToAdd)
    {
        RunOrAtama = runOrAtama;
        CardToAdd = cardToAdd;
    }
    
    public PlayAction(CardList runOrAtama, CardList cardsToAdd)
    {
        RunOrAtama = runOrAtama;
    }

    public override string ToString()
    {
        return $"Play action with {RunOrAtama} \n with addition of: {CardToAdd}";
    }

    public override bool Equals(object? obj)
    {
        PlayAction other = (PlayAction)obj;

        if (other == null)
        {
            return false;
        }

        return other.RunOrAtama.Equals(this.RunOrAtama) &&
               other.CardToAdd.Equals(this.CardToAdd);
    }
}