using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveObject : MonoBehaviour
{
    public Vector3 position;
    public RectTransform rectTransform;
    public bool endOfFirstRunDisplayed = false;

    private void Start()
    {

     //  MoveToCenter();
        
    }

    private void Update()
    {
        if ((endOfFirstRunDisplayed) && (Input.GetMouseButtonDown(0))) {
            rectTransform.transform.DOMove( new Vector3(0f, -800f, 0f), 1.0f);
            endOfFirstRunDisplayed=false;
        }
    }

    public IEnumerator MoveToCenter ()
    {
        yield return new WaitForSeconds(2.0f);
        rectTransform.transform.localPosition = new Vector3(0f, -800f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 122.0f), 1.0f, false).SetEase(Ease.OutElastic);
        endOfFirstRunDisplayed=true;  
    }

    
}


