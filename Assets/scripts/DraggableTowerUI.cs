using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//gpt
public class DraggableTowerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject TowerPrefab;    // e.g. Base, GoldMine, MachineGun, DualMachineGun, Cannon, PiercingCannon, MissileLauncher
    public LayerMask PlayerLayer;
    public LayerMask TowerLayer;
    public int MaxAllowed = 4;
    public Text PlacementText;

    private int _placedCount = 0;
    private GameObject _preview;
    private SpriteRenderer _previewRenderer;
    private readonly Color _invalidColor = new Color(1f, 0f, 0f, 0.5f);
    private readonly Color _validColor   = new Color(1f, 1f, 1f, 0.5f);
    private Vector2 _towerSize   = new Vector2(1f, 1f);
    private float   _checkRadius = 0.1f;

    private void Start()
    {
        string name = TowerPrefab.name;
        if (name.Contains("Base"))
            _placedCount = BasePlacementTracker.Instance?.GetPlacedCount() ?? 0;
        else if (name.Contains("GoldMine"))
            _placedCount = GoldMinePlacementTracker.Instance?.GetPlacedCount() ?? 0;
        else if (name.Contains("DualMachineGun"))
            _placedCount = DualMachineGunPlacementTracker.Instance?.GetPlacedCount() ?? 0;
        else if (name.Contains("MachineGun"))
            _placedCount = MachineGunPlacementTracker.Instance?.GetPlacedCount() ?? 0;
        else if (name.Contains("PiercingCannon"))
            _placedCount = PiercingCannonPlacementTracker.Instance?.GetPlacedCount() ?? 0;
        else if (name.Contains("Cannon"))
            _placedCount = CannonPlacementTracker.Instance?.GetPlacedCount() ?? 0;
        else if (name.Contains("MissileLauncher"))
            _placedCount = MissileLauncherPlacementTracker.Instance?.GetPlacedCount() ?? 0;

        UpdatePlacementText();
    }

    private void Update()
    {
        int current = 0;
        string name = TowerPrefab.name;

        if (name.Contains("Base"))
            current = BasePlacementTracker.Instance.GetPlacedCount();
        else if (name.Contains("GoldMine"))
            current = GoldMinePlacementTracker.Instance.GetPlacedCount();
        else if (name.Contains("DualMachineGun"))
            current = DualMachineGunPlacementTracker.Instance.GetPlacedCount();
        else if (name.Contains("MachineGun"))
            current = MachineGunPlacementTracker.Instance.GetPlacedCount();
        else if (name.Contains("PiercingCannon"))
            current = PiercingCannonPlacementTracker.Instance.GetPlacedCount();
        else if (name.Contains("Cannon"))
            current = CannonPlacementTracker.Instance.GetPlacedCount();
        else if (name.Contains("MissileLauncher"))
            current = MissileLauncherPlacementTracker.Instance.GetPlacedCount();

        if (current != _placedCount)
        {
            _placedCount = current;
            UpdatePlacementText();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _preview = Instantiate(TowerPrefab);
        _previewRenderer = _preview.GetComponent<SpriteRenderer>();
        _previewRenderer.color = _invalidColor;
        _preview.GetComponent<Collider2D>().enabled = false;

        foreach (var t in _preview.GetComponentsInChildren<Tower>())
            t.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        _preview.transform.position = pos;

        if (!TowerPrefab.name.Contains("Base") &&
            (BasePlacementTracker.Instance?.GetPlacedCount() ?? 0) == 0)
        {
            _previewRenderer.color = _invalidColor;
            return;
        }

        bool valid = IsValidPlacement(pos);
        _previewRenderer.color = valid ? _validColor : _invalidColor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        if (!TowerPrefab.name.Contains("Base") &&
            (BasePlacementTracker.Instance?.GetPlacedCount() ?? 0) == 0)
        {
            Debug.Log($"Cannot place {TowerPrefab.name} before placing a Base.");
            Destroy(_preview);
            return;
        }

        if (IsValidPlacement(pos))
        {
            GameObject placed = Instantiate(TowerPrefab, pos, Quaternion.identity);
            string name = TowerPrefab.name;

            if (name.Contains("Base"))
            {
                BasePlacementTracker.Instance.Increment();
                _placedCount = BasePlacementTracker.Instance.GetPlacedCount();
                WaveManager.Instance.BaseTransform = placed.transform;
                WaveManager.Instance.StartWaves();
            }
            else if (name.Contains("GoldMine"))
            {
                GoldMinePlacementTracker.Instance.Increment();
                _placedCount = GoldMinePlacementTracker.Instance.GetPlacedCount();
            }
            else if (name.Contains("DualMachineGun"))
            {
                DualMachineGunPlacementTracker.Instance.Increment();
                _placedCount = DualMachineGunPlacementTracker.Instance.GetPlacedCount();
            }
            else if (name.Contains("MachineGun"))
            {
                MachineGunPlacementTracker.Instance.Increment();
                _placedCount = MachineGunPlacementTracker.Instance.GetPlacedCount();
            }
            else if (name.Contains("PiercingCannon"))
            {
                PiercingCannonPlacementTracker.Instance.Increment();
                _placedCount = PiercingCannonPlacementTracker.Instance.GetPlacedCount();
            }
            else if (name.Contains("Cannon"))
            {
                CannonPlacementTracker.Instance.Increment();
                _placedCount = CannonPlacementTracker.Instance.GetPlacedCount();
            }
            else if (name.Contains("MissileLauncher"))
            {
                MissileLauncherPlacementTracker.Instance.Increment();
                _placedCount = MissileLauncherPlacementTracker.Instance.GetPlacedCount();
            }

            UpdatePlacementText();
        }

        Destroy(_preview);
    }

    private bool IsValidPlacement(Vector3 position)
    {
        if (_placedCount >= MaxAllowed) return false;
        if (Physics2D.OverlapBox(position, _towerSize + Vector2.one * _checkRadius, 0f, PlayerLayer) != null) return false;
        if (Physics2D.OverlapBox(position, _towerSize + Vector2.one * _checkRadius, 0f, TowerLayer)  != null) return false;
        return true;
    }

    private void UpdatePlacementText()
    {
        PlacementText.text = $"({_placedCount}/{MaxAllowed})";
    }
}
