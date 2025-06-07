using UnityEngine;
using TMPro;  // TextMeshPro namespace

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int totalCoins = 0;
    public TMP_Text coinText; // Drag your TextMeshPro - Text component here

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + totalCoins;
    }

    public int GetCoins()
    {
        return totalCoins;
    }
}
