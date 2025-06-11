using UnityEngine;

public class UpgradeComponent
{
    private int _level = 1;

    public void Upgrade()
    {
        _level++;
    }

    public int GetLevel()
    {
        return _level;
    }
}
