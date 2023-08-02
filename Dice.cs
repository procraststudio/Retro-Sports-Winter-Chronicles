using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] Sprite[] diceSides;
    private SpriteRenderer rend;
    Competition competition;
    [SerializeField]  GameObject firstImage;
    [SerializeField]  GameObject secondImage;
    [SerializeField] GameObject thirdImage;


    private void Start()
    {

       // rend = GetComponent<SpriteRenderer>();
        competition = FindObjectOfType<Competition>();  
    }

    //private IEnumerator RollTheDice()
    //{
       // int randomDiceSide = 0;
       // int finalSide;

    //for (int i = 0; i <= 20; i++)
    // {
    // randomDiceSide = Random.Range(0, 5);
    // rend.sprite =  diceSides[randomDiceSide];
    // yield return new WaitForSeconds(0.10f);
    // }

    //rend.sprite = diceSides[finalSide-1];
    public void showDice() {
        firstImage.GetComponent<SpriteRenderer>().sprite = diceSides[competition.firstD6-1];
        secondImage.GetComponent<SpriteRenderer>().sprite = diceSides[competition.secondD6+5];
        thirdImage.GetComponent<SpriteRenderer>().sprite = diceSides[competition.thirdD6 +11];
    }
        

        }
    
    


