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
    [SerializeField] TMP_Text competitorSurpriseChance;
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
    Surprises surprise;


    public void Start()
    {
        competition = Competition.Instance;
        flagSprite = Resources.Load<Sprite>(flagsFolderPath);
        surprise = FindObjectOfType<Surprises>();
    }

    public void Update()
    {
        if ((currentCompetitorPanel != null) && ((competition.partsOfRun == 1) || (competition.partsOfRun == 2) ||
             competition.myState == GameState.PresentationPhase))
        {
            currentCompetitorPanel.SetActive(false);
        }
    }


    public void DisplayCompetitor(Player player, int actualRun)
    {
        competition = Competition.Instance;
        string boldSecondName = "<b>" + player.secondName + "</b>";
        if ((gameObject.CompareTag("finishers_list")) || (gameObject.CompareTag("firstRun_list")))
        {
            ShowPosition(player);
            ShowFlag(player);
            arrowIndicator = null;
            competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
            TimeDisplay(player);
            HighlightCurrentCompetitor(player);
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
            if (competitorSurpriseChance != null)
            {
                competitorSurpriseChance.text = player.ranking.ToString(); //surprise.DisplaySurpriseChance(player).ToString(); //("F0"); // + "%"
            }
        }

    }

    public void ShowFormIndicators(Player player)
    {

        if (player.goodFormEffect)
        {
            formIndicator.GetComponent<SpriteRenderer>().sprite = formIndicators[0];
        }
        else if (player.poorFormEffect)
        {
            formIndicator.GetComponent<SpriteRenderer>().sprite = formIndicators[1];
        }
        else
        {
            formIndicator.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void ShowPosition(Player player)
    {
        if (gameObject.CompareTag("firstRun_list"))
        {
            position.text = player.firstRunPlace.ToString();
        }
        else
        {
            position.text = player.place.ToString();
        }
    }

    public void TimeDisplay(Player player)
    {
        float pointsDifference = competition.bestFinalPerformance - player.finalPerformance;

        if (gameObject.CompareTag("firstRun_list"))
        {
            if (player.firstRunPlace == 1)
            {
                timeDisplay.text = player.ConvertPointsToTime(player.firstRunPoints).ToString();
            }
            else
            {
                timeDisplay.text = player.ConvertDifference(competition.bestFirstRunPerformance - player.firstRunPoints).ToString();
            }
        }

        else
        {
            if (player.place == 1)
            {
                timeDisplay.text = player.ConvertPointsToTime(player.finalPerformance).ToString();
            }
            else
            {
                timeDisplay.text = player.ConvertDifference(pointsDifference).ToString();
            }

        }
    }

        public void ShowFlag(Player player)
        {
            playerFlag.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + player.nationality);
        }

        public void HighlightCurrentCompetitor(Player player)
        {
            if ((currentCompetitorPanel != null) && (competition.myState != GameState.DecorationPhase)
                && (competition.myState != GameState.EndOfRun))

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
