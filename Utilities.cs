using UnityEngine;

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

    public void MinimumOutsiders()
    {
        int index = 5;
        competition.outsiders.RemoveRange(competition.outsiders.Count - index, index);
        // competition.currentCompetitorNo = competition.players.Count - 1;
        competition.underdogs.RemoveRange(competition.underdogs.Count - index/2, index/2);
        competition.UpdateLists();
    }
    public void LowerSurprisesChance()
    {
        gamemanager.surprisesModifier *= 0.50f;

    }
    public void HigherSurprisesChance()
    {
        gamemanager.surprisesModifier *= 1.50f;

    }
    public void ResetRecords()
    {
        PlayerPrefs.SetInt("competitionRecord", 0);

    }
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

    }
    public void AllSwiss()
    {
        for (int i = 0; i < competition.players.Count; i++)
        {
            competition.players[i].nationality = "SUI";
        }

    }


    // TO DO: DEACTIVATE WEATHER & PRESENTATION

}