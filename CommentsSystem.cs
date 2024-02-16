using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CommentsSystem : MonoBehaviour
{
    Competition competition;
    private string textPath = "commentator_good";
    private List<string> unusedComments = new List<string>();
    [SerializeField] public TMP_Text[] commentText;
    void Start()
    {
        competition = Competition.Instance;
        LoadFileLines();
        // favourites = competition.players;
       // CheckEmptyFile();
    }

    // Update is called once per frame
    void Update()
    {
       // CheckEmptyFile();
    }

    private void CheckEmptyFile()
    {
        if (unusedComments.Count == 0)
        {
            LoadFileLines();
        }
    }

    public void showComments(Player player, int partOfRun, float scoredPoints)
    {
         
        if ((unusedComments.Count > 0) && (scoredPoints > 18))
        {
            int randomIndex = Random.Range(0, unusedComments.Count);
            string randomLine = unusedComments[randomIndex];
            commentText[partOfRun-1].text = "</color=green>" +randomLine.ToString() + "<color=green>";
            unusedComments.RemoveAt(randomIndex);  
        }
        else if (scoredPoints < 8)
        {
            commentText[partOfRun - 1].text = "</color=red>" + "POOR RUN..." + "<color=red>";
        }
        else
        {
            Debug.Log("No more unused lines/No comment.");
        }
        CheckEmptyFile();
    }
    private void LoadFileLines()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(textPath);
        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    unusedComments.Add(line.Trim());
                }
            }
            Debug.Log("Comments loaded");
        }
        else
        {
            Debug.LogError("Text file not found.");
        }
    }

    public void ResetComments()
    {
        for (int i = 0; i <commentText.Length; i++)
        {
            commentText[i].text = "";
        }
    }
}
