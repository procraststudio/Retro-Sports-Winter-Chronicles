using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsToTimeConverter : MonoBehaviour
{
    private void Start()
    {
       // int performancePoints = 8523; // Przyk�adowa ilo�� punkt�w
       // string formattedTime = ConvertPointsToTime(performancePoints);

      //  Debug.Log("Czas przejazdu: " + formattedTime);
    }

    private string ConvertPointsToTime(int points)
    {
        int totalHundredths = points; // Zak�adamy, �e 1 punkt = 1 setna sekundy
        int seconds = totalHundredths / 100;
        int hundredths = totalHundredths % 100;

        int minutes = seconds / 60;
        seconds %= 60;

        string formattedTime = string.Format("TIME {0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);

        return formattedTime;
    }
}
