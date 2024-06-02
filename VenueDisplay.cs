using TMPro;
using UnityEngine;

public class VenueDisplay : MonoBehaviour
{
    public Venue venue;
    [SerializeField] Sprite venueImage;
    [SerializeField] TMP_Text venueName;
    [SerializeField] TMP_Text venueNation;
    [SerializeField] TMP_Text temperatureAverage;
    [SerializeField] GameObject nationFlag;
    public SpriteRenderer flagRenderer;
    private Sprite flagSprite;
    public string flagsFolderPath = "flags/";

    public void Start()
    {
        flagSprite = Resources.Load<Sprite>(flagsFolderPath);
       // DisplayVenue();
    }

    private void Update()
    {
       // DisplayVenue();
    }



    public void DisplayVenue(Venue venue)
    {
        venueName.text = venue.venueName.ToString();
        venueNation.text = venue.venueNation.ToString();
       // temperatureAverage.text = venue.averageTemperature.ToString();
        DisplayVenueFlag(venue);
    }

    public void DisplayVenueFlag(Venue venue)
    {
        nationFlag.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + venue.venueNation);
    }
}
