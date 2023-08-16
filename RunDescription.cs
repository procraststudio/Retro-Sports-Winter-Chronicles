using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class RunDescription : MonoBehaviour
{
    [SerializeField] TMP_Text[] descriptionText;
    public int textIndex;
    Competition competition;
    public Color descriptionColor;
    public string storedText;


    void Start()
    {
        competition = FindObjectOfType<Competition>();
        textIndex = 0;

    }

    public void ShowDescription()
    {
        descriptionText[textIndex].color = descriptionColor;
        descriptionText[textIndex].text = storedText.ToString();
        textIndex++;
       

    }

    public void StoreDescription(Color color, string text)
    {
        descriptionColor = color;
        storedText = text.ToString();
    }

    public void ResetDescription()
    {
        if (competition.partsOfRun == 0)
        {
            for (int i = 0; i < descriptionText.Length; i++)
            {
                descriptionText[i].text = "";

            }
            textIndex = 0;
        }
    }
}
