using TMPro;
using UnityEngine;
using static Competition;


public class PlayerDisplay : MonoBehaviour
{
    public Player player;
    [SerializeField] TMP_Text competitorName;
    [SerializeField] TMP_Text competitorGrade;
    [SerializeField] TMP_Text competitorExperience;
    [SerializeField] TMP_Text competitorRanking;
    [SerializeField] TMP_Text position;
    [SerializeField] TMP_Text timeDisplay;
    public TMP_Text secondName;
    [SerializeField] GameObject background;
    [SerializeField] GameObject currentCompetitorPanel;
    [SerializeField] GameObject formIndicator;
    [SerializeField] GameObject arrowIndicator;
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
        //currentCompetitorPanel.SetActive(false);
    }

    public void Update()
    {
        if ((currentCompetitorPanel != null) && ((competition.partsOfRun == 1) || (competition.partsOfRun == 2)||
             competition.myState == GameState.PresentationPhase))
        {
            currentCompetitorPanel.SetActive(false);
        }
    }


    public void DisplayCompetitor(Player player)
    {
        competition = Competition.Instance;
        string boldSecondName = "<b>" + player.secondName + "</b>";
        // currentCompetitorPanel.SetActive(false);
        // float pointsDifference = competition.bestFinalPerformance - player.finalPerformance;
        // FINISHERS
        if (gameObject.CompareTag("finishers_list"))
        {
            float pointsDifference = competition.bestFinalPerformance - player.finalPerformance;
            position.text = player.place.ToString();
            ShowFlag(player);
            arrowIndicator = null;
            competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
            if (player.place == 1)
            {
                timeDisplay.text = player.ConvertPointsToTime(player.finalPerformance).ToString();
            }
            else
            {
                timeDisplay.text = player.ConvertDifference(pointsDifference).ToString();
            }

            HighlightCurrentCompetitor(player);
            Debug.Log(competition.bestFinalPerformance - player.finalPerformance);
        }
        else if (gameObject.CompareTag("losers"))
        {
            ShowFlag(player);
            competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
            HighlightCurrentCompetitor(player);
        }

        else
        {
            competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
            competitorGrade.text = player.grade.ToString();
            competitorExperience.text = player.experience.ToString();
            competitorRanking.text = player.ranking.ToString();
            ShowFormIndicators(player);
            ShowFlag(player);
            //HighlightCurrentCompetitor(player);
        }

        // TO DO: arrow display in 2nd run
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

    public void HighlightCurrentCompetitor(Player player)
    {
        //competition = Competition.Instance;
        if ((currentCompetitorPanel != null) && (competition.myState == GameState.CompetitionPhase))
        {
            if (player.secondName == competition.currentCompetitor.secondName)
            {
                currentCompetitorPanel.SetActive(true);

            }
            else
            {
                currentCompetitorPanel.SetActive(false);
            }
        }
    }
}
