using DG.Tweening;
using UnityEngine;

public class MenuButtonEffects : MonoBehaviour
{
    public AudioClip hoverOnButtonSound;


    public void OnMouseOver()
    {
        Debug.Log("MOUSE OVER");
        SoundManager.PlaySound(hoverOnButtonSound);
        this.gameObject.GetComponent<RectTransform>().DOShakePosition(0.3f, new Vector3(0f, 2f, 0f), 0, 0f, false, true);

    }


}
