using UnityEngine;
using System.Collections.Generic;
//gpt
public class OptimizedGrassGenerator : MonoBehaviour
{
    public GameObject grassPrefab;       // Prefab with SpriteRenderer of grass tile
    public Transform player;             // Player transform to follow
    public Camera mainCamera;            // Main Camera reference
    public int extraTilesMargin = 2;     // Extra tiles beyond camera view (buffer)

    private Vector2 tileSize;            // Size of one tile in world units
    private Vector2Int currentPlayerTilePos;

    private int tilesX, tilesY;          // Number of tiles to cover screen + margin
    private Dictionary<Vector2Int, GameObject> spawnedTiles = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        if (grassPrefab == null || player == null || mainCamera == null)
        {
            Debug.LogError("Please assign grassPrefab, player, and mainCamera in the inspector.");
            enabled = false;
            return;
        }

        // Get tile size from prefab's SpriteRenderer bounds
        SpriteRenderer sr = grassPrefab.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Grass prefab must have a SpriteRenderer component.");
            enabled = false;
            return;
        }

        tileSize = sr.bounds.size;

        CalculateTilesAmount();

        currentPlayerTilePos = GetPlayerTilePos();
        UpdateTiles(true);
    }

    void Update()
    {
        Vector2Int newPlayerTilePos = GetPlayerTilePos();

        if (newPlayerTilePos != currentPlayerTilePos)
        {
            currentPlayerTilePos = newPlayerTilePos;
            UpdateTiles(false);
        }
    }

    Vector2Int GetPlayerTilePos()
    {
        return new Vector2Int(
            Mathf.FloorToInt(player.position.x / tileSize.x),
            Mathf.FloorToInt(player.position.y / tileSize.y)
        );
    }

    void CalculateTilesAmount()
    {
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        tilesX = Mathf.CeilToInt(width / tileSize.x) + extraTilesMargin * 2;
        tilesY = Mathf.CeilToInt(height / tileSize.y) + extraTilesMargin * 2;
    }

    void UpdateTiles(bool spawnAll)
    {
        HashSet<Vector2Int> newVisibleTiles = new HashSet<Vector2Int>();

        // Calculate starting tile position so tiles are centered around player
        int startX = currentPlayerTilePos.x - tilesX / 2;
        int startY = currentPlayerTilePos.y - tilesY / 2;

        for (int x = 0; x < tilesX; x++)
        {
            for (int y = 0; y < tilesY; y++)
            {
                Vector2Int tilePos = new Vector2Int(startX + x, startY + y);
                newVisibleTiles.Add(tilePos);

                if (!spawnedTiles.ContainsKey(tilePos))
                {
                    Vector3 worldPos = new Vector3(tilePos.x * tileSize.x, tilePos.y * tileSize.y, 0f);
                    GameObject tile = Instantiate(grassPrefab, worldPos, Quaternion.identity, transform);
                    spawnedTiles.Add(tilePos, tile);
                }
            }
        }

        if (!spawnAll)
        {
            // Destroy tiles no longer in visible area
            List<Vector2Int> tilesToRemove = new List<Vector2Int>();
            foreach (var kvp in spawnedTiles)
            {
                if (!newVisibleTiles.Contains(kvp.Key))
                {
                    Destroy(kvp.Value);
                    tilesToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in tilesToRemove)
            {
                spawnedTiles.Remove(key);
            }
        }
    }
}
