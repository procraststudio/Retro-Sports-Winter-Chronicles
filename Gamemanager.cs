using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gamemanager : MonoBehaviour
{
    public List<Player> favourites, outsiders, underdogs;
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
    public CompetitionType competitionType;
    public CompetitionType[] sampleCompetitions;

    void Start()
    {
        competition = Competition.Instance;
        //competitionType = new CompetitionType {competitionName ="Slalom Women", competitionDate = new DateTime (1986,2,23), competitionVenueName ="Sestriere"  };
        competitionName = GameStart.currentCompetition != null ? GameStart.currentCompetition.competitionName.ToString()
            : sampleCompetitions[1].competitionVenueName;
        //venueLoader = new VenueLoader();
        //venueLoader.LoadVenue(competitionName);

        // typeOfCompetition = "alpine skiing";
        // TODO: select competition type, number of runs, surprises modifier, ??number of competitors
        competitionName = sampleCompetitions[1].competitionName;    // "CALGARY 1988 Alpine Ski: Downhill MEN. RUN: "; //Downhill
        //venueNation = "CAN";
        numbersOfRun = sampleCompetitions[1].numberOfRuns;
        surprisesModifier = 1.00f;//1.00f; // default should be 1.00f, slalom: 2.00

        // FAVOURITES:
        Player player01 = new Player("Alberto", "Tomba", 1, 'X', 1, "ITA");
        Player player02 = new Player("Gunther", "Mader", 2, 'A', 2, "AUT");
        Player player03 = new Player("Felix", "McGrath", 3, 'B', 1, "USA");
        Player player04 = new Player("Paul", "Frommelt", 4, 'B', 2, "LIE");
        Player player05 = new Player("Armin", "Bittner", 5, 'B', 1, "FRG");
        Player player06 = new Player("Hubert", "Strolz", 6, 'B', 2, "AUT");
        Player player07 = new Player("Bernhard", "Gstrein", 7, 'B', 1, "AUT");
        Player player08 = new Player("Jonas", "Nilsson", 8, 'B', 2, "SWE");
        Player player09 = new Player("Pirmin", "Zurbriggen", 9, 'C', 3, "SUI");
        Player player10 = new Player("Grega", "Benedik", 10, 'C', 2, "YUG");
        Player player11 = new Player("Peter", "Muller", 11, 'A', 2, "SUI");
        Player player12 = new Player("Franck", "Piccard", 12, 'A', 2, "FRA");
        Player player13 = new Player("Leonhard", "Stock", 13, 'B', 1, "AUT");
        Player player14 = new Player("Gerhard", "Pfaffenbichler", 14, 'B', 1, "AUT");
        Player player15 = new Player("Tetsuya", "Okabe", 15, 'C', 1, "JPN");

        // OUTSIDERS:
        Player player16 = new Player("Thomas", "Stangassinger", 16, 'C', 1, "AUT");
        Player player17 = new Player("Richard", "Pramotton", 17, 'C', 1, "ITA");
        Player player18 = new Player("Carlo", "Gerosa", 18, 'C', 1, "ITA");
        Player player19 = new Player("Dietmar", "Kohlbichler", 19, 'C', 1, "AUT");
        Player player20 = new Player("Bojan", "Krizaj", 20, 'D', 3, "YUG");
        Player player21 = new Player("Finn Christian", "Jagge", 21, 'E', 0, "NOR");
        Player player22 = new Player("Peter", "Jurko", 22, 'E', 0, "TCH");
        Player player23 = new Player("Niklas", "Henning", 23, 'E', 1, "SWE");
        Player player24 = new Player("Adrian", "Bires", 24, 'E', 0, "TCH");
        Player player25 = new Player("Jorge", "Birkner", 25, 'E', 0, "ARG");

        // UNDERDOGS:
        Player player26 = new Player("Rok", "Petrovic", 26, 'E', 1, "YUG");
        Player player27 = new Player("Cyrille", "Bouvet", 27, 'E', 0, "FRA");
        Player player28 = new Player("Oskar", "Sundqvist", 28, 'E', 2, "SWE");
        Player player29 = new Player("Frank", "Besse", 29, 'E', 1, "SUI");
        Player player30 = new Player("Massimo", "Piantanida", 30, 'E', 2, "ITA");

        favourites = new List<Player> { player01, player02, player03, player04, player05,
        player06, player07, player08, player09, player10, player11, player12, player13,
            player14, player15};
        outsiders = new List<Player> { player16, player17, player18, player19, player20,
        player21, player22, player23, player24, player25};
        underdogs = new List<Player> { player26, player27, player28, player29, player30 };
        lists = new List<Player>[] { favourites, outsiders, underdogs };
        RandomizeLists(lists);
        bestTimeInSec = 49.42f;         // DH 119.63f; 2 runs: average from 2 best runs
        tenthTime = 50.51f;       // DH 122.69f; przy 15 faworytach to czas 15-go?
        timeDifference = tenthTime - bestTimeInSec;
        bestOverallTime = bestTimeInSec;
        numberOfFavourites = favourites.Count;

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








}
