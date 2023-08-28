using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro.EditorUtilities;
using TMPro;

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



    public Player(string name, int ranking, char grade, int experience, string nationality)
    {
        this.ranking = ranking;
        this.name = name;
        this.experience = experience;
        this.grade = grade;
        this.nationality = nationality;

    }

    public int calculateAverage()
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

        return averagePerformance;

    }

    public void AddRunModifier(int modifier)
    {
        runModifier += modifier;
    }
    public void calculateFinal(int finalModifier)
    {
        finalPerformance += averagePerformance + runModifier;
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

    public string ConvertPointsToTime(int finalPerformance)
    {
        float realTime = FindObjectOfType<Gamemanager>().bestTimeInSec;
        // Zak³adamy, ¿e 1 punkt = 1/100 sekundy
        float realPerformance = (finalPerformance / 80.00f);
        float differenceInSeconds = realTime - (realTime * realPerformance);
        totalSeconds = realTime + differenceInSeconds * 0.09f; // or 0.114
        Debug.Log(totalSeconds);
        int minutes = (int)totalSeconds / 60;
        int seconds = (int)totalSeconds % 60;
        int hundredths = Mathf.RoundToInt((totalSeconds - Mathf.Floor(totalSeconds)) * 100);

        string formattedTime = string.Format(" {0:00}:{1:00}.{2:00}", minutes.ToString(), seconds, hundredths);

        return formattedTime;
    }
    public string ConvertDifference (int difference)
    {
        //string.alignment = TextAnchor.MiddleRight;
        float totalDifference = difference * 0.09f; // or 0.114 :)
        int minutes = (int)totalDifference / 60;
        int seconds = (int)totalDifference % 60;
        int hundredths = Mathf.RoundToInt((totalDifference - Mathf.Floor(totalDifference)) * 100);
        string formattedTime = (minutes >0) ?
           // < align = right > This text is aligned to the right.</ align >
           string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths) :
            string.Format("+"+"{0:00}.{1:00}", seconds.ToString(), hundredths);
        
        return  formattedTime;
    }

}
