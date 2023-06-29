public class PlayAction
{
    public CardList RunOrAtama { get; }
    public CardList CardsToAdd { get; }

    public PlayAction(CardList runOrAtama, Card cardToAdd)
    {
        RunOrAtama = runOrAtama;
        CardsToAdd = new CardList() { cardToAdd };
    }

    public PlayAction(CardList runOrAtama, CardList cardsToAdd)
    {
        RunOrAtama = runOrAtama;
        CardsToAdd = cardsToAdd;
    }

    public override string ToString()
    {
        return $"Play action with {RunOrAtama} \n with addition of: {CardsToAdd}";
    }

    public override bool Equals(object? obj)
    {
        PlayAction other = (PlayAction)obj;

        if (other == null)
        {
            return false;
        }

        return other.RunOrAtama.Equals(this.RunOrAtama)
               && other.CardsToAdd.Equals(this.CardsToAdd);
    }
}