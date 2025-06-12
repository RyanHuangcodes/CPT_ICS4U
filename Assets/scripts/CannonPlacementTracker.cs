using UnityEngine;

/// <summary>
/// Singleton that tracks how many Cannon towers have been placed.
/// </summary>
public class CannonPlacementTracker : MonoBehaviour
{
    public static CannonPlacementTracker Instance;

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
        _placedCount--;
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
