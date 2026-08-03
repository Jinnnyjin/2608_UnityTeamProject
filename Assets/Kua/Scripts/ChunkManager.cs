using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ChunkManager : MonoBehaviour
{
    [Header("Tile Settings")]
    [SerializeField] private GameObject m_tilePrefab;
    [SerializeField] private Sprite[] m_tileSprites;
    [SerializeField] private float m_tileSize = 1f;

    [Header("Render Range (1920x1080 + Extra Padding)")]
    [SerializeField] private int m_horizontalHalfRange = 15;
    [SerializeField] private int m_verticalHalfRange = 10;

    [Header("Target")]
    [SerializeField] private Transform m_playerTransform;

    private IObjectPool<GameObject> m_tilePool;

    // private 멤버 변수는 m_ 접두사 + 카멜 케이스 규칙 준수
    private Dictionary<Vector2Int, MapTile> m_activeTiles = new Dictionary<Vector2Int, MapTile>();
    private List<Vector2Int> m_tilesToRemove = new List<Vector2Int>();
    private Vector2Int m_lastPlayerGridPos = new Vector2Int(-9999, -9999);

    // 함수는 무조건 파스칼 케이스
    private void Awake()
    {
        m_tilePool = new ObjectPool<GameObject>(
            CreateTile, OnGetTile, OnReleaseTile, OnDestroyTile,
            true, 200, 500
        );
    }

    private void Update()
    {
        if (m_playerTransform == null) return;

        Vector2Int currentPlayerGridPos = new Vector2Int(
            Mathf.RoundToInt(m_playerTransform.position.x / m_tileSize),
            Mathf.RoundToInt(m_playerTransform.position.y / m_tileSize)
        );

        if (currentPlayerGridPos != m_lastPlayerGridPos)
        {
            m_lastPlayerGridPos = currentPlayerGridPos;
            UpdateChunks(currentPlayerGridPos);
        }
    }

    // 매개 변수는 _ 접두사 사용 (_playerGridPos)
    private void UpdateChunks(Vector2Int _playerGridPos)
    {
        m_tilesToRemove.Clear();
        
        foreach (var pair in m_activeTiles)
        {
            Vector2Int tilePos = pair.Key;

            if (Mathf.Abs(tilePos.x - _playerGridPos.x) > m_horizontalHalfRange + 2 ||
                Mathf.Abs(tilePos.y - _playerGridPos.y) > m_verticalHalfRange + 2)
            {
                m_tilesToRemove.Add(tilePos);
            }
        }

        for (int i = 0; i < m_tilesToRemove.Count; i++)
        {
            Vector2Int posToRemove = m_tilesToRemove[i];
            
            if (m_activeTiles.TryGetValue(posToRemove, out MapTile tile))
            {
                tile.ReleaseTile();
                m_activeTiles.Remove(posToRemove);
            }
        }

        for (int x = -m_horizontalHalfRange; x <= m_horizontalHalfRange; x++)
        {
            for (int y = -m_verticalHalfRange; y <= m_verticalHalfRange; y++)
            {
                Vector2Int targetGridPos = new Vector2Int(_playerGridPos.x + x, _playerGridPos.y + y);

                if (m_activeTiles.ContainsKey(targetGridPos)) continue;

                GameObject tileObj = m_tilePool.Get();
                tileObj.transform.position = new Vector3(targetGridPos.x * m_tileSize, targetGridPos.y * m_tileSize, 0f);

                if (tileObj.TryGetComponent<MapTile>(out var mapTile))
                {
                    Sprite randomSprite = m_tileSprites[Random.Range(0, m_tileSprites.Length)];
                    mapTile.InitTile(targetGridPos, randomSprite, m_tilePool);

                    m_activeTiles.Add(targetGridPos, mapTile);
                }
            }
        }
    }

    private GameObject CreateTile()
    {
        return Instantiate(m_tilePrefab, transform);
    }

    // 매개 변수는 무조건 _ 접두사 사용 규칙 반영 (_tile)
    private void OnGetTile(GameObject _tile)
    {
        _tile.SetActive(true);
    }

    private void OnReleaseTile(GameObject _tile)
    {
        _tile.SetActive(false);
    }

    private void OnDestroyTile(GameObject _tile)
    {
        Destroy(_tile);
    }
}
