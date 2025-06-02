using UnityEngine;

public class Weapon
{
    private int damage;
    public Weapon()
    {
        damage = damage = Random.Range(5, 50);
    }
    public int GetDamage()
    {
        return damage;
    }
    public void Use(GameObject target)
    {
        Entity entity = target.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(damage);
        }
    }
}
