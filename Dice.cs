using DamageNumbersPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] GameObject[] timeGapBackground;
    [SerializeField] TMP_Text[] timeGapTexts;
    [SerializeField] TMP_Text[] sectionTitles;
    [SerializeField] GameObject[] commentatorIcon;
    [SerializeField] public GameObject[] competitorImage;
    [SerializeField] GameObject summaryImage;
    [SerializeField] Sprite[] summarySprites;
    //[SerializeField] Sprite[] skiersIcons;
    [SerializeField] Sprite[] imagesForSurprise;
    [SerializeField] Sprite[] defaultBackgroundImages;
    [SerializeField] Sprite[] skiJumpingBackgroundImages;
    [SerializeField] GameObject[] panelsSections;
    [SerializeField] GameObject[] backgroundImagesSections;
    [SerializeField] GameObject[] commentBackgrounds;
    [SerializeField] GameObject judgesNotesPanel;
    [SerializeField] TMP_Text[] judgesNotes;

    public bool diceActive;
    public int diceIndex;
    public int combosActivated = 0;
    RunDescription description;
    private float pause = 0.40f;   //0.18f;
    private float animatePause = 0.40f;
    private bool diceChanging;
    public int currentCompetitorImage;
    public Sprite currentSkierIcon;
    public RectTransform rectTransform;
    public DamageNumber numberPrefab;
    public DamageNumber hatTrickPrefab;
    public RectTransform[] rectParents;
    PointsSystem pointsSystem;
    AchievementsManager achievements;
    Gamemanager gamemanager;



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
        pointsSystem = PointsSystem.Instance;
        achievements = AchievementsManager.Instance;
        gamemanager = FindObjectOfType<Gamemanager>();
        LoadSectionTitles();
        if (Gamemanager.GetCompetitionType().competitionType.Contains("skiJumping"))
        {
            defaultBackgroundImages = skiJumpingBackgroundImages;
        }

    }

    private void LoadSectionTitles()
    {
        sectionTitles[0].text = Gamemanager.GetCompetitionType().firstSectionTitle.ToString();
        sectionTitles[1].text = Gamemanager.GetCompetitionType().secondSectionTitle.ToString();
        sectionTitles[2].text = Gamemanager.GetCompetitionType().thirdSectionTitle.ToString();
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
        // ShowBackgroundImage(sectorNumber);
        panelActivate(sectorNumber);
        competition.runButton.SetActive(false);
        //StartCoroutine("ChangingDiceFacesEffect");
        //firstDieImages[diceIndex].GetComponentInParent<RectTransform>().DOLocalRotate(new Vector3(0f, 0f, 180f), 0.48f, RotateMode.Fast);
        firstDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.firstD6 - 1];
        DiceRollingEffect(1);
        // firstDieImages[diceIndex].GetComponentInParent<RectTransform>().DOLocalRotate(new Vector3(0f, 0f, 180f), 0.40f, RotateMode.Fast);
        yield return new WaitForSeconds(pause);
        secondDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.secondD6 + 5];
        DiceRollingEffect(2);
        //secondDieImages[diceIndex].GetComponent<RectTransform>().DOLocalRotate(new Vector3(0f, 0f, -180f), 0.40f, RotateMode.Fast);
        yield return new WaitForSeconds(pause);
        thirdDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.thirdD6 + 11];
        DiceRollingEffect(3);
        diceChanging = false;
        CheckDiceCombos();
        diceIndex++;
        yield return new WaitForSeconds(pause);
        competition.runButton.SetActive(true);
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
            ResetTimeGapBackgrounds();
            ResetBackgrounds();
            ResetDiceEffect();
            ResetCommentsBackgrounds();
            ResetCompetitorsImages();
            FindObjectOfType<CommentsSystem>().ResetComments();
            diceIndex = 0;
            combosActivated = 0;
            judgesNotesPanel.SetActive(false);
        }
    }

    public void UpdateTimeGap(Player player, float timeDifference)
    {
        if (Gamemanager.GetCompetitionType().timeIntervals == true)
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
        int randomImage = Random.Range(0, defaultBackgroundImages.Length);
        switch (sectorNumber)
        {
            case 1:
                panelsSections[0].SetActive(false); panelsSections[1].SetActive(true); panelsSections[2].SetActive(true);
                backgroundImagesSections[0].GetComponent<SpriteRenderer>().sprite = defaultBackgroundImages[0]; break;
            case 2:
                panelsSections[0].SetActive(true); panelsSections[1].SetActive(false); panelsSections[2].SetActive(true);
                backgroundImagesSections[1].GetComponent<SpriteRenderer>().sprite = defaultBackgroundImages[1]; break;
            case 3:
                panelsSections[0].SetActive(true); panelsSections[1].SetActive(true); panelsSections[2].SetActive(false);
                backgroundImagesSections[2].GetComponent<SpriteRenderer>().sprite = defaultBackgroundImages[2]; break;
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
            backgroundImagesSections[i].GetComponent<SpriteRenderer>().sprite = null;// defaultBackgroundImages[i];
            backgroundImagesSections[i].GetComponent<SpriteRenderer>().DOColor(Color.white, 0.1f);
        }
    }

    public void ShowBackgroundImage(int sectorIndex)
    {
        int randomImage = Random.Range(0, defaultBackgroundImages.Length);
        if (sectorIndex > -1)
        {
            if (Gamemanager.GetCompetitionType().competitionType.Contains("skiJumping"))
            {
                backgroundImagesSections[sectorIndex - 1].GetComponent<SpriteRenderer>().sprite = skiJumpingBackgroundImages[sectorIndex - 1];
            }
            else
            {
                backgroundImagesSections[sectorIndex - 1].GetComponent<SpriteRenderer>().sprite = defaultBackgroundImages[randomImage];
            }
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

    public void CheckDiceCombos()
    {
        // if (firstDieImages[0].GetComponent<SpriteRenderer>().sprite == diceSides[0])
        if ((competition.firstD6 == competition.secondD6) && (competition.thirdD6 != competition.firstD6))
        {
            DiceEffect("double");
            SpawnComboInfo("DOUBLE!");
            pointsSystem.AddGamePoints(150);
            achievements.AddDouble();
            combosActivated++;

        }
        else if ((competition.firstD6 == competition.secondD6) && (competition.firstD6 == competition.thirdD6))
        {
            DiceEffect("triple");
            SpawnComboInfo("TRIPLE!");
            pointsSystem.AddGamePoints(800);
            achievements.AddTriple();
            combosActivated++;
        }
        else if (((competition.secondD6 - competition.firstD6) == 1) && ((competition.thirdD6 - competition.firstD6) == 2))
        {
            DiceEffect("straight");
            SpawnComboInfo("STRAIGHT!");
            pointsSystem.AddGamePoints(1000);
            achievements.AddStraight();
            combosActivated++;
        }
        CheckHatTrick();
    }

    public void CheckHatTrick()
    {
        if (combosActivated == 3)
        {
            DamageNumber damageNumber = hatTrickPrefab.SpawnGUI(rectParents[diceIndex], new Vector2(0f, 220f), "HAT TRICK!".ToString());
            pointsSystem.AddGamePoints(1500);
            achievements.AddHatTrick();
        }
    }

    public void DiceEffect(string comboType)
    {
        SoundManager.PlayOneSound("dice_combo");
        firstDieImages[diceIndex].GetComponentInParent<RectTransform>().DOShakePosition(0.5f, 20.0f, 3, 4f, true, true);
        secondDieImages[diceIndex].GetComponentInParent<RectTransform>().DOShakePosition(0.5f, 20.0f, 3, 4f, true, true);
        switch (comboType)
        {
            case "double":
                firstDieImages[diceIndex].GetComponent<SpriteRenderer>().DOColor(Color.cyan, 0.5f);
                secondDieImages[diceIndex].GetComponent<SpriteRenderer>().DOColor(Color.cyan, 0.5f);
                break;
            case "triple":
            case "straight":
                thirdDieImages[diceIndex].GetComponentInParent<RectTransform>().DOShakePosition(0.5f, 20.0f, 3, 4f, true, true);
                firstDieImages[diceIndex].GetComponent<SpriteRenderer>().DOColor(Color.green, 0.5f);
                secondDieImages[diceIndex].GetComponent<SpriteRenderer>().DOColor(Color.green, 0.5f);
                thirdDieImages[diceIndex].GetComponent<SpriteRenderer>().DOColor(Color.green, 0.5f);
                break;
                //case "homefactor":
                //    SpawnComboInfo("HOME FACTOR"); break;

        }
    }
    public void DiceRollingEffect(int diceNumber)
    {
        if (diceNumber == 1)
        {
            firstDieImages[diceIndex].GetComponentInParent<RectTransform>().DOLocalRotate(new Vector3(0f, 0f, 720f), 0.35f, RotateMode.FastBeyond360);
        }
        else if (diceNumber == 2)
        {
            secondDieImages[diceIndex].GetComponentInParent<RectTransform>().DOLocalRotate(new Vector3(0f, 0f, -720f), 0.35f, RotateMode.FastBeyond360);
        }
        else if (diceNumber == 3)
        {
            thirdDieImages[diceIndex].GetComponentInParent<RectTransform>().DOLocalRotate(new Vector3(0f, 0f, 720f), 0.35f, RotateMode.FastBeyond360);
        }

    }


    public void SpawnComboInfo(string comboType)
    {
        DamageNumber damageNumber = numberPrefab.SpawnGUI(rectParents[diceIndex], new Vector2(0f, 180f), comboType.ToString());
    }

    public void ResetDiceEffect()
    {
        int diceImagesIndex = 0;

        for (int i = 0; i < allDices.Length; i++)
        {
            //allDices[i].GetComponent<SpriteRenderer>().sprite = defaultBackgroundImages[i];
            allDices[i].GetComponent<SpriteRenderer>().DOColor(Color.white, 0.1f);
        }
    }

    IEnumerator ChangingDiceFacesEffect()
    {
        for (int i = 0; i < 4; i++)
        {
            firstDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[Random.Range(0, diceSides.Length)];// diceSides[competition.firstD6 - 1];
            yield return new WaitForSeconds(0.04f);
        }
    }

    public void ShowJudgesNotes(List<double> notes)
    {
        judgesNotesPanel.SetActive(true);

        for (int i = 0; i < notes.Count; i++)
        {
            if ((i == 0) || (i == notes.Count - 1))
            {
                judgesNotes[i].text = "<color=grey>" + notes[i].ToString("F1");
            }
            else
            {
                judgesNotes[i].text = notes[i].ToString("F1");
            }
        }
    }

}





