using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    //public Text scoreText;
    public TextMeshProUGUI scoreTextTMP;
    public float countDuration = 3f;

    private int targetScore = 0; // Docelowa liczba punktów
    private int currentScore = 0; // Aktualna liczba punktów
    public bool scoreUpdated;

    void Start()
    {
        // Inicjalizacja tekstu (na pocz¹tku wyœwietl 0)
        UpdateScoreText(0);
    }

    // Metoda do rozpoczêcia efektu nabijania punktów
    public void AddScore(int scoreToAdd)
    {
        scoreUpdated = false;
        targetScore += scoreToAdd; // Zwiêksz docelow¹ liczbê punktów
        StartCoroutine(CountScore()); // Uruchom korutynê
    }


    private IEnumerator CountScore()
    {
        int startScore = currentScore; // Punkt startowy
        float elapsedTime = 0f; // Czas, który up³yn¹³ od rozpoczêcia efektu

        while (elapsedTime < countDuration)
        {
            elapsedTime += Time.deltaTime; // Zwiêksz czas
            float progress = elapsedTime / countDuration; // Postêp (0 do 1)
            currentScore = (int)Mathf.Lerp(startScore, targetScore, progress); // Interpolacja
            UpdateScoreText(currentScore); // Aktualizuj tekst
            yield return null; // Poczekaj na nastêpn¹ klatkê
        }

        currentScore = targetScore;
        UpdateScoreText(currentScore);
        scoreUpdated = true;
    }


    private void UpdateScoreText(int score)
    {

        if (scoreTextTMP != null)
            scoreTextTMP.text = " " + score.ToString();
    }
}