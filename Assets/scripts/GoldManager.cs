using UnityEngine;
using TMPro; 

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private int _totalGold = 0;

    private void Awake()
    {
        //gpt
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional if you want it to persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
        //endgpt
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public int GetGold()
    {
        return _totalGold;
    }

    public void AddGold(int amount)
    {
        _totalGold += amount;
        UpdateUI();
    }

    public void SetGold(int amount)
    {
        _totalGold = amount;
        UpdateUI();
    }

    public void RemoveGold(int amount)
    {
        _totalGold -= amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_goldText != null)
        {
            _goldText.text = $"Gold: {_totalGold}";
        }
        else
        {
            _goldText = GameObject.FindWithTag("GoldText")?.GetComponent<TMP_Text>();
        }
    }
}
