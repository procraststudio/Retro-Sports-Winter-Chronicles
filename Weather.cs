using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weather : MonoBehaviour
{
    public float minTemp;
    public float maxTemp;
    public float averageTemp;
    public float actualTemperature = 0.00f;
    public bool diceChanging;
    public int diceIndex;
    public int firstD6;
    public int secondD6;
    public bool temperatureRolled;
    private float pause = 1.00f;
    public TMP_Text buttonText;
    [SerializeField] Sprite[] diceSides;
    [SerializeField] TMP_Text[] temperatureText;
    [SerializeField] TMP_Text snowConditionText;
    [SerializeField] TMP_Text precipitationText;
    [SerializeField] TMP_Text windConditionText;
    [SerializeField] GameObject[] weatherCharts;
    [SerializeField] TMP_Text[] descriptionTexts;
    [SerializeField] GameObject precipitationImage;
    [SerializeField] Sprite[] precipitationSprites;
    [SerializeField] GameObject[] chartIndicator;
    [SerializeField] GameObject[] weatherSections;
    [SerializeField] GameObject setupButton;
    [SerializeField] TMP_Text surprisesModifierText;
    public static string precipitation { get; set; }
    public string snowCondition { get; set; }
    public static string windCondition { get; set; }
    public static string windDirection = "";
    private int snowConditionModifier;
    private int precipitationModifier = 0;
    public bool weatherPhaseOver;
    Competition competition;
    public float weatherModifier; //affects probability of surprises 
    VenueLoader venueLoader;


    public void Start()
    {
        competition = Competition.Instance;
        diceIndex = 0;
        precipitation = string.Empty;
        temperatureRolled = false;
        weatherPhaseOver = false;
        weatherModifier = 1.00f; //default
        venueLoader = FindObjectOfType<Gamemanager>().GetComponent<VenueLoader>();
        firstD6 = Random.Range(1, 7);
        secondD6 = Random.Range(1, 7);
    }

    public void CalculateTemperature()
    {
        averageTemp = venueLoader.actualVenue.averageTemperature;
        //minTemp = FindObjectOfType<Gamemanager>().temperatureMin;
        // maxTemp = FindObjectOfType<Gamemanager>().temperatureMax;
        actualTemperature = averageTemp;
        // float averageTemperature = Random.Range(minTemp, maxTemp);
        //string description = "";
        Debug.Log("TEMPERATURE ROLL: " + (firstD6 + secondD6));

        switch (firstD6 + secondD6)
        {
            case 2:
                actualTemperature += Random.Range(-13.00f, -7.10f);
                //description = "EXTREME COLD"; //+ "\n" + "+2 to snow condition roll";
                snowConditionModifier += 2; weatherModifier *= 1.50f; break;//EXTREME COLD
            case 3:
                actualTemperature += Random.Range(-7.00f, -5.10f);
                //description = "VERY COLD"; // + "\n" + "+1 to snow condition roll";
                snowConditionModifier += 1; weatherModifier *= 1.30f; precipitationModifier -= 1; break;//VERY COLD
            case 4:
                actualTemperature += Random.Range(-5.00f, -3.10f); break;
            //description = "COLD"; weatherModifier *= 1.10f; // COLD
            case 5:
                actualTemperature += Random.Range(-3.00f, -1.10f); break;
            //description = "BELOW AVERAGE"; // BELOW AVERAGE
            case 9:
                actualTemperature += Random.Range(1.10f, 3.00f); break;
            // description = "ABOVE AVERAGE"; // ABOVE AVERAGE
            case 10:
                actualTemperature += Random.Range(3.10f, 5.00f); break;
            //description = "HOT"; weatherModifier *= 0.90f; precipitationModifier += 1; // HOT
            case 11:
                actualTemperature += Random.Range(5.10f, 7.00f);
                //description = "VERY HOT";// + "\n" + "-1 to snow condition roll";
                snowConditionModifier -= 1; weatherModifier *= 0.70f; precipitationModifier += 2; break;// VERY HOT
            case 12:
                //description = "EXTREME HOT"; // + "\n" + "-2 to snow condition roll";
                snowConditionModifier -= 2; weatherModifier *= 0.50f; precipitationModifier += 3;
                actualTemperature += Random.Range(7.10f, 13.00f); break;
            // EXTREME HOT
            default:
                actualTemperature += Random.Range(-2.00f, 2.00f); break;
                //description = "AVERAGE"; // AVERAGE

        }
        //descriptionTexts[0].text = description.ToString();
        // weatherCharts[0].SetActive(true);
        temperatureText[0].text = actualTemperature.ToString("F0");
        temperatureText[1].text = actualTemperature.ToString("F0");
        ChangeButtonName("NEXT");
        temperatureRolled = true;
        //StartCoroutine("WeatherDice");
        // return actualTemperature;
    }

    public IEnumerator GenerateWeather()
    {
        setupButton.SetActive(false);
        yield return new WaitForSeconds(pause);
        CalculateTemperature();
        weatherSections[0].SetActive(true);
        yield return new WaitForSeconds(pause);
        CalculatePrecipitation();
        weatherSections[1].SetActive(true);
        yield return new WaitForSeconds(pause);
        CalculateSnowCondition();
        weatherSections[2].SetActive(true);
        yield return new WaitForSeconds(pause);
        weatherSections[3].SetActive(true);
        yield return new WaitForSeconds(pause);
        weatherSections[4].SetActive(true);
        ShowSurpriseChance();
        yield return new WaitForSeconds(pause);
        setupButton.SetActive(true);
    }

    private void ShowSurpriseChance()
    {
        float surpriseModifier = FindObjectOfType<Gamemanager>().surprisesModifier * weatherModifier;

        weatherSections[3].SetActive(true);
        if (surpriseModifier <= 1.1)
        {
            surprisesModifierText.text = "LOW CHANCE".ToString();
        }
        else if ((surpriseModifier > 1.1) && (surpriseModifier < 1.5))
        {
            surprisesModifierText.text = "NORMAL CHANCE".ToString();
        }
        else if ((surpriseModifier >= 1.5) && (surpriseModifier < 2.00))
        {
            surprisesModifierText.text = "HIGH CHANCE".ToString();
        }
        else if (surpriseModifier >= 2.0)
        {
            surprisesModifierText.text = "VERY HIGH CHANCE".ToString();
        }
        Debug.Log("SURPRISE MOD: " + surpriseModifier.ToString());
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
        snowConditionText.text = snowCondition.ToUpper().ToString();

        descriptionTexts[1].text = snowCondition.ToUpper().ToString();
        // weatherCharts[2].SetActive(true);
        Debug.Log("WEATHER MODIFIER: " + weatherModifier);
        ChangeButtonName("PRESENTATION");
        weatherPhaseOver = true;
    }

    public void WeatherButton()
    {
        if (!weatherPhaseOver)
        {
            StartCoroutine("GenerateWeather");
        }
        else
        {
            competition.ChangeState(Competition.GameState.PresentationPhase);
            Debug.Log("PRESENTATION PHASE");
        }
    }

    public void CalculatePrecipitation()
    {
        int snowDays = venueLoader.actualVenue.averageSnowingDays;
        int rainDays = venueLoader.actualVenue.averageRainingDays;

        int chance = Random.Range(1, 51);
        Debug.Log("WEATHER ROLL: " + chance);
        if (chance <= snowDays)
        {
            precipitation = "snowing"; snowConditionModifier -= 2; weatherModifier *= 1.30f;
            precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[0];// break;//SNOWING
        }
        else if ((chance <= (snowDays + rainDays)) && (chance > snowDays))
        {
            precipitation = "raining"; snowConditionModifier += 1; weatherModifier *= 1.50f;
            precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[1];// break;//RAINING
        }
        else
        {
            precipitation = "none";
            //ADD "clear sky" image
        }

        descriptionTexts[2].text = precipitation.ToUpper().ToString();
        precipitationText.text = precipitation.ToUpper().ToString();
        CheckWindCondition(chance);
        ChangeButtonName("NEXT");
    }

    public void CheckWindCondition(int chance)
    {    //TO DO if roll==50 Chinook wind - special effect
        if (chance > 47)
        {
            windCondition = "strong";
        }
        else if ((chance > 41) && (chance < 48))
        {
            windCondition = "medium";
        }
        else if ((chance > 35) && (chance < 42))
        {
            windCondition = "light";
        }
        else
        {
            windCondition = "calm";
        }
        //descriptionTexts[3].text = windCondition.ToUpper().ToString();
        windConditionText.text = windCondition.ToUpper().ToString();
        CheckWindDirection();

    }

    public void CheckWindDirection()
    {
        int chance = Random.Range(1, 11);

        if (windCondition != "calm")
        {
            if (chance < 4)
            {
                windDirection = "tail";
            }
            else if ((chance > 3) && (chance < 7))
            {
                windDirection = "head";
            }
            else if (chance == 10)
            {
                windDirection = "gusts";
            }
            else
            {
                windDirection = "";
            }

            windConditionText.text += " (" + windDirection.ToUpper().ToString() + ")";
        }
        else
        {
            windDirection = "";
        }
        UpdateWind();
    }

    public void ChangeButtonName(string buttonName)
    {
        buttonText.text = buttonName.ToString();
    }

    public string CheckPrecipitationChange(float chance)
    {
        string weatherChangeInfo = ">>>WEATHER DIDN'T CHANGE ";
        if (chance > 1.00f)
        {
            if ((precipitation.Contains("snowing")) || (precipitation.Contains("raining")))
            {
                Debug.Log("SNOW/RAIN STOPPED");
                weatherChangeInfo = ">>>SNOW/RAIN STOPPED. ";
                weatherModifier *= 0.80f;
                precipitation = "";
            }
        }
        else if ((chance < 0.60f) && (chance > 0.46f) && (!precipitation.Contains("snowing")))
        {
            Debug.Log("SNOW STARTED");
            weatherChangeInfo = ">>>IT HAS STARTED TO SNOW. ";
            snowConditionModifier -= 1;
            weatherModifier *= 1.20f;
            precipitation = "snowing";
        }
        else if ((chance < 0.47f) && (!precipitation.Contains("raining")))
        {
            Debug.Log("IT STARTED TO RAIN");
            weatherChangeInfo = ">>>IT HAS STARTED TO RAIN. ";
            snowConditionModifier += 1;
            weatherModifier *= 1.40f;
            precipitation = "raining";
        }
        UpdatePrecipitation();
        return weatherChangeInfo;
    }

    public string CheckWindChange()
    {
        string windChangeInfo = "";
        int chance = Random.Range(1, 7);
        Debug.Log("WEATHER CHANCE ROLL: " + chance);
        if (chance < 3)
        {
            windChangeInfo = ">>>WIND CHANGES TO ";
            int index = Random.Range(1, 7);
            switch (windCondition)
            {
                case "strong":
                    if (index > 4 && windDirection != "gusts")
                    {
                        windDirection = "gusts";
                        windChangeInfo += "GUSTS. ";
                    }
                    else { windCondition = "medium"; }
                    break;
                case "medium":
                    if (index > 3) { windCondition = "strong"; }
                    else { windCondition = "light"; }
                    break;
                case "light":
                    if (index > 3) { windCondition = "medium"; }
                    else { windCondition = "calm"; }
                    break;
                case "calm":
                    windCondition = "light"; break;
            };
            CheckWindDirection();
            // TO DO: CHANGE WIND DIRECTION tail/head/gusts
            UpdateWind();
            windChangeInfo += windCondition.ToUpper().ToString();
        }
        return windChangeInfo;
    }

    public static string GetWindCondition()
    {
        return windCondition;
    }
    public void UpdatePrecipitation()
    {
        if (precipitation == "snowing")
        {
            precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[0];
        }

        else if (precipitation == "raining")
        {
            precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[1];// break;//RAINING
        }
        descriptionTexts[2].text = precipitation.ToUpper().ToString();
        precipitationText.text = precipitation.ToUpper().ToString();
    }

    public void UpdateWind()
    {
        descriptionTexts[3].text = windCondition.ToUpper().ToString() + " " + windDirection.ToUpper().ToString();
    }

    public void ConditionsChange()
    {
        float probabilityToChange = Random.Range(0.40f, 1.21f);
        float timeModifier = probabilityToChange < 0.60 ? Random.Range(-0.09f, -0.05f) :
                             probabilityToChange > 1.00 ? Random.Range(-0.04f, 0.09f) :
                             -0.05f;//AVERAGE IS 0.80
        Gamemanager _gameManager = FindObjectOfType<Gamemanager>();
        _gameManager.surprisesModifier *= probabilityToChange * 0.90f;
        _gameManager.ModifyTimes(timeModifier);
        Debug.Log("CONDITITINS CHANGE: " + probabilityToChange.ToString("F2"));
        Debug.Log("TIME MODIFIER: " + timeModifier.ToString("F2"));
        Debug.Log("NEW BEST TIME: " + _gameManager.bestTimeInSec.ToString("F2"));
        var shortEvent = FindObjectOfType<ShortEvent>();
        shortEvent.eventObject.SetActive(true);
        // shortEvent.eventTitle.text = "CONDITIONS CHANGE";

        shortEvent.descriptionText.text += CheckPrecipitationChange(probabilityToChange);
        shortEvent.descriptionText.text += CheckWindChange();
        //shortEvent.descriptionText.text += "CONDITIONS CHANGE: " + probabilityToChange.ToString("F2");
        shortEvent.eventResolved = true;

    }

}



