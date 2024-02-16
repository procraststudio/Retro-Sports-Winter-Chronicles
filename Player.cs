using UnityEngine;

public enum grade
{
    A, B, C, D, E, F
}


public class Player : MonoBehaviour
{
    public string secondName { get; set; }
    public string surname { get; set; }
    public int ranking { get; set; }
    public char grade { get; set; }
    public int experience { get; set; }
    public string nationality { get; set; }
    public int averagePerformance { get; set; }
    public int runModifiers { get; set; }
    public int currentModifier { get; set; }
    public float firstRunPoints { get; set; } = 0;
    public int firstRunModifiers { get; set; } = 0;
    public float secondRunPoints { get; set; } = 0;
    public int secondRunModifiers { get; set; } = 0;
    public float finalPerformance { get; set; }
    public int place { get; set; }
    public float totalSeconds { get; set; }
    public bool goodFormEffect { get; set; }
    public bool poorFormEffect { get; set; }
    public bool homeFactor = false;
    public PlayerState myState;

    // ? TODO: ADD Dominator like Albert Tomba = grade A+

    public enum PlayerState
    {
        DidNotStart = 0,  // DNS
        Running = 1,
        OutOf15 = 2,
        DidNotFinish = 3,  //DNF
        Disqualified = 4,  //DQ
    }


    public Player(string surname, string name, int ranking, char grade, int experience, string nationality)
    {
        this.ranking = ranking;
        this.surname = surname;
        this.secondName = name;
        this.experience = experience;
        this.grade = grade;
        this.nationality = nationality;
        this.myState = PlayerState.Running;
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

    public void AddRunModifier(int currentRun, int modifier)
    {
        if (currentRun == 1)
        {
            firstRunModifiers += modifier;
            Debug.Log(modifier + " pts added.");
        }
        else if (currentRun == 2)
        {
            secondRunModifiers += modifier;
            Debug.Log(modifier + " pts added.");
        }
    }
    public float CalculateActualRun(int currentRun)
    {
        if (currentRun == 1)
        {
            firstRunPoints += (float)CalculateAverage() + (float)firstRunModifiers + Random.Range(-0.50f, 0.50f);
            return firstRunPoints;
            // finalPerformance = (float)firstRunPoints + Random.Range(0.00f, 1.00f);
        }
        else if (currentRun == 2)
        {
            secondRunPoints += (float)CalculateAverage() + (float)secondRunModifiers + Random.Range(-0.50f, 0.50f);
            return secondRunPoints;
            // finalPerformance += (float)secondRunPoints;
        }
        else
        {
            return 0.0f;
        }

    }
    public int actualModifiers(int currentRun)
    {
        if (currentRun == 1)
        {
            return firstRunModifiers;
        }
        else
        {
            return secondRunModifiers;
        }
    }

    public float CalculateFinal()
    {
        finalPerformance = firstRunPoints + secondRunPoints; // + Random.Range(0.00f, 1.00f);
        //finalPerformance += Random.Range(0.00f, 1.00f); // Random value to break ties

        return finalPerformance;
    }


    public void GoodFormEffect()
    {
        if ((this.grade != 'A') && (!poorFormEffect))
        {
            grade--;
            ranking -= 5;
            if (ranking < 1)
            {
                ranking = 1;
            }   
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
    public bool CheckHomeFactor()
    {
        if ((this.nationality == FindObjectOfType<Gamemanager>().venueNation))
        {
            Debug.Log("HOME FACTOR!");
            return true;
        }
        else { return false; }
    }

    public void SetPlayerState(PlayerState state)
    {
        myState = state;
    }

    public string ConvertPointsToTime(float points)
    {
        float realTime = FindObjectOfType<Gamemanager>().bestTimeInSec;
        // Assume that 1 point = 1/100 sec
        float realPerformance = (points / 80.00f);
        float differenceInSeconds = realTime - (realTime * realPerformance);
        totalSeconds = realTime + differenceInSeconds * 0.0875f; // or 0.114
        int minutes = (int)totalSeconds / 60;
        int seconds = (int)totalSeconds % 60;
        int hundredths = Mathf.RoundToInt((totalSeconds - Mathf.Floor(totalSeconds)) * 100);

        string formattedTime = string.Format(" {0:00}:{1:00}.{2:00}", minutes.ToString(), seconds, hundredths);

        return formattedTime;
    }
    public string ConvertDifference(float difference)
    {
        float realDifference = FindObjectOfType<Gamemanager>().timeDifference;
        float modifier = realDifference / 40; // Assume that 10th competitor has 40 points
                                              //string.alignment = TextAnchor.MiddleRight;
        float totalDifference = Mathf.Abs(difference) * modifier;
        int minutes = (int)totalDifference / 60;
        int seconds = (int)totalDifference % 60;
        int hundredths = Mathf.RoundToInt((totalDifference - Mathf.Floor(totalDifference)) * 100);
        string formattedTime = (minutes > 0)  ?

        string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths) :
        (difference<0) ?
        string.Format("-" + "{0:00}.{1:00}", seconds.ToString(), hundredths):
        string.Format("+" + "{0:00}.{1:00}", seconds.ToString(), hundredths);

         if (difference < 0) {
           string.Format("-" + "{0:00}.{1:00}", seconds.ToString(), hundredths);
        }

        return formattedTime;
    }

}
