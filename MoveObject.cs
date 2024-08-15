using DG.Tweening;
using System.Collections;
using UnityEngine;

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
        if ((endOfFirstRunDisplayed) && (Input.GetMouseButtonDown(0)))
        {
            rectTransform.transform.DOMove(new Vector3(0f, -1800f, 0f), 1.0f);
            endOfFirstRunDisplayed = false;
        }
       
    }

    public IEnumerator MoveToCenter()
    {
        yield return new WaitForSeconds(2.0f);
        rectTransform.transform.localPosition = new Vector3(0f, -300f, 0.0f);
        // rectTransform.transform.Rotate(Vector3.forward, 180.0f * Time.deltaTime);

        rectTransform.DOAnchorPos(new Vector2(0f, 340.0f), 1.0f, false).SetEase(Ease.OutElastic);
        endOfFirstRunDisplayed = true;
    }

   


}


