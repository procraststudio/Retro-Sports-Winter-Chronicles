using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;


public class Competition : MonoBehaviour
{
    public static Competition Instance { get; private set; }
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
    [SerializeField] TMP_Text secondRunResults;
    [SerializeField] TMP_Text [] timeDifferenceText;
    [SerializeField] TMP_Text competitionName;
    [SerializeField] private GameObject setupButton;
    [SerializeField] private GameObject eventButton;
    [SerializeField] private PlayerDisplay _playerDisplay;
    [SerializeField] private GameObject DecorationPanel;

    public GameObject runButton;
    public GameObject weatherPanel;
    public GameObject presentationPanel;
    public GameObject eventPanel;
    public GameObject competitorPanel;
    public List<Player> players = new List<Player>();
    public List<Player> finishers = new List<Player>();
    public List<Player> outsiders = new List<Player>();
    public List<Player> underdogs = new List<Player>();
    public List<Player> disqualified = new List<Player>();
    public List<Player> didNotFinish = new List<Player>();
    public List<Player> outOf15Competitors;
    public Player currentCompetitor { get; set; }
    public int currentCompetitorNo;
    public int currentRun = 1;
    public GameObject dicePanel;
    Dice dice;
    Surprises surprise;
    RunDescription description;
    public Player goldMedal;
    public Player silverMedal;
    public Player bronzeMedal;
    Decoration decoration;
    public bool competitionIsOver = false;
    public float bestFinalPerformance;
    public float bestRunPerformance;
    public GameState myState;
    private bool eventHappened = false;
    private bool decorationSpawned = false;
    Gamemanager gamemanager;
    public float firstSectorMostPoints = 0f;
    public float secondSectorMostPoints = 0f;
    public float thirdSectorMostPoints = 0f;

    public enum GameState
    {
        WeatherPhase = 0,
        PresentationPhase = 1,
        StartPhase = 2,
        CompetitionPhase = 3,
        EventPhase = 4,
        DecorationPhase = 5,
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //TO DO highScore = PlayerPrefs.GetInt("highScore"); //loading high scores
    }

    void Start()
    {
        myState = GameState.WeatherPhase;
        gamemanager = FindObjectOfType<Gamemanager>();
        players = gamemanager.favourites;
        outsiders = gamemanager.outsiders;
        underdogs = gamemanager.underdogs;
        currentRun = 1;

        decoration = FindObjectOfType<Decoration>();
        currentCompetitorNo = players.Count - 1;
        dice = FindObjectOfType<Dice>();
        description = FindObjectOfType<RunDescription>();
        UpdatePlayerList(players, startingList);
        FindObjectOfType<PlayerDataLoader>().LoadPlayers(players);
        UpdatePlayerList(outsiders, outsidersList);
        UpdatePlayerList(underdogs, underdogsList);
        surprise = FindObjectOfType<Surprises>();
        partsOfRun = 0;
        outOf15Competitors = new List<Player> { };
        // didNotFinish = new List<Player> { };
        // disqualified = new List<Player> { };    
        currentCompetitor = null;
        competitionIsOver = false;
        //currentRun = 1;

    }

    void Update()
    {
        competitionName.text = gamemanager.competitionName.ToString() + currentRun.ToString() + "/" + Gamemanager.numbersOfRun;
        GameStatesManager();
        HandleNextRun();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Run();
        }
        // DecorationPhase();
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
        //  SURPRISE CHECK
        surprise.CheckSurprise(currentCompetitor);
        //SurpriseEffect(currentCompetitor);

        if ((currentCompetitorNo >= 0) && (!surprise.surpriseEffect))
        {
            currentCompetitor = players[currentCompetitorNo];
            firstD6 = Random.Range(1, 7);
            secondD6 = Random.Range(1, 7);
            thirdD6 = Random.Range(1, 7);
            dice.StartCoroutine("showDice");
            //dice.showDice();
            competitionRoll = firstD6 + secondD6;
            Debug.Log("NAME: " + currentCompetitor.secondName + ". SUM OF 2D6: " + competitionRoll + ". THIRD D6: " + thirdD6);
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
                    if (currentCompetitor.CheckHomeFactor()) //HOME FACTOR +thirdD6
                    {
                        currentCompetitor.AddRunModifier(currentRun, thirdD6);
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

    public void CalculateSectorTime(Player player, float sectorPoints)
    {
        if (player.myState == Player.PlayerState.Running)
        {
            float differenceInPoints = 0;
            switch (partsOfRun)
            {
                case 1:
                    if (sectorPoints > firstSectorMostPoints)
                    {
                        differenceInPoints = firstSectorMostPoints - sectorPoints;
                        firstSectorMostPoints = sectorPoints;
                        Debug.Log("1st SECTOR RECORD: " + sectorPoints);
                        Debug.Log("DIFFERENCE: " + differenceInPoints);
                        // GREEN ARROW UP 
                    }
                    else
                    {
                        differenceInPoints = firstSectorMostPoints - sectorPoints;
                    }
                    break;
                case 2:
                    if (sectorPoints > secondSectorMostPoints)
                    {
                        secondSectorMostPoints = sectorPoints;
                        Debug.Log("2nd SECTOR RECORD: " + sectorPoints);
                    };
                    differenceInPoints = secondSectorMostPoints - sectorPoints; break;
                case 3:
                    if (sectorPoints > thirdSectorMostPoints)
                    {
                        thirdSectorMostPoints = sectorPoints;
                        Debug.Log("3rd SECTOR RECORD: " + sectorPoints);
                    };
                    differenceInPoints = thirdSectorMostPoints - sectorPoints; break;
            }
            // SHOW DIFFERENCE
            Debug.Log(player.ConvertDifference(differenceInPoints));

            if (differenceInPoints > 0)
            {
                timeDifferenceText[partsOfRun - 1].text = "<color=red>" + (player.ConvertDifference(differenceInPoints).ToString()) + "<color=red>";
            }
            else
            {
                timeDifferenceText[partsOfRun - 1].text = "<color=green>" + (player.ConvertDifference(differenceInPoints).ToString()) + "<color=green>";
            }
        }
        
    }

    public void EndRun()
    {
        if (partsOfRun > 2) // and player is not out
        {
            if (currentRun < 2)
            {
                finishers.Add(currentCompetitor);
            }

            else if (currentRun > 1)
            {
                bool playerExists = false;
                for (int i = finishers.Count - 1; i >= 0; i--)
                {
                    if (finishers[i].secondName == currentCompetitor.secondName)
                    {
                        playerExists = true;
                    }

                }
                if (!playerExists)
                {
                    finishers.Add(currentCompetitor);
                }
            }
            eventHappened = false;
            players.RemoveAt(players.Count - 1);
            UpdatePlayerList(players, startingList);
            FindObjectOfType<PlayerDataLoader>().UpdateCompetitors(players);
            currentCompetitor.CalculateFinal();
            updateResults();
            currentCompetitorNo--;
            partsOfRun = 0;
        }
    }

    public void calculatePerformance(Player player, int modifier, int thirdDie)
    {
        _playerDisplay.DisplayCompetitor(currentCompetitor);

        switch (modifier)
        {
            case -1:
                if (modifier > -6) { player.AddRunModifier(currentRun, -6); }
                else { player.AddRunModifier(currentRun, thirdDie * (-2)); };
                description.StoreDescription(Color.red, "DISASTER"); break;
            case 0: player.AddRunModifier(currentRun, thirdDie * (-2)); description.StoreDescription(Color.red, "VERY POOR"); break;
            case 1: player.AddRunModifier(currentRun, thirdDie * (-1)); description.StoreDescription(Color.red, "POOR"); break;
            case 2: player.AddRunModifier(currentRun, (thirdDie + 1) / -2); description.StoreDescription(Color.red, "NOT GOOD"); break;
            case 3:
                if (thirdDie < 3) { player.AddRunModifier(currentRun, Random.Range(-4, -1)); }
                else if (thirdDie > 4) { player.AddRunModifier(currentRun, Random.Range(1, 4)); }
                else { player.AddRunModifier(currentRun, 0); }; description.StoreDescription(Color.white, "AVERAGE"); break;
            case 4:
                if (thirdDie < 3) { player.AddRunModifier(currentRun, Random.Range(-3, 0)); }
                else if (thirdDie > 4) { player.AddRunModifier(currentRun, Random.Range(2, 5)); }
                else { player.AddRunModifier(currentRun, 0); }; description.StoreDescription(Color.white, "AVERAGE"); break;
            case 5: player.AddRunModifier(currentRun, (thirdDie + 1) / 2); description.StoreDescription(Color.green, "GOOD"); break;
            case 6: player.AddRunModifier(currentRun, thirdDie); description.StoreDescription(Color.green, "GREAT!"); break;
            case 7:
                if (modifier < 6) { player.AddRunModifier(currentRun, 6); }
                else { player.AddRunModifier(currentRun, thirdDie * 2); }
                description.StoreDescription(Color.green, "WONDERFUL!"); break;
        }

        // player.CalculateAverage();
        float currentPlayerPoints = player.firstRunPoints;
        player.CalculateActualRun(currentRun);
        CalculateSectorTime(player, player.firstRunPoints - currentPlayerPoints);
        player.CalculateFinal();
        showResults(currentCompetitor);
        player.homeFactor = false;
        // player.actualRunPoints = 0;

    }


    public void showResults(Player player)
    {
        if (player.myState == Player.PlayerState.Running)
        {
            finalText.text += "\n" + player.secondName +
            ": AVERAGE: " + player.averagePerformance +
            ", RUN MODIFIERS: " + player.actualModifiers(currentRun) + ", SUM: ";
            if (currentRun == 2)
            {
                finalText.text += player.secondRunPoints.ToString();
            }
            else
            {
                finalText.text += player.firstRunPoints.ToString();
            }
        }
    }
    public void showStarters(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            startingList.text = players[i].secondName + " (" + players[i].nationality + ")" + " . GRADE: " + players[i].grade + "\n";
        }

    }

    public void UpdatePlayerList(List<Player> list, TMP_Text listText)
    {
        // listText.text = "";
        //  string[] lines = listText.text.Split('\n');
        int highlightedRowIndex = list.Count;
        //TMP_Text textComponent = listText;

        if (listText.ToString().Contains("startingList"))
        {
            listText.text = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    listText.text += "=>" + list[i].secondName.ToUpper() + ". Grade: " + list[i].grade + ". Rank: " + list[i].ranking + "\n";
                }
                else
                {
                    listText.text += list[i].secondName + ". Grade: " + list[i].grade + "\n";
                }
            }
        }

        else
        {
            listText.text = "";
            foreach (Player player in list)
            {
                listText.text += player.secondName + ". Grade: " + player.grade + "\n";
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

        if (finishers.Count > 0)
        {
            bestFinalPerformance = finishers[0].finalPerformance;
        }
        //bestRunPerformance = finishers[0].CalculateActualRun(currentRun);
        //Debug.Log("BEST: " + bestFinalPerformance);

        for (int i = 0; i < finishers.Count; i++)

        {
            if (finishers[i].secondName == currentCompetitor.secondName)
            {
                string fullText = "<color=green>" + finishers[i].place + ". " + finishers[i].secondName.ToUpper() + " (" + finishers[i].nationality + "): " + finishers[i].finalPerformance.ToString("F1") + "<color=green>" + "\n";
                finishersList.text += fullText;
                resultsList.text += "<color=green>" + TimeDisplay(finishers[i]) + "<color=green>";
            }
            else
            {
                string fullText = "<color=white>" + finishers[i].place + ". " + finishers[i].secondName.ToUpper() + " (" + finishers[i].nationality + "): " + finishers[i].finalPerformance.ToString("F1") + "<color=white>" + "\n";
                finishersList.text += fullText;
                resultsList.text += "<color=white>" + TimeDisplay(finishers[i]) + "<color=white>";
            }


            //if (currentRun == 2)
            //{
            //    secondRunResults.text = TimeDisplay(finishers[i]);
            //}
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
        Player playerToDelete = player;

        for (int i = finishers.Count - 1; i >= 0; i--)
        {
            if (finishers[i].secondName == playerToDelete.secondName) // Mo¿esz u¿yæ Equals lub innych metod porównywania
            {
                finishers.RemoveAt(i); // Usuñ obiekt z listy
            }
        }

        UpdatePlayerList(didNotFinish, didNotFinishList);
        UpdatePlayerList(disqualified, disqualifiedList);
        int d6Roll = Random.Range(1, 7);
        Player surpriseCompetitor;
        if (surprise.surpriseEffect)
        {
            if ((underdogs.Count > 0) && (d6Roll == 1) || (outsiders.Count == 0))// IF 1 ON D6: BIG SURPRISE, UNDERDOG ENTERS
            {
                surpriseCompetitor = underdogs[0];
                underdogs.Remove(surpriseCompetitor);
            }

            else
            {
                if (underdogs.Count == 0)
                {
                    return;
                }
                else
                {
                    surpriseCompetitor = outsiders[0];
                    outsiders.Remove(surpriseCompetitor);
                }
            }

            players.RemoveAt(players.Count - 1);
            players.Add(surpriseCompetitor);
            UpdatePlayerList(players, startingList);
            UpdatePlayerList(outsiders, outsidersList);
            UpdatePlayerList(underdogs, underdogsList);
            UpdatePlayerList(outOf15Competitors, outOf15List);
            updateResults();
            // UpdatePlayerList(didNotFinish, didNotFinishList);
            // UpdatePlayerList(disqualified, disqualifiedList);

            // UpdateLists();
            partsOfRun = 0;
            eventHappened = false;
            // descriptionText.color = Color.red; descriptionText.text = "SURPRISE! OUT OF 15!";
            finalText.text += "\n" + player.secondName + " IS OUT OF 15/DQ/DNF!" + "\n" + surpriseCompetitor.secondName + " ENTERS!";
        }
    }

    public void DecorationPhase()
    {
        if ((competitionIsOver) && (!decorationSpawned))
        {
            finalText.text = "";
            currentCompetitor = null;
            myState = GameState.DecorationPhase;
            GameObject newObject = Instantiate(DecorationPanel);
            newObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            newObject.transform.localPosition = new Vector3(7.17f, 0.00f, 0.00f);
            decorationSpawned = true;

        }
    }

    public string TimeDisplay(Player player)
    {
        float pointsDifference = bestFinalPerformance - player.finalPerformance;
        // float pointsRunDifference = bestRunPerformance - player.
        if (Gamemanager.numbersOfRun < 2)
        {
            if (player.place == 1)
            {
                return player.ConvertPointsToTime(player.finalPerformance) + "\n";
            }
            else
            {
                return "    " + player.ConvertDifference(pointsDifference) + "\n";
            }
        }
        else
        {
            //Player bestInSecondRun = finishers.OrderByDescending(player => player.secondRunPoints).FirstOrDefault();
            //Debug.Log("BEST IN 2nd ROUND WAS: " + bestInSecondRun.name);
            //pointsDifference = bestInSecondRun.secondRunPoints - player.secondRunPoints;
            //return player.ConvertDifference(pointsDifference) + "\n";
            if (currentRun == 2)
            {
                if (player.secondRunPoints != 0)
                {
                    return player.ConvertPointsToTime(player.firstRunPoints) + "......" + player.ConvertPointsToTime(player.secondRunPoints) + "\n";
                }
                else
                {
                    return player.ConvertPointsToTime(player.firstRunPoints) + "......" + "\n";
                }
            }
            else
            {
                return player.ConvertPointsToTime(player.finalPerformance) + "\n";
            }
        }
        // player.ConvertPointsToTime(timeDifference) + "\n";
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
        }
    }
    public void HandleNextRun()
    {
        if ((partsOfRun == 0) && (players.Count == 0))// && (currentRun < Gamemanager.numbersOfRun))
        {
            currentRun++;
            if (currentRun > Gamemanager.numbersOfRun)
            {
                competitionIsOver = true;
                DecorationPhase();
            }

            else
            {
                players.AddRange(finishers);
                UpdatePlayerList(players, startingList);
                currentCompetitorNo = players.Count - 1;
                List<Player> blackHorses = new List<Player>();
                blackHorses.AddRange(outsiders);
                blackHorses.AddRange(underdogs);
                blackHorses.AddRange(outOf15Competitors);
                foreach (var player in blackHorses)
                {
                    player.firstRunPoints = finishers[finishers.Count - 1].firstRunPoints - 3;
                }
                Debug.Log("NEXT RUN PREPARED!");
                Debug.Log("BLACKHORSES POINTS: " + blackHorses[0].firstRunPoints);
                // possible weather change, +/- weather modifier
            }

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
            presentationPanel.SetActive(false);
            //  eventButton.SetActive(false);
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
        }
        else if (myState == GameState.DecorationPhase)
        {
            weatherPanel.SetActive(false);
            setupButton.SetActive(false);
            presentationPanel.SetActive(false);
            runButton.SetActive(false);
            dicePanel.SetActive(false);
            competitorPanel.SetActive(false);
        }




    }
}




