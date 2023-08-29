using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gamemanager : MonoBehaviour
{
    public List<Player> favourites, outsiders, underdogs;
    public List<Player>[] lists;

    Competition competition;
    public String competitionName;
    public float surprisesModifier;
    public int numberOfFavourites { get; set; }
    public float bestTimeInSec { get; private set; } // Time in secs
    private float tenthTime; // Time of 10th competitor (in secs)
    public float timeDifference;
    public string venueNation { get; private set; } 
    public float temperatureMin { get; private set; }
    public float temperatureMax { get; private set; }

void Start()
    {
        competition = FindObjectOfType<Competition>();
        competitionName = "CALGARY 1988 Alpine Ski: Downhill MEN"; //Downhill
        venueNation = "CAN";
        surprisesModifier = 3.00f;
        temperatureMin = -11.00f;
        temperatureMax = -4.00f;
        // FAVOURITES:
        Player player01 = new Player("Zurbriggen", 1, 'A', 3, "SUI");
        Player player02 = new Player("Muller", 2, 'A', 2, "SUI" );
        Player player03 = new Player("Piccard", 3, 'A', 2, "FRA");
        Player player04 = new Player("Stock", 4, 'B', 1, "AUT");
        Player player05 = new Player("Pfaffenbichler", 5, 'B', 1, "AUT");
        Player player06 = new Player("Wasmeier", 6, 'B', 2, "FRG");
        Player player07 = new Player("Steiner", 7, 'B', 2, "AUT");
        Player player08 = new Player("Bell", 8, 'B', 3, "GBR");
        Player player09 = new Player("Girardelli", 9, 'B', 3, "LUX");
        Player player10 = new Player("Sbardelotto", 10, 'C', 2, "ITA");
        // OUTSIDERS:
        Player player11 = new Player("Boyd", 11, 'C', 1, "CAN");
        Player player12 = new Player("Heinzer", 12, 'C', 1, "SUI");
        Player player13 = new Player("Belczyk", 13, 'D', 2, "CAN");
        Player player14 = new Player("Mader", 14, 'D', 2, "AUT");
        Player player15 = new Player("Tauscher", 15, 'D', 3, "FRG");
        // UNDERDOGS:
        Player player16 = new Player("Henning", 16, 'E', 1, "SWE");
        Player player17 = new Player("Bires", 17, 'E', 0, "TCH");
        Player player18 = new Player("Birkner", 18, 'E', 0, "ARG");

        favourites = new List<Player> { player01, player02, player03, player04, player05,
        player06, player07, player08, player09, player10};
        outsiders = new List<Player> { player11, player12, player13, player14, player15 };
        underdogs = new List<Player> { player16, player17, player18 };
        lists = new List<Player>[] { favourites, outsiders, underdogs };
        RandomizeLists(lists);
        numberOfFavourites = favourites.Count;
        Debug.Log("FAVOURITES: " + numberOfFavourites);
        bestTimeInSec = 119.63f;
        tenthTime = 122.69f;
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
