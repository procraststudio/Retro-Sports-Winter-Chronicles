using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Competition;

public class Presentation : MonoBehaviour
{
    public Player actualFavourite;
    [SerializeField] private TMP_Text[] descriptionText;
    [SerializeField] private TMP_Text[] favouriteName;
    [SerializeField] private TMP_Text[] favouriteGradeExp;
    [SerializeField] private GameObject[] flagSection;
    [SerializeField] private GameObject[] headsSection;
    [SerializeField] private TMP_Text buttonText;
    Competition competition;
    public string flagsFolderPath = "flags/";
    private string textPath = "comments";
    private Sprite flagSprite;
    private bool favouritesGenerated;
    private int noOfBigFavourites = 0;
    private bool presentationPhaseOver;
    private bool absenceChecked = false;
    public List<Player> favourites;
    public List<Player> bigFavourites = new List<Player>();
    private List<string> unusedComments = new List<string>();
    [SerializeField] Sprite[] headImages;


    void Start()
    {
        favouritesGenerated = false;
        actualFavourite = null;
        flagSprite = Resources.Load<Sprite>(flagsFolderPath);
        competition = Competition.Instance;
        favourites = competition.players;
        if (unusedComments.Count == 0)
        {
            LoadFileLines();
        }

    }

    void Update()
    {
        // CheckEndPhase();
    }

    public void GenerateFavourites()
    {
        if (!absenceChecked)
        {
            FindObjectOfType<Absences>().CheckAbsence();
            absenceChecked = true;
        }

        competition.LoadLists();
        List<Player> favourites = competition.players;
        if (!presentationPhaseOver)
        {
            for (int i = 0; i < favourites.Count; i++)
            {
                //CHECK HOW MANY 'X' COMPETITORS
                if ((favourites[i].grade == 'X') && (noOfBigFavourites < 3))
                {
                    bigFavourites.Add(favourites[i]);
                    noOfBigFavourites++;
                }
            }
            for (int i = 0; i < favourites.Count; i++)
            {
                if ((favourites[i].grade == 'A') && (noOfBigFavourites < 3))
                {
                    bigFavourites.Add(favourites[i]);
                    noOfBigFavourites++;
                }

            }
            for (int i = 0; i < favourites.Count; i++)
            {
                if ((favourites[i].grade == 'B') && (noOfBigFavourites < 3))
                {
                    bigFavourites.Add(favourites[i]);
                    noOfBigFavourites++;
                }

            }


            switch (noOfBigFavourites)
            {
                case 1:
                    showPlayerData(0, bigFavourites[0]);
                    showComments(0); break;
                case 2:
                    showPlayerData(0, bigFavourites[0]);
                    showComments(0);
                    showPlayerData(1, bigFavourites[1]);
                    showComments(1); break;
                case 3:
                    showPlayerData(0, bigFavourites[0]);
                    showComments(0);
                    showPlayerData(1, bigFavourites[1]);
                    showComments(1);
                    showPlayerData(2, bigFavourites[2]);
                    showComments(2);
                    break;
            }

            presentationPhaseOver = true;
            buttonText.text = "START".ToString();
        }

        else
        {
            // competition.myState = GameState.CompetitionPhase;
            competition.ChangeState(GameState.CompetitionPhase);
        }

    }



    public void showPlayerData(int favouriteNo, Player player)
    {
        favouriteName[favouriteNo].text = player.secondName;
        favouriteGradeExp[favouriteNo].text = "GRADE: " + player.grade.ToString() + "    EXP: " + player.experience.ToString(); ;
        flagSection[favouriteNo].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(flagsFolderPath + player.nationality);
        if (competition.gamemanager.competitionName.Contains("Men"))
        {
            headsSection[favouriteNo].GetComponent<SpriteRenderer>().sprite = headImages[0];
        }
        else
        {
            headsSection[favouriteNo].GetComponent<SpriteRenderer>().sprite = headImages[1];
        }
    }

        public void showComments(int player)
        {
            //TextAsset textAsset = Resources.Load<TextAsset>(textPath);
            if (unusedComments.Count > 0)
            {
                int randomIndex = Random.Range(0, unusedComments.Count);
                string randomLine = unusedComments[randomIndex];
                descriptionText[player].text = randomLine;
                unusedComments.RemoveAt(randomIndex);
            }
            else
            {
                Debug.Log("No more unused lines.");
            }
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
            }
            else
            {
                Debug.LogError("Text file not found.");
            }
        }

        void CheckEndPhase()
        {
            if (favouritesGenerated)
            {
                presentationPhaseOver = true;

            }
        }
    }


