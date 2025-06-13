using UnityEngine;

public class Tower : Entity
{
    private bool _isInitialized = false;

    [SerializeField]
    private int _level = 1;

    [SerializeField]
    private GameObject _healthBarRoot;

    [SerializeField]
    private Transform _fillTransform;

    private float _regenAccumulator;
    private float _initialFillScaleX;
    private Vector3 _initialFillLocalPos;

    public bool IsInitialized()
    {
        return _isInitialized;
    }

    public void SetInitialized(bool state)
    {
        _isInitialized = state;
    }

    public int GetLevel()
    {
        return _level;
    }

    public void SetLevel(int level)
    {
        _level = level;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        RefreshHealthBar();
    }

    protected virtual void Update()
    {
        int current = GetHealth();
        int max     = GetMaxHealth();

        if (current < max)
        {
            _regenAccumulator += max * 0.02f * Time.deltaTime;
            int healAmount = Mathf.FloorToInt(_regenAccumulator);
            if (healAmount > 0)
            {
                Heal(healAmount);
                _regenAccumulator -= healAmount;
                RefreshHealthBar();
            }
        }
    }

    private void RefreshHealthBar()
    {
        if (_healthBarRoot == null || _fillTransform == null)
        {
            return;
        }

        float pct = (float)GetHealth() / GetMaxHealth();

        if (pct < 1f)
        {
            _healthBarRoot.SetActive(true);
            Vector3 scale = _fillTransform.localScale;
            scale.x = pct;
            _fillTransform.localScale = scale;
            float halfFull = _initialFillScaleX * 0.5f;
            float halfNow = scale.x * 0.5f;
            Vector3 pos = _initialFillLocalPos;
            pos.x = halfNow-halfFull-0.5f;
            _fillTransform.localPosition = pos;
        }
        else
        {
            _healthBarRoot.SetActive(false);
            _regenAccumulator = 0f;
        }
    }
}