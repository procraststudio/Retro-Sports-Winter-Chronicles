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
    public CompetitionType thisCompetition;
    public CompetitionType[] sampleCompetitions;
    private string competitorsPath = "Competitors";
    public string filePath = "Assets/Resources/Competitors/";

    void Start()
    {
        competition = Competition.Instance;
        // gameStart = FindObjectOfType<GameStart>();

        if (GameStart.currentCompetition == null)
        {
            thisCompetition = sampleCompetitions[0];
        }
        else
        {
            thisCompetition = GameStart.currentCompetition;
        }
        //competitionType = new CompetitionType {competitionName ="Slalom Women", competitionDate = new DateTime (1986,2,23), competitionVenueName ="Sestriere"  };
        competitionName = thisCompetition.competitionName.ToString();
        numbersOfRun = thisCompetition.numberOfRuns;
        CalculateSurpriseModifier();
        //surprisesModifier =  1.00f; //
        bestTimeInSec = thisCompetition.bestTimeinSec; // DH 119.63f; 2 runs: average from 2 best runs
        tenthTime = thisCompetition.tenthTimeinSec;   //  przy 15 faworytach to czas 15-go?
        timeDifference = tenthTime - bestTimeInSec;
        bestOverallTime = bestTimeInSec;

        //competitionName = GameStart.currentCompetition != null ? GameStart.currentCompetition.competitionName.ToString()
        // : sampleCompetitions[0].competitionName;
        //venueLoader = new VenueLoader();
        //venueLoader.LoadVenue(competitionName);
        // typeOfCompetition = "alpine skiing";
        // TODO: select competition type, number of runs, surprises modifier, ??number of competitors
        //  competitionName = sampleCompetitions[0].competitionName;
        //filePath += competitionName + ".txt"; // "CALGARY 1988 Alpine Ski: Downhill MEN. RUN: "; //Downhill
        //venueNation = "CAN";
        // numbersOfRun = sampleCompetitions[0].numberOfRuns;
        filePath += thisCompetition.competitorsDatabase + ".txt";
        //bonusCompetitorsFilePath += thisCompetition.bonusDatabaseName + ".txt";
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
            // Divide players into 3 groups
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

    void LoadCompetitionInfo(CompetitionType competitionType)
    {
        // all infos abou competition
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
            surprisesModifier = 1.00f;
        }

        //Slalom Men = œr 0,40 // Super G men 0,31


    }

    public void createCompetitorsList() // what's this? what for?
    {
        // var playerList = new List<Player>();
        //TextAsset txtFile = null;
        //txtFile = Resources.Load<TextAsset>(competitorsPath);
        // textAsset = Resources.Load<TextAsset>(goodTextPath)
        favourites = new List<Player> { };
        // foreach (var line in File.ReadLines(txtFile))
        {
            // var parts = line.Split(','); // Za³ó¿my, ¿e dane s¹ oddzielone przecinkami
            // if (parts.Length >= 4)
            // {
            //  Player player00 = new Player(parts[0], parts[1], int.Parse(parts[2]), char.Parse(parts[3]), int.Parse(parts[4]), parts[5]);
            //Player player = new Player
            //{
            //    surname = parts[0],
            //    name = parts[1],
            //    ranking = int.Parse(parts[2]),
            //    grade = char.Parse(parts[3]),
            //    experience = int.Parse(parts[4]),
            //    nationality = parts[5],

            //};
            // favourites.Add(player00);

            //public Player(string surname, string name, int ranking, char grade, int experience, string nationality)
        }
    }


}









