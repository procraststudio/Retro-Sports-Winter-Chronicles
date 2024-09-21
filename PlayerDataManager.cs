using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]

public class PlayerDataManager : MonoBehaviour
{
    public int[] worldCupPoints = new int[15];
    public List<Player> players = new List<Player>();
    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(new PlayerListWrapper { players = this.players });
        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerListWrapper data = JsonUtility.FromJson<PlayerListWrapper>(json);

            foreach (Player savedPlayer in data.players)
            {
                Player existingPlayer = players.Find(p => p.name == savedPlayer.name && p.surname == savedPlayer.surname);
                if (existingPlayer != null)
                {
                    existingPlayer.worldCupPoints = savedPlayer.worldCupPoints;
                }
            }
        }



    }
}
[System.Serializable]
public class PlayerListWrapper
{
    public List<Player> players;
}
