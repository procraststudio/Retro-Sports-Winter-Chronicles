using TMPro;
using UnityEngine;


public class PlayerDisplay : MonoBehaviour
{
    public Player player;
    [SerializeField] TMP_Text competitorName;
    [SerializeField] TMP_Text competitorGrade;
    [SerializeField] TMP_Text competitorExperience;
    [SerializeField] TMP_Text competitorRanking;
    public TMP_Text secondName;
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
        //player = null;
        competition = Competition.Instance;
       // formIndicator.GetComponent<SpriteRenderer>().sprite = null;
        //competitorName.text = "";
       // competitorGrade.text = "";
       // competitorExperience.text = "";
        flagSprite = Resources.Load<Sprite>(flagsFolderPath);
    }

    public void Update()
    {
       // if (competition.currentCompetitor != null)
       // {
           // player = competition.currentCompetitor;
           // DisplayCompetitor(player);
      //  }
    }


    public void DisplayCompetitor(Player player)
    {
        string boldSecondName = "<b>"+player.secondName+"</b>";
        competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
        competitorGrade.text = player.grade.ToString();
        competitorExperience.text = player.experience.ToString();
        competitorRanking.text = player.ranking.ToString();
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
        playerFlag.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + player.nationality);
    }
}
