using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenueLoader : MonoBehaviour
{
    public string jsonFileName = "venues";
    private List<Venue> venuesList;
    public GameObject venueObject;
    public Venue actualVenue;

    void Start()
    {

        LoadVenuesFromJSON();

    //}

    //public void LoadVenue(string venueName)
    //{ 

        if (venuesList != null && venuesList.Count > 0)
        {
            string targetVenueName = GetComponent<Gamemanager>().sampleCompetitions[1].competitionVenueName.ToString();   //   Competition.  //"Calgary";
            //string targetVenueName = GameStart.currentCompetition.competitionVenueName.ToString();
            actualVenue = venuesList.Find(venue => venue.venueName == targetVenueName);

            if (actualVenue != null)
            {
                Debug.Log("Found Venue - Name: " + actualVenue.venueName + ", Location: " + actualVenue.venueNation);
                venueObject.GetComponent<VenueDisplay>().DisplayVenue(actualVenue);
            }
            else
            {
                Debug.LogError("Venue with name '" + targetVenueName + "' not found in JSON data.");
            }
        }
        else
        {
            Debug.LogError("Failed to load venues data from JSON.");
        }
    }

    private void LoadVenuesFromJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile != null)
        {
            // Deserializacja danych JSON do listy obiektów Venue
            venuesList = JsonUtility.FromJson<VenueList>(jsonFile.text).venues;
        }
        else
        {
            Debug.LogError("Failed to load JSON file: " + jsonFileName);
        }
    }
}

[System.Serializable]
public class VenueList
{
    public List<Venue> venues;
}
