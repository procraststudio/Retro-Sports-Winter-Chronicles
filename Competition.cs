using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class Competition : MonoBehaviour
{
    public int firstD6;
    public int secondD6;
    public int thirdD6;
    public int competitionRoll;
    public int performance;
    public int partsOfRun;
    public TextMeshProUGUI textMeshPro;
    [SerializeField] public TMP_Text finalText;
    [SerializeField] TMP_Text startingList;
    [SerializeField] public TMP_Text outsidersList;
    [SerializeField] public TMP_Text underdogsList;
    [SerializeField] TMP_Text outOf15List;
    [SerializeField] TMP_Text disqualifiedList;
    [SerializeField] TMP_Text didNotFinishList;
    [SerializeField] TMP_Text finishersList;
    [SerializeField] TMP_Text resultsList;
    [SerializeField] TMP_Text competitionName;
    [SerializeField] private GameObject setupButton;
    [SerializeField] private GameObject eventButton;
    [SerializeField] private PlayerDisplay _playerDisplay;

    public GameObject runButton;
    public GameObject weatherPanel;
    public GameObject presentationPanel;
    public GameObject eventPanel;
    public GameObject competitorPanel;
    public List<Player> players = new List<Player>();
    List<Player> finishers = new List<Player>();
    public List<Player> outsiders = new List<Player>();
    public List<Player> underdogs = new List<Player>();
    public List<Player> disqualified = new List<Player>();
    public List<Player> didNotFinish = new List<Player>();
    public List<Player> outOf15Competitors;
    public Player currentCompetitor { get;  set; }
    int currentCompetitorNo;
    public GameObject dicePanel;
    Dice dice;
    Surprises surprise;
    RunDescription description;
    public Player goldMedal;
    public Player silverMedal;
    public Player bronzeMedal;
    Decoration decoration;
    public bool competitionIsOver;
    public float bestFinalPerformance;
    public GameState myState;
    private bool eventHappened = false;

    public enum GameState
    {
        WeatherPhase = 0,
        PresentationPhase = 1,
        StartPhase = 2,
        CompetitionPhase = 3,
        EventPhase = 4,
        DecorationPhase = 5,
    }


    void Start()
    {
        myState = GameState.WeatherPhase;
        players = FindObjectOfType<Gamemanager>().favourites;
        outsiders = FindObjectOfType<Gamemanager>().outsiders;
        underdogs = FindObjectOfType<Gamemanager>().underdogs;
        competitionName.text = FindObjectOfType<Gamemanager>().competitionName.ToString();
        decoration = FindObjectOfType<Decoration>();
        currentCompetitorNo = players.Count - 1;
        dice = FindObjectOfType<Dice>();
        description = FindObjectOfType<RunDescription>();
        UpdatePlayerList(players, startingList);
        UpdatePlayerList(outsiders, outsidersList);
        UpdatePlayerList(underdogs, underdogsList);
        surprise = FindObjectOfType<Surprises>();
        partsOfRun = 0;
        outOf15Competitors = new List<Player> { };
        // didNotFinish = new List<Player> { };
        // disqualified = new List<Player> { };    
        currentCompetitor = null;
        competitionIsOver = false;

    }

    void Update()
    {
        GameStatesManager();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Run();
        }
        //DecorationPhase();
    }

    public void Run()
    {
        surprise.surpriseEffect = false;
        surprise.disqualification = false;
        dice.ResetDice();
        description.ResetDescription();
        currentCompetitor = players[currentCompetitorNo];
        _playerDisplay.DisplayCompetitor(currentCompetitor);    
        partsOfRun++;
        Debug.Log("RUN: " + partsOfRun);
        //  SURPRISE CHECK
        surprise.CheckSurprise(currentCompetitor);
        //SurpriseEffect(currentCompetitor);

        if ((currentCompetitorNo >= 0) && (!surprise.surpriseEffect))
        {
            currentCompetitor = players[currentCompetitorNo];
            firstD6 =  Random.Range(1, 7);
            secondD6 = Random.Range(1, 7);
            thirdD6 =  Random.Range(1, 7);
            dice.StartCoroutine("showDice");
            //dice.showDice();
            competitionRoll = firstD6 + secondD6;
            Debug.Log("NAME: " + currentCompetitor.name + ". SUM OF 2D6: " + competitionRoll + ". THIRD D6: " + thirdD6);
            CheckEvent(currentCompetitor);

            switch (competitionRoll)
            {
                case 2:
                    currentCompetitor.PoorFormEffect();
                    calculatePerformance(currentCompetitor, -1, thirdD6); break; //DISASTER
                case 3: currentCompetitor.PoorFormEffect(); calculatePerformance(currentCompetitor, 0, thirdD6); break; // MEDIOCRE
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
                    if (currentCompetitor.CheckHomeFactor()) //HOME FACTOR +D6
                    {
                        currentCompetitor.AddRunModifier(thirdD6);
                        Debug.Log("HOME FACTOR: +" + thirdD6);
                    }
                    if ((currentCompetitor.grade == 'A') || (currentCompetitor.grade == 'B'))
                    { calculatePerformance(currentCompetitor, 5, thirdD6); }
                    else if ((currentCompetitor.grade == 'C'))
                    { calculatePerformance(currentCompetitor, 4, thirdD6); }

                    else { calculatePerformance(currentCompetitor, 3, thirdD6); };
                    break; //GOOD

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
                    currentCompetitor.GoodFormEffect();
                    if ((currentCompetitor.grade == 'E'))
                    { calculatePerformance(currentCompetitor, 5, thirdD6); }
                    else { calculatePerformance(currentCompetitor, 6, thirdD6); }; break; //GREAT!
                case 12:
                    {
                        currentCompetitor.GoodFormEffect();
                        calculatePerformance(currentCompetitor, 7, thirdD6);
                    };
                    break; // CRIT SUCCESS
            }
            //CheckEvent(currentCompetitor);
            EndRun();
        }
    }

    public void EndRun()
    {
        if (partsOfRun > 2) // and player is not out
        {
            eventHappened = false;
            finishers.Add(currentCompetitor);
            players.RemoveAt(players.Count - 1);
            UpdatePlayerList(players, startingList);
            currentCompetitor.CalculateFinal();
            Debug.Log("SUM OF FLOAT: " + currentCompetitor.finalPerformance);
            updateResults();
            currentCompetitorNo--;
            partsOfRun = 0;
        }
    }

    public void calculatePerformance(Player player, int modifier, int thirdDie)
    {
        _playerDisplay.DisplayCompetitor(currentCompetitor);
        int homeBonus = 0;
        if (player.homeFactor)
        {
            homeBonus = thirdDie;
            finalText.text += "\n" + "HOME FACTOR: +" + thirdDie;
        };
        switch (modifier)
        {
            case -1:
                if (modifier > -6) { player.AddRunModifier(-6); }
                else { player.AddRunModifier(thirdDie * (-2)); };
                description.StoreDescription(Color.red, "DISASTER"); break;
            case 0: player.AddRunModifier(thirdDie * (-2)); description.StoreDescription(Color.red, "MEDIOCRE"); break;
            case 1: player.AddRunModifier(thirdDie * (-1)); description.StoreDescription(Color.red, "VERY POOR"); break;
            case 2: player.AddRunModifier((thirdDie + 1) / -2); description.StoreDescription(Color.red, "POOR"); break;
            case 3:
                if (thirdDie < 3) { player.AddRunModifier(Random.Range(-4, -1)); }
                else if (thirdDie > 4) { player.AddRunModifier(Random.Range(1, 4)); }
                else { player.AddRunModifier(0); }; description.StoreDescription(Color.white, "NOT GOOD"); break;
            case 4:
                if (thirdDie < 3) { player.AddRunModifier(Random.Range(-3, 0)); }
                else if (thirdDie > 4) { player.AddRunModifier(Random.Range(2, 5)); }
                else { player.AddRunModifier(0); }; description.StoreDescription(Color.white, "AVERAGE"); break;
            case 5: player.AddRunModifier((thirdDie + 1) / 2); description.StoreDescription(Color.green, "GOOD"); break;
            case 6: player.AddRunModifier(thirdDie); description.StoreDescription(Color.green, "GREAT!"); break;
            case 7:
                if (modifier < 6) { player.AddRunModifier(6); }
                else { player.AddRunModifier(thirdDie * 2); }
                description.StoreDescription(Color.green, "WONDERFUL!"); break;
        }

        player.CalculateAverage();
        //player.CalculateFinal();
        showResults(currentCompetitor);
        player.homeFactor = false;
        player.actualRunPoints = 0;

    }


    public void showResults(Player player)
    {

        finalText.text += "\n" + player.name +
        ": AVERAGE: " + player.averagePerformance +
        ", RUN MODIFIERS: " + player.actualRunPoints +
        ", SUM: " + player.CalculateActualRun().ToString();

    }
    public void showStarters(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            startingList.text = players[i].name + " (" + players[i].nationality + ")" + " . GRADE: " + players[i].grade + "\n";
        }

    }

    public void UpdatePlayerList(List<Player> list, TMP_Text listText)
    {
        listText.text = "";

        if (listText.text.Contains("startingList"))
        {
            foreach (Player player in players)
            {
                startingList.text += player.name +
                 " (" + player.nationality + ")" + ". Rank: " + player.ranking + ", Grade: " + player.grade + "\n";
            }
        }
        else
        {
            foreach (Player player in list)
            {
                listText.text += player.name + " . Grade: " + player.grade + "\n";
            }
        }
    }

    public void updateResults()
    {
        finishersList.text = "";
        resultsList.text = "";
        finishers.Sort((a, b) => b.finalPerformance.CompareTo(a.finalPerformance));
        for (int i = 0; i < finishers.Count; i++)
        {
            finishers[i].place = i + 1;
        }
        bestFinalPerformance = finishers[0].finalPerformance;
        Debug.Log("BEST: " + bestFinalPerformance);

        for (int i = 0; i < finishers.Count; i++)

        {
            finishersList.text += finishers[i].place +
             ". " + finishers[i].name.ToUpper() + " (" + finishers[i].nationality + "): " + finishers[i].finalPerformance.ToString("F1") + "\n";
            resultsList.text += TimeDisplay(finishers[i]);

        }

    }

    public void SurpriseEffect(Player player)
    {
        surprise.surpriseEffect = true;
        var state = player.myState;

        switch (state)
        {
            case Player.PlayerState.OutOf15: outOf15Competitors.Add(player); break;
            case Player.PlayerState.DidNotFinish: didNotFinish.Add(player); break;
            case Player.PlayerState.Disqualified: disqualified.Add(player); break;
        }
        UpdatePlayerList(didNotFinish, didNotFinishList);
        UpdatePlayerList(disqualified, disqualifiedList);
        int d6Roll = Random.Range(1, 7);
        Player surpriseCompetitor;
        if (surprise.surpriseEffect)
        {
            if ((d6Roll == 1) || (outsiders.Count == 0)) // IF 1 ON D6: BIG SURPRISE, UNDERDOG ENTERS
            {
                surpriseCompetitor = underdogs[0];
                underdogs.Remove(surpriseCompetitor);
            }

            else
            {
                surpriseCompetitor = outsiders[0];
                outsiders.Remove(surpriseCompetitor);
            }

            players.RemoveAt(players.Count - 1);
            players.Add(surpriseCompetitor);
            UpdatePlayerList(players, startingList);
            UpdatePlayerList(outsiders, outsidersList);
            UpdatePlayerList(underdogs, underdogsList);
            UpdatePlayerList(outOf15Competitors, outOf15List);
            // UpdatePlayerList(didNotFinish, didNotFinishList);
           //  UpdatePlayerList(disqualified, disqualifiedList);

            // UpdateLists();
            partsOfRun = 0;
            eventHappened = false;
            // descriptionText.color = Color.red; descriptionText.text = "SURPRISE! OUT OF 15!";
            finalText.text += "\n" + player.name + " IS OUT OF 15/DQ/DNF!" + "\n" + surpriseCompetitor.name + " ENTERS!";
        }
    }

    public void DecorationPhase()
    {
        if (players.Count < 1)
        {
            finalText.text = "";
            currentCompetitor = null;
            runButton.SetActive(false);
            competitionIsOver = true;
            decoration.winner = finishers[0];
            decoration.secondPlayer = finishers[1];
            decoration.thirdPlayer = finishers[2];
            //TO DO: Decoration effects
            decoration.StartCoroutine("DecorateMedalists");

        }
    }

    public string TimeDisplay(Player player)
    {
        float pointsDifference = bestFinalPerformance - player.finalPerformance;
        if (player.place == 1)
        {
            return player.ConvertPointsToTime(player.finalPerformance) + "\n";

        }
        else
        {
            return player.ConvertDifference(pointsDifference) + "\n";
            // player.ConvertPointsToTime(timeDifference) + "\n";
        }

    }

    void CheckEvent(Player player)
    {
        if ((!eventHappened) && ((firstD6 == secondD6) && (firstD6 + secondD6 != 2) && (firstD6 + secondD6 != 12)))
        {
            eventHappened = true;
            var shortEvent = FindObjectOfType<ShortEvent>();

            Debug.Log("EVENT!");
            finalText.text += "\n" + "EVENT!";
            shortEvent.GetEventType();

            // runButton.SetActive(false);
            // eventButton.SetActive(true);
            // myState = GameState.EventPhase;
        }
    }


    void GameStatesManager()
    {
        if (myState == GameState.WeatherPhase)
        {
            runButton.SetActive(false);
            dicePanel.SetActive(false);
            presentationPanel.SetActive(false);

        }
        else if (myState == GameState.PresentationPhase)
        {
            weatherPanel.SetActive(false);
            setupButton.SetActive(false);
            runButton.SetActive(false);
            presentationPanel.SetActive(true);
            dicePanel.SetActive(false);
        }
        else if (myState == GameState.CompetitionPhase)
        {

            weatherPanel.SetActive(false);
            setupButton.SetActive(false);
            presentationPanel.SetActive(false);
            eventButton.SetActive(false);
            runButton.SetActive(true);
            dicePanel.SetActive(true);

        }
        else if (myState == GameState.EventPhase)
        {
            weatherPanel.SetActive(false);
            setupButton.SetActive(false);
            presentationPanel.SetActive(false);
            runButton.SetActive(false);
            dicePanel.SetActive(false);


            // eventPanel.SetActive(true);

        }

    }

}




