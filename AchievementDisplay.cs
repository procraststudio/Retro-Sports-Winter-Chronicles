using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{
    public Achievement achievement;
    [SerializeField] TMP_Text achievementName;
    [SerializeField] TMP_Text rarity;
    [SerializeField] TMP_Text description;
    public Image achievementImage;
    Competition competition;

    //public TextMeshProUGUI nationalityText; 
    //public SpriteRenderer flagRenderer;
    //public string flagsFolderPath = "flags/";
    void Start()
    {
        competition = Competition.Instance;
        //flagSprite = Resources.Load<Sprite>(flagsFolderPath);

    }


    void Update()
    {
        DisplayAchievement(achievement);    
    }

    public void DisplayAchievement(Achievement achievement)
    {
        // competition = Competition.Instance;
        achievementName.text = achievement.achievementName.ToString();
        rarity.text = achievement.rarity.ToString();
        description.text = achievement.description.ToString();
       // achievementImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + player.nationality);
        //achievementImage = achievement.achievementImage;
        // achievementImage.sprite = achievement.artwork;

    }
}
