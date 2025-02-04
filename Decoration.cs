using System.Collections;
using TMPro;
using UnityEngine;

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
    public Player winner { get; set; }
    public Player secondPlayer { get; set; }
    public Player thirdPlayer { get; set; }
    public bool worldCupDecoration = false;


    void Start()
    {
        //decorationPanel.SetActive(false);
        // competition = FindObjectOfType<Competition>();
        competition = Competition.Instance;
        thirdPlacePanel.SetActive(false);
        secondPlacePanel.SetActive(false);
        winnerPanel.SetActive(false);
        podiumImage.SetActive(false);
        // check alternate winners if WC final decoration
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
        secondPlacePanel.SetActive(true);
        SoundManager.PlayOneSound("dice_combo");
        winnersNames[1].text = secondPlayer.secondName.ToUpper();
        flags[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + secondPlayer.nationality);
        yield return new WaitForSeconds(2.00f);
        winnerPanel.SetActive(true);
        SoundManager.PlayOneSound("crowd01");
        winnersNames[2].text = winner.secondName.ToUpper();
        flags[2].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("flags/" + winner.nationality);
        yield return new WaitForSeconds(1.00f);
        //ExitButton.SetActive(true);
        if (!worldCupDecoration)
        {
            FindObjectOfType<CompetitionSummary>().ShowGainedPoints();
        }
    }

    public void WorldCupFinalPodium()
    {
        winnerPanel.SetActive(false);
        secondPlacePanel.SetActive(false);
        thirdPlacePanel.SetActive(false);
        winner = null; secondPlayer = null; thirdPlayer = null;
        // WHAT IF players with identical points
        // stand next to each other, lower position on podium is empty
        winner = competition.worldCupClassification[0];
        secondPlayer = competition.worldCupClassification[1];
        thirdPlayer = competition.worldCupClassification[2];
        // cups graphics instead of medals
        worldCupDecoration = true;
        StartCoroutine("DecorateMedalists");
    }


}
