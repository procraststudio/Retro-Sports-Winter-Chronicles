using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Surprises : MonoBehaviour
{
    public float surpriseChance { get; set; }
    Gamemanager gamemanager;
    public bool surpriseEffect { get; set; }
    [SerializeField] TMP_Text surpriseInfo;

    void Start()
    {
        Debug.Log("SURPRISE CHANCE TODAY: " + surpriseChance);
        surpriseEffect = false;
        gamemanager = FindObjectOfType<Gamemanager>();
    }



    public void CheckSurprise(Player player)
    {
        surpriseInfo.text = "";
        int favourites = gamemanager.numberOfFavourites;
        float realSurpriseChance = player.ranking * FindObjectOfType<Gamemanager>().surprisesModifier;
        float surpriseRoll = Random.Range(1, 100);
        Debug.Log("SURPRISE ROLL: " + surpriseRoll);
        if (((player.ranking) <= (favourites)) && (surpriseRoll <= realSurpriseChance))
        {
            Debug.Log("SURPRISE!");
            surpriseInfo.text = player.name + " (" + player.nationality.ToString() + ") IS OUT OF 15!";
            player.PoorFormEffect();
            surpriseEffect = true;
        }

        else
        {
            Debug.Log("NO SURPRISE");
        }
    }
}










