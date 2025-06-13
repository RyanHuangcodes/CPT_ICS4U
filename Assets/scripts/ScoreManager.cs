using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TMP_Text ScoreText;

    private int _score;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        UpdateUI();
    }

    public int GetScore()
    {
        return _score;
    }

    public void SetScore(int score)
    {
        _score = score;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (ScoreText != null)
            ScoreText.text = $"Score: {_score}";
    }
}
