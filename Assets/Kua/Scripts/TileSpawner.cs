using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject TilePrefab;
    public Transform PlayerTransform;

    // 가로, 세로 시야 반경을 독립적으로 분리 (최적화)
    public int ViewRadiusX = 16;         // 가로 범위 (예: 16)
    public int ViewRadiusY = 10;         // 세로 범위 (예: 10으로 줄임)
    public int PoolSize = 1000;

    private Dictionary<Vector2Int, GameObject> m_activeTiles = new Dictionary<Vector2Int, GameObject>();
    private Queue<GameObject> m_tilePool = new Queue<GameObject>();
    private Vector2Int m_lastPlayerGrid = new Vector2Int(-999, -999);

    public void Start()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject tile = Instantiate(TilePrefab, transform);
            tile.SetActive(false);
            m_tilePool.Enqueue(tile);
        }

        UpdateTiles();
    }

    public void Update()
    {
        if (PlayerTransform == null) return;

        Vector2Int currentPlayerGrid = new Vector2Int(
            Mathf.RoundToInt(PlayerTransform.position.x),
            Mathf.RoundToInt(PlayerTransform.position.y)
        );

        if (currentPlayerGrid != m_lastPlayerGrid)
        {
            m_lastPlayerGrid = currentPlayerGrid;
            UpdateTiles();
        }
    }

    public void UpdateTiles()
    {
        HashSet<Vector2Int> requiredPositions = new HashSet<Vector2Int>();

        // 가로(X)와 세로(Y) 반복문 범위를 다르게 적용
        for (int x = -ViewRadiusX; x <= ViewRadiusX; x++)
        {
            for (int y = -ViewRadiusY; y <= ViewRadiusY; y++)
            {
                requiredPositions.Add(new Vector2Int(m_lastPlayerGrid.x + x, m_lastPlayerGrid.y + y));
            }
        }

        List<Vector2Int> tilesToRemove = new List<Vector2Int>();
        foreach (KeyValuePair<Vector2Int, GameObject> pair in m_activeTiles)
        {
            if (!requiredPositions.Contains(pair.Key))
            {
                pair.Value.SetActive(false);
                m_tilePool.Enqueue(pair.Value);
                tilesToRemove.Add(pair.Key);
            }
        }

        foreach (Vector2Int pos in tilesToRemove)
        {
            m_activeTiles.Remove(pos);
        }

        foreach (Vector2Int pos in requiredPositions)
        {
            if (!m_activeTiles.ContainsKey(pos))
            {
                GameObject tile = GetTileFromPool();
                if (tile != null)
                {
                    tile.transform.position = new Vector3(pos.x, pos.y, 0f);
                    tile.SetActive(true);
                    m_activeTiles.Add(pos, tile);
                }
            }
        }
    }

    private GameObject GetTileFromPool()
    {
        if (m_tilePool.Count > 0)
        {
            return m_tilePool.Dequeue();
        }
        return Instantiate(TilePrefab, transform);
    }
}
