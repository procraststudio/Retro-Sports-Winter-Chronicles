using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CalculatePerformance : MonoBehaviour
{
    Competition competition;
    int thirdD6 = 0;
    int currentRun;
    Player currentCompetitor;
    [SerializeField] GameObject dicePanel;
    //[SerializeField] GameObject judgesSystemScript;
    private JudgesSystem judgesSystem;
    void Start()
    {
        competition = Competition.Instance;
        judgesSystem = this.gameObject.GetComponent<JudgesSystem>();
    }

    public void GetPerformanceModifier(Player currentCompetitor, int competitionRoll)
    {
        //currentCompetitor = competition.currentCompetitor;
        thirdD6 = competition.thirdD6;
        switch (competitionRoll)
        {
            case 2:
                currentCompetitor.PoorFormEffect();
                calculatePerformance(currentCompetitor, -1, thirdD6); break; //DISASTER
            case 3: currentCompetitor.PoorFormEffect(); calculatePerformance(currentCompetitor, 0, thirdD6); break; // MEDIOCRE
            case 4:
                if (currentCompetitor.grade > 2)
                { calculatePerformance(currentCompetitor, 1, thirdD6); }
                else { calculatePerformance(currentCompetitor, 0, thirdD6); }
                break;

            case 5:
                if (currentCompetitor.grade > 3)
                { calculatePerformance(currentCompetitor, 2, thirdD6); }
                else { calculatePerformance(currentCompetitor, 1, thirdD6); }
                break; // POOR
            case 6:
                if (currentCompetitor.grade > 3)
                { calculatePerformance(currentCompetitor, 3, thirdD6); }
                else if (currentCompetitor.grade == 'E')
                { calculatePerformance(currentCompetitor, 1, thirdD6); }
                else { calculatePerformance(currentCompetitor, 2, thirdD6); }
                break; // BELOW AVERAGE

            case 7:
                if (currentCompetitor.grade > 3)
                { calculatePerformance(currentCompetitor, 4, thirdD6); }
                else if (currentCompetitor.grade == 'C')
                { calculatePerformance(currentCompetitor, 3, thirdD6); }
                else { calculatePerformance(currentCompetitor, 2, thirdD6); }
                break; //AVERAGE

            case 8:
                if (currentCompetitor.CheckHomeFactor()) //HOME FACTOR +thirdD6
                {
                    currentCompetitor.AddRunModifier(competition.currentRun, thirdD6);
                    Debug.Log("HOME FACTOR: +" + thirdD6 * 2);
                }
                if (currentCompetitor.grade > 3)
                { calculatePerformance(currentCompetitor, 5, thirdD6); }
                else if (currentCompetitor.grade == 'C')
                { calculatePerformance(currentCompetitor, 4, thirdD6); }

                else { calculatePerformance(currentCompetitor, 3, thirdD6); };
                break; //GOOD

            case 9:
                if ((currentCompetitor.grade > 3) || (currentCompetitor.grade == 'C'))
                { calculatePerformance(currentCompetitor, 5, thirdD6); }
                else if (currentCompetitor.grade == 'D')
                { calculatePerformance(currentCompetitor, 4, thirdD6); }
                else { calculatePerformance(currentCompetitor, 3, thirdD6); }; break; //PRETTY GOOD

            case 10:

                if (currentCompetitor.grade > 4)
                { calculatePerformance(currentCompetitor, 6, thirdD6); }
                else if ((currentCompetitor.grade == 'B') || (currentCompetitor.grade == 'C') || (currentCompetitor.grade == 'D'))
                { calculatePerformance(currentCompetitor, 5, thirdD6); }
                else
                { calculatePerformance(currentCompetitor, 4, thirdD6); }; break; // VERY GOOD

            case 11:
                currentCompetitor.GoodFormEffect();
                if (currentCompetitor.grade > 4)
                {
                    calculatePerformance(currentCompetitor, 6, thirdD6 + 1);
                }
                else if (currentCompetitor.grade == 'E')
                { calculatePerformance(currentCompetitor, 5, thirdD6); }

                else { calculatePerformance(currentCompetitor, 6, thirdD6); }; break; //GREAT!
            case 12:
                if (currentCompetitor.grade == 'A')
                {
                    calculatePerformance(currentCompetitor, 7, thirdD6 + 3);
                }
                else if (currentCompetitor.grade == 'X')
                {
                    calculatePerformance(currentCompetitor, 7, thirdD6 + 5);
                }
                else
                {
                    currentCompetitor.GoodFormEffect();
                    calculatePerformance(currentCompetitor, 7, thirdD6);
                };
                break; // CRIT SUCCESS
        }
    }

    private void calculatePerformance(Player player, int modifier, int thirdDie)
    {
        currentRun = competition.currentRun;
        currentCompetitor = competition.currentCompetitor;
        //competition.playerDisplay.DisplayCompetitor(currentCompetitor, currentRun);
        float currentPlayerPoints;

        switch (modifier)
        {
            case -1:
                if (modifier > -6) { player.AddRunModifier(currentRun, -6); }
                else { player.AddRunModifier(currentRun, thirdDie * (-2)); }; break;
            // description.StoreDescription(Color.red, "DISASTER");
            case 0: player.AddRunModifier(currentRun, thirdDie * (-2)); break;
            case 1: player.AddRunModifier(currentRun, thirdDie * (-1)); break;
            case 2: player.AddRunModifier(currentRun, (thirdDie + 1) / -2); break;
            case 3:
                if (thirdDie < 3) { player.AddRunModifier(currentRun, Random.Range(-4, -1)); }
                else if (thirdDie > 4) { player.AddRunModifier(currentRun, Random.Range(1, 4)); }
                else { player.AddRunModifier(currentRun, 0); }; break;
            case 4:
                if (thirdDie < 3) { player.AddRunModifier(currentRun, Random.Range(-3, 0)); }
                else if (thirdDie > 4) { player.AddRunModifier(currentRun, Random.Range(2, 5)); }
                else { player.AddRunModifier(currentRun, 0); }; break;
            case 5: player.AddRunModifier(currentRun, (thirdDie + 1) / 2); break;
            case 6: player.AddRunModifier(currentRun, thirdDie); break;
            case 7:
                if (modifier < 6) { player.AddRunModifier(currentRun, 6); }
                else if (competition.thirdD6 == 6) // 666 ROLL EFFECT
                {
                    player.AddRunModifier(currentRun, thirdDie * 3);
                    SoundManager.PlayOneSound("dice_combo");
                }
                else { player.AddRunModifier(currentRun, thirdDie * 2); }
                break;
        }


        if (currentRun == 1)
        {
            currentPlayerPoints = player.firstRunPoints;
        }
        else
        {
            currentPlayerPoints = player.secondRunPoints;
        }



        player.CalculateActualRun(currentRun);
        if (FindObjectOfType<Gamemanager>().thisCompetition.timeIntervals)
        {
            checkSectorTime(currentRun, currentPlayerPoints, player);
        }
        player.CalculateFinal();
        
        if ((competition.partsOfRun == 3) && (player.myState == Player.PlayerState.Running))
        {
            if (currentRun == 1)
            {
                ConvertPointToDistance(player, player.firstRunPoints);
            }
            else
            {
                ConvertPointToDistance(player, player.secondRunPoints);
            }
        }

        competition.showResults(currentCompetitor);
        player.homeFactor = false;
    }

    private void checkSectorTime(int currentRun, float currentPlayerPoints, Player player)
    {
        if (currentRun == 1)
        {
            CalculateSectorTime(player, player.firstRunPoints - currentPlayerPoints);
        }
        else
        {
            CalculateSectorTime(player, player.secondRunPoints - currentPlayerPoints);
        }

    }

    private void CalculateSectorTime(Player player, float sectorPoints)
    {
        int partsOfRun = competition.partsOfRun;
        //float firstSectorMostPoints = competition.firstSectorMostPoints;
        //float secondSectorMostPoints = competition.secondSectorMostPoints;
        //float thirdSectorMostPoints = competition.thirdSectorMostPoints;


        if (player.myState == Player.PlayerState.Running)
        {
            float differenceInPoints = 0;
            switch (partsOfRun)
            {
                case 1:
                    if (sectorPoints > competition.firstSectorMostPoints)
                    {
                        differenceInPoints = competition.firstSectorMostPoints - sectorPoints;
                        competition.firstSectorMostPoints = sectorPoints;
                        Debug.Log("1st SECTOR RECORD: " + sectorPoints);
                        Debug.Log("DIFFERENCE: " + differenceInPoints);
                        // GREEN ARROW UP 
                    }
                    else
                    {
                        differenceInPoints = competition.firstSectorMostPoints - sectorPoints;
                    }
                    break;
                case 2:
                    if (sectorPoints > competition.secondSectorMostPoints)
                    {
                        differenceInPoints = competition.secondSectorMostPoints - sectorPoints;
                        competition.secondSectorMostPoints = sectorPoints;
                        Debug.Log("2nd SECTOR RECORD: " + sectorPoints);
                    }
                    else
                    {
                        differenceInPoints = competition.secondSectorMostPoints - sectorPoints;
                    }
                    break;
                case 3:
                    if (sectorPoints > competition.thirdSectorMostPoints)
                    {
                        differenceInPoints = competition.thirdSectorMostPoints - sectorPoints;
                        competition.thirdSectorMostPoints = sectorPoints;
                        Debug.Log("3rd SECTOR RECORD: " + sectorPoints);
                    }
                    else
                    {
                        differenceInPoints = competition.thirdSectorMostPoints - sectorPoints;
                    }
                    break;
            }
            // SHOW DIFFERENCE
            Debug.Log(player.ConvertDifference(differenceInPoints));
            dicePanel.GetComponent<Dice>().UpdateTimeGap(player, differenceInPoints);
            dicePanel.GetComponent<CommentsSystem>().showComments(player, partsOfRun, sectorPoints);
        }

    }

    private void ConvertPointToDistance(Player competitor, float points)
    {
        // locate Kpoint of actual jumping hill
        float distance = 89.0f - ((80.0f - points) * 0.2f);
        float roundedDistance = RoundToNearestHalf(distance);//89 is K point    
        if (currentRun == 1)
        {
            competitor.firstRunDistance = roundedDistance;
            competitor.skiJumpingPoints += ConvertDistanceToSkiJumpingPoints(competitor, roundedDistance);
        }
        else
        {
            competitor.secondRunDistance = roundedDistance;
            competitor.skiJumpingPoints += ConvertDistanceToSkiJumpingPoints(competitor, roundedDistance);
        }

    }
    private double ConvertDistanceToSkiJumpingPoints(Player competitor, float distance)
    {
        double baseDistance = 89.0;
        double basePoints = 60.0;
        double pointsPerHalfMeter = 0.8;
        double difference = Convert.ToDouble(distance) - baseDistance;
        double points = basePoints + (difference / 0.5) * pointsPerHalfMeter;
        double judgesPoints = judgesSystem.GetSkiJumpingNotes(competitor, distance);
        points += judgesPoints;
        return points;
    }

    public static float RoundToNearestHalf(float value)
    {
        return (float)(Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2);
    }



}
