using DamageNumbersPro;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    //[SerializeField] public TMP_Text finalText; // scrollViewText
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
    public string message;
    [SerializeField] private GameObject setupButton;
    [SerializeField] private GameObject eventButton;
    [SerializeField] private PlayerDisplay _playerDisplay;
    [SerializeField] public GameObject messageWindow;

    [Header("List Sections")] //ACTUAL SECTIONS
    [SerializeField] GameObject startersSection;
    [SerializeField] GameObject outsidersSection;
    [SerializeField] GameObject underdogsSection;
    [SerializeField] GameObject finishersSection;
    [SerializeField] GameObject outOf15Section;
    [SerializeField] GameObject didNotFinishSection;
    [SerializeField] GameObject disqualifiedSection;
    [SerializeField] GameObject endOfFirstRunList;
    [SerializeField] GameObject endOfSecondRunList;
    [SerializeField] GameObject[] listsSection;

    [Header("Panels")]
    public GameObject weatherPanel;
    public GameObject presentationPanel;
    public GameObject eventPanel;
    public GameObject competitorPanel;
    public GameObject scrollViewPanel;
    //[SerializeField] private GameObject DecorationPanel;

    public GameObject runButton;
    public GameObject tabSection;
    public List<Player> players = new List<Player>();
    public List<Player> finishers = new List<Player>();
    public List<Player> currentClassification = new List<Player>();
    public List<Player> outsiders = new List<Player>();
    public List<Player> underdogs = new List<Player>();
    public List<Player> bonusCompetitors = new List<Player>();
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
    public float bestSecondRunPerformance;
    public float bestRunPerformance;
    public GameState myState { get; set; }
    public bool eventHappened = false;
    // private bool decorationSpawned = false;
    public Gamemanager gamemanager;
    public float firstSectorMostPoints = 20f; // average from early competitors runs
    public float secondSectorMostPoints = 20f;
    public float thirdSectorMostPoints = 20f;
    public CalculatePerformance _calculatePerformance;
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public Canvas canvasGroup;
    CultureInfo ci = new CultureInfo("en-US");
    public Camera mainCam;
    private string[] startSounds = new string[] { "start01", "start02", "start03" };
    PointsSystem pointsSystem;
    public bool bonusCompetitorsUnlocked = false;
    public DamageNumber numberPrefab;
    public GameObject summary;

    public enum GameState
    {
        WeatherPhase = 0,
        PresentationPhase = 1,
        StartPhase = 2,
        CompetitionPhase = 3,
        CheckSurprisePhase = 4,
        EventPhase = 5,
        EndOfRun = 6,
        SummaryPhase = 7,
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
        bonusCompetitors = gamemanager.bonusCompetitors;
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
        competitionName.text = gamemanager.competitionName.ToString() + " RUN " + currentRun.ToString() + "/" + Gamemanager.numbersOfRun;
        AlpineCombinedModifiers();
        //DateTime DISPLAY:
        //competitionName.text = gamemanager.competitionType.competitionDate.ToString("d MMMM yyyy", ci) +", "+
        //   gamemanager.competitionType.competitionVenueName.ToString() + ", " +
        //   gamemanager.competitionType.competitionName.ToString() ;  
        _calculatePerformance = FindObjectOfType<CalculatePerformance>();
        numberOfFavourites = players.Count;
        pointsSystem = PointsSystem.Instance;
        pointsSystem.ResetCompetitionPoints();

    }

    void Update()
    {
        // GameStatesManager();
        //HandleNextRun();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Run();
        }
        // DecorationPhase();
    }

    public void Run()
    {
        //SpawnEndOfRunClassification();
        // shake.start = true;
        Camera.main.GetComponent<ScreenShake>().CameraShake();
        mainCam.transform.DOShakePosition(2.0f, 100.0f, 10, 10f, true, true);
        if (competitionIsOver)
        {
            //DecorationPhase(); 
            ChangeState(GameState.SummaryPhase);
            summary.GetComponent<CompetitionSummary>().DoDecoration();
            return;
        }

        // PLAY AUDIO-START sound
        surprise.surpriseEffect = false;
        surprise.disqualification = false;
        dicePanel.GetComponent<Dice>().ResetDice();
        currentCompetitor = players[currentCompetitorNo];
        _playerDisplay.DisplayCompetitor(currentCompetitor, currentRun);
        partsOfRun++;
        //if (partsOfRun == 1)
        // {
        //  SoundManager.PlayRandomSound(startSounds);
        // }

        firstD6 = Random.Range(1, 7);
        secondD6 = Random.Range(1, 7);
        thirdD6 = Random.Range(1, 7);
        //firstD6 = 4; secondD6 = 4; thirdD6 = 4;
        competitionRoll = firstD6 + secondD6;
        pointsSystem.AddTemporaryPoints(competitionRoll * thirdD6);
        if (((outsiders.Count > 0) || (underdogs.Count > 0)) && (firstD6 + secondD6 + thirdD6 != 18))
        {
            surprise.CheckSurprise(currentCompetitor); //SURPRISE CHECK
            CheckEvent(currentCompetitor);           // EVENT CHECK
        }
        dicePanel.GetComponent<Dice>().StartCoroutine("showDice");
        if ((currentCompetitorNo >= 0) && (currentCompetitor.myState == Player.PlayerState.Running)
             && (!surprise.surpriseEffect) && (myState == GameState.CompetitionPhase)) // and Event resolved with no surprise
        {
            // dicePanel.GetComponent<Dice>().StartCoroutine("showDice");
            _calculatePerformance.GetPerformanceModifier(currentCompetitor, competitionRoll);
            _playerDisplay.DisplayCompetitor(currentCompetitor, currentRun);
            if ((partsOfRun == 1) && (currentCompetitor.myState == Player.PlayerState.Running))
            {
                SoundManager.PlayRandomSound(startSounds);
            }
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
                    secondRunClassification.Add(currentCompetitor);
                    UpdateSecondRun();
                }
            }
            eventHappened = false;

            //players.RemoveAt(players.Count - 1);
            RemoveCurrentCompetitor();
            UpdatePlayerList(players, startingList);
            currentCompetitor.CalculateFinal();
            pointsSystem.AddGamePoints(pointsSystem.temporaryPoints);
            pointsSystem.ResetTemporaryPoints();
            updateResults();

            UpdateLists();
            currentCompetitorNo--;
            partsOfRun = 0;
            HandleNextRun(); // przesuniête tutaj z UPDATE();   
        }
        else
        {
            UpdateLists();
        }

    }
    public void showResults(Player player)
    {

        if (player.myState == Player.PlayerState.Running) //"\n"
        {
            message = player.secondName.ToString() +
            ":" + "\n" + "AVG: " + player.averagePerformance.ToString() +
            "\n" + "RUN MOD: " + player.actualModifiers(currentRun).ToString("F2") + "\n" +
            "SECTOR SUM: " + (player.averagePerformance + player.actualModifiers(currentRun)).ToString("F2") +
            "\n" + "SUM: ";
            if (currentRun == 2)
            {
                message += player.secondRunPoints.ToString("F2");
            }
            else
            {
                message += player.firstRunPoints.ToString("F2");
            }
            message += "\n" + "TOTAL: " + player.finalPerformance.ToString("F2") + "\n";

        }
        else if ((myState == GameState.PresentationPhase) && (didNotStarted != null))
        {
            scrollViewPanel.SetActive(true);
            message = "DID NOT STARTED: " + "\n";
            foreach (Player item in didNotStarted)
            {
                message += ">>> " + item.secondName.ToString() + " ";
            }
        }
        messageWindow.GetComponent<ScrollViewManager>().AddMessage(message);

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
        endOfFirstRunList.SetActive(false);
        endOfSecondRunList.SetActive(false);
        for (int i = 0; i < listsSection.Length; i++)
        {
            listsSection[i].GetComponent<PlayerDataLoader>().UpdateCompetitors();
        }
    }
    public void ShowFirstRunList()
    {
        if (currentRun > 1)
        {
            endOfFirstRunList.SetActive(true);
            endOfSecondRunList.SetActive(false);
            finishersSection.SetActive(false);
            for (int i = 0; i < listsSection.Length; i++)
            {
                listsSection[i].GetComponent<PlayerDataLoader>().UpdateCompetitors();
            }
            endOfFirstRunList.GetComponent<PlayerDataLoader>().listFrozen = true;
        }
        else
        {
            UpdateLists();
        }
    }

    public void ShowSecondRunList()
    {
        if (currentRun > 1)
        {
            endOfFirstRunList.SetActive(false);
            endOfSecondRunList.SetActive(true);
            finishersSection.SetActive(false);
            for (int i = 0; i < listsSection.Length; i++)
            {
                listsSection[i].GetComponent<PlayerDataLoader>().UpdateCompetitors();
            }
        }
    }

    public void UpdateSecondRun()
    {
        secondRunClassification.Sort((a, b) => b.secondRunPoints.CompareTo(a.secondRunPoints));
        for (int i = 0; i < secondRunClassification.Count; i++)
        {
            secondRunClassification[i].secondRunPlace = i + 1;
        }
        if (secondRunClassification.Count > 0)
        {
            bestSecondRunPerformance = secondRunClassification[0].secondRunPoints;
        }
    }


    public void updateResults()
    {
        finishersList.text = "";
        resultsList.text = "";
        finishers.Sort((a, b) => b.finalPerformance.CompareTo(a.finalPerformance));
        // SKI JUMPING Sort b.finalPoints (distance points+judges points) 
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
        else if ((outsiders.Count == 0) && (underdogs.Count == 0))
        {
            // BONUS COMPETITORS APPEAR
            bonusCompetitorsUnlocked = true;
            outsiders.AddRange(bonusCompetitors);
            DamageNumber damageNumber = numberPrefab.SpawnGUI(runButton.GetComponent<RectTransform>(), Vector2.zero, "BONUS DATABASE UNLOCKED!".ToString());
            pointsSystem.AddGamePoints(2500);

        }

    }

    //public void DecorationPhase()
    //{
    //    if ((competitionIsOver) && (!decorationSpawned))
    //    {
    //        // TODO: FINAL COMMENTS appear
    //        ChangeState(GameState.SummaryPhase);
    //        GameObject newObject = Instantiate(DecorationPanel);
    //        newObject.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
    //        newObject.transform.localPosition = new Vector3(7.17f, 122.00f, 0.00f);
    //        decorationSpawned = true;

    //    }
    //}

    public string TimeDisplay(Player player)
    {
        float pointsDifference = bestFinalPerformance - player.finalPerformance;
        if (Gamemanager.numbersOfRun < 2)
        {
            if (player.place == 1)
            {
                return player.ConvertPointsToTime(player.finalPerformance, "finalPoints"); // + "\n";
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
                    return player.ConvertPointsToTime(player.firstRunPoints, "firstRunPoints") + "......" + player.ConvertPointsToTime(player.secondRunPoints, "secondRunPoints") + "\n";
                }
                else
                {
                    return player.ConvertPointsToTime(player.firstRunPoints, "firstRunPoints") + "......" + "\n";
                }
            }
            else
            {
                return player.ConvertPointsToTime(player.finalPerformance, "finalPoints") + "\n";
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
            SoundManager.PlayOneSound("crowd01");
            if (currentRun > Gamemanager.numbersOfRun) // DECORATION PHASE starts
            {
                competitionIsOver = true;
                //currentCompetitor = null;
                SpawnEndOfRunClassification();
                // FINAL COMMENTS appear
                runButton.GetComponentInChildren<TextMeshProUGUI>().text = "DECORATION".ToString();
            }

            else
            {
                ChangeState(GameState.EndOfRun);
                tabSection.SetActive(true);
                firstRunClassification.AddRange(finishers);
                competitionName.text = gamemanager.competitionName.ToString() + " " + currentRun.ToString() + "/" + Gamemanager.numbersOfRun;
                AlpineCombinedModifiers();
                // runButton.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT RUN".ToString();
                players.AddRange(finishers);
                bestFirstRunPerformance = finishers[0].firstRunPoints;
                //bestSecondRunPerformance = secondRunClassification[0].secondRunPoints;
                possibleReturnsFromOutOf15 = outOf15Competitors.Count;

                // TODO: change weather, decrease surprise/weather effect
                ConditionsChange();
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
                scrollViewPanel.SetActive(false);
                presentationPanel.SetActive(false); break;

            case GameState.PresentationPhase:
                weatherPanel.SetActive(false);
                setupButton.SetActive(false);
                runButton.SetActive(false);
                presentationPanel.SetActive(true);
                dicePanel.SetActive(false); break;

            case GameState.CompetitionPhase:
                presentationPanel.SetActive(false);
                endOfFirstRunList.SetActive(false);
                runButton.SetActive(true);
                dicePanel.SetActive(true);
                scrollViewPanel.SetActive(true); break;

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
                endOfFirstRunList.SetActive(true);
                scrollViewPanel.SetActive(true);
                dicePanel.SetActive(false); break;

            case GameState.SummaryPhase:
                weatherPanel.SetActive(false);
                setupButton.SetActive(false);
                presentationPanel.SetActive(false);
                runButton.SetActive(false);
                dicePanel.SetActive(false);
                competitorPanel.SetActive(false);
                scrollViewPanel.SetActive(false); break;

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
        bool objectDuplicated = false;
        if ((!objectDuplicated) && (competitionIsOver))
        {
            GameObject listToCopy = finishersSection;
            GameObject copiedObject = Instantiate(listToCopy);
            copiedObject.transform.SetParent(canvasGroup.transform, false);
            // copiedObject.transform.localPosition= new Vector3(0.0f, 0.0f, 0.00f);
            //copiedObject.transform.localPosition = new Vector3(690.0f, 508.0f, 0.00f);
            //copiedObject.DOAnchorPos(new Vector2(690.0f, 508.0f), 1.0f, false).SetEase(Ease.OutElastic);
            objectDuplicated = true;
            //canvasGroup.GetComponent<MoveObject>().rectTransform = copiedObject.GetComponent<RectTransform>();
            //rectTransform.DOAnchorPos(new Vector2(690.0f, 508.0f), 1.0f, false).SetEase(Ease.OutElastic);
            //canvasGroup.GetComponent<MoveObject>().StartCoroutine("MoveToCenter");
        }

        //    finishersSection.SetActive(true);
        //   // GameObject finalChart = finishersSection.Clone;

        //    RectTransform obj = finishersSection.GetComponent<RectTransform>();    
        //   // obj.transform.localPosition = new Vector3(0f, 0f, 0f);
        //    obj.transform.DOMove(new Vector3(0f, 0f, 0f), 0.2f);
        //    obj.DOAnchorPos(new Vector2(0f, 122.0f), 1.0f, false).SetEase(Ease.OutElastic);
        //}
        else
        {
            canvasGroup.GetComponent<MoveObject>().StartCoroutine("MoveToCenter");
        }
    }

    public void ConditionsChange()
    {
        float probabilityToChange = Random.Range(0.40f, 1.21f);
        float timeModifier = probabilityToChange < 0.60 ? Random.Range(-0.09f, -0.05f) :
                             probabilityToChange > 1.00 ? Random.Range(-0.04f, 0.09f) :
                             -0.05f;//AVERAGE IS 0.80
        gamemanager.surprisesModifier *= probabilityToChange * 0.90f;
        gamemanager.ModifyTimes(timeModifier);
        Debug.Log("CONDITITINS CHANGE: " + probabilityToChange.ToString("F2"));
        Debug.Log("TIME MODIFIER: " + timeModifier.ToString("F2"));
        Debug.Log("NEW BEST TIME: " + gamemanager.bestTimeInSec.ToString("F2"));
        var shortEvent = FindObjectOfType<ShortEvent>();
        shortEvent.eventObject.SetActive(true);
        // shortEvent.eventTitle.text = "CONDITIONS CHANGE";
        shortEvent.descriptionText.text += weatherPanel.GetComponent<Weather>().CheckPrecipitationChange(probabilityToChange);
        //shortEvent.descriptionText.text += "CONDITIONS CHANGE: " + probabilityToChange.ToString("F2");
        shortEvent.eventResolved = true;

    }
    public void AlpineCombinedModifiers()
    {
        if (gamemanager.thisCompetition.IsCombined == true)
        {
            if (currentRun == 1)
            {
                competitionName.text += " " + gamemanager.thisCompetition.firstCombinedCompetition.ToString();
            }
            else if (currentRun == 2)
            {
                competitionName.text += " " + gamemanager.thisCompetition.secondCombinedCompetition.ToString();
                gamemanager.competitionName = "Slalom";
            }
        }

    }
}




