using UnityEngine;

public class Player : MonoBehaviour
{
    public string name { get; set; }
    public char grade { get; set; }
    public int experience { get; set; }
    public int averagePerformance { get; set; }
    public int finalModifier  { get; set; }
    public int finalPerformance { get; set; }

    public Player(string name, char grade, int experience)
    {

        this.name = name;
        this.experience = experience;
        this.grade = grade;

    }

    public int calculateAverage()
    {
        if (grade == 'A')
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
