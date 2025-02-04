using TMPro;
using UnityEngine;
using static Competition;


public class PlayerDisplay : MonoBehaviour
{
    public Player player;
    public Player playerLoaded;
    [SerializeField] public TMP_Text competitorName;
    [SerializeField] public TMP_Text competitorGrade;
    [SerializeField] public TMP_Text competitorExperience;
    [SerializeField] public TMP_Text competitorRanking;
    [SerializeField] public TMP_Text competitorStatus;
    [SerializeField] TMP_Text competitorSurpriseChance;
    [SerializeField] TMP_Text position;
    [SerializeField] TMP_Text timeDisplay;
    [SerializeField] TMP_Text worldCupPoints;
    [SerializeField] TMP_Text worldCupPlace;
    [SerializeField] TMP_Text currentWorldCupPoints;
    //[SerializeField] TMP_Text worldCupPosition;
    public TMP_Text secondName;
    [SerializeField] GameObject background;
    [SerializeField] GameObject[] backgrounds;
    //[SerializeField] GameObject imageComponent;
    [SerializeField] GameObject currentCompetitorPanel;
    [SerializeField] GameObject formIndicator;
    [SerializeField] GameObject arrowIndicator;
    [SerializeField] public GameObject playerFlag;
    [SerializeField] public GameObject headGraphic;
    [SerializeField] public GameObject injurySymbol;
    [SerializeField] Sprite[] formIndicators;
    [SerializeField] Sprite[] headImages;
    //public TextMeshProUGUI nationalityText; 
    public SpriteRenderer flagRenderer;
    public string flagsFolderPath = "flags/";
    private Sprite flagSprite;
    Competition competition;
    Surprises surprise;
    private bool headsDisplayChecked = false;
    Gamemanager gamemanager;
    private bool resultInMetres;
    private bool wayOfPointsDisplayChecked = false;

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

    private void CheckPerformanceDisplay()
    {
        if (Gamemanager.GetCompetitionType().resultsInMetres == true)
        {
            resultInMetres = true;
        }
        else
        {
            resultInMetres = false;
        }
    }


    public void DisplayCompetitor(Player player, int actualRun)
    {
        playerLoaded = player;
        competition = Competition.Instance;
        if (!wayOfPointsDisplayChecked)
        {
            CheckPerformanceDisplay();
            wayOfPointsDisplayChecked = true;
        }
        DisplayHeadImage();

        string boldSecondName = "<b>" + player.secondName + "</b>";
        if ((gameObject.CompareTag("finishers_list")) || (gameObject.CompareTag("firstRun_list")) ||
            (gameObject.CompareTag("secondRun_list")) || (gameObject.CompareTag("hoverCard")))
        {
            ShowPosition(player);
            ShowFlag(player);
            ShowStatus(player);

            arrowIndicator = null;
            competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
            if (resultInMetres == false)
            {
                TimeDisplay(player);
            }
            else if (resultInMetres == true)
            {
                PointsDisplay(player);
            }
            HighlightCurrentCompetitor(player);
        }
        else if (gameObject.CompareTag("losers"))
        {
            ShowFlag(player);
            competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
            HighlightCurrentCompetitor(player);
            ShowInjuryStatus(player);
        }

        else if (gameObject.CompareTag("worldcup_list"))
        {
            if ((player.worldCupPoints > 0) && (player.worldCupPlace < 16))
            {
                ShowFlag(player);
                ShowPosition(player);
                //TO DO: IF points equal lower player position blank
                competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
                worldCupPoints.text = player.worldCupPoints.ToString();
                if (player.currentWorldCupPoints > 0)
                {
                    currentWorldCupPoints.text = "+" + player.currentWorldCupPoints.ToString();
                }
                else
                {
                    return;

                }
            }
            else { this.gameObject.SetActive(false); }
        }


        else
        {
            competitorName.text = player.surname.ToString() + " " + boldSecondName.ToUpper() + "  " + player.nationality;
            competitorGrade.text = player.grade.ToString();
            competitorExperience.text = player.experience.ToString();
            competitorRanking.text = player.ranking.ToString();
            if ((worldCupPlace != null) && (player.worldCupPlace > 0))
            {
                worldCupPlace.text = player.worldCupPlace.ToString();
            }
            ShowFormIndicators(player);
            ShowFlag(player);
            if (competitorSurpriseChance != null)
            {
                competitorSurpriseChance.text = surprise.DisplaySurpriseChance(player).ToString(); //("F0"); // + "%"
            }
        }

    }

    public void ShowFormIndicators(Player player)
    {
        //Color color = new Color;
        if (player.goodFormEffect)
        {
            backgrounds[0].SetActive(false);
            backgrounds[2].SetActive(true);
            backgrounds[1].SetActive(false);
            //formIndicator.GetComponent<SpriteRenderer>().sprite = formIndicators[0];
        }
        else if (player.poorFormEffect)
        {
            backgrounds[0].SetActive(false);
            backgrounds[2].SetActive(false);
            backgrounds[1].SetActive(true);
            //formIndicator.GetComponent<SpriteRenderer>().sprite = formIndicators[1];
        }
        else
        {
            //formIndicator.GetComponent<SpriteRenderer>().sprite = null;
            backgrounds[0].SetActive(true);
            backgrounds[1].SetActive(false);
            backgrounds[2].SetActive(false);
        }
    }

    public void ShowPosition(Player player)
    {
        if (gameObject.CompareTag("firstRun_list"))
        {
            position.text = player.firstRunPlace.ToString();
        }
        else if (gameObject.CompareTag("secondRun_list"))
        {
            position.text = player.secondRunPlace.ToString();
        }
        else if (gameObject.CompareTag("worldcup_list"))
        {
            position.text = player.worldCupPlace.ToString();
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
                timeDisplay.text = player.ConvertPointsToTime(player.firstRunPoints, "firstRunPoints").ToString();
            }
            else
            {
                timeDisplay.text = player.ConvertDifference(competition.bestFirstRunPerformance - player.firstRunPoints).ToString();
            }
        }
        else if (gameObject.CompareTag("secondRun_list"))
        {
            if (player.secondRunPlace == 1)
            {
                timeDisplay.text = player.ConvertPointsToTime(player.finalPerformance - player.firstRunPoints, "secondRunPoints").ToString();
            }
            else
            {
                timeDisplay.text = player.ConvertDifference(competition.bestSecondRunPerformance - player.secondRunPoints).ToString();
            }
        }

        else
        {
            if ((player.place == 1) && (competition.currentRun < 2))
            {
                timeDisplay.text = player.ConvertPointsToTime(player.firstRunPoints, "firstRunPoints").ToString();
            }
            else if ((player.place == 1) && (competition.currentRun > 1))
            {
                timeDisplay.text = player.ConvertPointsToTime(player.finalPerformance, "finalPoints").ToString();
            }
            else
            {
                timeDisplay.text = player.ConvertDifference(pointsDifference).ToString();
            }

        }
    }

    public void PointsDisplay(Player player)
    {
        // timeDisplay.text = player.finalPerformance.ToString();
        if (gameObject.CompareTag("firstRun_list"))
        {
            //timeDisplay.text = player.firstRunPoints.ToString("F1");
            timeDisplay.text = player.firstRunDistance.ToString("F1");
        }
        else if (gameObject.CompareTag("secondRun_list"))
        {
            // timeDisplay.text = player.secondRunPoints.ToString("F1");
            timeDisplay.text = player.secondRunDistance.ToString("F1");
        }
        else
        {
            if (competition.currentRun < 2)
            {
                // timeDisplay.text = player.firstRunPoints.ToString("F1");
                timeDisplay.text = player.firstRunDistance.ToString("F1");
            }
            else
            {
                timeDisplay.text = player.skiJumpingPoints.ToString("F1");
            }
        }
    }


    public void ShowFlag(Player player)
    {
        playerFlag.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + player.nationality);
    }
    public void ShowStatus(Player player)
    {
        if (competitorStatus != null)
        {
            competitorStatus.text = player.myState.ToString();
        }
    }

    public void ShowInjuryStatus(Player player)
    {
        if (player.isInjured)
        {
            injurySymbol.SetActive(true);
        }
        else
        {
            injurySymbol.SetActive(false);
        }
    }


    public void HighlightCurrentCompetitor(Player player)
    {
        if ((currentCompetitorPanel != null) && (competition.myState != GameState.SummaryPhase)
            && (competition.myState != GameState.EndOfRun))

        {
            if ((player.secondName == competition.currentCompetitor.secondName) && (player.surname == competition.currentCompetitor.surname))
            {
                currentCompetitorPanel.SetActive(true);
            }
            else
            {
                currentCompetitorPanel.SetActive(false);
            }
        }
    }

    public void DisplayHeadImage()
    {
        if ((!headsDisplayChecked) && (headGraphic != null))
        {

            if (competition.gamemanager.competitionName.Contains("Men"))
            {
                headGraphic.GetComponent<SpriteRenderer>().sprite = headImages[0];
            }
            else
            {
                headGraphic.GetComponent<SpriteRenderer>().sprite = headImages[1];
            }
            headsDisplayChecked = true;

        }
    }

}
