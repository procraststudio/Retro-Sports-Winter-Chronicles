using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;

public class Gamemanager : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Player> favourites, outsiders;
    public List<Player>[] lists;

    Competition competition;
    public String competitionName;
    public int surprisesModifier; 
    void Start()
    {
        competition = FindObjectOfType<Competition>();
        competitionName = "CALGARY 1988 Alpine Ski: Downhill";
        surprisesModifier = 0;
        Player player01 = new Player("Zurbriggen", 1, 'A', 3);
        Player player02 = new Player("Muller", 2, 'A', 2);
        Player player03 = new Player("Piccard", 3, 'A', 2);
        Player player04 = new Player("Stock", 4, 'B', 1);
        Player player05 = new Player("Pfaffenbichler", 5, 'B', 1);
        Player player06 = new Player("Wasmeier", 6, 'B', 2);
        Player player07 = new Player("Steiner",7, 'B', 2);
        Player player08 = new Player("Bell", 8, 'B', 3);
        Player player09 = new Player("Girardelli", 9, 'B', 3);
        Player player10 = new Player("Sbardelotto", 10, 'C', 2);
        // OUTSIDERS
        Player player11 = new Player("Boyd", 11, 'C', 1);
        Player player12 = new Player("Heinzer",12, 'C', 1);
        Player player13 = new Player("Belczyk", 13, 'D', 2);
        Player player14 = new Player("Mader", 14, 'D', 2);
        Player player15 = new Player("Tauscher", 15, 'D', 3);

        favourites = new List<Player> { player01, player02, player03, player04, player05,
        player06, player07, player08, player09, player10};
        outsiders = new List<Player> { player11, player12, player13, player14, player15};
        lists = new List<Player>[] {favourites, outsiders };
        RandomizeLists(lists);
       
    }


    void Update()
    {
        
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
