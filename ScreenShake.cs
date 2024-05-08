using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // code by: Thomas Friday 
    public float duration = 1f;
    public bool start = false;
    public AnimationCurve curve;


    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 startPosition = this.gameObject.transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }

    public void CameraShake()
    {
        this.gameObject.transform.DOShakePosition(2.0f, 50.0f, 2, 0.5f, true, true);
       // Camera.main.DOShakePosition(2.0f, 50.0f, 2, 0.5f, true, true, true);
    }
}
