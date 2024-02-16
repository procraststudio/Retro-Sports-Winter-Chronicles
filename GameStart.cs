using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject SportsPanel;
    [SerializeField] GameObject WinterGamesPanel;  
    [SerializeField] GameObject DisciplinesPanel;
    public RectTransform rectTransform;
    public List<GameObject> winterGames = new List<GameObject>();
    public bool winterOlympicsMode = false;
    public CanvasGroup canvasGroup;

    void Start()
    {
        SportsPanel.SetActive(false);
        DisciplinesPanel.SetActive(false);
        WinterGamesPanel.SetActive(false);
    }

    
    void Update()
    {
        
    }
    public void PlayEvent()
    {
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
    public void ShowDisciplines()
    {
        MenuPanel.SetActive(false);
        SportsPanel.SetActive(false);
        DisciplinesPanel.SetActive(true);

    }
    public void StartAlpineSki()
    {
        SceneManager.LoadScene(1);
    }
    public void BackToMenu()
    {
        MenuPanel.SetActive(true);
        SportsPanel.SetActive(false);
        DisciplinesPanel.SetActive(false);
        WinterGamesPanel.SetActive(false);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
