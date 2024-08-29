using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New CompetitionType", menuName = "CompetitionType")] 

public class CompetitionType : ScriptableObject
{
    public string competitionType;
    public string competitionName;
    public DateTime competitionDate;
    public string competitionVenueName;
    public string competitorsDatabase;
    public int numberOfRuns;
    //public GameObject competitorsPack;
    public float bestTimeinSec; // TO DO make it common: time, points, metres 
    public float tenthTimeinSec;
    public bool IsCombined;
    public bool timeIntervals;
    public string firstCombinedCompetition;
    public string secondCombinedCompetition;
    public string bonusDatabaseName;
    public bool noWeatherImpact = false;
    public bool judgesSystem = false;   
    public bool resultsInMetres = false;


    //public CompetitionType(string competitionName, DateTime date, string venueName, Player[] competitors, float bestTime, float tenthTime)
    //{
    //    this.competitionName = competitionName;
    //    this.competitionDate = date;
    //    this.competitionVenueName = venueName;
    //    this.competitorsPack = competitors;
    //    this.bestTimeinSec = bestTime;
    //    this.tenthTimeinSec = tenthTime;

    //}
}
public enum typesOfCompetitions
{
    alpineSki,
    biathlon,
    skiJumping,
    bobsleigh
}

public class WorldCupCompetition
{
    public CompetitionType[] WCCompetitions; //place here competitions with date/name/Venues
    public int[] pointsToClassification = { 25, 20, 15, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
    // public string competition;
}