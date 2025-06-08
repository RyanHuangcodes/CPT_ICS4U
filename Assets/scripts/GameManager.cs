using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _placedGoldMines = 0;
    private bool _basePlaced = false;
    public static GameManager Instance;

    private void Awake()
    {
        //make sure only 1 gamemanager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsBasePlaced()
    {
        return _basePlaced;
    }

    public void SetBasePlaced(bool state)
    {
        _basePlaced = state;
    }

    public int GetPlacedGoldMineCount()
    {
        return _placedGoldMines;
    }

    public void IncrementGoldMines()
    {
        _placedGoldMines += 1;
    }
}
