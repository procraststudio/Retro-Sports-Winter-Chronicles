using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float bestTimeInSec { get;  set; } 
    public float bestOverallTime { get; set; }
    public float tenthTime; // Time of 10th competitor (in secs)
    public float timeDifference;
    public static int numbersOfRun;
    public string venueNation { get; private set; } 
    public float temperatureMin { get; private set; }
    public float temperatureMax { get; private set; }
    public VenueLoader venueLoader;
    public CompetitionType competitionType ;

void Start()
    {
        competition = Competition.Instance;
        competitionType = new CompetitionType {competitionName ="Slalom Women", competitionDate = new DateTime (1986,2,23), competitionVenueName ="Sestriere"  };

       // typeOfCompetition = "alpine skiing";
        // TODO: select competition type, number of runs, surprises modifier, ??number of competitors
       // competitionName = "1988 Alpine Ski: Downhill WOMEN. RUN: ";    // "CALGARY 1988 Alpine Ski: Downhill MEN. RUN: "; //Downhill
        //venueNation = "CAN";
        numbersOfRun = 2;
        surprisesModifier = 1.00f;//1.00f; // default should be 1.00f, slalom: 2.00
   
        // FAVOURITES:
        Player player01 = new Player("Michela", "Figini", 1, 'X', 2, "SUI");
        Player player02 = new Player("Brigitte", "Oertli", 2, 'X', 2, "SUI");
        Player player03 = new Player("Maria", "Walliser", 3, 'A', 2, "USA");
        Player player04 = new Player("Veronika", "Wallinger", 4, 'B', 2, "AUT");
        Player player05 = new Player("Karen", "Percy", 5, 'B', 1, "CAN");
        Player player06 = new Player("Sigrid", "Wolf", 6, 'B', 2, "AUT");
        Player player07 = new Player("Beatrice", "Gafner", 7, 'B', 0, "SUI");
        Player player08 = new Player("Marina", "Kiehl", 8, 'B', 2, "FRG");
        Player player09 = new Player("Regine", "Moesenlechner", 9, 'C', 3, "FRG");
        Player player10 = new Player("Petra", "Kronberger", 10, 'C', 0, "AUT");
        // OUTSIDERS:
        Player player11 = new Player("Kerrin", "Lee", 11, 'C', 1, "CAN");
        Player player12 = new Player("Laurie", "Graham", 12, 'C', 3, "CAN");
        Player player13 = new Player("Katrin", "Gutensohn", 13, 'C', 1, "AUT");
        Player player14 = new Player("Chantal", "Bournissen", 14, 'C', 1, "SUI");
        Player player15 = new Player("Elisabeth", "Kirchler", 15, 'C', 2, "AUT");
        // UNDERDOGS:
        Player player16 = new Player("Deborah", "Compagnoni", 16, 'D', 0, "ITA");
        Player player17 = new Player("Golnur", "Postnikova", 17, 'E', 0, "URS");
        Player player18 = new Player("Carole", "Merle", 18, 'E', 2, "FRA");

        favourites = new List<Player> { player01, player02, player03, player04, player05,
        player06, player07, player08, player09, player10};
        outsiders = new List<Player> { player11, player12, player13, player14, player15 };
        underdogs = new List<Player> { player16, player17, player18 };
        lists = new List<Player>[] { favourites, outsiders, underdogs };
        RandomizeLists(lists);
        bestTimeInSec = 49.42f;         // DH 119.63f; 2 runs: average from 2 best runs
        tenthTime =    50.51f;       // DH 122.69f; przy 15 faworytach to czas 15-go?
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
