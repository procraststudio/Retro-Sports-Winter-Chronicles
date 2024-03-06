using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gamemanager : MonoBehaviour
{
    public List<Player> favourites, outsiders, underdogs;
    public List<Player>[] lists;

    Competition competition;
    public String typeOfCompetition;
    public String competitionName;
    public float surprisesModifier;
    public float disqalificationModifier = 0.18f; // default percentage of surprisesModifier
    public int numberOfFavourites { get; set; }
    public float bestTimeInSec { get; private set; } 
    private float tenthTime; // Time of 10th competitor (in secs)
    public float timeDifference;
    public static int numbersOfRun;
    public string venueNation { get; private set; } 
    public float temperatureMin { get; private set; }
    public float temperatureMax { get; private set; }

void Start()
    {
        competition = Competition.Instance;
        typeOfCompetition = "alpine skiing";
        competitionName = "CALGARY 1988 Alpine Ski: Slalom MEN. RUN: ";    // "CALGARY 1988 Alpine Ski: Downhill MEN. RUN: "; //Downhill
        venueNation = "CAN";
        numbersOfRun = 2;
        surprisesModifier = 0.50f;  //1.00f; // default should be 1.00f, slalom 2.00
        temperatureMin = -11.00f;
        temperatureMax = -4.00f;
        // FAVOURITES:
        Player player01 = new Player("Alberto", "Tomba", 1, 'A', 1, "ITA");
        Player player02 = new Player("Gunther", "Mader", 2, 'B', 2, "AUT");
        Player player03 = new Player("Felix", "McGrath", 3, 'B', 1, "USA");
        Player player04 = new Player("Paul", "Frommelt", 4, 'B', 2, "LIE");
        Player player05 = new Player("Armin", "Bittner", 5, 'B', 1, "FRG");
        Player player06 = new Player("Hubert", "Strolz", 6, 'B', 2, "AUT");
        Player player07 = new Player("Bernhard", "Gstrein", 7, 'B', 1, "AUT");
        Player player08 = new Player("Jonas", "Nilsson", 8, 'B', 2, "SWE");
        Player player09 = new Player("Pirmin", "Zurbriggen", 9, 'C', 3, "SUI");
        Player player10 = new Player("Grega", "Benedik", 10, 'C', 2, "YUG");
        // OUTSIDERS:
        Player player11 = new Player("Tetsuya", "Okabe", 11, 'C', 1, "JPN");
        Player player12 = new Player("Thomas", "Stangassinger", 12, 'C', 1, "AUT");
        Player player13 = new Player("Richard", "Pramotton", 13, 'C', 1, "ITA");
        Player player14 = new Player("Carlo", "Gerosa", 14, 'C', 1, "ITA");
        Player player15 = new Player("Dietmar", "Kohlbichler", 15, 'C', 1, "AUT");
        // UNDERDOGS:
        Player player16 = new Player("Bojan", "Krizaj", 16, 'D', 3, "YUG");
        Player player17 = new Player("Finn Christian", "Jagge", 17, 'E', 0, "NOR");
        Player player18 = new Player("Peter", "Jurko", 18, 'E', 0, "TCH");

        favourites = new List<Player> { player01, player02, player03, player04, player05,
        player06, player07, player08, player09, player10};
        outsiders = new List<Player> { player11, player12, player13, player14, player15 };
        underdogs = new List<Player> { player16, player17, player18 };
        lists = new List<Player>[] { favourites, outsiders, underdogs };
        RandomizeLists(lists);
        bestTimeInSec = 49.42f;         // DH 119.63f; 2 runs: average from 2 best runs
        tenthTime =    50.51f;       // DH 122.69f; przy 15 faworytach to czas 15-go?
        timeDifference = tenthTime - bestTimeInSec;

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

    


}
