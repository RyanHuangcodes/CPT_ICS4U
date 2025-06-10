using UnityEngine;

public class UpgradeComponent
{
    private int _level = 1;
    private float _projectileSpeed = 1;
    private float _damage = 20;

    public void Upgrade()
    {
        _level++;
        _projectileSpeed += 0.15f; // Example upgrade effect
        _damage += 10; // Example upgrade effect
    }

    public int GetLevel()
    {
        return _level;
    }
}
