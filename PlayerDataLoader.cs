using System.Collections.Generic;
using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    public GameObject playerDataPrefab;
    public GameObject playerDataObject;
    public Transform playerDataParent;
    [SerializeField] string nameOfTheList;
    public List<Player> listToLoad;
    public List<GameObject> gameObjects;
    Competition competition;
    public bool competitorsLoaded;


    void Start()
    {
        competitorsLoaded = false;
        competition = Competition.Instance;
        listToLoad = competition.players;

       // LoadPlayers(listToLoad);

        // load initial lists
    }

    void Update()
    {
       // if ((competitorsLoaded)&&(listToLoad.Count != gameObjects.Count))
       // {
         //   UpdateCompetitors(listToLoad);
       // }
    }


    public void LoadPlayers(List<Player> players)
    {
        if (players.Count > 0)
        {
            foreach (Player competitor in players)
            {
                playerDataObject = Instantiate(playerDataPrefab, playerDataParent);
                gameObjects.Add(playerDataObject);
                PlayerDisplay playerData = playerDataObject.GetComponent<PlayerDisplay>();
                playerData.DisplayCompetitor(competitor);

            }
            competitorsLoaded=true;
        }
    }

    public void UpdateCompetitors(List<Player> competitors)
    {
        foreach (GameObject obj in gameObjects)
        {
            Destroy(obj);

        }
        LoadPlayers(competitors);

    }
}

