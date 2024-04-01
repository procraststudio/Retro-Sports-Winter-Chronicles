using System.Collections;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Surprises : MonoBehaviour
{
    public float surpriseChance { get; set; }
    [SerializeField] public float realSurpriseChance { get; set; }
    Gamemanager gamemanager;
    Weather weather;
    ShortEvent currentEvent;
    public bool surpriseEffect { get; set; }
    public bool disqualification { get; set; }
    [SerializeField] TMP_Text surpriseInfo;
    [SerializeField] TMP_Text surpriseModifier;
    [SerializeField] TMP_Text playerSurpriseChance;
    [SerializeField] TMP_Text surpriseRollIndicator;
    public GameObject eventWindow;
    public TMP_Text eventTitle;
    Competition competition;
    public float surpriseMod;

    void Start()
    {
        //Debug.Log("SURPRISE CHANCE TODAY: " + surpriseChance);
        surpriseEffect = false;
        disqualification = false;
        gamemanager = FindObjectOfType<Gamemanager>();
        weather = FindObjectOfType<Weather>();
        currentEvent = FindObjectOfType<ShortEvent>();
        competition = Competition.Instance;
        // favouritesCount = gamemanager.numberOfFavourites;

    }


    public void CheckSurprise(Player player)
    {

        competition.ChangeState(Competition.GameState.CheckSurprisePhase);
        surpriseInfo.text = "";
        surpriseMod = gamemanager.surprisesModifier * weather.weatherModifier;
        realSurpriseChance = player.ranking * surpriseMod;
        playerSurpriseChance.text = realSurpriseChance.ToString("F0");
        int surpriseRoll = Random.Range(1, 101);
        surpriseRollIndicator.text = surpriseRoll.ToString();
        Debug.Log("SURPRISE CHANCE: " + realSurpriseChance);
        Debug.Log("SURPRISE ROLL: " + surpriseRoll);

        if ((surpriseRoll == 1) || ((player.ranking) <= 15) && (surpriseRoll <= realSurpriseChance))
        {

            surpriseEffect = true;
            eventWindow.SetActive(true);
            eventTitle.text = "SURPRISE!".ToString();
            if (gamemanager.competitionName.Contains("Slalom") && (surpriseRoll % 2 == 0))
            {
                if (((int)realSurpriseChance + player.ranking) % 5 == 0)
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
                surpriseInfo.text = player.secondName.ToUpper() + " IS SLOW: OUT OF 15!";
                player.PoorFormEffect();
                player.myState = Player.PlayerState.OutOf15;
            }
            Debug.Log("SURPRISE! NEW STATE " + player.myState);
            SurpriseEffect(player);
            StartCoroutine("CloseWindow");
        }

        else
        {
            surpriseEffect = false;
            Debug.Log("NO SURPRISE");
            competition.ChangeState(Competition.GameState.CompetitionPhase);
        }
    }


    public float SurpriseModifier()
    {
        float surpriseMod = gamemanager.surprisesModifier * weather.weatherModifier;
        return surpriseMod;
    }

    public void SurpriseEffect(Player player)
    {
        surpriseEffect = true;
        var state = player.myState;

        switch (state)
        {
            case Player.PlayerState.OutOf15: competition.outOf15Competitors.Add(player); break;
            case Player.PlayerState.DidNotFinish: competition.didNotFinish.Add(player); break;
            case Player.PlayerState.Disqualified: competition.disqualified.Add(player); break;
        }
        FindObjectOfType<CommentsSystem>().SummaryComment(player);

        int d6Roll = Random.Range(1, 7);
        Player surpriseCompetitor;
        if (surpriseEffect)
        {
            if ((competition.underdogs.Count > 0) && (d6Roll == 1) || (competition.outsiders.Count == 0))// IF 1 ON D6: BIG SURPRISE, UNDERDOG ENTERS
            {
                surpriseCompetitor = competition.underdogs[0];
                competition.underdogs.Remove(surpriseCompetitor);
            }

            else
            {
                if (competition.underdogs.Count == 0)
                {
                    Debug.Log("No more underdogs");
                    surpriseCompetitor = competition.outsiders[0];
                    competition.outsiders.Remove(surpriseCompetitor);
                }
                else
                {
                    surpriseCompetitor = competition.outsiders[0];
                    competition.outsiders.Remove(surpriseCompetitor);
                }
            }

            if (!competition.competitionIsOver) // if not decoration phase
            {
                competition.players.RemoveAt(competition.players.Count - 1);// ??podwójne usuwanie po 3 sektorze
                competition.players.Add(surpriseCompetitor);
            }
            competition.CheckIfEmptyLists();


            //competition.UpdatePlayerList(competition.players, startingList);
            //competition.UpdatePlayerList(competition.outsiders, outsidersList);

            //competition.UpdatePlayerList(competition.underdogs, underdogsList);
            //competition.UpdatePlayerList(competition.outOf15Competitors, outOf15List);
            //competition.updateResults();

            //competition.UpdatePlayerList(competition.didNotFinish, didNotFinishList);
            //competition.UpdatePlayerList(competition.disqualified, disqualifiedList);

            competition.UpdateLists();
            competition.partsOfRun = 0;  // ?????
            competition.finalText.text += "\n" + player.secondName + " IS OUT OF 15/DQ/DNF!" + "\n" + surpriseCompetitor.secondName + " ENTERS!";
            competition.eventHappened = false;
            Debug.Log(player.secondName + " IS OUT OF 15/DQ/DNF! " + surpriseCompetitor.secondName + " ENTERS!");
            competition.EndRun();
        }
    }

    public int DisplaySurpriseChance(Player player)
    {
        int surpriseCh = player.ranking * (int)surpriseMod;
        return surpriseCh;


    }
    private IEnumerator CloseWindow()
    {
        yield return new WaitForSeconds(2.00f);
        eventWindow.SetActive(false);

        // competition.eventHappened = false;
        competition.ChangeState(Competition.GameState.CompetitionPhase);


        // 


        // runButton.SetActive(true);

    }

}










