using DG.Tweening;
using System.Collections;
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
    [SerializeField] public GameObject WorldCupSummaryButton;
    [SerializeField] private GameObject DecorationPanel;
    [SerializeField] private GameObject PointsPanel;
    [SerializeField] private GameObject AchievementsPanel;
    [SerializeField] TMP_Text gamePointsScored;
    [SerializeField] TMP_Text newRecordIndicator;
    [SerializeField] TMP_Text diceCombosInfo;
    private GameObject spawnedDecorationPanel;
    public ScoreCounter scoreCounter;


    void Start()
    {
        PointsPanel.SetActive(false);
        pointsSystem = PointsSystem.Instance;
        competition = Competition.Instance;
        achievements = AchievementsManager.Instance;
        gamemanager = FindObjectOfType<Gamemanager>();
        spawnedDecorationPanel = new GameObject();
    }

    void Update()
    {

    }

    public void DoDecoration()
    {
        if (!decorationSpawned)
        {
            // TODO: FINAL COMMENTS appear
            //ChangeState(GameState.SummaryPhase);
            GameObject newObject = Instantiate(DecorationPanel);
            spawnedDecorationPanel = newObject;
            newObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            newObject.transform.localPosition = new Vector3(7.17f, 122.00f, 0.00f);
            decorationSpawned = true;
        }
        // ShowGainedPoints();
    }

    public void ShowGainedPoints()
    {
        PointsPanel.SetActive(true);
        //gamePointsScored.text = pointsSystem.actualCompetitionGamePoints.ToString();
        // ADD SPECIAL EFFECT
        PointsPanel.GetComponent<ScoreCounter>().AddScore(pointsSystem.actualCompetitionGamePoints);
        StartCoroutine("CheckRecords");
        ShowGainedCombos();
        ExitButton.SetActive(true);
        if ((Gamemanager.actualWorldCupCompetition != null) && (gamemanager.IsNextWorldCupEventPossible()))
        {
            NextWorldCupButton.SetActive(true);
        }
        else
        {
            NextWorldCupButton.SetActive(false);
        }
        achievements.CheckAchievements();
        AchievementsPanel.SetActive(true);
        CheckWorldCupFinalDecoration();

    }

    private IEnumerator CheckRecords()
    {
        yield return new WaitForSeconds(3f);
        if (pointsSystem.CheckRecords() == true)
        {
            newRecordIndicator.text = "NEW RECORD!";
            SoundManager.PlayOneSound("dice_combo");
            newRecordIndicator.GetComponentInParent<RectTransform>().DOShakePosition(1f, 20.0f, 3, 4f, true, true);
            StartCoroutine("TextChangingColor");
        }
        else
        {
            newRecordIndicator.text = "";
        }
    }

    private IEnumerator TextChangingColor()
    {
        while (newRecordIndicator.text != null)
        {
            yield return new WaitForSeconds(0.5f);
            newRecordIndicator.GetComponent<TMP_Text>().DOBlendableColor(Color.yellow, 0.1f);
            yield return new WaitForSeconds(0.5f);
            newRecordIndicator.GetComponent<TMP_Text>().DOBlendableColor(Color.white, 0.1f);
        }
    }

    public void ShowGainedCombos()
    {
        diceCombosInfo.text += "DOUBLES: " + "<color=white>" + achievements.actualCompetitionDoubles.ToString() + "<color=white>";
        if (achievements.actualCompetitionTriples > 0)
        {
            diceCombosInfo.text += ". TRIPLES: " + "<color=yellow>" + achievements.actualCompetitionTriples.ToString() + "<color=yellow>";
        }
        if (achievements.actualCompetitionStraights > 0)
        {
            diceCombosInfo.text += ". STRAIGHTS: " + "<color=blue>" + achievements.actualCompetitionStraights.ToString() + "<color=blue>";
        }
        if (achievements.actualCompetitionHatTricks > 0)
        {
            diceCombosInfo.text += ". HAT TRICKS: " + "<color=green>" + achievements.actualCompetitionHatTricks.ToString() + "<color=green>";
        }
    }

    public void CheckWorldCupFinalDecoration()
    {
        if ((Gamemanager.actualWorldCupCompetition != null) && (!NextWorldCupButton.activeInHierarchy))
        {
            // summon button Decorate WC SUMMARY
            WorldCupSummaryButton.SetActive(true);
            //Destroy(DecorationPanel.gameObject);
        }
    }

    public void DoDecorationButton()
    {
        //FindObjectOfType < "Decoration_panel" > ().first;
        FindObjectOfType<Decoration>().WorldCupFinalPodium();
        WorldCupSummaryButton.SetActive(false);
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
