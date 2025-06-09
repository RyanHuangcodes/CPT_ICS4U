using UnityEngine;

public class Tower : Entity
{
    // tracks if tower has been placed
    private bool _isInitialized = false;
    [SerializeField] private int _level = 1;

    public bool IsInitialized()
    {
        return _isInitialized;
    }

    // set initialization status
    public void SetInitialized(bool state)
    {
        _isInitialized = state;
    }

    protected virtual void Start()
    {

    }

    public int GetLevel()
    {
        return _level;
    }

    public void SetLevel(int level)
    {
        _level = level; 
    }
}
