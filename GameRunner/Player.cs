using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    private readonly CardList _cards;
    public string Name { get; }
    public bool HasPlayedDashita { get; private set; }

    public Player(string name)
    {
        _cards = new CardList();
        Name = name;
        HasPlayedDashita = false;
    }


    public int CardCount()
    {
        return _cards.Count;
    }

    public void AddToHand(Card card)
    {
        _cards.Add(card);
    }

    public CardList Hand()
    {
        return _cards;
    }

    public string FormatHandForPrint()
    {
        string printableHand = "";

        foreach (Card card in _cards)
        {
            printableHand += card.Printable();

            if (!Equals(card, _cards.Last()))
            {
                printableHand += " | ";
            }
        }

        return printableHand;
    }

    public Card DiscardFromHand()
    {
        Card discard = ChooseDiscard();
        _cards.Remove(discard);
        GameWriter.PrintDiscardAction(this, discard);
        return discard;
    }

    private Card ChooseDiscard()
    {
        Random random = new Random();
        Card card = _cards[random.Next(_cards.Count)];
        return card;
    }

    public static DrawSource ChooseDrawSource(bool pileIsAvailable)
    {
        DrawSource source = DrawSource.Deck;

        Random random = new Random();
        double randomValue = random.NextDouble();

        if (randomValue < 0.25 && pileIsAvailable)
        {
            source = DrawSource.Pile;
        }

        return source;
    }

    public static bool DecideWhetherToTakePenalty()
    {
        return ChooseDrawSource(true) == DrawSource.Pile;
    }

    public Dashita PlayDecision()
    {
        if (!DecidesToPlayAtRandom()) return null;

        HasPlayedDashita = true;
        HashSet<Dashita> dashitaOptions = DashitaGenerator.GenerateOptions(Hand());
        Dashita chosenDashita = new List<Dashita>(dashitaOptions)[0];
        RemoveDashita(chosenDashita);

        return chosenDashita;
    }

    private void RemoveDashita(Dashita dashita)
    {
        foreach (Run run in dashita.Runs)
        {
            Hand().RemoveRange(run);
        }

        Hand().RemoveRange(dashita.Atama);
    }

    private static bool DecidesToPlayAtRandom()
    {
        Random random = new Random();
        double randomValue = random.NextDouble();
        return randomValue < 0.50;
    }

    public bool CanPlay(List<CardList> playZone)
    {
        return true;
    }

    public CardList AvailablePlays(List<CardList> playZone)
    {
        CardList availablePlays = new();

        foreach (Card card in Hand())
        {
            foreach (CardList cardList in playZone)
            {
                if (cardList.GetType() == typeof(Run))
                {
                    Run appendToEnd = new Run(cardList);
                    appendToEnd.Add(card);
                    if (RunFinder.IsRun(appendToEnd))
                    {
                        availablePlays.Add(card);
                    }
                    
                }
            }
        }

        return availablePlays;
    }
}