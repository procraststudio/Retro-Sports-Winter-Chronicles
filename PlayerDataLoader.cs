using System.Collections.Generic;
using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    public GameObject playerDataPrefab;
    public GameObject playerDataObject;
    public GameObject header;
    public Transform playerDataParent;
    public List<Player> listToLoad = new List<Player>();
    public List<GameObject> gameObjects;
    Competition competition;
    public bool competitorsLoaded;
    private string nameOfTheList;
    public bool listFrozen = false;
   


    void Start()
    {
        competitorsLoaded = false;
        competition = Competition.Instance;
        tag = this.gameObject.tag;

    }


    public void LoadCompetitorsList()
    {
        CheckListToLoad();

        if ((listToLoad.Count > 0) && (!competitorsLoaded))
        {
            ActivateHeaders();
            foreach (Player competitor in listToLoad)
            {
                playerDataObject = Instantiate(playerDataPrefab, playerDataParent);
                gameObjects.Add(playerDataObject);
                PlayerDisplay playerData = playerDataObject.GetComponent<PlayerDisplay>();
                playerData.DisplayCompetitor(competitor, competition.currentRun);

            }
            competitorsLoaded = true;
        }
    }

    public void UpdateCompetitors()
    {
        if (!listFrozen)
        {
            foreach (GameObject obj in gameObjects)
            {
                Destroy(obj);
                competitorsLoaded = false;
            }
            LoadCompetitorsList();
        }
    }

    public List<Player> CheckListToLoad()
    {

        if (gameObject.CompareTag("favourites_list"))
        {
            listToLoad = competition.players;
        }
        else if (gameObject.CompareTag("outsiders_list"))
        {
            listToLoad = competition.outsiders;
        }
        else if (gameObject.CompareTag("underdogs_list"))
        {
            listToLoad = competition.underdogs;
        }
        else if (gameObject.CompareTag("finishers_list"))
        {
            listToLoad = competition.finishers;
        }
        else if (gameObject.CompareTag("outOf15_list"))
        {
            listToLoad = competition.outOf15Competitors;
        }
        else if (gameObject.CompareTag("didNotFinish_list"))
        {
            listToLoad = competition.didNotFinish;
        }
        else if (gameObject.CompareTag("disqualified_list"))
        {
            listToLoad = competition.disqualified;
        }
        else if (gameObject.CompareTag("firstRun_list"))
        {
            if ((competition.firstRunClassification.Count > 0) && (!listFrozen))
            {
                listToLoad = competition.firstRunClassification;
            }
            else if (competition.currentRun > 1)
            {
                listFrozen = true;
            }

        }
        else if (gameObject.CompareTag("secondRun_list"))
        {
            if ((competition.secondRunClassification.Count > 0) && (!listFrozen))
            {
                listToLoad = competition.secondRunClassification;
            }
             else if (competition.currentRun > 2)
            {
                listFrozen = true;
            }

        }

            return listToLoad;

    }

    public void ActivateHeaders()
    {
        header.SetActive(true); 
    }
}

