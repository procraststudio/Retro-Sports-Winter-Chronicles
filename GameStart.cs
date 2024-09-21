using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject SportsPanel;
    [SerializeField] GameObject WinterGamesPanel;
    [SerializeField] GameObject DisciplinesPanel;
    [SerializeField] GameObject WorldCupPanel;
    [SerializeField] GameObject WorldCupDisciplinesPanel;
    public RectTransform rectTransform;
    public List<GameObject> winterGames = new List<GameObject>();
    public bool winterOlympicsMode = false;
    public bool worldCupMode = false;
    public CanvasGroup canvasGroup;
    [SerializeField] public CompetitionType[] availableCompetitions;
    [SerializeField] public static WorldCupCompetition[] availableWorldCups;
    public static CompetitionType currentCompetition;
    public static WorldCupCompetition currentWorldCup;
    public gameModes actualGameMode;

    public enum gameModes
    {
        noMode,
        casualMode,
        worldCupMode,
        olympicsMode,
        worldChampionshipMode
    }


    void Start()
    {
        SportsPanel.SetActive(false);
        DisciplinesPanel.SetActive(false);
        WorldCupDisciplinesPanel.SetActive(false);
        WinterGamesPanel.SetActive(false);
        WorldCupPanel.SetActive(false);
        actualGameMode = gameModes.noMode;
    }


    void Update()
    {
    }
    public void PlayEvent()
    {
        actualGameMode = gameModes.casualMode;
        MenuPanel.SetActive(false);
        SportsPanel.SetActive(true);
        DisciplinesPanel.SetActive(false);

        //SceneManager.LoadScene(1);
        // ResetScore();
        //  ResetGame();

    }
    public void PlayWinterGames()
    {
        MenuPanel.SetActive(false);
        WinterGamesPanel.SetActive(true);
        DisciplinesPanel.SetActive(false);
        //canvasGroup.alpha = 0f;
        //rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        //foreach (var item in winterGames)
        //{
        //    item.transform.localScale = Vector3.zero;

        //}

    }
    public void PlayWorldCup()
    {
        WorldCupPanel.SetActive(true);
        actualGameMode = gameModes.worldCupMode;
    }

    public void ShowDisciplines()
    {
        MenuPanel.SetActive(false);

        if (actualGameMode == gameModes.casualMode)
        {
            SportsPanel.SetActive(false);
            DisciplinesPanel.SetActive(true);
        }
        if (actualGameMode == gameModes.worldCupMode)
        {
            WorldCupPanel.SetActive(false);
            WorldCupDisciplinesPanel.SetActive(true);
        }
    }
    public static void StartCompetition(CompetitionType actualCompetition)
    {
        //var competitionIndex = availableCompetitions[click];
        currentCompetition = actualCompetition;
        SceneManager.LoadScene(1);
    }
    public static void StartWorldCup(WorldCupCompetition actualWorldCupCompetition)
    {
        //var competitionIndex = availableCompetitions[click];
        currentWorldCup = actualWorldCupCompetition;
        currentCompetition = currentWorldCup.worldCupEvents[0];
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        MenuPanel.SetActive(true);
        SportsPanel.SetActive(false);
        DisciplinesPanel.SetActive(false);
        WinterGamesPanel.SetActive(false);
        WorldCupDisciplinesPanel.SetActive(false);
        actualGameMode = gameModes.noMode;


    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
