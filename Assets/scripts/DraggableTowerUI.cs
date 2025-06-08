using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableTowerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject TowerPrefab;            // Prefab to spawn
    public LayerMask PlayerLayer;             // Set to "Player" layer
    public LayerMask TowerLayer;              // Optional: prevent tower-tower overlap
    public int MaxAllowed = 4;
    public Text PlacementText;                // UI Text for (x / max)

    private int _placedCount = 0;
    private GameObject _preview;
    private SpriteRenderer _previewRenderer;

    private readonly Color _invalidColor = new Color(1f, 0f, 0f, 0.5f); // Red with 50% transparency
    private readonly Color _validColor = new Color(1f, 1f, 1f, 0.5f);   // White with 50% transparency

    private Vector2 _towerSize = new Vector2(1f, 1f);  // Approximate size of the tower
    private float _checkRadius = 0.1f;

    void Start()
    {
        if (TowerPrefab.name.Contains("Base"))
            _placedCount = BasePlacementTracker.Instance?.GetPlacedCount() ?? 0;
        else if (TowerPrefab.name.Contains("GoldMine"))
            _placedCount = GoldMinePlacementTracker.Instance?.GetPlacedCount() ?? 0;

        UpdatePlacementText();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _preview = Instantiate(TowerPrefab);
        _previewRenderer = _preview.GetComponent<SpriteRenderer>();
        _previewRenderer.color = _invalidColor;
        _preview.GetComponent<Collider2D>().enabled = false; // disable collisions for preview
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        _preview.transform.position = mouseWorld;

        // Base not placed and trying to place a dependent tower like GoldMine
        if (!TowerPrefab.name.Contains("Base") && (BasePlacementTracker.Instance?.GetPlacedCount() ?? 0) == 0)
        {
            _previewRenderer.color = _invalidColor;
            return;
        }

        bool isValid = IsValidPlacement(mouseWorld);
        _previewRenderer.color = isValid ? _validColor : _invalidColor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Prevent placing GoldMine unless base is placed
        if (TowerPrefab.name.Contains("GoldMine") && (BasePlacementTracker.Instance?.GetPlacedCount() ?? 0) == 0)
        {
            Debug.Log("Cannot place gold mine before placing a base.");
            Destroy(_preview);
            return;
        }

        if (IsValidPlacement(mouseWorld))
        {
            Instantiate(TowerPrefab, mouseWorld, Quaternion.identity);

            if (TowerPrefab.name.Contains("Base"))
            {
                BasePlacementTracker.Instance?.Increment();
                _placedCount = BasePlacementTracker.Instance.GetPlacedCount();
            }
            else if (TowerPrefab.name.Contains("GoldMine"))
            {
                GoldMinePlacementTracker.Instance?.Increment();
                _placedCount = GoldMinePlacementTracker.Instance.GetPlacedCount();
            }

            UpdatePlacementText();
        }

        Destroy(_preview);
    }

    private bool IsValidPlacement(Vector3 position)
    {
        if (_placedCount >= MaxAllowed)
            return false;

        Collider2D hitPlayer = Physics2D.OverlapBox(position, _towerSize + Vector2.one * _checkRadius, 0f, PlayerLayer);
        if (hitPlayer != null)
            return false;

        Collider2D hitTower = Physics2D.OverlapBox(position, _towerSize + Vector2.one * _checkRadius, 0f, TowerLayer);
        if (hitTower != null)
            return false;

        return true;
    }

    private void UpdatePlacementText()
    {
        if (PlacementText != null)
            PlacementText.text = $"({_placedCount}/{MaxAllowed})";
    }
}
