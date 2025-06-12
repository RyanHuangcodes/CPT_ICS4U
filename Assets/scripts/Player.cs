using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Entity
{
    public TMP_Text HealthText;
    private float _healAccumulator;

    protected override void Start()
    {
        base.Start();
        UpdateHealthText();
    }

    void Update() {
        if (GetHealth() < GetMaxHealth())
        {
            _healAccumulator += GetMaxHealth() * 0.02f * Time.deltaTime;
            int healAmount = Mathf.FloorToInt(_healAccumulator);
            if (healAmount > 0)
            {
                HealPlayer(healAmount);
                _healAccumulator -= healAmount;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateHealthText();
    }

    public void HealPlayer(int amount)
    {
        base.Heal(amount);
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (HealthText != null)
        {
            int current = GetHealth();
            int max = GetMaxHealth();
            HealthText.text = $"Health: {current}/{max}";
        }
    }

    protected override void Die()
    {
        // Respawn at the Base when health reaches zero
        GameObject baseObj = GameObject.FindGameObjectWithTag("Base");
        if (baseObj != null)
        {
            transform.position = baseObj.transform.position;
        }

        SetHealth(GetMaxHealth());
        UpdateHealthText();
        _healAccumulator = 0f;
    }
}
