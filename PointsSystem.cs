using TMPro;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{

    public static PointsSystem Instance { get; private set; }

    public int gamePointsTotal = 0;
    public int actualCompetitionGamePoints = 0;
    public int normalPoints;
    public int temporaryPoints = 0;
    public int competitionRecord = 0;
    [SerializeField] TMP_Text normalPointsTotal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            competitionRecord = PlayerPrefs.GetInt("competitionRecord");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //gamePointsTotal = 0;
        // actualCompetitionGamePoints = 0;
        // normalPoints = 0;
        // comboPoints = 0;
        //competitionRecord=0;
    }

    public void AddGamePoints(int normalPts)
    {
        actualCompetitionGamePoints += normalPts;
        normalPointsTotal.text = actualCompetitionGamePoints.ToString();
    }

    public bool CheckRecords()
    {
        gamePointsTotal += actualCompetitionGamePoints;
        if (actualCompetitionGamePoints > competitionRecord)
        {
            competitionRecord = actualCompetitionGamePoints;
            PlayerPrefs.SetInt("competitionRecord", competitionRecord);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetCompetitionPoints()
    {
        actualCompetitionGamePoints = 0;
    }

    public void AddTemporaryPoints(int tempPoints)
    {
        temporaryPoints += tempPoints;
    }


    public void ResetTemporaryPoints()
    {
        temporaryPoints = 0;
    }
}
