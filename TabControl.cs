using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabControl : MonoBehaviour
{
    Competition competition;
    Gamemanager gamemanager;
    [SerializeField] GameObject tabLayer;
    public GameObject[] tabButtons;
    [SerializeField] GameObject[] listsToShow;
    [SerializeField] TMP_Text[] namesOfTabButtons;


    void Start()
    {
       // tabLayer.SetActive(false);
        gamemanager = FindObjectOfType<Gamemanager>();
    }

    void Update()
    {
        ChangeButtonsNames();
    }

    public void ChangeButtonsNames()
    {
        if (Gamemanager.GetCompetitionType().IsCombined == true)
        {
            namesOfTabButtons[0].text = Gamemanager.GetCompetitionType().firstCombinedCompetition.ToString();
            namesOfTabButtons[1].text = Gamemanager.GetCompetitionType().secondCombinedCompetition.ToString();
        }
    }
}
