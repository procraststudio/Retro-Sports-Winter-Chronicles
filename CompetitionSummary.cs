using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompetitionSummary : MonoBehaviour
{
    PointsSystem pointsSystem;
    AchievementsManager achievements;
    Competition competition;
    Gamemanager gamemanager;
    private bool decorationSpawned = false;
    [SerializeField] GameObject ExitButton;
    [SerializeField] public GameObject NextWorldCupButton;
    [SerializeField] private GameObject DecorationPanel;
    [SerializeField] private GameObject PointsPanel;
    [SerializeField] private GameObject AchievementsPanel;
    [SerializeField] TMP_Text gamePointsScored;
    [SerializeField] TMP_Text newRecordIndicator;
    [SerializeField] TMP_Text diceCombosInfo;


    void Start()
    {
        PointsPanel.SetActive(false);
        pointsSystem = PointsSystem.Instance;
        competition = Competition.Instance;
        achievements = AchievementsManager.Instance;
        gamemanager = FindObjectOfType<Gamemanager>();
    }

    void Update()
    {
        // ShowGainedPoints();
    }

    public void DoDecoration()
    {
        if (!decorationSpawned)
        {
            // TODO: FINAL COMMENTS appear
            //ChangeState(GameState.SummaryPhase);
            GameObject newObject = Instantiate(DecorationPanel);
            newObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            newObject.transform.localPosition = new Vector3(7.17f, 122.00f, 0.00f);
            decorationSpawned = true;
        }
        // ShowGainedPoints();
    }

    public void ShowGainedPoints()
    {
        PointsPanel.SetActive(true);
        gamePointsScored.text = pointsSystem.actualCompetitionGamePoints.ToString();
        if (pointsSystem.CheckRecords() == true)
        {
            newRecordIndicator.text = "NEW RECORD!";
            SoundManager.PlayOneSound("dice_combo");
        }
        else
        {
            newRecordIndicator.text = "";
        }

        ShowGainedCombos();
        ExitButton.SetActive(true);
        if ((gamemanager.actualWorldCupCompetition!=null) &&(gamemanager.IsNextWorldCupEventPossible()))
        {
            NextWorldCupButton.SetActive(true);
        }
        else
        {
            NextWorldCupButton.SetActive(false);
        }
        achievements.CheckAchievements();
        AchievementsPanel.SetActive(true);

    }

    public void ShowGainedCombos()
    {
        diceCombosInfo.text += "DOUBLES: " + achievements.actualCompetitionDoubles.ToString();
        if (achievements.actualCompetitionTriples > 0)
        {
            diceCombosInfo.text += ". TRIPLES: " + achievements.actualCompetitionTriples.ToString();
        }
        if (achievements.actualCompetitionStraights > 0)
        {
            diceCombosInfo.text += ". STRAIGHTS: " + achievements.actualCompetitionStraights.ToString();
        }
        if (achievements.actualCompetitionHatTricks > 0)
        {
            diceCombosInfo.text += ". HAT TRICKS: " + achievements.actualCompetitionHatTricks.ToString();
        }
    }


    public void GoToMenu()
    {
        ResetActualCompetitionData();
        ExitButton.SetActive(false);
        NextWorldCupButton.SetActive(false);
        SceneManager.LoadScene(0);

    }

    public void ResetActualCompetitionData()
    {
        achievements.ResetActualCompetitionCombos();
        PlayerPrefs.DeleteKey("currentWorldCupNumber");
        PlayerPrefs.Save();
    }

}
