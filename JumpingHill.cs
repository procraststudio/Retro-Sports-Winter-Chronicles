using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Jumping Hill", menuName = "Jumping Hill")]

public class JumpingHill : ScriptableObject
{
    public string jumpingHillName;
    public string hillType; // normal, large, flying...
    public float pointK;
    public float hillRecord;
    public string recordHolder;
    public DateTime dateOfRecord;
    public string description;
    // public Sprite jumpingHillArtwork;
    
}
