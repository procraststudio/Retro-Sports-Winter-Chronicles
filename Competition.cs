using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class Competition : MonoBehaviour
{
    public int firstD6;
    public int secondD6;
    public int thirdD6;
    public int competitionRoll;
    public int performance;
    [SerializeField] TMP_Text finalText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text startingList;
    [SerializeField] TMP_Text outsidersList;
    [SerializeField] TMP_Text resultsText;
    [SerializeField] TMP_Text competitionName; 
    List<Player> players = new List<Player>();
    List<Player> finishers = new List<Player>();
    List<Player> outsiders = new List<Player>();
    public Player currentCompetitor;
    int currentCompetitorNo;
    public GameObject dicePanel;
    Dice dice;
    Surprises surprise;   


    void Start()
    {
        players = FindObjectOfType<Gamemanager>().favourites;
        outsiders = FindObjectOfType<Gamemanager>().outsiders;
        competitionName.text = FindObjectOfType<Gamemanager>().competitionName.ToString();
        currentCompetitorNo = players.Count - 1;
        dice = FindObjectOfType<Dice>();
        updatePlayerListText();
        updateOutsiders();
        surprise = FindObjectOfType<Surprises>();

    }

    void Update()
    {


    }

    public void Run()
    {
        currentCompetitor = players[currentCompetitorNo];
        bool surpriseHappened = surprise.IsSurprise(currentCompetitor);
        if (!surpriseHappened)
        {
            if (currentCompetitorNo >= 0)
            {
                currentCompetitor = players[currentCompetitorNo];
                firstD6 = Random.Range(1, 7);
                secondD6 = Random.Range(1, 7);
                thirdD6 = Random.Range(1, 7);
                //dice.StartCoroutine("RollTheDice");
                dice.showDice();
                competitionRoll = firstD6 + secondD6;
                Debug.Log("NAME: " + currentCompetitor.name + ". SUM OF 2D6: " + competitionRoll + ". THIRD D6: " + thirdD6);
                switch (competitionRoll)
                {
                    case 2: calculatePerformance(currentCompetitor, -1, thirdD6); break; //DISASTER
                    case 3: calculatePerformance(currentCompetitor, 0, thirdD6); break; // MEDIOCRE
                    case 4:
                        if ((currentCompetitor.grade == 'A') || (currentCompetitor.grade == 'B') || (currentCompetitor.grade == 'C'))
                        { calculatePerformance(currentCompetitor, 1, thirdD6); }
                        else { calculatePerformance(currentCompetitor, 0, thirdD6); }
                        break;

                    case 5:
                        if ((currentCompetitor.grade == 'A') || (currentCompetitor.grade == 'B'))
                        { calculatePerformance(currentCompetitor, 2, thirdD6); }
                        else { calculatePerformance(currentCompetitor, 1, thirdD6); }
                        break; // POOR
                    case 6:
                        if ((currentCompetitor.grade == 'A') || (currentCompetitor.grade == 'B'))
                        { calculatePerformance(currentCompetitor, 3, thirdD6); }
                        else if ((currentCompetitor.grade == 'E'))
                        { calculatePerformance(currentCompetitor, 1, thirdD6); }
                        else { calculatePerformance(currentCompetitor, 2, thirdD6); }
                        break; // BELOW AVERAGE

                    case 7:
                        if ((currentCompetitor.grade == 'A') || (currentCompetitor.grade == 'B'))
                        { calculatePerformance(currentCompetitor, 4, thirdD6); }
                        else if ((currentCompetitor.grade == 'C'))
                        { calculatePerformance(currentCompetitor, 3, thirdD6); }
                        else { calculatePerformance(currentCompetitor, 2, thirdD6); }
                        break; //AVERAGE

                    case 8:
                        if ((currentCompetitor.grade == 'A') || (currentCompetitor.grade == 'B'))
                        { calculatePerformance(currentCompetitor, 5, thirdD6); }
                        else if ((currentCompetitor.grade == 'C'))
                        { calculatePerformance(currentCompetitor, 4, thirdD6); }
                        else { calculatePerformance(currentCompetitor, 3, thirdD6); }; break; //GOOD

                    case 9:
                        if ((currentCompetitor.grade == 'A') || (currentCompetitor.grade == 'B') || (currentCompetitor.grade == 'C'))
                        { calculatePerformance(currentCompetitor, 5, thirdD6); }
                        else if ((currentCompetitor.grade == 'D'))
                        { calculatePerformance(currentCompetitor, 4, thirdD6); }
                        else { calculatePerformance(currentCompetitor, 3, thirdD6); }; break; //PRETTY GOOD

                    case 10:
                        if ((currentCompetitor.grade == 'B') || (currentCompetitor.grade == 'C') || (currentCompetitor.grade == 'D'))
                        { calculatePerformance(currentCompetitor, 5, thirdD6); }
                        else if ((currentCompetitor.grade == 'E'))
                        { calculatePerformance(currentCompetitor, 4, thirdD6); }
                        else if ((currentCompetitor.grade == 'A')) { calculatePerformance(currentCompetitor, 6, thirdD6); };
                        break; // VERY GOOD

                    case 11:
                        if ((currentCompetitor.grade == 'E'))
                        { calculatePerformance(currentCompetitor, 5, thirdD6); }
                        else { calculatePerformance(currentCompetitor, 6, thirdD6); }; break; //GREAT!
                    case 12:
                        Debug.Log(currentCompetitor.name + ": GREAT! GREAT! GREAT");
                        { calculatePerformance(currentCompetitor, 7, thirdD6); }; break; // CRIT SUCCESS
                }
            }

            // players.Remove(currentCompetitor);
            // 
            currentCompetitorNo--;

            finishers.Add(currentCompetitor);
            players.RemoveAt(players.Count - 1);

            updatePlayerListText();
            updateResults();
        }
        else
        {
            SurpriseEffect();
            return;
        }
        // 
    }

    public void calculatePerformance(Player player, int modifier, int thirdDie)
    {
        //int finalModifier;
        switch (modifier)
        {
            case -1:
                player.finalModifier = thirdDie * (-2);
                if (player.finalModifier > -6) { player.finalModifier = -6; };
                descriptionText.color = Color.red; descriptionText.text = "DISASTER..."; break;
            case 0: player.finalModifier = thirdDie * (-2); descriptionText.color = Color.red; descriptionText.text = "MEDIOCRE"; break;
            case 1: player.finalModifier = thirdDie * (-1); descriptionText.color = Color.red; descriptionText.text = "VERY POOR"; break;
            case 2: player.finalModifier = Random.Range(-3, 0); descriptionText.color = Color.red; descriptionText.text = "POOR"; break;
            case 3:
                if (thirdDie < 3) { player.finalModifier = Random.Range(-4, -1); }
                else if (thirdDie > 4) { player.finalModifier = Random.Range(1, 4); }
                else { player.finalModifier = 0; }; descriptionText.color = Color.white; descriptionText.text = "BELOW AVERAGE"; break;
            case 4:
                if (thirdDie < 3) { player.finalModifier = Random.Range(-3, 0); }
                else if (thirdDie > 4) { player.finalModifier = Random.Range(2, 5); }
                else { player.finalModifier = 0; }; descriptionText.color = Color.white; descriptionText.text = "AVERAGE"; break;
            case 5: player.finalModifier = Random.Range(1, 4); descriptionText.color = Color.green; descriptionText.text = "GOOD"; break;
            case 6: player.finalModifier = thirdDie; descriptionText.color = Color.green; descriptionText.text = "GREAT!"; break;
            case 7:
                player.finalModifier = thirdDie * 2;
                if (player.finalModifier < 6) { player.finalModifier = 6; }
                descriptionText.color = Color.green; descriptionText.text = "WONDERFUL!"; break;
        }
        player.calculateAverage();
        player.calculateFinal(player.finalModifier);
        showResults(currentCompetitor, currentCompetitor.finalPerformance);



    }


    public void showResults(Player player, int result)
    {
        finalText.text += "\n" + player.name +
        ": AVERAGE: " + player.averagePerformance +
        ", MODIFIER: " + player.finalModifier +
        ", FINAL: " + player.finalPerformance;

    }
    public void showStarters(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            startingList.text = players[i].name + " . GRADE: " + players[i].grade + "\n";
        }

    }

    public void updatePlayerListText()
    {
        startingList.text = "";
       

        foreach (Player player in players)
        {
            startingList.text += player.name + " . Grade: " + player.grade + "\n";
        }

    }

    public void updateOutsiders()
    {
        outsidersList.text = "";


        foreach (Player player in outsiders)
        {
            outsidersList.text += player.name + " . Grade: " + player.grade + "\n";
        }

    }

    public void updateResults()
    {
        resultsText.text = "";
        finishers.Sort((a, b) => b.finalPerformance.CompareTo(a.finalPerformance));
        for (int i = 0; i < finishers.Count; i++)
        {
            finishers[i].place = i + 1;
        }

        foreach (Player player in finishers)
        {
            resultsText.text += player.place +". "+ player.name + " RESULT :" + player.finalPerformance+ "\n";
        }
    }

    public void SurpriseEffect()
    {
        //outOf15List.Add(currentCompetitor)
        players.RemoveAt(players.Count - 1);
        descriptionText.color = Color.red; descriptionText.text = "SURPRISE! OUT OF 15!";
    }

    //public void showDice(int firstDie, int secondDie, int thirdDie)
    //{
    // dicePanel.SetActive(true);
    //firstDieImage.GetComponent<SpriteRenderer>().sprite = diceImages[firstDie-1];
    // secondDieImage.GetComponent<SpriteRenderer>().sprite = diceImages[secondDie+5];
    // thirdDieImage.GetComponent<SpriteRenderer>().sprite = diceImages[thirdDie + 11];
    // Debug.Log("DICE SHOWN");
    // }




}


