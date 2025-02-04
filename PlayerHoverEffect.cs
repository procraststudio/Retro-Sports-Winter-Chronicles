using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerHoverEffect : MonoBehaviour, IPointerExitHandler, IPointerClickHandler
{
    public GameObject playerCardPrefab;
    public GameObject playerCardInstance;
    public GameObject playerBasicData;
    public RectTransform playerBasicDataRect;
    public GameObject hoverOnPlayerCard;
    [SerializeField] public TMP_Text competitorName;
    [SerializeField] public TMP_Text competitorGrade;
    [SerializeField] public TMP_Text competitorRanking;
    [SerializeField] public TMP_Text competitorExperience;
    [SerializeField] public TMP_Text competitorStatus;
    [SerializeField] public TMP_Text competitorForm;
    [SerializeField] public TMP_Text competitorPerformancePoints;
    [SerializeField] public TMP_Text injuryInformation;
    [SerializeField] GameObject playerFlag;
    [SerializeField] public Sprite headGraphic;
    public PlayerDisplay script;
    private bool objectClicked;


    public void Start()
    {
        script = this.gameObject.GetComponent<PlayerDisplay>();
        objectClicked = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (objectClicked)
        {
            objectClicked = false;
            HidePlayerCard();
        }
        else
        {
            objectClicked = true;
            ShowPlayerCard();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HidePlayerCard();
        objectClicked = false;
    }

    private void ShowPlayerCard()
    {
        playerCardInstance.SetActive(true);
        competitorName.text = script.competitorName.text.ToString();
        competitorGrade.text = script.playerLoaded.grade.ToString();
        competitorRanking.text = script.playerLoaded.ranking.ToString();
        competitorExperience.text = script.playerLoaded.experience.ToString();
        playerFlag.GetComponent<SpriteRenderer>().sprite = script.playerFlag.GetComponent<SpriteRenderer>().sprite;
        competitorStatus.text = script.playerLoaded.myState.ToString();
        competitorPerformancePoints.text = script.playerLoaded.finalPerformance.ToString("F2");
        script.DisplayHeadImage();
        //headGraphic.GetComponent<SpriteRenderer>().sprite = script.headGraphic.GetComponent<SpriteRenderer>().sprite;
        ShowCompetitorForm(script.playerLoaded);
        ShowInjuryStatus(script.playerLoaded);
        //competitorGrade.text = script.competitorGrade.text.ToString();
        //competitorRanking.text = script.competitorRanking.text.ToString();
        //competitorExperience.text = script.competitorExperience.text.ToString();
        // playerFlag.GetComponent<SpriteRenderer>().sprite = script.playerFlag.GetComponent<SpriteRenderer>().sprite;
        AdjustPositionToScreenBounds();
    }

    private void HidePlayerCard()
    {
        if (playerCardInstance != null)
        {
            playerCardInstance.SetActive(false);
        }
    }
    private void AdjustPositionToScreenBounds()
    {
        if (IsChildOfTaggedObject(playerCardInstance, "underdogs_list"))
        {
            Debug.Log("UNDERDOGS: CARD TOO LOW ");
            playerCardInstance.transform.localPosition = new Vector3(47.4f, 50f, 0f);
        }

    }
    bool IsChildOfTaggedObject(GameObject obj, string tag)
    {
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            if (parent.CompareTag(tag))
            {
                return true;
            }
            parent = parent.parent;
        }
        return false;
    }

    private void ShowCompetitorForm(Player player)
    {
        if (player.goodFormEffect)
        {
            competitorForm.text = "GOOD";
        }
        else if (player.poorFormEffect)
        {
            competitorForm.text = "POOR";
        }
        else
        {
            competitorForm.text = "AVERAGE";
        }
    }

    private void ShowInjuryStatus(Player player)
    {
        if ((player.isInjured) && (injuryInformation != null))
        {
            injuryInformation.gameObject.SetActive(true);
            injuryInformation.text = "Injury: " + player.weeksOfInjury.ToString() + " weeks";
        }
        else
        {
            injuryInformation.text = "";
        }
    }

}

