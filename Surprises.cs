using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Surprises : MonoBehaviour
{
    public int surpriseChance { get; set; }
    void Start()
    {
       
        surpriseChance  =  Random.Range(13, 81) + FindObjectOfType<Gamemanager>().surprisesModifier;
        Debug.Log("SURPRISE CHANCE TODAY: "+surpriseChance);
    }

    // Update is called once per frame



    public bool IsSurprise(Player player)
    {
        int surpriseRoll = Random.Range(1, 100);
        if ((surpriseRoll) <= (surpriseChance))
        {
            int surpriseChance = Random.Range(1, 100);
            if (((player.ranking * 3) + (surpriseChance)) > 75)
            {
                Debug.Log("SURPRISE!");
                return true;
            }
            else { Debug.Log("THAT WAS CLOSE...");
                 return false; }

        }
        else {
            Debug.Log("NO SURPRISE");
            return false; }
    }


    }

