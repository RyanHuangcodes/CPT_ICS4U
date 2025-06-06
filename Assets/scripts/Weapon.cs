using UnityEngine;

[System.Serializable]
public class Weapon
{
    private int _avgDamage;
    private int _damage;
    public Weapon(int avgDamage)
    {
        _avgDamage = avgDamage;
        _damage = Random.Range((int)(_avgDamage * 0.85f), (int)(_avgDamage * 1.15f));
    }
    public int GetDamage()
    {
        return _damage;
    }

    public int GetAvgDamage()
    {
        return _avgDamage;
    }
}
