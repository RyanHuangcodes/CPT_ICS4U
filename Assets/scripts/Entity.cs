public class Entity
{
    private int _health;
    private int _maxHealth;
    private int _damage;
    private float _speed;

    public int GetHealth()
    {
        return _health;
    }

    public void SetHealth(int health)
    {
        _health = health;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            _health = 0;
        }
    }

    public void Heal(int heal)
    {
        _health += heal;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public int GetDamage()
    {
        return _damage;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
