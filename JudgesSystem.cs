using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class JudgesSystem : MonoBehaviour
{
    // This script is in CalculatePerformance script

    public List<double> judgesNotes = new List<double> { };
    void Start()
    {

    }

    public void GetJudgesNotes(string discipline, Player player, float performance)
    {
        switch (discipline)
        {
            case "skiJumping": GetSkiJumpingNotes(player, performance); break;

        }

    }

    // Update is called once per frame
    public double GetSkiJumpingNotes(Player player, float performance)
    {
        judgesNotes.Clear();
        double judgesPoints = 0; //3x18.0
        double averageNote = Random.Range(16, 18);
        int index = 0;
        double note = 0;

        // modify for grade/rank/perf pts

        for (int i = 0; i < 5; i++)
        {
            note = RatingDiversity(averageNote);
            if (performance >50)
            {
                note += 2.0;
                //note += ((performance-50)/15);
            }
            else if (performance < 30)
            {
                note -= 2.0;
                //note -= ((performance - 30)/10);
            }
            else if (player.grade > 'B')
            {
                note += 1.0;
            }
            else if (player.grade <'C')
            {
                note -= 1.0;
            }
            if (note>=19.5)
            {
                note = 19.5;
            }
            judgesNotes.Add(note);
        }
        judgesPoints = GetAverageNotes(judgesNotes);

        return judgesPoints;
    }

    public double RatingDiversity(double rating)
    {
        int index = Random.Range(1, 11);
        float diversity = 0;
        double modifiedRating = 0;

        if (index > 8)
        {
            diversity = Random.Range(-1.5f, 1.5f);
        }
        else if ((index > 5) && (index < 9))
        {
            diversity = Random.Range(-0.5f, 0.5f);
        }
        else
        {
            diversity = 0;
        }
        modifiedRating = rating + diversity;
        modifiedRating = CalculatePerformance.RoundToNearestHalf((float)modifiedRating);
        return Convert.ToDouble(modifiedRating);

    }

    public void ResetNotes()
    {
        // TO DO?
    }

    public double GetAverageNotes(List<double> list)
    {
        list.RemoveAt(0); // remove highest & lowest 
        list.RemoveAt(list.Count - 1); 
        foreach (double note in list)
        {
            Debug.Log("NOTE: " + note.ToString() +" ");   
        }
        Debug.Log(". NOTES SUM: " +  list.Sum().ToString());
        return list.Sum();
    }


}
