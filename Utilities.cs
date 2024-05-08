using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Utilities : MonoBehaviour
{
    Competition competition;
    Gamemanager gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        competition = Competition.Instance;
        gamemanager = FindObjectOfType<Gamemanager>();

    }

    void GenerateDoublets()
    {
        competition.GetComponent<Competition>().firstD6 = 4;
        competition.GetComponent<Competition>().secondD6 = 4;
        Debug.Log("Doublets generated");
    }

    public void MinimumCompetitors()
    {
        int index = 5;
        competition.players.RemoveRange(competition.players.Count - index, index);
        competition.currentCompetitorNo = competition.players.Count - 1;
        competition.UpdateLists();
    }
    public void LowerSurprisesChance()
    {
        gamemanager.surprisesModifier *= 0.50f;
        
    }


    // TO DO: DEACTIVATE WEATHER & PRESENTATION

}