using TMPro;
using UnityEngine;

public class ScrollViewManager : MonoBehaviour
{
    //public GameObject textPrefab; // Prefab tekstowy, kt�ry b�dzie dodawany
    // public RectTransform content; // RectTransform elementu zawarto�ci Scroll View
    //public ScrollRect scrollRect; // ScrollRect komponent Scroll View

    //private float yOffset = 0f; // Offset dla nowych element�w
    [SerializeField] TMP_Text messageText;
    [SerializeField] TMP_Text eventReportText;
    public string eventReport;

    public void AddMessage(string message)
    {
        //messageText.text = "";
        messageText.text = message.ToString();
        eventReport +=  message;
        eventReportText.text +=  "----------" +"\n" + message.ToString() + "\n";

    }

    //public void AddTextToTop(string newText)
    //{
    //    // Tworzenie nowego obiektu tekstowego
    //    GameObject newTextObject = Instantiate(textPrefab, content);
    //    RectTransform newTextRectTransform = newTextObject.GetComponent<RectTransform>();

    //    // Ustawienie tekstu w nowym obiekcie
    //    TMP_Text textComponent = newTextObject.GetComponent<TextMeshProUGUI>();
    //    if (textComponent != null)
    //    {
    //        textComponent.text = newText;
    //    }

    //    // Przesuni�cie wszystkich istniej�cych element�w w d�
    //    foreach (RectTransform child in content)
    //    {
    //        if (child != newTextRectTransform) // Pomijamy nowo dodany element
    //        {
    //            child.anchoredPosition = new Vector2(child.anchoredPosition.x, child.anchoredPosition.y - newTextRectTransform.rect.height);
    //        }
    //    }

    //    // Ustawienie pozycji nowego elementu na g�rze
    //    newTextRectTransform.anchoredPosition = new Vector2(newTextRectTransform.anchoredPosition.x, 0);

    //    // Aktualizacja wysoko�ci zawarto�ci Scroll View
    //    yOffset += newTextRectTransform.rect.height;
    //    content.sizeDelta = new Vector2(content.sizeDelta.x, yOffset);

    //    // Ustawienie Scroll View na g�r�
    //    Canvas.ForceUpdateCanvases();
    //    scrollRect.verticalNormalizedPosition = 1f;
    //    Canvas.ForceUpdateCanvases();
    //}

}
