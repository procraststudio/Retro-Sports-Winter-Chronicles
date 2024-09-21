using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New WorldCupCompetition", menuName = "WorldCupCompetition")]

public class WorldCupCompetition : ScriptableObject
{
    public string worldCupName;
    [SerializeField] public CompetitionType[] worldCupEvents;
    public int[] pointsToClassification = { 25, 20, 15, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
    
}
