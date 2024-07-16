using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompetitionTypeDisplay : MonoBehaviour
{
    public CompetitionType competitionType;
    [SerializeField] TMP_Text competitionName;
    GameStart gameStart;


    void Start()
    {
       competitionName.text = competitionType.competitionName.ToString(); 
        gameStart = FindObjectOfType<GameStart>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartThisCompetition()
    {
       gameStart.StartCompetition(competitionType);
    }
}


