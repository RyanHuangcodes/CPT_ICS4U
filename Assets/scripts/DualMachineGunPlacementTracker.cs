using UnityEngine;

public class DualMachineGunPlacementTracker : MonoBehaviour
{
    public static DualMachineGunPlacementTracker Instance;

    private int _placedCount = 0;

    private void Awake()
    {
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

    public void Increment()
    {
        _placedCount++;
    }

    public void Decrement()
    {
        _placedCount = Mathf.Max(0, _placedCount - 1);
    }

    public void SetPlacedCount(int count)
    {
        _placedCount = count;
    }

    public int GetPlacedCount()
    {
        return _placedCount;
    }
}
