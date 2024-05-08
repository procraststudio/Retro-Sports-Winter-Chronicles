using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New CompetitionType", menuName = "CompetitionType")]

public class CompetitionType
{
    public string competitionName;
    public DateTime competitionDate;
    public string competitionVenueName;
}

public class WorldCupCompetition
{
    public CompetitionType [] WCCompetitions; //place here competitions with date/name/Venues
    public int[] pointsToClassification = {25,20,15,12,11,10,9,8,7,6,5,4,3,2,1};
    // public string competition;
}