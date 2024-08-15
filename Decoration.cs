using System.Collections;
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
    //[SerializeField] private Competition competition;
    Competition competition;
    [SerializeField] GameObject ExitButton;
    [SerializeField] AudioClip fanfairSound;
    public Player winner { get; set; }
    public Player secondPlayer { get; set; }
    public Player thirdPlayer { get; set; }


    void Start()
    {
        //decorationPanel.SetActive(false);
        // competition = FindObjectOfType<Competition>();
        competition = Competition.Instance;
        thirdPlacePanel.SetActive(false);
        secondPlacePanel.SetActive(false);
        winnerPanel.SetActive(false);
        podiumImage.SetActive(false);
        winner = competition.finishers[0];
        secondPlayer = competition.finishers[1];
        thirdPlayer = competition.finishers[2];
        StartCoroutine("DecorateMedalists");
    }

    // Update is called once per frame


    public IEnumerator DecorateMedalists()
    {
        yield return new WaitForSeconds(2.00f);
        podiumImage.SetActive(true);
        yield return new WaitForSeconds(2.00f);

        thirdPlacePanel.SetActive(true);
        SoundManager.PlayOneSound("dice_combo");
        winnersNames[0].text = thirdPlayer.secondName.ToUpper();
        flags[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + thirdPlayer.nationality);
        yield return new WaitForSeconds(2.00f);
        // AudioSource.PlayClipAtPoint(fanfairSound, Camera.main.transform.position);

        secondPlacePanel.SetActive(true);
        SoundManager.PlayOneSound("dice_combo");
        winnersNames[1].text = secondPlayer.secondName.ToUpper();
        flags[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + secondPlayer.nationality);
        yield return new WaitForSeconds(2.00f);
        winnerPanel.SetActive(true);
        SoundManager.PlayOneSound("crowd01");
        winnersNames[2].text = winner.secondName.ToUpper();
        flags[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + winner.nationality);
        yield return new WaitForSeconds(2.00f);
        ExitButton.SetActive(true);

    }


    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
