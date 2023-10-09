using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Decoration : MonoBehaviour
{
    //public GameObject decorationPanel;
    public GameObject winnerPanel;
    public GameObject secondPlacePanel;
    public GameObject thirdPlacePanel;
    public GameObject[] flags;
    public GameObject podiumImage;
    [SerializeField] TMP_Text[] winnersNames;
    [SerializeField] private Competition competition;
    [SerializeField] GameObject ExitButton;

    public Player winner { get; set; }
    public Player secondPlayer { get; set; }
    public Player thirdPlayer { get; set; }


    void Start()
    {
        //decorationPanel.SetActive(false);
        competition = FindObjectOfType<Competition>();

        thirdPlacePanel.SetActive(false);
        secondPlacePanel.SetActive(false);
        winnerPanel.SetActive(false);   
        podiumImage.SetActive(false);

         winner = competition.finishers[0];
         secondPlayer = competition.finishers[1];
         thirdPlayer = competition.finishers[2];
        //TO DO: Decoration effects
         StartCoroutine("DecorateMedalists");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DecorateMedalists()
    {  
        yield return new WaitForSeconds(2.00f);
        podiumImage.SetActive(true);
        yield return new WaitForSeconds(2.00f);
        thirdPlacePanel.SetActive(true);
        winnersNames[0].text = thirdPlayer.name;
        flags[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + thirdPlayer.nationality);
        yield return new WaitForSeconds(2.00f);
        secondPlacePanel.SetActive(true);
        winnersNames[1].text = secondPlayer.name;
        flags[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + secondPlayer.nationality);
        yield return new WaitForSeconds(2.00f);
        winnerPanel.SetActive(true);
        winnersNames[2].text = winner.name;
        flags[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + winner.nationality);
        ExitButton.SetActive(true);    

    }


    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
