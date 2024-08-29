using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]


public class Achievement : ScriptableObject
{
    public string achievementName;
    public string achievementType;
    public bool isHighlight;
    public string rarity;
    public string description;
    public Sprite artwork;

    //public Achievement(string achievementName, string achievementType, bool isHighlight, string rarity, string description, Sprite achievementImage)
    //{
    //    this.achievementName = achievementName;
    //    this.achievementType = achievementType;
    //    this.isHighlight = isHighlight; 
    //    this.rarity = rarity;
    //    this.description = description;
    //    this.achievementImage = achievementImage;
    //}
    public enum rarityOfAchievement
    {
        common = 1, rare = 2, legendary = 3, epic = 4
    }

    public enum typesOfAchievement
    {
        alpineSki, biathlon, skiJumping, winterSports, allSports
    }


    public int CalculatePointsForAchievement(string rarity)
    {
        switch (rarity)
        {
            case "common": return 50; 
            case "rare": return 500;
            case "legendary": return 5000;
            case "epic": return 50000;
            default: return 0;
        }
    }
}
