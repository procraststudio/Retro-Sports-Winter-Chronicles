using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject SportsPanel;
    [SerializeField] GameObject DisciplinesPanel;

    void Start()
    {
        SportsPanel.SetActive(false);
        DisciplinesPanel.SetActive(false);
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

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
