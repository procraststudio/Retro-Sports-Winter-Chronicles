using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UIElements;


public enum grade
{
   X=6, A=5, B=4, C=3, D=2, E=1, F=0
}
public enum startingGrade
{
    X=6, A=5, B=4, C=3, D=2, E=1, F=0
}

[System.Serializable]
public class Player: MonoBehaviour
{
    public string secondName { get; set; }
    public string surname { get; set; }
    public int ranking { get; set; }
    public char grade { get; set; }
    public char startingGrade { get; set; } 
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
    public int worldCupPoints { get; set; } = 0;
    public int currentWorldCupPoints { get; set; } = 0;
    public int place { get; set; }
    public int firstRunPlace { get; set; }
    public int secondRunPlace { get; set; }
    public int worldCupPlace { get; set; }
    public float totalSeconds { get; set; } = 0;
    public float firstRunDistance { get; set; } = 0;
    public float secondRunDistance { get; set; } = 0;

    public double skiJumpingPoints { get; set; } = 0;

    public bool goodFormEffect { get; set; }
    public bool poorFormEffect { get; set; }
    public bool homeFactor = false;
    public int praisesByCommentator { get; set; } = 0; 
    public PlayerState myState;

    public enum PlayerState
    {
        DidNotStart = 0,  // DNS
        Running = 1,
        OutOf15 = 2,
        DidNotFinish = 3,  //DNF
        Disqualified = 4,  //DQ
        Injured = 5, // INJ
    }


    public Player(string surname, string name, int ranking, char grade, int experience, string nationality)
    {
        this.ranking = ranking;
        this.surname = surname;
        this.secondName = name;
        this.experience = experience;
        this.grade = grade;
        this.startingGrade = grade;
        this.nationality = nationality;
        this.myState = PlayerState.Running;
        this.worldCupPoints = 0;
    }

    public int CalculateAverage()
    {
        // ORIGINAL AVERAGES: X 27, A 24, B 19, C 14, D 9, E 4 
        if (this.grade == 'X')
        {
            averagePerformance = 28;
        }

        else if (this.grade == 'A')
        {
            averagePerformance = 24;
        }
        else if (grade == 'B')
        {
            averagePerformance = 19;
        }
        else if (grade == 'C')
        {
            averagePerformance = 15;
        }
        else if (grade == 'D')
        {
            averagePerformance = 11;
        }
        else if (grade == 'E')
        {
            averagePerformance = 8;
        }
        else if (grade == 'F')
        {
            averagePerformance = 6;
        }

        return averagePerformance;

    }
    public int GetGradeModifier()
    {
        switch (grade)
        {
            case 'X': return 3;
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
            //Debug.Log(modifier + " pts added.");
        }
        else if (currentRun == 2)
        {
            secondRunModifiers += modifier;
           // Debug.Log(modifier + " pts added.");
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
        else if (currentRun== 2)
        {
            return secondRunModifiers;
        }
        else { return 0; }  
    }

    public float CalculateFinal()
    {
        finalPerformance = firstRunPoints + secondRunPoints; // + Random.Range(0.00f, 1.00f);
        //finalPerformance += Random.Range(0.00f, 1.00f); // Random value to break ties
        
        return finalPerformance;
    }


    public void GoodFormEffect()
    {
        if ((this.grade != 'X') &&(this.grade != 'A') && (!poorFormEffect))
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
            grade--;
            ranking -= 5;
        }

    }
    public void PoorFormEffect()
    {
        if ((this.grade != 'F') && (this.grade !='X') && (!goodFormEffect))
        {
            grade++;
            ranking += 5;
            poorFormEffect = true;
            Debug.Log("Poor form!" + " New ranking: " + ranking);
        }
        else if ((this.grade=='X') && (!goodFormEffect))
        {
            this.grade = 'B';
            ranking += 5;
            poorFormEffect = true;
        }
        else if (goodFormEffect)
        {
            goodFormEffect = false;
            grade++;
            ranking += 5;
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

    public string ConvertPointsToTime(float points, string typeOfPoints) // OR Convert Points to Metres
    {
        float realTime = (!typeOfPoints.Contains("finalPoints")) ? FindObjectOfType<Gamemanager>().bestTimeInSec
           : FindObjectOfType<Gamemanager>().bestTimeInSec * 2;//bestOverallTime; //;//*(timeModifier); // Real best time in secs. What if 2 runs?
        // Assume that 1 point = 1/100 sec
        float realPerformance = (!typeOfPoints.Contains("finalPoints")) ? points / 80.00f
        :  points / 160.00f;// 80 pts to miara najlepszego wystêpu w jednej serii
        float differenceInSeconds = realTime - (realTime * realPerformance);
        totalSeconds = realTime + (differenceInSeconds * 0.0875f); // or 0.114
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

        if (hundredths ==100) // FIX +0.100 bug, SHOULD BE: +1.00, +1.100 should be +2.00
        {
            hundredths = 0;
            seconds += 1;
        }
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

    public string ConvertPointsToDistance(float points, string typeOfPoints)
    {
        // 80 pts = K point in ski jumping // 10th competitor has 40 points and 93,5% of winner distance
        float kPoint = 80.0f; 
        Competition competition;
        competition = Competition.Instance;
        float jumpDistance = (kPoint) + (points-80.0f / 100f); //+/- 0.5 m in ski jumping




        string formattedDistance = jumpDistance.ToString(); //string.Format(" {0:00}:{1:00}.{2:00}", minutes.ToString(), seconds, hundredths);
        return formattedDistance;
    }

    public void AddWorldCupPoints(int points)
    {
        currentWorldCupPoints += points;
        worldCupPoints += points;
    }

    public void ResetPlayerStats()
    {

    }
    public string GetKey()
    {
        return $"{secondName}_{surname}";
    }


}
