using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class AchievementsManager : MonoBehaviour
{
    public static AchievementsManager Instance { get; private set; }
    public int doublesTotal = 0;
    public int triplesTotal = 0;
    public int straightsTotal = 0;
    public int hatTricksTotal = 0;
    public int actualCompetitionDoubles = 0;
    public int actualCompetitionTriples = 0;
    public int actualCompetitionStraights = 0;
    public int actualCompetitionHatTricks = 0;
    public List<string> achievementsKeywords = new List<string> { };
    Competition competition;
    public bool homeCompetitorOnPodium;

    //[SerializeField] TMP_Text normalPointsTotal;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            // GeT data from PlayerPrefs
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        competition = Competition.Instance;
    }

    public void AddDouble()
    {
        actualCompetitionDoubles++;
    }
    public void AddTriple()
    {
        actualCompetitionTriples++;
    }
    public void AddStraight()
    {
        actualCompetitionStraights++;
    }
    public void AddHatTrick()
    {
        actualCompetitionHatTricks++;
    }

    public void SumDiceCombos()
    {
        doublesTotal += actualCompetitionDoubles;
        triplesTotal += actualCompetitionTriples;
        straightsTotal += actualCompetitionStraights;
        hatTricksTotal += actualCompetitionHatTricks;   
    }

    public void ResetActualCompetitionCombos()
    {
        actualCompetitionDoubles = 0;
        actualCompetitionTriples = 0;
        actualCompetitionStraights = 0;
        actualCompetitionHatTricks = 0;
    }
    public void CheckAchievements()
    {
        //competition = Competition.Instance;
        var firstThree = competition.finishers.Where(f => f.place < 4);

        homeCompetitorOnPodium = competition.finishers.Any(player => player.place < 4 && player.nationality.Contains(FindObjectOfType<VenueDisplay>().venueNation.text));
        bool tragedy = ((competition.disqualified.Count) + (competition.didNotFinish.Count)) > 4;
        bool favouriteOut = competition.outOf15Competitors.Any(player => player.startingGrade =='A');
        bool favouriteDisaster = ((competition.disqualified.Any(player => player.startingGrade =='A')) ||
                                  (competition.didNotFinish.Any(player => player.startingGrade =='A')));
        bool veteranWins = competition.finishers.Any(player => player.place == 1 && player.experience == 3);
        bool talentWins = competition.finishers.Any(player => player.place == 1 && player.experience == 0);
        bool sameNationalityOnPodium =firstThree.GroupBy(f => f.nationality)
                                                      .Any(g => g.Count() >= 2);
        bool oneNationalityOnPodium = firstThree.GroupBy(f => f.nationality)
                                                      .Any(g => g.Count() == 3);

        // summary - after points display
        //What data: winner, 2-3 places, player initial ranking, weather, Venue
        //out of 15, DQ, DNF, good/bad comments 65%, 
        // creates actual achievements
        // "Tragedy of nation" 3 competitors from one country OUTOF15/DQ/DNF
        if (homeCompetitorOnPodium)
        {
            achievementsKeywords.Add("Home podium");
        }
        if (tragedy)
        {
            achievementsKeywords.Add("Tragedy");
        }
        if (favouriteOut)
        {
            achievementsKeywords.Add("Favourite out"); // TO DO: add favourite name to Achievement description
        }
        if (favouriteDisaster)
        {
            achievementsKeywords.Add("Favourite disaster"); // TO DO: add favourite name to Achievement description
        }
        if (veteranWins)
        {
            achievementsKeywords.Add("Veteran wins"); 
        }
        if (talentWins)
        {
            achievementsKeywords.Add("Talent wins");
        }
        if (sameNationalityOnPodium)
        {
            achievementsKeywords.Add("Pride of nation");
        }
        if (oneNationalityOnPodium)
        {
            achievementsKeywords.Add("Nation domination");
        }
    }
}

    
