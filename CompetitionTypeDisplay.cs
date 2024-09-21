using TMPro;
using UnityEngine;

public class CompetitionTypeDisplay : MonoBehaviour
{
    public CompetitionType competitionType;
    public WorldCupCompetition worldCupCompetition;
    [SerializeField] TMP_Text competitionName;
    GameStart gameStart;
    public int worldCupIndex;


    void Start()
    {
        if (competitionType != null)
        {
            competitionName.text = competitionType.competitionName.ToString();
        }
        if (worldCupCompetition != null)
        {
            competitionName.text = worldCupCompetition.worldCupName.ToString();
        }
        gameStart = FindObjectOfType<GameStart>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartThisCompetition()
    {
        if (competitionType != null)
        {
            GameStart.StartCompetition(competitionType);

        }
        if (worldCupCompetition != null)
        {
           // GameStart.currentWorldCup = GameStart.availableWorldCups[0];
            worldCupIndex = 1;
            PlayerPrefs.SetInt("currentWorldCupNumber", worldCupIndex);
            GameStart.StartWorldCup(worldCupCompetition);   
            

        }
    }



    }


