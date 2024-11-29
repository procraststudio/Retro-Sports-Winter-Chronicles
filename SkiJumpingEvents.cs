using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkiJumpingEvents : MonoBehaviour
{
    public int saveRoll;
    ShortEvent shortEvent;
    public TMP_Text descriptionText;
    string description = "";
    Weather weather;
    Competition competition;
    void Start()
    {
        shortEvent = GetComponentInParent<ShortEvent>();
        weather = FindObjectOfType<Weather>();
        competition = Competition.Instance;
    }


    public void ResolveSkiJumpingEvent(Player player, int dieRoll)
    {
        Debug.Log("SKI JUMPING EVENT");
        // Weather.windCondition = "light";
        //Weather.windDirection = "tail";
        description = "";
        string windDescription = Weather.windCondition.ToString();
        string windDirection = Weather.windDirection.ToString();
        string precipitation = Weather.precipitation.ToString();
        switch (dieRoll)
        {
            case 1:
                if (Weather.windCondition != "calm")
                {
                    // string windDescription = Weather.windCondition.ToString(); 
                    Debug.Log("WIND IMPACT");
                    EventHappened();
                    WindImpact(player, windDescription, windDirection);
                    //actualCompetitor.AddRunModifier(competition.currentRun, 6);
                    //descriptionText.text += "GREAT SPEED of " + actualCompetitor.secondName + "!" + "  +6pts. "; break;
                }
                else
                {
                    return;
                }
                break;
            case 2:
                if ((Weather.windCondition != "calm") || (Weather.windCondition != "light"))
                {
                    EventHappened();
                    WindImpact(player, windDescription, windDirection);
                }
                else
                {
                    return;
                }
                break;
            case 3:
                if (Weather.windCondition == "strong")
                {
                    EventHappened();
                    WindImpact(player, windDescription, windDirection);
                }
                break;
            case 4:
                EventHappened();
                JumpControl(player, 0); // average 25% to OUT of 15
                break;
            case 5:
                if (windDirection == "gusts")
                {
                    EventHappened();
                    JumpControl(player, 0);// average 25% to OUT of 15
                }
                break;
            case 6:
                // if (precipitation != "none")
                // {
                EventHappened();
                shortEvent.UnderdogEnters();
                description += " OUTSIDER'S GOOD FORM. ";
                break;
            default: Debug.Log("NO IMPACT"); break;
                //        saveRoll = 100 * ((player.experience - weatherModifier)) / 6;
                //        if (saveRoll < 1)
                //        {
                //            saveRoll = 10;
                //        }
                //        Debug.Log("SAVE ROLL: " + saveRoll);//Convert.ToInt32(((actualCompetitor.experience - weatherModifier)/6)*100);
                //        description += "BUMP!".ToUpper(); // + "\n" +
                //                                          // saveRoll.ToString() + "%: the competitor stays on the route" + "\n"
                //                                          // + (100 - saveRoll).ToString() + "%: OUT, DNF! ";
                //        actualEvent = RolledEvents.Bump;
                //        break;
                //    case 2:
                //        saveRoll = 100 * (actualCompetitor.GetGradeModifier() - weatherModifier - GetCompetitionModifier()) / 6;
                //        if (saveRoll < 1)
                //        {
                //            saveRoll = 10;
                //        }
                //        description += "CURVE!".ToUpper(); // + "\n" +
                //                                           // saveRoll.ToString() + "%: no DQ" + "\n"
                //                                           // + (100 - saveRoll).ToString() + "%: DISQUALIFIED! ";
                //        actualEvent = RolledEvents.Curve;
                //        break;
                //    case 3:
                //        description += "WEATHER EFFECT!".ToUpper(); // + "\n" + "If snowing/raining: BUMP. " +
                //                                                    //"OTHERWISE: If 7 or less competitors to run: CLEARED RUN" + "\n" +
                //                                                    // "If 8 or more: POOR STRATEGY";
                //        actualEvent = RolledEvents.Weather;
                //        break;
                //    case 4:
                //        description += "TAKING RISK!".ToUpper(); // + "\n" + "Roll d6. If result EVEN: +d6 points. " +
                //                                                 //  "\n" + "If result ODD: -d6 points." + "\n" + "If d6 is 1 and GRADE C or worse: OUT OF 15/DNF/DQ.";
                //        actualEvent = RolledEvents.Risk;
                //        break;
                //    case 5:
                //        description += "EXPERIENCE MATTERS".ToUpper(); // + "\n" + "Roll d6. If EXP 0 or 3 see d6. " +
                //                                                       // "\n" + "If 1: -6 points, if 6: +6 points." + "\n" + "OTHERWISE: If HARD snow +d6 points.";
                //        actualEvent = RolledEvents.Talent;
                //        break;
                //    case 6:
                //        description += "BIG SURPRISE!".ToUpper(); // + "\n" + "If 2nd run: competitor from OUT OF 15 is back. " +
                //                                                  // "\n" + "OTHERWISE: UNDERDOG (with +2 grade) enters.";
                //        actualEvent = RolledEvents.Surprise;
                //        break;

                //}

                //


                //Debug.Log("Event described");
                //eventRolled = true;
                //ResolveEvent();
        }
        descriptionText.text = description.ToString();
    }

    public void WindImpact(Player player, string windDescription, string windDirection)
    {
        if (windDirection == "head")
        {
            description += "HEAD WIND!".ToUpper();
            switch (windDescription)
            {
                case "light":
                    player.AddRunModifier(competition.currentRun, -5 + (player.GetGradeModifier() * 2));
                    description += " LOW DISTANCE LOSS. ";
                    JumpControl(player, -20);
                    break;
                case "medium":
                    player.AddRunModifier(competition.currentRun, -10 + (player.GetGradeModifier() * 2));
                    description += " DISTANCE LOSS. ";
                    JumpControl(player, -10);
                    break;
                case "strong":
                    player.AddRunModifier(competition.currentRun, -15 + (player.GetGradeModifier() * 2));
                    description += " HIGH DISTANCE LOSS. ";
                    JumpControl(player, 5);


                    break;
            }
        }
        else if (windDirection == "tail")
        {
            description += "TAIL WIND!".ToUpper();
            switch (windDescription)
            {
                case "light":
                    player.AddRunModifier(competition.currentRun, 5 + (player.GetGradeModifier() * 2));
                    description += " LOW DISTANCE GAIN. ";
                    break;
                case "medium":
                    JumpControl(player, -15);
                    player.AddRunModifier(competition.currentRun, 10 + (player.GetGradeModifier() * 2));
                    description += " DISTANCE GAIN. ";
                    break;
                case "strong":
                    JumpControl(player, -5);
                    player.AddRunModifier(competition.currentRun, 15 + (player.GetGradeModifier() * 2));
                    description += " LARGE DISTANCE GAIN. ";
                    break;
            }
        }
        else if (windDirection == "gusts")
        {
            description += "GUSTS! ".ToUpper();
            switch (windDescription)
            {
                case "light":
                    JumpControl(player, -15);
                    break;
                case "medium":
                    JumpControl(player, 0);
                    break;
                case "strong":
                    JumpControl(player, 15);
                    break;
            }
        }
        else
        {
            int smallDistance = Random.Range(-3, 4);
            player.AddRunModifier(competition.currentRun, smallDistance);
            description += "SMALL DISTANCE GAIN/LOSS";
            Debug.Log("SMALL DISTANCE DAIN/LOSS: " + smallDistance);
        }
    }

    public void JumpControl(Player player, int modifier)
    {
        // modifier -13 and player rank 8 & exp 1 means 3% for losing jump control
        int lostControlChance = Random.Range(1, 101) - modifier - (player.ranking - player.experience);
        if (lostControlChance < 10)
        {
            Debug.Log("JUMPER LOST CONTROL");
            description += player.secondName.ToString().ToUpper() + " LOST CONTROL. OUT OF 15";
            player.myState = Player.PlayerState.OutOf15;
            shortEvent.surprise.SurpriseEffect(player);
            //player.AddRunModifier(competition.currentRun, -25 + (player.GetGradeModifier() * 2));
        }
        else
        {
            description += player.secondName.ToString().ToUpper() + " DIDN'T LOSE CONTROL.";
        }
        //descriptionText.text = description.ToString();
    }
    public void EventHappened()
    {
        shortEvent.runButton.SetActive(false);
        shortEvent.eventObject.SetActive(true);
        shortEvent.eventTitle.text = "EVENT".ToString();
        shortEvent.descriptionText.text += "\n" + ">>>>>";
    }
}
