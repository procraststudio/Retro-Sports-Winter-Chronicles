using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommentsSystem : MonoBehaviour
{
    Competition competition;
    Dice dice;
    private string goodTextPath = "commentator_good";
    private string badTextPath = "commentator_bad";
    private List<string> unusedGoodComments = new List<string>();
    private List<string> unusedBadComments = new List<string>();
    private List<string> neutralComments = new List<string>() { "Average run...", "Not good, not bad...", "Nearly falls down...", "More can be expected...", "It's not the day...", "Fans are cheering..." };
    [SerializeField] public TMP_Text[] commentText;

    void Start()
    {
        competition = Competition.Instance;
        dice = FindObjectOfType<Dice>();
        LoadFileLines("good");
        LoadFileLines("bad");
    }

    void Update()
    {
        // CheckEmptyFile();
    }

    private void CheckEmptyFile()
    {
        if (unusedGoodComments.Count == 0)
        {
            LoadFileLines("good");
        }
        else if (unusedBadComments.Count == 0)
        {
            LoadFileLines("bad");
        }
    }

    public void showComments(Player player, int partOfRun, float scoredPoints)
    {
        Color color = Color.white;
        if ((scoredPoints > 22) || ((player.GetGradeModifier() < 2) && (scoredPoints > 17)))
        {
            if (unusedGoodComments.Count > 0)
            {
                ShowCommentBackground(partOfRun - 1);
                color = Color.green;
                int randomIndex = Random.Range(0, unusedGoodComments.Count);
                string randomLine = unusedGoodComments[randomIndex];
                commentText[partOfRun - 1].text = "</color=green>" + randomLine.ToString() + "<color=green>";
                commentText[partOfRun - 1].color = color;
                unusedGoodComments.RemoveAt(randomIndex);
                player.praisesByCommentator++;
            }
            else
            {
                Debug.Log("No more unused lines/No comment.");
            }
        }

        else if ((scoredPoints < 8) || ((player.GetGradeModifier() > 1) && (scoredPoints < 13)))
        {
            if (unusedBadComments.Count > 0)
            {
                ShowCommentBackground(partOfRun - 1);
                color = color = Color.red;
                int randomIndex = Random.Range(0, unusedBadComments.Count);
                string randomLine = unusedBadComments[randomIndex];
                commentText[partOfRun - 1].text = "</color=red>" + randomLine.ToString() + "<color=red>";
                commentText[partOfRun - 1].color = color;
                unusedBadComments.RemoveAt(randomIndex);
                player.praisesByCommentator--;
            }
            else
            {
                Debug.Log("No more unused lines/No comment.");
            }
        }

        else
        {
            // Average comment
            // color = color = Color.white;
            if ((player.ranking + partOfRun) % 2 == 0)
            {
                int index = Random.Range(0, neutralComments.Count);
                commentText[partOfRun - 1].text = neutralComments[index].ToString();
                commentText[partOfRun - 1].color = Color.white;
                ShowCommentBackground(partOfRun - 1);
            }
            // 
        }
        CheckEmptyFile();
        SummaryComment(player);
        // color = Color.white;
    }

    public void SummaryComment(Player player)
    {
        Color color = Color.white;
        var state = player.myState;
        if (competition.partsOfRun > 2)
        {
            switch (state)
            {
                case Player.PlayerState.Running:
                    {
                        if (player.praisesByCommentator > 0)
                        {
                            commentText[3].text = "GOOD!".ToString();
                            commentText[3].color = Color.green;
                            dice.ShowSummaryImage("good");
                        }
                        else if (player.praisesByCommentator < 0)
                        {
                            commentText[3].text = "BAD...".ToString();
                            commentText[3].color = Color.red;
                            dice.ShowSummaryImage("bad");
                        }
                        else
                        {
                            commentText[3].text = "AVERAGE";
                            commentText[3].color = Color.white;
                            dice.ShowSummaryImage("average");
                        }
                    }
                    break;
                case Player.PlayerState.OutOf15:
                    commentText[3].text = "DISAPPOINTMENT".ToString();
                    commentText[3].color = Color.red;
                    dice.ShowSummaryImage("bad"); break;
                case Player.PlayerState.DidNotFinish:
                    commentText[3].text = "DID NOT FINISH".ToString();
                    commentText[3].color = Color.red;
                    dice.ShowSummaryImage("bad"); break;

                default:
                    commentText[3].text = player.myState.ToString();
                    commentText[3].color = Color.red; break;
            }
        }
    }

    private void LoadFileLines(string TypeOfComments)
    {
        TextAsset textAsset = null;

        switch (TypeOfComments)
        {
            case "good": textAsset = Resources.Load<TextAsset>(goodTextPath); break;
            case "bad": textAsset = Resources.Load<TextAsset>(badTextPath); break;
        };

        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    if (TypeOfComments.Contains("good"))
                    {
                        unusedGoodComments.Add(line.Trim());
                    }
                    else if (TypeOfComments.Contains("bad"))
                    {
                        unusedBadComments.Add(line.Trim());
                    }
                }
            }
            Debug.Log("Comments loaded: " + TypeOfComments);
        }

        else
        {
            Debug.LogError("Text file not found.");
        }
    }

    public void ResetComments()  //AND COMMENTS BACKGROUNDS
    {
        for (int i = 0; i < commentText.Length; i++)
        {
            commentText[i].text = "";
        }
    }

    public void ShowCommentBackground(int index)
    {
        dice.ShowCommentBackground(index);
    }
}
