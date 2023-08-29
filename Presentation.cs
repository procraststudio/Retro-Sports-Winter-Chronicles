using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Competition;

public class Presentation : MonoBehaviour
{

    public Player actualFavourite;
    [SerializeField] private TMP_Text[] descriptionText;
    [SerializeField] private TMP_Text[] favouriteName;
    [SerializeField] private TMP_Text[] favouriteGradeExp;
    [SerializeField] private GameObject[] flagSection;
    Competition competition;
    public string flagsFolderPath = "flags/";
    private string textPath = "comments";
    private Sprite flagSprite;
    private int favouritesGenerated;
    private bool presentationPhaseOver;
    public List<Player> favourites;
    private List<string>[] comments;


    void Start()
    {
        favouritesGenerated = 0;
        actualFavourite = null;
        flagSprite = Resources.Load<Sprite>(flagsFolderPath);
        competition = FindObjectOfType<Competition>();
        favourites = competition.players;
        Debug.Log("START PRESENTATION DONE");
    }

    void Update()
    {
        CheckEndPhase();
    }

    public void GenerateFavourites()
    {
        Debug.Log("CLICK!");
        List<Player> favourites = competition.players;
        if (!presentationPhaseOver ) {
            for (int i = 0; i < favourites.Count; i++)
            {
                if ((favouritesGenerated < 3) && (favourites[i].grade == 'A'))
                {
                    actualFavourite = favourites[i];
                    Debug.Log("FAVOURITE IS: " + actualFavourite.name);
                    showPlayerData(favouritesGenerated, actualFavourite);
                    showComments(favouritesGenerated);
                    // TO DO: Time delay
                    favouritesGenerated++;
                }
            }
           
        }
        else
        {
            competition.myState = GameState.CompetitionPhase;
        }

    }



    public void showPlayerData(int favouriteNo, Player player)
    {
        favouriteName[favouriteNo].text = player.name;
        favouriteGradeExp[favouriteNo].text = "GRADE: " + player.grade.ToString() + "    EXP: " + player.experience.ToString(); ;
        flagSection[favouriteNo].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + player.nationality);
    }

    public void showComments(int player)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');
            if (lines.Length > 0)
            {
                string randomLine = lines[Random.Range(0, lines.Length)];
                descriptionText[player].text = randomLine;  
                Debug.Log("Random Line: " + randomLine);
            }
            else
            {
                Debug.LogError("Text file is empty.");
            }
        }
        else
        {
            Debug.LogError("Text file not found.");
        }
    }

    void CheckEndPhase()
    {
        if (favouritesGenerated>2)
        {
            presentationPhaseOver = true;
        }
    }
}


