using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{

    public static PointsSystem Instance { get; private set; }

    public int gamePoints;
    public int normalPoints;
    public int comboPoints;
    [SerializeField] TMP_Text normalPointsTotal;
    [SerializeField] TMP_Text comboPointsTotal;

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


    // Start is called before the first frame update
    void Start()
    {
        gamePoints = 0;
        normalPoints = 0;
        comboPoints = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGamePoints(int normalPts, int comboPts)
    {
       // normalPoints += normalPts;
      //  comboPoints += comboPts;
        normalPoints += normalPts;
        comboPoints += comboPts;
        //Debug.Log("POINTS TOTAL:" + gamePoints);
        normalPointsTotal.text = normalPoints.ToString();
        comboPointsTotal.text = comboPoints.ToString();  
        gamePoints+=(normalPts+comboPts);

    }


}
