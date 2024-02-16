using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Absences : MonoBehaviour
{
    Competition competition;
    public List<Player> _starters;
    public List<Player> _outsiders;
    public List<Player> _underdogs;
    Weather weather;
    private float weatherFactor;

    public void Start()
    {
        competition = Competition.Instance;
        weather = FindObjectOfType<Weather>();
    }

    public void CheckAbsence()
    {
        _starters = competition.players;
        _outsiders = competition.outsiders;
        _underdogs = competition.underdogs;
        weatherFactor = weather.weatherModifier * 2;

        int absenceRoll = Random.Range(1, 101) - (int)(weatherFactor);
        Debug.Log("ABSENCE ROLL: " + absenceRoll + ". Weather factor: " + (int)weatherFactor);
        if ((absenceRoll < 16) && (absenceRoll > 8))
        {
            RemovePlayerFromList(_starters, 1);
        }
        else if ((absenceRoll < 9) && (absenceRoll > 3))
        {
            RemovePlayerFromList(_starters, 1);
            RemovePlayerFromList(_outsiders, 1);
        }
        if (absenceRoll < 4)
        {
            RemovePlayerFromList(_starters, 1);
            RemovePlayerFromList(_outsiders, 1);
            RemovePlayerFromList(_underdogs, 1);
        }
    }

    public void RemovePlayerFromList(List<Player> listOfPlayers, int playersDidNotStarted)
    {
        Player absencePlayer = null;
        if (listOfPlayers == _starters)
        {
            for (int i = 0; i < playersDidNotStarted; i++)
            {
                absencePlayer = competition.players[competition.players.Count - 4];
                absencePlayer.myState = Player.PlayerState.DidNotStart;
                Debug.Log("DNS: " + competition.players[competition.players.Count - 4].secondName);
                competition.didNotStarted.Insert(competition.didNotStarted.Count, competition.players[competition.players.Count - 4]);
                competition.players.RemoveAt(competition.players.Count - 4);
                competition.players.Insert(competition.players.Count, competition.outsiders[competition.outsiders.Count - 4]);
                competition.outsiders.RemoveAt(competition.outsiders.Count - 4);
                competition.outsiders.Insert(competition.outsiders.Count, competition.underdogs[competition.underdogs.Count - 1]);
                competition.underdogs.RemoveAt(competition.underdogs.Count - 1);
            }
        }
        else if (listOfPlayers == _outsiders)
        {
            for (int i = 0; i < playersDidNotStarted; i++)
            {
                absencePlayer = competition.outsiders[competition.outsiders.Count - 3];
                absencePlayer.myState = Player.PlayerState.DidNotStart;
                Debug.Log("DNS: " + competition.outsiders[competition.outsiders.Count - 3].secondName);
                competition.didNotStarted.Insert(competition.didNotStarted.Count, competition.outsiders[competition.outsiders.Count - 3]);
                competition.outsiders.RemoveAt(competition.outsiders.Count - 3);
                competition.outsiders.Insert(competition.outsiders.Count, competition.underdogs[competition.underdogs.Count - 1]);
                competition.underdogs.RemoveAt(competition.underdogs.Count - 1);
            }
        }
        else if (listOfPlayers == _underdogs)
        {
            for (int i = 0; i < playersDidNotStarted; i++)
            {
                absencePlayer = competition.underdogs[competition.underdogs.Count - 1];
                absencePlayer.myState = Player.PlayerState.DidNotStart;
                Debug.Log("DNS: " + competition.underdogs[competition.underdogs.Count - 1].secondName);
                competition.didNotStarted.Insert(competition.didNotStarted.Count, competition.underdogs[competition.underdogs.Count - 1]);
                competition.underdogs.RemoveAt(competition.underdogs.Count - 1);
            }
        }

        competition.showResults(absencePlayer);
        // Debug.Log("DNS: " + competition.players[competition.players.Count - 1].secondName);
    }

}


