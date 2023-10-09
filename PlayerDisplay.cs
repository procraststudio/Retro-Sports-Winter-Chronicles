using TMPro;
using UnityEngine;
using System.Collections.Generic;


public class PlayerDisplay : MonoBehaviour
{
    public Player player;
    [SerializeField] TMP_Text competitorName;
    [SerializeField] GameObject formIndicator;
    [SerializeField] GameObject playerFlag;
    [SerializeField] Sprite[] formIndicators;
    //public TextMeshProUGUI nationalityText; 
    public SpriteRenderer flagRenderer;
    public string flagsFolderPath = "flags/";
    private Sprite flagSprite;
    Competition competition;


    public void Start()
    {
        player = null; 
        competition = FindObjectOfType<Competition>();
        formIndicator.GetComponent<SpriteRenderer>().sprite = null;
        competitorName.text = "";
        flagSprite = Resources.Load<Sprite>(flagsFolderPath);
    }


    public void DisplayCompetitor(Player player)
    {   
            competitorName.text = player.name;  
            ShowFormIndicators(player);
            ShowFlag(player);
    }

    public void ShowFormIndicators(Player player)
    {
        // formIndicator.GetComponent<SpriteRenderer>().sprite = null;

        if (player.goodFormEffect)
        {
            formIndicator.GetComponent<SpriteRenderer>().sprite = formIndicators[0];
            // Debug.Log("SPRITE CHANGED");

        }
        else if (player.poorFormEffect)
        {
            formIndicator.GetComponent<SpriteRenderer>().sprite = formIndicators[1];
            // Debug.Log("SPRITE CHANGED");

        }
        else
        {
            formIndicator.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void ShowFlag(Player player)
    {
        //if (flagSprite != null)
        //{
        playerFlag.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + player.nationality);
        // }
        //else
        //{ 
        // Debug.LogWarning("Flag not found.");
        //}

    }
}
