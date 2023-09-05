using UnityEngine;

public enum grade
{
    A, B, C, D, E, F
}


public class Player : MonoBehaviour
{
    public string name { get; set; }
    public int ranking { get; set; }
    public char grade { get; set; }
    public int experience { get; set; }
    public string nationality { get; set; }
    public int averagePerformance { get; set; }
    public int runModifier { get; set; }
    public int finalPerformance { get; set; }
    public int place { get; set; }
    public float totalSeconds { get; set; }
    public bool goodFormEffect { get; set; }
    public bool poorFormEffect { get; set; }
    public bool homeFactor = false;


    public Player(string name, int ranking, char grade, int experience, string nationality)
    {
        this.ranking = ranking;
        this.name = name;
        this.experience = experience;
        this.grade = grade;
        this.nationality = nationality;

    }

    public int CalculateAverage()
    {
        // ORIGINAL AVERAGES: A 24, B 18, C 12, D 6, E 0 
        if (this.grade == 'A')
        {
            averagePerformance = 24;
        }
        else if (grade == 'B')
        {
            averagePerformance = 19;
        }
        else if (grade == 'C')
        {
            averagePerformance = 14;
        }
        else if (grade == 'D')
        {
            averagePerformance = 9;
        }
        else if (grade == 'E')
        {
            averagePerformance = 4;
        }

        return averagePerformance;

    }
    public int GetGradeModifier()
    {
        switch (grade)
        {
            case 'A': return 3;
            case 'B': return 2;
            case 'C': return 1;
            default: return 0;
        }
    }

    public void AddRunModifier(int modifier)
    {
        runModifier += modifier;
    }

    public void calculateFinal(int finalModifier)
    {
        finalPerformance += CalculateAverage() + finalModifier;
    }


    public void GoodFormEffect()
    {
        if ((this.grade != 'A') && (!poorFormEffect))
        {
            grade--;
            ranking -= 5;
            goodFormEffect = true;
            Debug.Log("Good form!" + " New ranking: " + ranking);
        }
        else if (poorFormEffect)
        {
            poorFormEffect = false;
        }

    }
    public void PoorFormEffect()
    {
        if ((this.grade != 'F') && (!goodFormEffect))
        {
            grade++;
            ranking += 5;
            poorFormEffect = true;
            Debug.Log("Poor form!" + " New ranking: " + ranking);
        }
        else if (goodFormEffect)
        {
            goodFormEffect = false;
        }
    }
    public void CheckHomeFactor()
    {
        if ((this.nationality == FindObjectOfType<Gamemanager>().venueNation))
        {
            this.homeFactor = true;
            Debug.Log("HOME FACTOR!");
        }
    }

    public string ConvertPointsToTime(int finalPerformance)
    {
        float realTime = FindObjectOfType<Gamemanager>().bestTimeInSec;
        // Assume that 1 point = 1/100 sec
        float realPerformance = (finalPerformance / 80.00f);
        float differenceInSeconds = realTime - (realTime * realPerformance);
        totalSeconds = realTime + differenceInSeconds * 0.0875f; // or 0.114
        int minutes = (int)totalSeconds / 60;
        int seconds = (int)totalSeconds % 60;
        int hundredths = Mathf.RoundToInt((totalSeconds - Mathf.Floor(totalSeconds)) * 100);

        string formattedTime = string.Format(" {0:00}:{1:00}.{2:00}", minutes.ToString(), seconds, hundredths);

        return formattedTime;
    }
    public string ConvertDifference(int difference)
    {
        float realDifference = FindObjectOfType<Gamemanager>().timeDifference;
        float modifier = realDifference / 40; // Assume that 10th competitor has 40 points
        //string.alignment = TextAnchor.MiddleRight;
        float totalDifference = difference * modifier;
        int minutes = (int)totalDifference / 60;
        int seconds = (int)totalDifference % 60;
        int hundredths = Mathf.RoundToInt((totalDifference - Mathf.Floor(totalDifference)) * 100);
        string formattedTime = (minutes > 0) ?
           // < align = right > This text is aligned to the right.</ align >
           string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths) :
            string.Format("+" + "{0:00}.{1:00}", seconds.ToString(), hundredths);

        return formattedTime;
    }

}
