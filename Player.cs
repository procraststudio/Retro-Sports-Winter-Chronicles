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

        if (this.grade == 'A')
        {
            averagePerformance = 24;
        }
        else if (grade == 'B')
        {
            averagePerformance = 18;
        }
        else if (grade == 'C')
        {
            averagePerformance = 12;
        }
        else if (grade == 'D')
        {
            averagePerformance = 6;
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
        if (this.grade != 'A')
        {
            grade--;
            ranking -= 5;
            Debug.Log("Good form!" + " New ranking: " + ranking);
        }
    }
    public void PoorFormEffect()
    {
        if (this.grade != 'F')
            grade++;
            ranking += 5;
            Debug.Log("Poor form!"+ " New ranking: " + ranking);
    }

}
