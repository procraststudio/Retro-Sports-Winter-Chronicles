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
    [SerializeField] GameObject[] weatherCharts;
    [SerializeField] TMP_Text[] descriptionTexts;
    [SerializeField] GameObject precipitationImage;
    [SerializeField] Sprite[] precipitationSprites;
    [SerializeField] GameObject[] chartIndicator;
    [SerializeField] GameObject[] weatherSections;
    [SerializeField] GameObject setupButton;
    [SerializeField] TMP_Text surprisesModifierText;
    public string precipitation { get; set; }
    public string snowCondition { get; set; }
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
    }

    public void CalculateTemperature()
    {
        averageTemp = venueLoader.actualVenue.averageTemperature;
        //minTemp = FindObjectOfType<Gamemanager>().temperatureMin;
        // maxTemp = FindObjectOfType<Gamemanager>().temperatureMax;
        actualTemperature = averageTemp;
        // float averageTemperature = Random.Range(minTemp, maxTemp);
        string description = "";

        switch (firstD6 + secondD6)
        {
            case 2:
                actualTemperature += Random.Range(-13.00f, -7.10f);
                description = "EXTREME COLD"; //+ "\n" + "+2 to snow condition roll";
                snowConditionModifier += 2; weatherModifier *= 1.50f; break;//EXTREME COLD
            case 3:
                actualTemperature += Random.Range(-7.00f, -5.10f);
                description = "VERY COLD"; // + "\n" + "+1 to snow condition roll";
                snowConditionModifier += 1; weatherModifier *= 1.30f; precipitationModifier -= 1; break;//VERY COLD
            case 4:
                actualTemperature += Random.Range(-5.00f, -3.10f);
                description = "COLD"; weatherModifier *= 1.10f; break;// COLD
            case 5:
                actualTemperature += Random.Range(-3.00f, -1.10f);
                description = "BELOW AVERAGE"; break;// BELOW AVERAGE
            case 9:
                actualTemperature += Random.Range(1.10f, 3.00f);
                description = "ABOVE AVERAGE"; break;// ABOVE AVERAGE
            case 10:
                actualTemperature += Random.Range(3.10f, 5.00f);
                description = "HOT"; weatherModifier *= 0.90f; precipitationModifier += 1; break;// HOT
            case 11:
                actualTemperature += Random.Range(5.10f, 7.00f);
                description = "VERY HOT";// + "\n" + "-1 to snow condition roll";
                snowConditionModifier -= 1; weatherModifier *= 0.70f; precipitationModifier += 2; break;// VERY HOT
            case 12:
                actualTemperature += Random.Range(7.10f, 13.00f);
                description = "EXTREME HOT"; // + "\n" + "-2 to snow condition roll";
                snowConditionModifier -= 2; weatherModifier *= 0.50f; precipitationModifier += 3;
                break;// EXTREME HOT
            default:
                actualTemperature += Random.Range(-1.10f, 1.10f);
                description = "AVERAGE"; break;// AVERAGE

        }
        descriptionTexts[0].text = description.ToString();
        // weatherCharts[0].SetActive(true);
        temperatureText[0].text = actualTemperature.ToString("F0");
        temperatureText[1].text = actualTemperature.ToString("F0");
        Debug.Log("TEMP IS: " + actualTemperature);
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
        surprisesModifierText.text = ((weatherModifier - 1.00f) * 100f).ToString("+0;F0") + "%";
        yield return new WaitForSeconds(pause);
        setupButton.SetActive(true);
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
            if (chance > 47)
            {
                precipitation = "very windy";
            }
            else if ((chance > 41) && (chance < 48))
            {
                precipitation = "overcast";
            }
            else if ((chance > 35) && (chance < 42))
            {
                precipitation = "windy";
            }
            else
            {
                precipitation = "sunny";
            }
        }
        //TO DO if roll==30 Chinook wind - special effect

        //switch (firstD6 + secondD6+precipitationModifier)
        //{
        //    case 2:
        //    case 3:
        //    case 4:
        //    case 5:
        //    case 6:
        //        precipitation = "snowing"; snowConditionModifier -= 2; weatherModifier *= 1.30f;
        //        precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[0]; break;//SNOWING
        //    case 9:
        //        precipitation = "raining"; snowConditionModifier += 1; weatherModifier *= 1.50f;
        //        precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[1]; break;//RAINING
        //    default:
        //        precipitation = "none"; break;// AVERAGE
        //        // TODO case 11: CHINOOK WIND Special weather effect

        //}
        descriptionTexts[2].text = precipitation.ToUpper().ToString();
        precipitationText.text = precipitation.ToUpper().ToString();

        ChangeButtonName("NEXT");
    }


    public void ChangeButtonName(string buttonName)
    {
        buttonText.text = buttonName.ToString();
    }

    public string CheckPrecipitationChange(float chance)
    {
        string weatherChangeInfo = ">>>>>WEATHER DIDN'T CHANGE ";
        if (chance > 1.00f)
        {
            if ((precipitation.Contains("snowing")) || (precipitation.Contains("raining")))
            {
                Debug.Log("SNOW/RAIN STOPPED");
                weatherChangeInfo = ">>>>>SNOW/RAIN STOPPED. ";
                precipitationImage.GetComponent<SpriteRenderer>().sprite = null;
                weatherModifier -= 0.20f;
                precipitation = "";
            }
        }
        else if ((chance < 0.60f) && (chance > 0.46f) && (!precipitation.Contains("snowing")))
        {
            Debug.Log("SNOW STARTED");
            weatherChangeInfo = ">>>>SNOW STARTED. ";
            snowConditionModifier -= 1;
            weatherModifier *= 1.20f;
            precipitation = "snowing";
            precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[0];
            descriptionTexts[1].text = precipitation.ToUpper().ToString();
        }
        else if ((chance < 0.47f) && (!precipitation.Contains("raining")))
        {
            Debug.Log("IT STARTED TO RAIN");
            weatherChangeInfo = ">>>>IT STARTED TO RAIN. ";
            snowConditionModifier += 1;
            weatherModifier *= 1.40f;
            precipitation = "raining";
            precipitationImage.GetComponent<SpriteRenderer>().sprite = precipitationSprites[1];
            descriptionTexts[1].text = precipitation.ToUpper().ToString();
        }
        return weatherChangeInfo;
    }

}



