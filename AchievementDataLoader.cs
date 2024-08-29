using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementDataLoader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject achievementObject;
    public RectTransform rectTransform;
    public List<GameObject> allAchievements = new List<GameObject>();
    public List<GameObject> currentAchievements = new List<GameObject>();
    public float fadeTime = 1f;
    AchievementsManager manager;
    public string achievementsFolderPath = "Achievements/";

    void Start()
    {
        manager = AchievementsManager.Instance;
        //achievementsLoaded = false;
        //competition = Competition.Instance;
        //tag = this.gameObject.tag;
        LoadAchievements();
    }


    public void LoadAchievements()
    {
        if (manager.achievementsKeywords != null)
        {
            // var files = Directory.GetFiles(resourcesPath);
            // Filtruj pliki, które maj¹ identyczne nazwy jak w liœcie "names"
            //newItems = files.Where(file => manager.achievementsKeywords.Contains(Path.GetFileName(file))).ToList();

            //if ()

            //if (manager.achievementsKeywords[i].Contains)
            //  var tempObj = new GameObject();
            //  tempObj = Resources.Load<GameObject>(achievementsFolderPath + manager.achievementsKeywords);
            // items.Add(tempObj);
            // Debug.Log("ACHIEVEMENT ADDED");

            currentAchievements = allAchievements.Where(file => manager.achievementsKeywords.Contains(file.name.ToString())).ToList();

            canvasGroup.alpha = 1f;
            rectTransform.transform.localPosition = new Vector3(0f, -300f, 0f);
            rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
            canvasGroup.DOFade(1, fadeTime);
            StartCoroutine("ItemsAnimation");
        }

    }
    IEnumerator ItemsAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        foreach (var item in currentAchievements)
        {
            achievementObject = Instantiate(item, rectTransform);
            item.transform.localScale = Vector3.zero;

        }
        foreach (var item in currentAchievements)
        {
            //    // play sound
            item.transform.DOScale(1f, fadeTime).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.25f);

        }
        Debug.Log("ACHIEVEMENTS LOADED");
    }


}
