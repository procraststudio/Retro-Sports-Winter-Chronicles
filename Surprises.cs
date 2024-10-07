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
    ScreenShake shake;
    Dice dice;
    private int surpriseRoll;
    [SerializeField] private GameObject scrollViewInfo;
    [SerializeField] AudioClip[] sounds;

    void Start()
    {
        //Debug.Log("SURPRISE CHANCE TODAY: " + surpriseChance);
        surpriseEffect = false;
        disqualification = false;
        gamemanager = FindObjectOfType<Gamemanager>();
        weather = FindObjectOfType<Weather>();
        currentEvent = FindObjectOfType<ShortEvent>();
        dice = FindObjectOfType<Dice>();
        competition = Competition.Instance;
        shake = FindObjectOfType<ScreenShake>();
        // favouritesCount = gamemanager.numberOfFavourites;
    }


    public void CheckSurprise(Player player)
    {
        competition.ChangeState(Competition.GameState.CheckSurprisePhase);
        surpriseInfo.text = "";
        surpriseMod = gamemanager.surprisesModifier * weather.weatherModifier;
        realSurpriseChance = player.ranking * surpriseMod;
        playerSurpriseChance.text = realSurpriseChance.ToString("F0");
        surpriseRoll = Random.Range(1, 101);
        surpriseRollIndicator.text = surpriseRoll.ToString();
        Debug.Log("SURPRISE CHANCE: " + realSurpriseChance);
        Debug.Log("SURPRISE ROLL: " + surpriseRoll);

        CheckCriticalRoll(player);

        if ((surpriseRoll == 1) || ((player.ranking) <= 15) && (surpriseRoll <= realSurpriseChance) && (!player.goodFormEffect))
             //&&(!Gamemanager.GetCompetitionType().competitionType.Contains("skiJumping")))
        {
            surpriseEffect = true;
            eventWindow.SetActive(true);
            eventTitle.text = "SURPRISE!".ToString();
            SoundManager.PlayOneSound("disqualified");
            if ((gamemanager.competitionName.Contains("Slalom") || (gamemanager.competitionName.Contains("Super G")) && (surpriseRoll % 2 == 0)))
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
                gamemanager.surprisesModifier -= 0.03f; //LOWER SURPRISE CHANCE
            }

            else
            {
                // eventTitle.text = "SURPRISE!".ToString();
                surpriseInfo.text = player.secondName.ToUpper() + " IS WEAK: OUT OF 15!";
                SoundManager.PlayOneSound("outof15");
                player.PoorFormEffect();
                player.myState = Player.PlayerState.OutOf15;
            }
            Debug.Log("SURPRISE! NEW STATE " + player.myState);
            SurpriseEffect(player);
            gamemanager.surprisesModifier -= 0.02f; //LOWER SURPRISE CHANCE
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
        // shake.start = true; 
        // shake IMAGE
        //dice.competitorImage[dice.currentCompetitorImage].transform.DOShakePosition(1.5f, 30.0f, 20, 50f, true, true);
        //dice.competitorImage[dice.currentCompetitorImage].GetComponent<SpriteRenderer>().DOFade(0.0f, 1.5f);
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
                competition.players.RemoveAt(competition.players.Count - 1);
                competition.players.Add(surpriseCompetitor);
            }
            competition.CheckIfEmptyLists();
            competition.UpdateLists();
            dice.panelActivate(competition.partsOfRun);
            dice.panelSurpriseEffect(competition.partsOfRun - 1);
            competition.partsOfRun = 0; // check this
                                        //competition.finalText.text += "\n" + player.secondName + " IS OUT OF 15/DQ/DNF!" + "\n" + surpriseCompetitor.secondName + " ENTERS!";
                                        // scrollViewInfo.GetComponent<ScrollViewManager>().AddMessage()
            competition.eventHappened = false;
            string msg = "\n" + player.secondName + " IS OUT OF 15/DQ/DNF!" + "\n" + surpriseCompetitor.secondName + " ENTERS!" + "\n";
            competition.messageWindow.GetComponent<ScrollViewManager>().AddMessage(msg);
            //competition.message = "\n" + player.secondName + " IS OUT OF 15/DQ/DNF!" + "\n" + surpriseCompetitor.secondName + " ENTERS!"+"\n";
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
    }
    public void CheckCriticalRoll(Player player) // HITTING THE GROUND
    {
        if ((competition.firstD6 == 1) && (competition.secondD6 == 1) && (competition.thirdD6 == 1))
        {
            surpriseRoll = 100;
            surpriseEffect = true;
            dice.SpawnComboInfo("DISASTER!");
            eventWindow.SetActive(true);
            if ((Gamemanager.GetCompetitionType().surprisesImpact >= 1) || (Weather.GetWindCondition().Contains("strong")))
            {
                HittingTheGround(player);
            }
            else if (Gamemanager.GetCompetitionType().competitionType.Contains("skiJumping"))
            {
                currentEvent.GetComponent<SkiJumpingEvents>().JumpControl(player, 10);
            }
            StartCoroutine("CloseWindow");
            return;
        }
        else if ((competition.firstD6 == 1) && (competition.secondD6 == 1) && (competition.thirdD6 != 1))
        {
            if (Gamemanager.GetCompetitionType().competitionType.Contains("skiJumping"))
            {
                // currentEvent.GetComponent<SkiJumpingEvents>().ResolveSkiJumpingEvent(player, 4);
                currentEvent.GetEventType( 4); 
                Debug.Log("JUMP CONTROL CHECKED");
            }
        }
    }

    public void HittingTheGround(Player player)
    {
        eventTitle.text = "DISASTER! POSSIBLE INJURY!".ToString();
        //TO DO : INJURY mechanics
        SoundManager.PlayOneSound("disqualified");
        surpriseInfo.text = player.secondName.ToUpper() + " HITS THE GROUND! TRAGEDY!";
        player.myState = Player.PlayerState.DidNotFinish;
        SurpriseEffect(player);
    }
}










