using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabControl : MonoBehaviour
{
    [SerializeField] GameObject tabLayer;
    public GameObject[] tabButtons;
    [SerializeField] GameObject[] listsToShow;


    void Start()
    {
        tabLayer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowList()
    {

    }
}
