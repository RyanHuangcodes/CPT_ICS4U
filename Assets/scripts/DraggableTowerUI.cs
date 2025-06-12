using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableTowerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Tower Settings")]
    public GameObject TowerPrefab;    // Prefab to spawn (Base, GoldMine, MachineGun, Cannon)
    public LayerMask PlayerLayer;     // LayerMask for Player collisions
    public LayerMask TowerLayer;      // LayerMask for existing towers
    public int MaxAllowed = 4;        // Maximum of this tower type
    public Text PlacementText;        // UI Text showing "(placed/max)"

    private int _placedCount = 0;
    private GameObject _preview;
    private SpriteRenderer _previewRenderer;

    private readonly Color _invalidColor = new Color(1f, 0f, 0f, 0.5f);
    private readonly Color _validColor   = new Color(1f, 1f, 1f, 0.5f);

    private Vector2 _towerSize   = new Vector2(1f, 1f);
    private float   _checkRadius = 0.1f;

    private void Start()
    {
        // Initialize placed count from the correct tracker
        if (TowerPrefab.name.Contains("Base"))
        {
            _placedCount = BasePlacementTracker.Instance?.GetPlacedCount() ?? 0;
        }
        else if (TowerPrefab.name.Contains("GoldMine"))
        {
            _placedCount = GoldMinePlacementTracker.Instance?.GetPlacedCount() ?? 0;
        }
        else if (TowerPrefab.name.Contains("MachineGun"))
        {
            _placedCount = MachineGunPlacementTracker.Instance?.GetPlacedCount() ?? 0;
        }
        else if (TowerPrefab.name.Contains("Cannon"))
        {
            _placedCount = CannonPlacementTracker.Instance?.GetPlacedCount() ?? 0;
        }

        UpdatePlacementText();
    }

    private void Update()
    {
        // Poll the tracker and refresh UI if it changed
        int current = 0;

        if (TowerPrefab.name.Contains("Base"))
        {
            current = BasePlacementTracker.Instance.GetPlacedCount();
        }
        else if (TowerPrefab.name.Contains("GoldMine"))
        {
            current = GoldMinePlacementTracker.Instance.GetPlacedCount();
        }
        else if (TowerPrefab.name.Contains("MachineGun"))
        {
            current = MachineGunPlacementTracker.Instance.GetPlacedCount();
        }
        else if (TowerPrefab.name.Contains("Cannon"))
        {
            current = CannonPlacementTracker.Instance.GetPlacedCount();
        }

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

        // Disable any shooting or Tower logic on the preview
        var shooters = _preview.GetComponentsInChildren<MachineGunTower>();
        foreach (var s in shooters)
        {
            s.enabled = false;
        }

        var cannons = _preview.GetComponentsInChildren<Cannon>();
        foreach (var c in cannons)
        {
            c.enabled = false;
        }

        var towers = _preview.GetComponentsInChildren<Tower>();
        foreach (var t in towers)
        {
            t.enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        _preview.transform.position = worldPos;

        // Prevent placing non-Base before Base exists
        if (!TowerPrefab.name.Contains("Base") &&
            (BasePlacementTracker.Instance?.GetPlacedCount() ?? 0) == 0)
        {
            _previewRenderer.color = _invalidColor;
            return;
        }

        bool valid = IsValidPlacement(worldPos);
        _previewRenderer.color = valid ? _validColor : _invalidColor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;

        // Prevent dependent towers before Base
        if (!TowerPrefab.name.Contains("Base") &&
            (BasePlacementTracker.Instance?.GetPlacedCount() ?? 0) == 0)
        {
            Debug.Log($"Cannot place {TowerPrefab.name} before placing a Base.");
            Destroy(_preview);
            return;
        }

        if (IsValidPlacement(worldPos))
        {
            // 1) Instantiate the tower
            GameObject placed = Instantiate(TowerPrefab, worldPos, Quaternion.identity);

            // 2) Update its tracker & our local count
            if (TowerPrefab.name.Contains("Base"))
            {
                BasePlacementTracker.Instance.Increment();
                _placedCount = BasePlacementTracker.Instance.GetPlacedCount();

                // 3) Assign the new Base into the WaveManager and start waves
                WaveManager.Instance.BaseTransform = placed.transform;
                WaveManager.Instance.StartWaves();
            }
            else if (TowerPrefab.name.Contains("GoldMine"))
            {
                GoldMinePlacementTracker.Instance.Increment();
                _placedCount = GoldMinePlacementTracker.Instance.GetPlacedCount();
            }
            else if (TowerPrefab.name.Contains("MachineGun"))
            {
                MachineGunPlacementTracker.Instance.Increment();
                _placedCount = MachineGunPlacementTracker.Instance.GetPlacedCount();
            }
            else if (TowerPrefab.name.Contains("Cannon"))
            {
                CannonPlacementTracker.Instance.Increment();
                _placedCount = CannonPlacementTracker.Instance.GetPlacedCount();
            }

            UpdatePlacementText();
        }

        Destroy(_preview);
    }

    private bool IsValidPlacement(Vector3 position)
    {
        if (_placedCount >= MaxAllowed)
        {
            return false;
        }

        if (Physics2D.OverlapBox(position, _towerSize + Vector2.one * _checkRadius, 0f, PlayerLayer) != null)
        {
            return false;
        }

        if (Physics2D.OverlapBox(position, _towerSize + Vector2.one * _checkRadius, 0f, TowerLayer) != null)
        {
            return false;
        }

        return true;
    }

    private void UpdatePlacementText()
    {
        if (PlacementText != null)
        {
            PlacementText.text = $"({_placedCount}/{MaxAllowed})";
        }
    }
}
