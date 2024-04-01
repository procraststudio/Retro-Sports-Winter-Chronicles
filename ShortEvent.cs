using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShortEvent : MonoBehaviour
{
    private int d6Roll = 0;
    private int eventRoll = 0;
    public bool eventRolled;
    public bool eventResolved;
    public int saveRoll;

    public TMP_Text descriptionText;
    public TMP_Text eventTitle;
    [SerializeField] GameObject eventPrefab;
    public GameObject runButton;
    public GameObject eventObject;
    public GameObject canvasObject;
    Competition competition;
    public int weatherModifier; // TO DO: from -3 (nightmare) to +3 (best) courseModifier
    private RolledEvents actualEvent;
    Player actualCompetitor;
    Weather weather;
    CommentsSystem comment;
    Gamemanager gamemanager;
    Surprises surprise;

    private enum RolledEvents
    {
        Bump = 1,
        Curve = 2,
        Weather = 3,
        Risk = 4,
        Talent = 5,
        Surprise = 6,
    }

    public void Start()
    {
        competition = Competition.Instance;
        weather = FindObjectOfType<Weather>();
        eventRolled = false;
        weatherModifier = (int)weather.weatherModifier; // TO DO" Check this value if not too high
        comment = FindObjectOfType<CommentsSystem>();
        eventObject.SetActive(false);
        gamemanager = FindObjectOfType<Gamemanager>();
        surprise = FindObjectOfType<Surprises>();
        eventResolved = false;

    }

    public void Update()
    {
        if ((eventRolled) && (Input.GetKeyDown(KeyCode.Mouse0)))
        {
            ResolveEvent();
        }
        CloseEventWindow();

        // runButton.SetActive(false);

    }
    public int GetCompetitionModifier()
    {
        if (gamemanager.competitionName.Contains("Slalom"))
        {
            return 1;
        }
        else
        {
            return 0;
        };
    }


    public void GetEventType()
    {
        actualCompetitor = competition.currentCompetitor;
        d6Roll = competition.thirdD6;
        string description = "";
        // eventObject = Instantiate(eventPrefab, new Vector3(-10.00f, -50.0f, 0.00f), Quaternion.identity);
        // eventObject.transform.SetParent(canvasObject.transform, false);
        runButton.SetActive(false);
        eventObject.SetActive(true);

        Debug.Log("WEATHER MODIFIER: " + weatherModifier);

        switch (d6Roll)
        {
            case 1:
                saveRoll = 100 * ((actualCompetitor.experience - weatherModifier)) / 6;
                if (saveRoll < 1)
                {
                    saveRoll = 10;
                }
                Debug.Log("SAVE ROLL: " + saveRoll);//Convert.ToInt32(((actualCompetitor.experience - weatherModifier)/6)*100);
                description += "BUMP!".ToUpper() + "\n" +
                    saveRoll.ToString() + "%: the competitor stays on the route" + "\n"
                    + (100 - saveRoll).ToString() + "%: OUT, DNF!";
                actualEvent = RolledEvents.Bump;
                break;
            case 2:
                saveRoll = 100 * (actualCompetitor.GetGradeModifier() - weatherModifier - GetCompetitionModifier()) / 6;
                if (saveRoll < 1)
                {
                    saveRoll = 10;
                }
                description += "CURVE!".ToUpper() + "\n" +
                    saveRoll.ToString() + "%: no DQ" + "\n"
                    + (100 - saveRoll).ToString() + "%: DISQUALIFIED!";
                actualEvent = RolledEvents.Curve;
                break;
            case 3:

                description += "WEATHER EFFECT!".ToUpper() + "\n" + "If snowing/raining: BUMP. " +
                    "OTHERWISE: If 7 or less competitors to run: CLEARED RUN" + "\n" +
                    "If 8 or more: POOR STRATEGY";
                actualEvent = RolledEvents.Weather;
                break;
            case 4:
                description += "TAKING RISK!".ToUpper() + "\n" + "Roll d6. If result EVEN: +d6 points. " +
                    "\n" + "If result ODD: -d6 points." + "\n" + "If d6 is 1 and GRADE C or worse: OUT OF 15/DNF/DQ.";
                actualEvent = RolledEvents.Risk;
                break;
            case 5:
                description += "EXPERIENCE MATTERS".ToUpper() + "\n" + "Roll d6. If EXP 0 or 3 see d6. " +
                    "\n" + "If 1: -6 points, if 6: +6 points." + "\n" + "OTHERWISE: If HARD snow +d6 points.";
                actualEvent = RolledEvents.Talent;
                break;
            case 6:
                description += "BIG SURPRISE!".ToUpper() + "\n" + "If 2nd run: competitor from OUT OF 15 is back. " +
                    "\n" + "OTHERWISE: UNDERDOG (with +2 grade) enters.";
                actualEvent = RolledEvents.Surprise;
                break;

        }
        // descriptionText.color = Color.white;
        descriptionText.text = description.ToString();
        // description += "\n" + "------------------------";

        Debug.Log("Event described");
        eventRolled = true;
        ResolveEvent();
    }

    public void ResolveEvent()
    {
        eventRolled = false;
        eventRoll = Random.Range(1, 7);
        eventTitle.text = "EVENT".ToString();
        descriptionText.text += "\n" + "-------------------------------------------" + "\n"; // + "Roll is: " + eventRoll + ". ";

        switch (actualEvent)
        {
            case RolledEvents.Bump: BumpTest(); break;
            case RolledEvents.Curve: DisqualificationTest(); break;
            case RolledEvents.Weather: WeatherTest(); break;
            case RolledEvents.Risk: RiskTest(); break;
            case RolledEvents.Talent: TalentTest(); break;
            case RolledEvents.Surprise: UnderdogEnters(); break;
        }
        eventResolved = true;
        //competition.eventHappened = true;
        competition.ChangeState(Competition.GameState.CompetitionPhase);
        //StartCoroutine("CloseEventWindow");
    }
    private void BumpTest()
    {
        if ((eventRoll == 6) || ((eventRoll + actualCompetitor.experience - weatherModifier) > 5))
        {
            descriptionText.text += "GREAT TECHNIQUE! " + actualCompetitor.secondName + " IS STILL FIGHTING";
        }
        else
        {
            descriptionText.text += "OH NO! " + actualCompetitor.secondName + " IS OUT";
            actualCompetitor.myState += 2; //DNF
            Debug.Log("STATE IS: " + actualCompetitor.myState.ToString());
            surprise.SurpriseEffect(actualCompetitor);
        }
    }
    private void DisqualificationTest()
    {
        if ((eventRoll == 6) || ((eventRoll + actualCompetitor.GetGradeModifier() + weatherModifier - GetCompetitionModifier()) > 5))
        {
            descriptionText.text += "GREAT FORM! " + actualCompetitor.secondName + " IS STILL IN THE GAME!";
        }
        else
        {
            descriptionText.text += "OH... NO! " + actualCompetitor.secondName + " IS DISQUALIFIED";
            actualCompetitor.myState += 3; // DQ
            surprise.SurpriseEffect(actualCompetitor);
        }
    }

    private void WeatherTest()
    {
        if ((weather.precipitation.Contains("snowing")) || (weather.precipitation.Contains("raining")))
        {
            descriptionText.text += "DANGEROUS BUMP...";
            BumpTest();
        }
        else
        {
            if (competition.players.Count < 8)
            {
                actualCompetitor.AddRunModifier(competition.currentRun, 5);
                descriptionText.text += "CLEARED RUN! ACCELERATION! +5 points";
            }

            else
            {
                actualCompetitor.AddRunModifier(competition.currentRun, -5);
                descriptionText.text += "POOR STRATEGY! -5 points";
            }
        }
    }

    private void RiskTest()
    {
        descriptionText.text += "ROLL IS: " + eventRoll + ". ";

        if ((eventRoll == 1) && (actualCompetitor.grade >= 'C'))
        {
            descriptionText.text += "COMPETITOR TAKES RISK... OH NO... OUT!";
            if (competition.players.Count < 8)
            {
                actualCompetitor.myState = Player.PlayerState.OutOf15;
            }
            else if ((competition.players.Count > 7) && (competition.players.Count < 12))
            {
                actualCompetitor.myState = Player.PlayerState.Disqualified;
            }
            else
            {
                actualCompetitor.myState = Player.PlayerState.DidNotFinish;
            }
            surprise.SurpriseEffect(actualCompetitor);
            return;
        }

        else if (eventRoll % 2 == 0)
        {
            actualCompetitor.AddRunModifier(competition.currentRun, eventRoll * 2);
            descriptionText.text += actualCompetitor.secondName + " TAKES RISK... GREAT SPEED!" + " +" + (eventRoll * 2) + " pts.";
        }
        else
        {
            actualCompetitor.AddRunModifier(competition.currentRun, -eventRoll * 2);
            descriptionText.text += actualCompetitor.secondName + " TAKES RISK... NO GOOD..." + " -" + (-eventRoll * 2) + " pts.";
        }
    }

    private void TalentTest()
    {
        descriptionText.text += "ROLL IS: " + eventRoll + ". ";
        if ((actualCompetitor.experience == 0) || (actualCompetitor.experience == 3))
        {
            if (eventRoll == 1)
            {
                actualCompetitor.AddRunModifier(competition.currentRun, -6);
                descriptionText.text += "HORRIBLE MISTAKE by " + actualCompetitor.secondName + ".  -6pts.";
            }
            else if (eventRoll == 6)
            {
                actualCompetitor.AddRunModifier(competition.currentRun, 6);
                descriptionText.text += "GREAT SPEED of " + actualCompetitor.secondName + "!" + "  +6pts.";
            }
            else
            {
                descriptionText.text += actualCompetitor.secondName + " nearly falls down. It was close...";
            }
        }
        else if (weather.snowCondition.Contains("hard"))
        {
            actualCompetitor.AddRunModifier(competition.currentRun, eventRoll);
            descriptionText.text += "GREAT ROUTE CONDITION. " + "  +" + eventRoll + "points";
        }

        else if (gamemanager.competitionName.Contains("Slalom"))
        {
            descriptionText.text += "COMPETITOR MISSED THE GATE... OUT!";
            actualCompetitor.myState = Player.PlayerState.DidNotFinish;
            surprise.SurpriseEffect(actualCompetitor);
        }
        else
        {
            descriptionText.text += "GREAT RUN. Crowd goes wild!";
        }


    }
    private void UnderdogEnters()  // UNDERDOG ENTERS INTO OUTSIDERS or "OUTOF15" COMPETITOR IS BACK
    {
        Player underdog = null;

        if ((competition.currentRun > 1) && (competition.secondD6 % 2 == 0) &&
            (competition.outOf15Competitors.Count > 0) && (competition.possibleReturnsFromOutOf15 > 0))
        {
            underdog = competition.outOf15Competitors[0];
            underdog.myState = Player.PlayerState.Running; 
            descriptionText.text += underdog.secondName.ToString() + " IS BACK!";
            competition.outOf15Competitors.Remove(underdog);
            competition.outsiders.Insert(0, underdog);
            competition.possibleReturnsFromOutOf15--;
        }

        else
        {
            if (competition.underdogs.Count > 0)
            {
                underdog = competition.underdogs[0];
                underdog.GoodFormEffect();
                underdog.GoodFormEffect();
                descriptionText.text += "GOOD FORM of " + underdog.secondName.ToString() + "!";
                competition.underdogs.Remove(underdog);
                competition.outsiders.Insert(0, underdog); // INSERT At start
                competition.UpdatePlayerList(competition.outsiders, competition.outsidersList);
                competition.UpdatePlayerList(competition.underdogs, competition.underdogsList);

            }
            else if (competition.outsiders.Count > 0)
            {
                int index = Random.Range(0, competition.outsiders.Count);
                competition.outsiders[index].GoodFormEffect(); // Random OUTSIDER gets GOOD FORM
                descriptionText.text += "GOOD FORM of " + competition.outsiders[index].secondName + "!";
            }
        }
        competition.UpdateLists();
    }
    private void CloseEventWindow()
    {
        if ((eventResolved) && (Input.GetMouseButtonDown(0)))
        {
            eventObject.SetActive(false);
            eventResolved = false;
            // competition.eventHappened = false;
            //competition.ChangeState(Competition.GameState.CompetitionPhase);
        }

        // yield return new WaitForSeconds(2.00f);


        // runButton.SetActive(true);

    }








}
