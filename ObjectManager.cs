using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject objectToActivate;
    public GameObject objectToSpawn;
    private GameObject spawnedObject;
    public RectTransform rectTransform;
    Competition competition;
    void Start()
    {
        //objectToActivate.SetActive(false);
        // SpawnObject();
        competition = Competition.Instance;
    }

    // Update is called once per frame
    void Update()
    {
       // if (competition.competitionIsOver)
        //{
          //  MoveToCenter();
        //}
        //OnMouseDown();
    }

    public void OnMouseDown()
    {
       // this.gameObject.SetActive(false);
    }

    public void SpawnObject()
    {
        //objectToActivate.SetActive(true);
        this.gameObject.SetActive(true);
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
        spawnedObject = Instantiate(this.gameObject, worldPosition, Quaternion.identity);
    }

    public IEnumerator MoveToCenter()
    {
        yield return new WaitForSeconds(5.0f);
        this.rectTransform.transform.DOMove(new Vector3(0f, -1800f, 0f), 1.0f);
        this.rectTransform.transform.localPosition = new Vector3(0f, -300f, 0.0f);
        // rectTransform.transform.Rotate(Vector3.forward, 180.0f * Time.deltaTime);

        this.rectTransform.DOAnchorPos(new Vector2(0f, 340.0f), 1.0f, false).SetEase(Ease.OutElastic);
       // endOfFirstRunDisplayed = true;
    }
}
