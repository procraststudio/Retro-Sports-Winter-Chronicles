using TMPro;
using UnityEngine;

public class VenueDisplay : MonoBehaviour
{
    public Venue venue;
    [SerializeField] Sprite venueImage;
    [SerializeField] TMP_Text venueName;
    [SerializeField] TMP_Text jumpingHillName;
    [SerializeField] TMP_Text pointK;
    [SerializeField] public TMP_Text venueNation;
    [SerializeField] TMP_Text temperatureAverage;
    [SerializeField] GameObject nationFlag;
    [SerializeField] GameObject jumpingHillInfo;
    public SpriteRenderer flagRenderer;
    private Sprite flagSprite;
    public string flagsFolderPath = "flags/";

    public void Start()
    {
        flagSprite = Resources.Load<Sprite>(flagsFolderPath);
    }

    public void DisplayVenue(Venue venue)
    {
        venueName.text = venue.venueName.ToString();
        venueNation.text = venue.venueNation.ToString();
        // temperatureAverage.text = venue.averageTemperature.ToString();
        DisplayVenueFlag(venue);
        DisplayJumpingHill();
    }

    public void DisplayVenueFlag(Venue venue)
    {
        nationFlag.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + venue.venueNation);
    }

    public void DisplayJumpingHill()
    {
        if (Gamemanager.GetCompetitionType().competitionType.Contains("skiJumping"))
        {
            jumpingHillInfo.SetActive(true);
            jumpingHillName.text = Gamemanager.GetCompetitionType().jumpingHill.jumpingHillName.ToString();
            pointK.text = Gamemanager.GetCompetitionType().jumpingHill.pointK.ToString();
        }
    }
}
