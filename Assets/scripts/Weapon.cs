using UnityEngine;

[System.Serializable]
public class Weapon
{
    private int _avgDamage;
    private int _damage;
    public Weapon(int avgDamage)
    {
        _avgDamage = avgDamage;
        _damage = Random.Range((int)(_avgDamage * 0.9f), (int)(_avgDamage * 1.1f));
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
