using System;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TMP_Text[] timeDifferenceText;
    [SerializeField] TMP_Text competitionName;
    [SerializeField] private GameObject setupButton;
    [SerializeField] private GameObject eventButton;
    [SerializeField] private PlayerDisplay _playerDisplay;

    [Header("List Sections")]
    [SerializeField] GameObject startersSection;
    [SerializeField] GameObject outsidersSection;
    [SerializeField] GameObject underdogsSection;
    [SerializeField] GameObject finishersSection;
    [SerializeField] GameObject outOf15Section;
    [SerializeField] GameObject didNotFinishSection;
    [SerializeField] GameObject disqualifiedSection;
    [SerializeField] GameObject endOfRunList;
    [SerializeField] GameObject[] listsSection;

    [Header("Panels")]
    public GameObject weatherPanel;
    public GameObject presentationPanel;
    public GameObject eventPanel;
    public GameObject competitorPanel;
    [SerializeField] private GameObject DecorationPanel;

    public GameObject runButton;
    public List<Player> players = new List<Player>();
    public List<Player> finishers = new List<Player>();
    public List<Player> currentClassification = new List<Player>();
    public List<Player> outsiders = new List<Player>();
    public List<Player> underdogs = new List<Player>();
    public List<Player> disqualified = new List<Player>();
    public List<Player> didNotFinish = new List<Player>();
    public List<Player> didNotStarted = new List<Player>();
    public List<Player> outOf15Competitors;
    public List<Player> firstRunClassification = new List<Player>();
    public List<Player> secondRunClassification = new List<Player>();
    public Player currentCompetitor { get; set; }
    public int currentCompetitorNo;
    public int possibleReturnsFromOutOf15 = 0;
    public int currentRun = 1;
    public int numberOfFavourites;
    public GameObject dicePanel;
    Dice dice;
    Surprises surprise;

    public Player goldMedal;
    public Player silverMedal;
    public Player bronzeMedal;
    Decoration decoration;
    public bool competitionIsOver = false;
    public float bestFinalPerformance;
    public float bestFirstRunPerformance;
    public float bestRunPerformance;
    public GameState myState { get; set; }
    public bool eventHappened = false;
    private bool decorationSpawned = false;
    Gamemanager gamemanager;
    public float firstSectorMostPoints = 20f; // average from early competitors runs
    public float secondSectorMostPoints = 20f;
    public float thirdSectorMostPoints = 20f;
    public CalculatePerformance _calculatePerformance;
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public Canvas canvasGroup;


    public enum GameState
    {
        WeatherPhase = 0,
        PresentationPhase = 1,
        StartPhase = 2,
        CompetitionPhase = 3,
        CheckSurprisePhase = 4,
        EventPhase = 5,
        EndOfRun = 6,
        DecorationPhase = 7,
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
        ChangeState(GameState.WeatherPhase);
        gamemanager = FindObjectOfType<Gamemanager>();
        players = gamemanager.favourites;
        outsiders = gamemanager.outsiders;
        underdogs = gamemanager.underdogs;
        currentRun = 1;
        decoration = FindObjectOfType<Decoration>();
        currentCompetitorNo = players.Count - 1;
        dice = FindObjectOfType<Dice>();
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
        competitionName.text = gamemanager.competitionName.ToString() + currentRun.ToString() + "/" + Gamemanager.numbersOfRun;
        _calculatePerformance = FindObjectOfType<CalculatePerformance>();
        numberOfFavourites = players.Count;
        //currentRun = 1;

    }

    void Update()
    {
        // GameStatesManager();
        HandleNextRun();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Run();
        }
        // DecorationPhase();
    }

    public void Run()
    {
        //SpawnEndOfRunClassification();
        if (competitionIsOver)
        {
            DecorationPhase(); return;
        }
        surprise.surpriseEffect = false;
        surprise.disqualification = false;
        dicePanel.GetComponent<Dice>().ResetDice();
        currentCompetitor = players[currentCompetitorNo];
        _playerDisplay.DisplayCompetitor(currentCompetitor, currentRun);
        partsOfRun++;

        //if (partsOfRun == 3)
        //{
        //    firstD6 = 4;
        //    secondD6 = 4;
        //    thirdD6 = 6;
        //}
        //else
        //{
        firstD6 = Random.Range(1, 7);
        secondD6 = Random.Range(1, 7);
        thirdD6 = Random.Range(1, 7);
        //}
        competitionRoll = firstD6 + secondD6;
        surprise.CheckSurprise(currentCompetitor); //SURPRISE CHECK

        CheckEvent(currentCompetitor);           // EVENT CHECK
        dicePanel.GetComponent<Dice>().StartCoroutine("showDice");
        if ((currentCompetitorNo >= 0) && (currentCompetitor.myState == Player.PlayerState.Running)
             && (!surprise.surpriseEffect) && (myState == GameState.CompetitionPhase)) // and Event resolved with no surprise
        {
            // dicePanel.GetComponent<Dice>().StartCoroutine("showDice");
            _calculatePerformance.GetPerformanceModifier(currentCompetitor, competitionRoll);
            _playerDisplay.DisplayCompetitor(currentCompetitor, currentRun);
            EndRun();
        }
    }
    public void EndRun()
    {
        ChangeState(GameState.CompetitionPhase);
        if (partsOfRun > 2)
        {
            if (currentRun < 2)
            {
                if (currentCompetitor.myState == Player.PlayerState.Running)
                {
                    finishers.Add(currentCompetitor);
                }
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

            //players.RemoveAt(players.Count - 1);
            RemoveCurrentCompetitor();
            UpdatePlayerList(players, startingList);
            currentCompetitor.CalculateFinal();
            updateResults();
            UpdateLists();
            currentCompetitorNo--;
            partsOfRun = 0;
        }
        else
        {
            UpdateLists();
        }

    }
    public void showResults(Player player)
    {
        if (player.myState == Player.PlayerState.Running)
        {
            finalText.text += "\n" + player.secondName +
            ": AVERAGE: " + player.averagePerformance +
            ", RUN MODIFIERS: " + player.actualModifiers(currentRun).ToString("F2") + ", SUM: ";
            if (currentRun == 2)
            {
                finalText.text += player.secondRunPoints.ToString();
            }
            else
            {
                finalText.text += player.firstRunPoints.ToString("F2");
            }
            finalText.text += ". TOTAL: " + player.finalPerformance.ToString("F2");

        }
        else if ((myState == GameState.PresentationPhase) && (didNotStarted != null))
        {
            finalText.text += "DID NOT STARTED: ";
            foreach (Player item in didNotStarted)
            {
                finalText.text += item.secondName.ToString() + " ";
            }
        }
    }

    public void UpdatePlayerList(List<Player> list, TMP_Text listText)
    {
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

    public void LoadLists()
    {
        for (int i = 0; i < listsSection.Length; i++)
        {
            listsSection[i].GetComponent<PlayerDataLoader>().LoadCompetitorsList();
        }
    }
    public void UpdateLists()
    {
        finishersSection.SetActive(true);
        endOfRunList.SetActive(false);
        for (int i = 0; i < listsSection.Length; i++)
        {
            listsSection[i].GetComponent<PlayerDataLoader>().UpdateCompetitors();
        }
    }
    public void ShowFirstRunList()
    {
        if (currentRun > 1)
        {
            endOfRunList.SetActive(true);
            finishersSection.SetActive(false);
            for (int i = 0; i < listsSection.Length; i++)
            {
                listsSection[i].GetComponent<PlayerDataLoader>().UpdateCompetitors();
            }
        }
        else
        {
            UpdateLists();
        }
    }


    public void updateResults()
    {
        finishersList.text = "";
        resultsList.text = "";
        finishers.Sort((a, b) => b.finalPerformance.CompareTo(a.finalPerformance));
        currentClassification = finishers;
        for (int i = 0; i < finishers.Count; i++)
        {
            finishers[i].place = i + 1;
        }

        if (finishers.Count > 0)
        {
            bestFinalPerformance = finishers[0].finalPerformance;
        }

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

        }
    }

    public void CheckIfEmptyLists()
    {
        Player newPlayer = null;
        if ((players.Count == 0) && (outsiders.Count > 0))
        {
            newPlayer = outsiders[0];
            players.Add(newPlayer);
            outsiders.Remove(newPlayer);
        }
        else if ((outsiders.Count == 0) && (underdogs.Count > 0))
        {
            newPlayer = underdogs[0];
            outsiders.Add(newPlayer);
            underdogs.Remove(newPlayer);
        }
    }

    public void DecorationPhase()
    {
        if ((competitionIsOver) && (!decorationSpawned))
        {
            // TODO: FINAL COMMENTS appear
            ChangeState(GameState.DecorationPhase);
            GameObject newObject = Instantiate(DecorationPanel);
            newObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            newObject.transform.localPosition = new Vector3(7.17f, 0.00f, 0.00f);
            decorationSpawned = true;

        }
    }

    public string TimeDisplay(Player player)
    {
        float pointsDifference = bestFinalPerformance - player.finalPerformance;
        if (Gamemanager.numbersOfRun < 2)
        {
            if (player.place == 1)
            {
                return player.ConvertPointsToTime(player.finalPerformance); // + "\n";
            }
            else
            {
                return "    " + player.ConvertDifference(pointsDifference); // + "\n";
            }
        }
        else
        {
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
    }


    void CheckEvent(Player player)
    {
        if ((!eventHappened) && (!surprise.surpriseEffect) && (partsOfRun != 0) && (firstD6 == secondD6) && (firstD6 + secondD6 != 2) && (firstD6 + secondD6 != 12))
        {
            eventHappened = true;
            var shortEvent = FindObjectOfType<ShortEvent>();
            Debug.Log("EVENT!");
            ChangeState(GameState.EventPhase);
            shortEvent.GetEventType();
        }
    }
    public void HandleNextRun()
    {
        if ((partsOfRun == 0) && (players.Count == 0))// (finishers.Count==10))// && (currentRun < Gamemanager.numbersOfRun))
        {
            currentRun++;
            if (currentRun > Gamemanager.numbersOfRun) // DECORATION PHASE starts
            {
                competitionIsOver = true;
                currentCompetitor = null;
                SpawnEndOfRunClassification();
                // FINAL COMMENTS appear
                runButton.GetComponentInChildren<TextMeshProUGUI>().text = "DECORATION".ToString();
            }

            else
            {
                ChangeState(GameState.EndOfRun);
                firstRunClassification.AddRange(finishers);
                competitionName.text = gamemanager.competitionName.ToString() + currentRun.ToString() + "/" + Gamemanager.numbersOfRun;
                // runButton.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT RUN".ToString();
                players.AddRange(finishers);
                bestFirstRunPerformance = finishers[0].firstRunPoints;
                possibleReturnsFromOutOf15 = outOf15Competitors.Count;
                // TODO:  create empty 2nd run list
                // TODO: change weather, decrease surprise/weather effect
                UpdatePlayerList(players, startingList);
                UpdateLists();
                currentCompetitorNo = players.Count - 1;
                List<Player> blackHorses = new List<Player>();
                blackHorses.AddRange(outsiders);
                blackHorses.AddRange(underdogs);
                blackHorses.AddRange(outOf15Competitors);
                // COMMENTATOR PRAISES RESET
                foreach (var player in players)
                {
                    player.praisesByCommentator = 0;
                    player.firstRunPlace = player.place;
                }
                foreach (var player in blackHorses)
                {
                    player.firstRunPoints = finishers[finishers.Count - 1].firstRunPoints - 3;
                }
                finishers.Clear();
                UpdateLists();
                Debug.Log("NEXT RUN PREPARED!");
                Debug.Log("BLACKHORSES POINTS: " + blackHorses[0].firstRunPoints);
                SpawnEndOfRunClassification();
                //possible weather change, +/ -weather modifier
            }

        }

    }


    public void ChangeState(GameState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);
        myState = newState;
        switch (newState)
        {
            case GameState.WeatherPhase:
                weatherPanel.SetActive(true);
                runButton.SetActive(false);
                dicePanel.SetActive(false);
                presentationPanel.SetActive(false); break;

            case GameState.PresentationPhase:
                weatherPanel.SetActive(false);
                setupButton.SetActive(false);
                runButton.SetActive(false);
                presentationPanel.SetActive(true);
                dicePanel.SetActive(false); break;

            case GameState.CompetitionPhase:
                presentationPanel.SetActive(false);
                endOfRunList.SetActive(false);
                runButton.SetActive(true);
                dicePanel.SetActive(true); break;

            case GameState.CheckSurprisePhase:
                presentationPanel.SetActive(false);
                runButton.SetActive(false);
                dicePanel.SetActive(true); break;

            case GameState.EventPhase:
                weatherPanel.SetActive(false);
                setupButton.SetActive(false);
                presentationPanel.SetActive(false);
                runButton.SetActive(false);
                dicePanel.SetActive(true); break;

            case GameState.EndOfRun:
                weatherPanel.SetActive(false);
                setupButton.SetActive(false);
                presentationPanel.SetActive(false);
                runButton.SetActive(true);
                endOfRunList.SetActive(true);
                dicePanel.SetActive(false); break;

            case GameState.DecorationPhase:
                weatherPanel.SetActive(false);
                setupButton.SetActive(false);
                presentationPanel.SetActive(false);
                runButton.SetActive(false);
                dicePanel.SetActive(false);
                competitorPanel.SetActive(false); break;

            default: throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnAfterStateChanged?.Invoke(newState);
        Debug.Log($"New state: {newState}");
    }

    public void RemoveCurrentCompetitor()
    {
        if (!surprise.surpriseEffect)
        {
            players.RemoveAt(players.Count - 1);
        }

    }

    public void SpawnEndOfRunClassification()
    {
        canvasGroup.GetComponent<MoveObject>().MoveToCenter();
    }
}




