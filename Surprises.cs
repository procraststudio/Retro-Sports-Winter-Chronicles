using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Surprises : MonoBehaviour
{
    public float surpriseChance { get; set; }
    [SerializeField] private float realSurpriseChance { get; set; }
    Gamemanager gamemanager;
    Weather weather;
    public bool surpriseEffect { get; set; }
    public bool disqualification { get; set; }
    [SerializeField] TMP_Text surpriseInfo;
    [SerializeField] TMP_Text surpriseModifier;
    Competition competition;

    void Start()
    {
        //Debug.Log("SURPRISE CHANCE TODAY: " + surpriseChance);
        surpriseEffect = false;
        disqualification = false;
        gamemanager = FindObjectOfType<Gamemanager>();
        weather = FindObjectOfType<Weather>();
        competition = Competition.Instance;
    }

    private void Update()
    {
        if (realSurpriseChance >= 1.00f)
        {
            surpriseModifier.text = realSurpriseChance.ToString("F0") + "%";
        }
    }


    public void CheckSurprise(Player player)
    {
        surpriseInfo.text = "";
        int favourites = gamemanager.numberOfFavourites;
        realSurpriseChance = player.ranking * SurpriseModifier();
        float surpriseRoll = Random.Range(1, 101);
        Debug.Log("SURPRISE ROLL: " + surpriseRoll);

        if ((surpriseRoll == 1) || ((player.ranking) <= (favourites)) && (surpriseRoll <= realSurpriseChance))
        {
            surpriseInfo.text = player.secondName + " (" + player.nationality.ToString() + ") IS OUT OF 15!";
            player.PoorFormEffect();
            player.myState = Player.PlayerState.OutOf15;
            Debug.Log("SURPRISE! NEW STATE " + player.myState);
            surpriseEffect = true;
            //Player playerToDelete = player;

            //for (int i = competition.finishers.Count - 1; i >= 0; i--)
            //{
            //    if (competition.finishers[i].name == playerToDelete.name) // Mo¿esz u¿yæ Equals lub innych metod porównywania
            //    {
            //        competition.finishers.RemoveAt(i); // Usuñ obiekt z listy
            //    }
            //}

            competition.SurpriseEffect(player);

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










