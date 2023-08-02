using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Player> favourites;
    Competition competition;
    public TMP_Text competitionName;
    void Start()
    {
        competition = FindObjectOfType<Competition>();
        competitionName.text = "Sarajevo 1984 Alpine Ski";
        Player player01 = new Player("Zurbriggen", 'A', 3);
        Player player02 = new Player("Muller", 'A', 2);
        Player player03 = new Player("Piccard", 'A', 2);
        Player player04 = new Player("Stock", 'B', 1);
        Player player05 = new Player("Pfaffenbichler", 'B', 1);
        Player player06 = new Player("Wasmeier", 'B', 2);
        Player player07 = new Player("Steiner", 'B', 2);
        Player player08 = new Player("Bell", 'B', 3);
        Player player09 = new Player("Girardelli", 'B', 3);
        Player player10 = new Player("Sbardelotto", 'C', 2);

        favourites = new List<Player> { player01, player02, player03, player04, player05,
        player06, player07, player08, player09, player10};
       
    }



    // Update is called once per frame
    void Update()
    {
        
    }

   
}
