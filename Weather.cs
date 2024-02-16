using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;

public class Weather : MonoBehaviour
{
    public float minTemp;
    public float maxTemp;
    public float actualTemperature = 0.00f;
    public bool diceChanging;
    public int diceIndex;
    public int firstD6;
    public int secondD6;
    public bool temperatureRolled;
    private float pause = 0.50f;
    public TMP_Text buttonText;
    [SerializeField] Sprite[] diceSides;
    [SerializeField] TMP_Text temperatureText;
    [SerializeField] TMP_Text snowConditionText;
    [SerializeField] GameObject[] firstDieImages;
    [SerializeField] GameObject[] secondDieImages;
    [SerializeField] GameObject[] weatherCharts;
    [SerializeField] TMP_Text[] descriptionTexts;
    [SerializeField] GameObject precipitationImage;
    [SerializeField] Sprite[] precipitationSprites;
    [SerializeField] GameObject[] chartIndicator;
    public string precipitation { get; set; }
    public string snowCondition { get; set; }
    private int snowConditionModifier;
    private int precipitationModifier = 0;
    public bool weatherPhaseOver;
    Competition competition;
    public float weatherModifier; //affects probability of surprises 

    public void Start()
    {
        competition = Competition.Instance; 
        diceIndex = 0;
        precipitation = string.Empty;
        temperatureRolled = false;
     //   ResetWeatherCharts();
        weatherPhaseOver = false;
        weatherModifier = 1.00f; //default
    }

    public void CalculateTemperature()
    {
        minTemp = FindObjectOfType<Gamemanager>().temperatureMin;
        maxTemp = FindObjectOfType<Gamemanager>().temperatureMax;

        float averageTemperature = Random.Range(minTemp, maxTemp);
        string description = "";

        switch (firstD6 + secondD6)
        {
            case 2:
                actualTemperature += (minTemp + Random.Range(-15.00f, -10.10f));
                description = "EXTREME COLD" + "\n" + "+2 to snow condition roll";
                snowConditionModifier += 2; weatherModifier *= 1.50f; break;//EXTREME COLD
            case 3:
                actualTemperature += (minTemp + Random.Range(-10.00f, -6.10f));
                description = "VERY COLD" + "\n" + "+1 to snow condition roll";
                snowConditionModifier += 1; weatherModifier *= 1.30f; precipitationModifier -= 1; break;//VERY COLD
            case 4:
                actualTemperature += (minTemp + Random.Range(-6.00f, -3.10f));
                description = "COLD"; weatherModifier *= 1.10f; break;// COLD
            case 5:
                actualTemperature += (minTemp + Random.Range(-3.00f, -0.10f));
                description = "BELOW AVERAGE"; break;// BELOW AVERAGE
            case 9:
                actualTemperature += (maxTemp + Random.Range(0.10f, 3.00f));
                description = "ABOVE AVERAGE"; break;// ABOVE AVERAGE
            case 10:
                actualTemperature += (maxTemp + Random.Range(3.10f, 6.00f));
                description = "HOT"; weatherModifier *= 0.90f; precipitationModifier += 1; break;// HOT
            case 11:
                actualTemperature += (maxTemp + Random.Range(6.10f, 10.00f));
                description = "VERY HOT" + "\n" + "-1 to snow condition roll";
                snowConditionModifier -= 1; weatherModifier *= 0.70f; precipitationModifier += 2; break;// VERY HOT
            case 12:
                actualTemperature += (maxTemp + Random.Range(10.10f, 15.00f));
                description = "EXTREME HOT" + "\n" + "-2 to snow condition roll";
                snowConditionModifier -= 2; weatherModifier *= 0.50f; precipitationModifier += 3;
                break;// EXTREME HOT
            default:
                actualTemperature = averageTemperature;
                description = "AVERAGE"; break;// AVERAGE

        }
        descriptionTexts[0].text = description.ToString();
        weatherCharts[0].SetActive(true);
        temperatureText.text = actualTemperature.ToString("F0");
        Debug.Log("TEMP IS: " + actualTemperature);
        ChangeButtonName("NEXT");
        temperatureRolled = true;
        // StartCoroutine("WeatherDice");
        // return actualTemperature;
    }

    public IEnumerator WeatherDice()
    {
        //ResetWeatherCharts();
        diceChanging = true;
        firstD6 = Random.Range(1, 7);
        secondD6 = Random.Range(1, 7);


        firstDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[firstD6 - 1];
        yield return new WaitForSeconds(pause);
        if (diceIndex < 2)
        {
            secondDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[secondD6 + 5];
            yield return new WaitForSeconds(pause);
        }
        if (!temperatureRolled)
        {
            CalculateTemperature();
        }
        if (diceIndex == 1)
        {
            CalculatePrecipitation();
        }
        else if (diceIndex == 2)
        {
            CalculateSnowCondition();

        }
        //thirdDieImages[diceIndex].GetComponent<SpriteRenderer>().sprite = diceSides[competition.thirdD6 + 11];
        // description.ShowDescription();
        // weatherCharts[0].SetActive(true);
      // TO DO:  MoveChartIndicator(firstD6 + secondD6, 0);
        diceChanging = false;
        diceIndex++;

    }

    private void CalculateSnowCondition()
    {
        int snowConditionChance = firstD6 + snowConditionModifier;

        if (snowConditionChance < 3)
        {
            snowCondition = "fresh"; weatherModifier *= 1.25f;//FRESH SHOW }
        }
        else
        {
            snowCondition = "hard";//HARD SHOW }
        }
        snowConditionText.text = "SNOW: " + snowCondition.ToUpper().ToString(); 
        descriptionTexts[2].text = snowCondition.ToUpper().ToString();
        weatherCharts[2].SetActive(true);
        ChangeButtonName("PRESENTATION");
        weatherPhaseOver = true;
    }

    public void WeatherButton()
    {
        if (!weatherPhaseOver)
        {
            StartCoroutine("WeatherDice");
        }
        else
        {
            competition.myState = Competition.GameState.PresentationPhase;
            Debug.Log("PRESENTATION PHASE");
        }
    }

    public void CalculatePrecipitation()
    {
        switch (firstD6 + secondD6+precipitationModifier)
        {
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                precipitation = "snowing"; snowConditionModifier -= 2; weatherModifier *= 1.40f;
                precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[0]; break;//SNOWING
            case 9:
                precipitation = "raining"; snowConditionModifier += 1; weatherModifier *= 1.20f;
                precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[1]; break;//RAINING
            default:
                precipitation = "no precipitation"; break;// AVERAGE
                // TODO case 11: CHINOOK WIND Special weather effect

        }
        descriptionTexts[1].text = precipitation.ToUpper().ToString();
        if (precipitation.Contains("snowing"))
        {
            descriptionTexts[1].text += "\n" + "-2 to snow condition roll";
        }
        else if (precipitation.Contains("raining"))
        {
            descriptionTexts[1].text += "\n" + "+1 to snow condition roll";


        }
        weatherCharts[1].SetActive(true);
        ChangeButtonName("NEXT");
    }

    public void ResetWeatherCharts()
    {
        for (int i = 0; i < weatherCharts.Length; i++)
        {
            weatherCharts[i].SetActive(false);
        }
    }

    public void ChangeButtonName(string buttonName)
    {
        buttonText.text = buttonName.ToString();
    }



    


}
