using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    [SerializeField] Sprite[] diceSides;
    Competition competition;
    [SerializeField] GameObject[] firstDieImages;
    [SerializeField] GameObject[] secondDieImages;
    [SerializeField] GameObject[] thirdDieImages;
    [SerializeField] GameObject[] allDices;
    [SerializeField] GameObject[] timeGapBackground;
    [SerializeField] TMP_Text[] timeGapTexts;
    [SerializeField] GameObject[] commentatorIcon;
    [SerializeField] public GameObject[] competitorImage;
    [SerializeField] GameObject summaryImage;
    [SerializeField] Sprite[] summarySprites;
    //[SerializeField] Sprite[] skiersIcons;
    [SerializeField] Sprite[] imagesForSurprise;
    [SerializeField] Sprite[] defaultBackgroundImages;
    [SerializeField] GameObject[] panelsSections;
    [SerializeField] GameObject[] backgroundImagesSections;
    [SerializeField] GameObject[] commentBackgrounds;
    public bool diceActive;
    public int diceIndex;
    RunDescription description;
    private float pause = 0.15f;
    private float animatePause = 0.40f;
    private bool diceChanging;
    public int currentCompetitorImage;
    public Sprite currentSkierIcon;
    public RectTransform rectTransform;

    void Start()
    {
        competition = Competition.Instance;
        diceActive = false;
        diceIndex = 0;
        description = FindObjectOfType<RunDescription>();
        diceChanging = false;
        currentCompetitorImage = 0;
       // currentSkierIcon = skiersIcons[0];
        ResetTimeGapBackgrounds();
        ResetDice();

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
        int sectorNumber = competition.partsOfRun;
        diceChanging = true;
        // StartCoroutine("animateEffect");
        panelActivate(sectorNumber);
        firstDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.firstD6 - 1];
        yield return new WaitForSeconds(pause);
        secondDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.secondD6 + 5];
        yield return new WaitForSeconds(pause);
        thirdDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.thirdD6 + 11];
        diceChanging = false;
        diceIndex++;
    }


    //public int GenerateObject(int length)
    //{
    //    var index = Random.Range(0, length);
    //    currentSkierIcon = skiersIcons[index];
    //    return index;
    //}

    //public IEnumerator animateEffect()
    //{
    //    // MIX THE IMAGES
    //    competitorImage[currentCompetitorImage].GetComponent<SpriteRenderer>().sprite = skiersIcons[GenerateObject(skiersIcons.Length)];
    //    // TODO Moving effect
    //    yield return new WaitForSeconds(animatePause);
    //    competitorImage[currentCompetitorImage].GetComponent<SpriteRenderer>().sprite = skiersIcons[GenerateObject(skiersIcons.Length)];
    //    yield return new WaitForSeconds(animatePause);
    //    competitorImage[currentCompetitorImage].GetComponent<SpriteRenderer>().sprite = skiersIcons[GenerateObject(skiersIcons.Length)];
    //    currentCompetitorImage++;
    //}

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
            ResetTimeGapBackgrounds();
            ResetBackgrounds();
            ResetCommentsBackgrounds(); 
            ResetCompetitorsImages();
            FindObjectOfType<CommentsSystem>().ResetComments();
            diceIndex = 0;
        }
    }

    public void UpdateTimeGap(Player player, float timeDifference)
    {
        timeGapTexts[competition.partsOfRun - 1].text = player.ConvertDifference(timeDifference).ToString();
        timeGapBackground[competition.partsOfRun - 1].SetActive(true);
        if (timeDifference > 0)
        {
            // timeGapTexts[competition.partsOfRun - 1].text = "<color=red>" + (player.ConvertDifference(timeDifference).ToString()) + "<color=red>";

            timeGapBackground[competition.partsOfRun - 1].GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            timeGapBackground[competition.partsOfRun - 1].GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    public void ResetTimeGapBackgrounds()
    {
        for (int i = 0; i < timeGapBackground.Length; i++)
        {
            timeGapBackground[i].SetActive(false);
        }
    }
    public void ResetCompetitorsImages()
    {
        for (int i = 0; i < competitorImage.Length; i++)
        {
            competitorImage[i].GetComponent<SpriteRenderer>().sprite = null;
            competitorImage[i].GetComponent<SpriteRenderer>().DOFade(1.0f, 0.01f);
        }
        summaryImage.GetComponent<SpriteRenderer>().sprite = null;
        currentCompetitorImage = 0;
    }

    public void ShowSummaryImage(string description)
    {
        //commentatorIcon[3].SetActive(true);
        switch (description)
        {
            case ("good"): summaryImage.GetComponent<SpriteRenderer>().sprite = summarySprites[0]; break;
            case ("bad"): summaryImage.GetComponent<SpriteRenderer>().sprite = summarySprites[2]; break;
            default: summaryImage.GetComponent<SpriteRenderer>().sprite = summarySprites[1]; break;
        }
    }

    public void panelActivate(int sectorNumber) // COVERING INACTIVE SECTORS
    {
        switch (sectorNumber)
        {
            case 1:
                panelsSections[0].SetActive(false); panelsSections[1].SetActive(true); panelsSections[2].SetActive(true); break;
            case 2:
                panelsSections[0].SetActive(true); panelsSections[1].SetActive(false); panelsSections[2].SetActive(true); break;
            case 3:
                panelsSections[0].SetActive(true); panelsSections[1].SetActive(true); panelsSections[2].SetActive(false); break;
        }
    }

    public void panelSurpriseEffect(int sectorNumber) //TODO: RED/GREY COLOR
    {
        // panelsSections[sectorNumber].GetComponent<Image>().color = new Color(255, 0, 0, 140f);
        // panelsSections[sectorNumber].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, 274.0f), 2.0f, false).SetEase(Ease.OutElastic);
        backgroundImagesSections[sectorNumber].GetComponent<SpriteRenderer>().sprite = imagesForSurprise[0];
        backgroundImagesSections[sectorNumber].GetComponent<SpriteRenderer>().DOColor(Color.red, 1.0f);
        panelsSections[sectorNumber].GetComponentInParent<RectTransform>().DOShakePosition(1.0f, 70.0f, 10, 10f, true, true);
        //  panelsSections[sectorNumber].GetComponent<Image>().DOFade(100f, 2f);
    }

    public void ResetBackgrounds() 
    {
        for (int i = 0; i < backgroundImagesSections.Length; i++)
        {
            backgroundImagesSections[i].GetComponent<SpriteRenderer>().sprite = defaultBackgroundImages[i];
            backgroundImagesSections[i].GetComponent<SpriteRenderer>().DOColor(Color.white, 0.1f);
        }
    }

    public void ShowCommentBackground(int index)
    {
        commentBackgrounds[index].SetActive(true);  
    }

    public void ResetCommentsBackgrounds()
    {
        for (int i = 0; i < commentBackgrounds.Length; i++)
        {
            commentBackgrounds[i].SetActive(false);
        }
    }





}





