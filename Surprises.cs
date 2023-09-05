using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Surprises : MonoBehaviour
{
    public float surpriseChance { get; set; }
    Gamemanager gamemanager;
    Weather weather;
    public bool surpriseEffect { get; set; }
    public bool disqualification { get; set; }
    [SerializeField] TMP_Text surpriseInfo;
    [SerializeField] TMP_Text surpriseModifier;

    void Start()
    {
        //Debug.Log("SURPRISE CHANCE TODAY: " + surpriseChance);
        surpriseEffect = false;
        disqualification = false;
        gamemanager = FindObjectOfType<Gamemanager>();
        weather = FindObjectOfType<Weather>();
    }

    private void Update()
    {
        surpriseModifier.text = SurpriseModifier().ToString("F2");
    }


    public void CheckSurprise(Player player)
    {
        surpriseInfo.text = "";
        int favourites = gamemanager.numberOfFavourites;
        float realSurpriseChance = player.ranking * SurpriseModifier();
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

    public float SurpriseModifier()
    {
        float surpriseMod = gamemanager.surprisesModifier * weather.weatherModifier;
        return surpriseMod;
    }

}










