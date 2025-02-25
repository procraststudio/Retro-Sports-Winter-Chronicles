using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    //public Text scoreText;
    public TextMeshProUGUI scoreTextTMP;
    public float countDuration = 3f;

    private int targetScore = 0; // Docelowa liczba punkt�w
    private int currentScore = 0; // Aktualna liczba punkt�w
    public bool scoreUpdated;

    void Start()
    {
        // Inicjalizacja tekstu (na pocz�tku wy�wietl 0)
        UpdateScoreText(0);
    }

    // Metoda do rozpocz�cia efektu nabijania punkt�w
    public void AddScore(int scoreToAdd)
    {
        scoreUpdated = false;
        targetScore += scoreToAdd; // Zwi�ksz docelow� liczb� punkt�w
        StartCoroutine(CountScore()); // Uruchom korutyn�
    }


    private IEnumerator CountScore()
    {
        int startScore = currentScore; // Punkt startowy
        float elapsedTime = 0f; // Czas, kt�ry up�yn�� od rozpocz�cia efektu

        while (elapsedTime < countDuration)
        {
            elapsedTime += Time.deltaTime; // Zwi�ksz czas
            float progress = elapsedTime / countDuration; // Post�p (0 do 1)
            currentScore = (int)Mathf.Lerp(startScore, targetScore, progress); // Interpolacja
            UpdateScoreText(currentScore); // Aktualizuj tekst
            yield return null; // Poczekaj na nast�pn� klatk�
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