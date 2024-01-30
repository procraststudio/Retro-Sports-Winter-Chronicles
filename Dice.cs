using System.Collections;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] Sprite[] diceSides;
    Competition competition;
    [SerializeField] GameObject[] firstDieImages;
    [SerializeField] GameObject[] secondDieImages;
    [SerializeField] GameObject[] thirdDieImages;
    [SerializeField] GameObject[] allDices;
    [SerializeField] TMP_Text[] timeGapTexts;
    public bool diceActive;
    public int diceIndex;
    RunDescription description;
    private float pause;
    private bool diceChanging;


    void Start()
    {
        competition = Competition.Instance;
        diceActive = false;
        diceIndex = 0;
        description = FindObjectOfType<RunDescription>();
        diceChanging = false;
        pause = 0.10f;

    }


    private void CheckClicks()
    {
        if ((diceChanging) && (Input.GetButtonDown("Fire1")))
        {
            Debug.Log("CLICK!");
            pause = 0.10f;
        }

    }

    public IEnumerator showDice()
    {
        diceChanging = true;

        firstDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.firstD6 - 1];
        yield return new WaitForSeconds(pause);
        secondDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.secondD6 + 5];
        yield return new WaitForSeconds(pause);
        thirdDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.thirdD6 + 11];
        description.ShowDescription();
        diceChanging = false;
        diceIndex++;

    }

    public void ResetDice()
    {
        if (competition.partsOfRun == 0)
        {
            for (int i = 0; i < allDices.Length; i++)
            {
                allDices[i].GetComponent<SpriteRenderer>().sprite = null;
                

            }
            timeGapTexts[0].text = "";
            timeGapTexts[1].text = "";
            timeGapTexts[2].text = "";
            diceIndex = 0;
        }



    }




}





