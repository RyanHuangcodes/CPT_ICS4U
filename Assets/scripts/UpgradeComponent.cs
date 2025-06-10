using UnityEngine;

public class UpgradeComponent
{
    private int _level = 1;
    private float _damage = 20;

    public void Upgrade()
    {
        _level++;
        _damage += 10; 
    }

    public int GetLevel()
    {
        return _level;
    }
}
