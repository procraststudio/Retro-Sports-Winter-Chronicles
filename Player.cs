using UnityEngine;

public class Player : MonoBehaviour
{
    public string name { get; set; }
    public int ranking { get; set; }    
    public char grade { get; set; }
    public int experience { get; set; }
    public int averagePerformance { get; set; }
    public int finalModifier  { get; set; }
    public int finalPerformance { get; set; }
    public int place { get; set; }

       

    public Player(string name, int ranking, char grade, int experience)
    {
        this.ranking = ranking;
        this.name = name;
        this.experience = experience;
        this.grade = grade;

    }

    public int calculateAverage()
    {
        if (this.grade =='A')
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
        return averagePerformance;

    }
    public void calculateFinal(int finalModifier)
    {
        finalPerformance = averagePerformance + finalModifier;
    }
}
