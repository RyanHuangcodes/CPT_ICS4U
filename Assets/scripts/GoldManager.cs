using UnityEngine;
using TMPro; 

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    private int _totalGold = 0;
    public TMP_Text GoldText; 

    private void Awake()
    {
    }

    public void AddGold(int amount)
    {
        _totalGold += amount;
        UpdateUI();
    }

    public void RemoveGold(int amount)
    {
        _totalGold -= amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (GoldText != null)
            GoldText.text = "Gold: " + _totalGold;
    }

    public int GetCoins()
    {
        return _totalGold;
    }
}
