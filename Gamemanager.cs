using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gamemanager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public List<Player> favourites, outsiders, underdogs, bonusCompetitors;
    public List<Player>[] lists;
    [SerializeField] Venue actualVenue;

    Competition competition;
    public String typeOfCompetition;
    public String competitionName;
    public float surprisesModifier;
    public float disqalificationModifier = 0.18f; // default percentage of surprisesModifier
    public int numberOfFavourites { get; set; }
    public float bestTimeInSec { get; set; }
    public float bestOverallTime { get; set; }
    public float tenthTime; // Time of 10th competitor (in secs)
    public float timeDifference;
    public static int numbersOfRun;
    public string venueNation { get; private set; }
    public float temperatureMin { get; private set; }
    public float temperatureMax { get; private set; }
    public VenueLoader venueLoader;
    public static CompetitionType thisCompetition { get; set; }
    public CompetitionType[] sampleCompetitions;
    public WorldCupCompetition actualWorldCupCompetition;
    private string competitorsPath = "Competitors";
    public string filePath = "Assets/Resources/Competitors/";

    void Start()
    {
        competition = Competition.Instance;
        if (GameStart.currentCompetition == null)
        {
            thisCompetition = sampleCompetitions[0];
        }
        else
        {
            if (GameStart.currentWorldCup != null)
            actualWorldCupCompetition = GameStart.currentWorldCup;
            thisCompetition = GameStart.currentCompetition;
        }
        competitionName = thisCompetition.competitionName.ToString();
        venueNation = thisCompetition.competitionVenueNation;
        numbersOfRun = thisCompetition.numberOfRuns;
        CalculateSurpriseModifier();
        bestTimeInSec = thisCompetition.bestTimeinSec; // DH 119.63f; 2 runs: average from 2 best runs
        tenthTime = thisCompetition.tenthTimeinSec;   //  przy 15 faworytach to czas 15-go?
        timeDifference = tenthTime - bestTimeInSec;
        bestOverallTime = bestTimeInSec;
        filePath += thisCompetition.competitorsDatabase + ".txt";
        LoadPlayersFromFile(filePath);
        if (thisCompetition.bonusDatabaseName != null)
        {
            LoadPlayersFromFile("Assets/Resources/Competitors/" + thisCompetition.bonusDatabaseName + ".txt");
        }
        lists = new List<Player>[] { favourites, outsiders, underdogs, bonusCompetitors };
        RandomizeLists(lists);
        numberOfFavourites = favourites.Count;
    }

    private void LoadPlayersFromFile(string path)
    {
        try
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                // Usuwanie zbêdnych spacji i cudzys³owów
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim().Trim('"');
                }
                // Parsowanie wartoœci
                string surname = values[0];
                string name = values[1];
                int ranking = int.Parse(values[2]);
                char grade = values[3][0];
                int experience = int.Parse(values[4]);
                string nationality = values[5];

                // Tworzenie nowego obiektu Player
                Player newPlayer = new Player(surname, name, ranking, grade, experience, nationality);
                players.Add(newPlayer);
            }
            // Dividing players into 3 groups
            favourites = players.Where(player => player.ranking >= 1 && player.ranking <= 15).ToList();
            outsiders = players.Where(player => player.ranking >= 16 && player.ranking <= 25).ToList();
            underdogs = players.Where(player => player.ranking >= 26 && player.ranking <= 30).ToList();
            bonusCompetitors = players.Where(player => player.ranking >= 31).ToList();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load players from file: " + e.Message);
        }
    }


    private void RandomizeLists(List<Player>[] lists)
    {
        for (int i = 0; i < lists.Length; i++)
        {
            int n = lists[i].Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                Player value = lists[i][k];
                lists[i][k] = lists[i][n];
                lists[i][n] = value;
            }
        }
    }

    public void ModifyTimes(float modifier)
    {
        bestTimeInSec += bestTimeInSec * modifier;
        bestOverallTime += bestTimeInSec;
        tenthTime += tenthTime * modifier;
        timeDifference = tenthTime - bestTimeInSec;

    }

    public void CalculateSurpriseModifier() // default should be 1.00f, slalom: 2.00 OR 1.50-2.50 depends on weather
    {
        if ((competitionName.Contains("Slalom Women")) || (competitionName.Contains("Super G Men")))
        {
            surprisesModifier = 1.55f;
            Debug.Log("SURPRISE CALCULATED");
        }
        else if (competitionName.Contains("Giant Slalom"))
        {
            surprisesModifier = 1.35f;
            Debug.Log("SURPRISE CALCULATED");
            // this collides with Slalom Men ??
        }

        else if (competitionName.Contains("Slalom Men"))
        {
            surprisesModifier = 2.00f;
            Debug.Log("SURPRISE CALCULATED");
        }
  
        else
        {
            surprisesModifier = thisCompetition.surprisesImpact;
            Debug.Log("SURPRISE CALCULATED");
        }
        // Slalom Men = œr 0,40 // Super G men 0,31
    }

    public void HandleWorldCupCompetition()
    {
        PlayerPrefs.SetInt("number", 1);
    }

    public void PlayNextWorldCupCompetition()
    {
        int index = PlayerPrefs.GetInt("currentWorldCupNumber");
        PlayerPrefs.SetInt("currentWorldCupNumber", index + 1);
        index = PlayerPrefs.GetInt("currentWorldCupNumber");
        PlayerPrefs.Save();
        if (index < actualWorldCupCompetition.worldCupEvents.Length)
        {
            Debug.Log("CURRENT WC No: " + PlayerPrefs.GetInt("currentWorldCupNumber"));
            GameStart.StartCompetition(actualWorldCupCompetition.worldCupEvents[index]);
        }
    }

    public bool IsNextWorldCupEventPossible()
    {
        int index = PlayerPrefs.GetInt("currentWorldCupNumber");
        if (index >= actualWorldCupCompetition.worldCupEvents.Length-1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static CompetitionType GetCompetitionType()
    {
        return thisCompetition;
    }
}









