using UnityEngine;

public class GoldMine : Tower
{
    protected override void Start()
    {
        SetHealth(100);
        SetDamage(0);
        SetSpeed(0f);
        SetInitialized(true);

        _timer = 0f;
    }
    [SerializeField] private float _goldInterval = 1f;
    [SerializeField] private int _goldPerInterval = 5;      

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _goldInterval)
        {
            GoldManager.Instance.AddGold(_goldPerInterval);
            _timer = 0f;
        }
    }
}
