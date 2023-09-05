using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static Competition;

public class EventsManager : MonoBehaviour
{
    public bool diceChanging;
    private int eventPhase;
    private int d6Roll = 0;
    public bool eventRolled;
    private float pause = 0.30f;
    public TMP_Text buttonText;
    [SerializeField] Sprite[] diceSides;
    [SerializeField] GameObject[] firstDieImages;
    // [SerializeField] GameObject[] secondDieImages;
    [SerializeField] GameObject[] eventCharts;
    [SerializeField] TMP_Text[] descriptionTexts;
    [SerializeField] private GameObject eventsPanelPrefab;
    [SerializeField] private GameObject resolveEventPanel;

    Competition competition;
    public int weatherModifier; // TO DO: from -3 (nightmare) to +3 (best) courseModifier
    private bool eventPhaseOver = false;
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
        eventPhase = 1;
        eventRolled = false;
        actualCompetitor = competition.currentCompetitor;
        //   ResetEventCharts();
        weatherModifier = (int)weather.weatherModifier;
        resolveEventPanel.SetActive(false);    
    }


    public void ShowEvent()
    {
        //Task.Delay(1000).Wait();
        if (eventPhase == 1)
        {
            d6Roll = Random.Range(1, 7);
            string description = "";
            firstDieImages[0].GetComponent<SpriteRenderer>().sprite = diceSides[d6Roll - 1];

            switch (d6Roll)
            {
                case 1:
                    description = "BUMP!" + "\n" + "Course difficulty check. " +
                        "If EXP (" + actualCompetitor.experience + ") + difficulty modifier + d6 = 6 or more: " +
                        "the competitor stayed on the route; OTHERWISE: OUT, DNF!";
                    actualEvent = RolledEvents.Bump;
                    break;
                case 2:
                    description = "CURVE!" + "\n" + "Disqualification check. " +
                        "If grade modifier (0-3) + d6 = 6 or more: no DQ. OTHERWISE: DQ";
                    actualEvent = RolledEvents.Curve;
                    break;
                case 3:
                    description = "WEATHER EFFECT!" + "\n" + "If snowing/raining: BUMP " +
                        "If no: 7 or less competitors to run: +5 points";
                    actualEvent = RolledEvents.Weather;
                    break;
                case 4:
                    description = "TAKE RISK!" + "\n" + "If d6 result EVEN: +d6 points " +
                        "If ODD: -d6 points. If >one< and GRADE C or worse: OUT OF 15/DNF/DQ";
                    actualEvent = RolledEvents.Risk;
                    break;
                case 5:
                    description = "TALENT/AM I TOO OLD?" + "\n" + "If EXP 0 or 3 see d6 " +
                        "If 1: -6 points, if 6: +6 points. OTHERWISE: If HARD snow +3 points";
                    actualEvent = RolledEvents.Talent;
                    break;
                case 6:
                    description = "BIG SURPRISE!" + "\n" + "If 2nd run: competitor from OUT OF 15 is back" +
                        "OTHERWISE: UNDERDOG(grade +2) enters";
                    actualEvent = RolledEvents.Surprise;
                    break;

            }
            descriptionTexts[0].text = description.ToString();
            eventPhase++;
            resolveEventPanel.SetActive(true);
            //eventCharts[0].SetActive(true);
            // StartCoroutine("EventDice");
            // return actualTemperature;
        }
    }

    public void ResolveEvent()
    {
        //Task.Delay(1000).Wait();
        if (eventPhase == 2)
        {
            d6Roll = Random.Range(1, 7);
            firstDieImages[1].GetComponent<SpriteRenderer>().sprite = diceSides[d6Roll - 1];
            //StartCoroutine("EventDice");
            // string description = "";
            descriptionTexts[1].text += "";

            switch (actualEvent)
            {
                case RolledEvents.Bump: BumpTest(); break;
                case RolledEvents.Curve: DisqualificationTest(); break;
                case RolledEvents.Weather: WeatherTest(); break;
                case RolledEvents.Risk: RiskTest(); break;
                case RolledEvents.Talent: TalentTest(); break;
                case RolledEvents.Surprise: Surprise(); break;
            }
            eventPhase++;
        }
        eventPhaseOver = true;
        // TO DO: END EVENT PHASE

    }

    private void BumpTest()
    {
        if ((d6Roll + actualCompetitor.experience - weatherModifier) > 5)
        {
            descriptionTexts[1].text += "GREAT TECHNIQUE! COMPETITOR IS NOT OUT";
        }
        else
        {
            descriptionTexts[1].text += "OH NO! "  + actualCompetitor.name +" IS OUT";
        }
    }
    private void DisqualificationTest()
    {
        if ((d6Roll + actualCompetitor.GetGradeModifier() + weatherModifier) > 5)
        {
            descriptionTexts[1].text = "GREAT FORM! COMPETITOR IS IN!";
        }
        else
        {
            descriptionTexts[1].text = "OH NO! " + actualCompetitor.name +" IS DISQUALIFIED";
            // TO DO: player to DQ list
        }
    }

    private void WeatherTest()
    {
        if ((weather.precipitation.Contains("snowing")) || (weather.precipitation.Contains("raining")))
        {
            descriptionTexts[1].text = "DANGEROUS BUMP...";
            BumpTest();
        }
        else if (competition.players.Count < 8)
        {
            actualCompetitor.runModifier += 5;
            descriptionTexts[1].text = "CLEARED RUN! +5 points";
        }
        else
        {
            descriptionTexts[1].text = "GREAT STRATEGY!";
        }
    }

    private void RiskTest()
    {
        if ((d6Roll == 1) && (actualCompetitor.grade >= 'C'))
        {
            descriptionTexts[1].text = "COMPETITOR TAKES RISK... OH NO... OUT!!!"; return;
            //TO DO: competitor OUT OF 15 / DQ / DNF
        }

        if (d6Roll % 2 == 0)
        {
            actualCompetitor.runModifier += d6Roll;
            descriptionTexts[1].text =  actualCompetitor.name + " TAKES RISK... GREAT SPEED! +" + d6Roll + " pts.";
        }
        else
        {
            actualCompetitor.runModifier -= d6Roll;
            descriptionTexts[1].text = actualCompetitor.name + " TAKES RISK... FAILURE! -" + d6Roll + " pts.";
        }
    }

    private void TalentTest()
    {
        if ((actualCompetitor.experience == 0) || (actualCompetitor.experience == 3))
        {
            if (d6Roll == 1)
            {
                actualCompetitor.runModifier -= 6;
                descriptionTexts[1].text = "BIG MISTAKE! POOR " + actualCompetitor.name + ". -6pts.";
            }
            else if (d6Roll == 6)
            {
                actualCompetitor.runModifier += 6;
                descriptionTexts[1].text = "GREAT SPEED OF " + actualCompetitor.name +"! +6pts.";
            }
            else
            {
                descriptionTexts[1].text = actualCompetitor.name + " NEARLY FELL. IT WAS CLOSE...";
            }
        }
        else if (weather.snowCondition.Contains("hard"))
        {
            actualCompetitor.runModifier += 3;
            descriptionTexts[1].text = "GREAT SNOW CONDITION. +3 points";
        }
        else
        {
            descriptionTexts[1].text = "GREAT ACCELERATION";
        }
        

        }
    private void Surprise ()
    {
        descriptionTexts[1].text = "UNDERDOG ENTERS";
        // TO DO: UNDERDOG ENTERS, whats with current competitor?
    }

        public IEnumerator EventDice()
        {
            // ResetEventCharts();
            diceChanging = true;
            // firstD6 = Random.Range(1, 7);
            // secondD6 = Random.Range(1, 7);


           // firstDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[d6Roll - 1];
           yield return new WaitForSeconds(pause);
            // ResolveEvent();


            //thirdDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.thirdD6 + 11];
            // description.ShowDescription();
            // weatherCharts[0].SetActive(true);
            diceChanging = false;
            //diceIndex++;

        }



        public void EventsButton()
        {
            if (competition.myState != GameState.EventPhase)
            {
                competition.myState = GameState.EventPhase;
                GameObject newObject = Instantiate(eventsPanelPrefab);
                newObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
                newObject.transform.localPosition = new Vector3(13.37f, 40.14f, -2657.00f);
                Debug.Log("EVENTS STATE");

            }
        }

        public void ResetEventCharts()
        {
            for (int i = 0; i < eventCharts.Length; i++)
            {
                eventCharts[i].SetActive(false);
            }
        }

        public void ChangeButtonName(string buttonName)
        {
            buttonText.text = buttonName.ToString();
        }

    }
