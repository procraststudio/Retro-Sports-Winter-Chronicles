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
    ShortEvent currentEvent;
    public bool surpriseEffect { get; set; }
    public bool disqualification { get; set; }
    [SerializeField] TMP_Text surpriseInfo;
    [SerializeField] TMP_Text surpriseModifier;
    public GameObject eventWindow;
    public TMP_Text eventTitle;
    Competition competition;

    void Start()
    {
        //Debug.Log("SURPRISE CHANCE TODAY: " + surpriseChance);
        surpriseEffect = false;
        disqualification = false;
        gamemanager = FindObjectOfType<Gamemanager>();
        weather = FindObjectOfType<Weather>();
        currentEvent = FindObjectOfType<ShortEvent>();
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
        int surpriseRoll = Random.Range(1, 101);
        Debug.Log("SURPRISE ROLL: " + surpriseRoll);

        if ((surpriseRoll == 1) || ((player.ranking) <= (favourites)) && (surpriseRoll <= realSurpriseChance))
        {
            eventWindow.SetActive(true);
            if (gamemanager.competitionName.Contains("Slalom") && (surpriseRoll % 2 == 0))
            {
                if (((int)realSurpriseChance+player.ranking) % 5 == 0)
                {
                    surpriseInfo.text = player.secondName.ToUpper() + " MISSES THE GATE! DISQUALIFIED!";
                    player.myState = Player.PlayerState.Disqualified;
                }
                else 
                {
                    surpriseInfo.text = player.secondName.ToUpper() + " FALLS DOWN! OUT!";
                    player.myState = Player.PlayerState.DidNotFinish;
                }
            }

            else
            {
                // eventTitle.text = "SURPRISE!".ToString();
                surpriseInfo.text = player.secondName.ToUpper() + " IS OUT OF 15!";
                player.PoorFormEffect();
                player.myState = Player.PlayerState.OutOf15;
            }

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
            currentEvent.StartCoroutine("CloseEventWindow");
            // eventTitle.text = "EVENT".ToString();

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










