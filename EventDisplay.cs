using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventDisplay : MonoBehaviour
{
    public TMP_Text descriptionText;
    public TMP_Text eventTitle;

 
    public void ShowDescription(string text)
    {

        descriptionText.text = text.ToString();
    }
}
