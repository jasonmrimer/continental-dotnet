using System.Collections.Generic;
using System.Linq;

public class Dashita
{
    public Dashita(Run run01, Run run02, Atama atama)
    {
        Runs = new List<Run>() { run01, run02 };
        Atama = atama;
    }
        
    public Dashita(List<Run> runs, Atama atama)
    {
        Runs = runs;
        Atama = atama;
    }

    public Atama Atama { get; }

    public List<Run> Runs { get; }

    public override bool Equals(object obj)
    {
        if (obj is not Dashita other)
        {
            return false;
        }

        List<Run> orderedOtherRuns = other.Runs.OrderBy(run => run[0].Suit).ToList();
        List<Run> orderedBaseRuns = this.Runs.OrderBy(run => run[0].Suit).ToList();


        // Check if the number of runs and atama is the same
        if (Runs.Count != other.Runs.Count || Atama.Count != other.Atama.Count)
            return false;

        // Check if the runs have the same rank and suits
        for (int i = 0; i < Runs.Count; i++)
        {
            Run baseRun = orderedBaseRuns[i];
            Run otherRun = orderedOtherRuns[i];
            if (!baseRun.Equals(otherRun))
                return false;
        }

        if (!other.Atama.Equals(Atama))
        {
            return false;
        }

        return true;
    }
    
    public override string ToString()
    {
        string message = "";

        foreach (List<Card> run in Runs)
        {
            message += $"{run}\n";
        }

        message += Atama.ToString();

        return message;
    }
}