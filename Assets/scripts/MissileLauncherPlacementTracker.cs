using UnityEngine;

public class MissileLauncherPlacementTracker : MonoBehaviour
{
    public static MissileLauncherPlacementTracker Instance;

    private int _placedCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
