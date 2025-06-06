using UnityEngine;

public class Weapon
{
    private int _damage;
    public Weapon()
    {
        _damage = _damage = Random.Range(5, 50);
    }
    public int GetDamage()
    {
        return _damage;
    }
    public void Use(GameObject target)
    {
        Entity entity = target.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(_damage);
        }
    }
}
