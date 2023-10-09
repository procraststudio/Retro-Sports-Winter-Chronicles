using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;

public class ShortEvent : MonoBehaviour
{
    private int d6Roll = 0;
    private int eventRoll = 0;
    public bool eventRolled;
    [SerializeField] TMP_Text descriptionText;
    Competition competition;
    public int weatherModifier; // TO DO: from -3 (nightmare) to +3 (best) courseModifier
    private RolledEvents actualEvent;
    Player actualCompetitor;
    Weather weather;

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
        competition = FindObjectOfType<Competition>();
        weather = FindObjectOfType<Weather>();
        eventRolled = false;
        weatherModifier = (int)weather.weatherModifier; // TO DO" Check this value if not too high
    }


    public void GetEventType()
    {
        actualCompetitor = competition.currentCompetitor;
        d6Roll = competition.thirdD6;
        string description = "EVENT! " + d6Roll + " ";

        switch (d6Roll)
        {
            case 1:
                description += "BUMP!" + "\n" + "Course difficulty check. " +
                    "If EXP (" + actualCompetitor.experience + ") + difficulty modifier + d6 = 6 or more: " +
                    "the competitor stayed on the route; OTHERWISE: OUT, DNF!";
                actualEvent = RolledEvents.Bump;
                break;
            case 2:
                description += "CURVE!" + "\n" + "Disqualification check. " +
                    "If grade modifier (0-3) + d6 = 6 or more: no DQ. OTHERWISE: DQ.";
                actualEvent = RolledEvents.Curve;
                break;
            case 3:
                description += "WEATHER EFFECT!" + "\n" + "If snowing/raining: BUMP. " +
                    "If no: 7 or less competitors to run: +5 points.";
                actualEvent = RolledEvents.Weather;
                break;
            case 4:
                description += "TAKE RISK!" + "\n" + "If d6 result EVEN: +d6 points. " +
                    "If ODD: -d6 points. If d6=1 and GRADE C or worse: OUT OF 15/DNF/DQ.";
                actualEvent = RolledEvents.Risk;
                break;
            case 5:
                description += "TALENT/AM I TOO OLD?" + "\n" + "If EXP 0 or 3 see d6. " +
                    "If 1: -6 points, if 6: +6 points. OTHERWISE: If HARD snow +3 points.";
                actualEvent = RolledEvents.Talent;
                break;
            case 6:
                description += "BIG SURPRISE!" + "\n" + "If 2nd run: competitor from OUT OF 15 is back. " +
                    "OTHERWISE: UNDERDOG (grade +2) enters.";
                actualEvent = RolledEvents.Surprise;
                break;

        }
        descriptionText.color = Color.white;
        descriptionText.text = description.ToString();
        Debug.Log("Event described");
        ResolveEvent();
    }

    public void ResolveEvent()
    {
        eventRoll = Random.Range(1, 7);
        descriptionText.text += "\n" + "D6: " + eventRoll + ". ";


        switch (actualEvent)
        {
            case RolledEvents.Bump: BumpTest(); break;
            case RolledEvents.Curve: DisqualificationTest(); break;
            case RolledEvents.Weather: WeatherTest(); break;
            case RolledEvents.Risk: RiskTest(); break;
            case RolledEvents.Talent: TalentTest(); break;
            case RolledEvents.Surprise: Surprise(); break;
        }

    }
    private void BumpTest()
    {
        if ((eventRoll + actualCompetitor.experience - weatherModifier) > 5)
        {
            descriptionText.text += "GREAT TECHNIQUE! COMPETITOR IS NOT OUT";
        }
        else
        {
            descriptionText.text += "OH NO! " + actualCompetitor.name + " IS OUT";
            actualCompetitor.myState +=2; //DNF
            Debug.Log("STATE IS: " + actualCompetitor.myState.ToString());
            competition.SurpriseEffect(actualCompetitor);
        }
    }
    private void DisqualificationTest()
    {
        if ((eventRoll + actualCompetitor.GetGradeModifier() + weatherModifier) > 5)
        {
            descriptionText.text += "GREAT FORM! COMPETITOR IS IN!";
        }
        else
        {
            descriptionText.text += "OH NO! " + actualCompetitor.name + " IS DISQUALIFIED";
            actualCompetitor.myState += 3; // DQ
            competition.SurpriseEffect(actualCompetitor);
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
                descriptionText.text += "GREAT STRATEGY!";
            }
        }
    }

    private void RiskTest()
    {
        if ((eventRoll == 1) && (actualCompetitor.grade >= 'C'))
        {
            descriptionText.text += "COMPETITOR TAKES RISK... OH NO... OUT!!!";
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
            competition.SurpriseEffect(actualCompetitor);
            return;
        }

        else if (eventRoll % 2 == 0)
        {
            actualCompetitor.AddRunModifier(competition.currentRun, eventRoll);
            descriptionText.text += actualCompetitor.name + " TAKES RISK... GREAT SPEED! +" + eventRoll + " pts.";
        }
        else
        {
            actualCompetitor.AddRunModifier(competition.currentRun, -eventRoll);
            descriptionText.text += actualCompetitor.name + " TAKES RISK... FAILURE! -" + eventRoll + " pts.";
        }
    }

    private void TalentTest()
    {
        if ((actualCompetitor.experience == 0) || (actualCompetitor.experience == 3))
        {
            if (eventRoll == 1)
            {
                actualCompetitor.AddRunModifier(competition.currentRun, -6);
                descriptionText.text += "HORRIBLE MISTAKE! POOR " + actualCompetitor.name + ". -6pts.";
            }
            else if (eventRoll == 6)
            {
                actualCompetitor.AddRunModifier(competition.currentRun, 6);
                descriptionText.text += "GREAT SPEED OF " + actualCompetitor.name + "! +6pts.";
            }
            else
            {
                descriptionText.text += actualCompetitor.name + " NEARLY FALLS DOWN. IT WAS CLOSE...";
            }
        }
        else if (weather.snowCondition.Contains("hard"))
        {
            actualCompetitor.AddRunModifier(competition.currentRun, 3);
            descriptionText.text += "GREAT SNOW CONDITION. +3 points";
        }
        else
        {
            descriptionText.text += "GREAT ACCELERATION";
        }


    }
    private void Surprise()  // UNDERDOG ENTERS INTO OUTSIDERS
    {
        Player underdog = competition.underdogs[0];
        underdog.GoodFormEffect();
        descriptionText.text += "GOOD FORM OF UNDERDOG! " + underdog.name.ToString();
        competition.underdogs.Remove(underdog);
        competition.outsiders.Insert(0, underdog);
        competition.UpdatePlayerList(competition.outsiders, competition.outsidersList);
        competition.UpdatePlayerList(competition.underdogs, competition.underdogsList);
    }




}
