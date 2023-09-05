using UnityEngine;
using System.Collections.Generic;

public class Utilities : MonoBehaviour
{
    public GameObject competition;

    // Start is called before the first frame update
    void Start()
    {
        competition = GameObject.FindGameObjectWithTag("competition_script");

    }

     void GenerateDoublets()
    {
        competition.GetComponent<Competition>().firstD6 = 4;
        competition.GetComponent<Competition>().secondD6 = 4;
        Debug.Log("Doublets generated");
    }

}