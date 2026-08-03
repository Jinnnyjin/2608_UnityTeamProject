using UnityEngine;
using UnityEngine.Pool;

public class MapTile : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private IObjectPool<GameObject> m_managedPool;

    // 현재 이 타일이 배치된 격자 좌표 (중복 스폰 방지 및 거리 체크용)
    public Vector2Int GridPosition { get; private set; }

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitTile(Vector2Int _gridPos, Sprite _sprite, IObjectPool<GameObject> _pool)
    {
        GridPosition = _gridPos;
        m_managedPool = _pool;
        m_spriteRenderer.sprite = _sprite;
    }

    // 관리자가 멀어졌다고 판단하면 이 함수를 호출해 풀에 반납
    public void ReleaseTile()
    {
        if (m_managedPool != null)
        {
            m_managedPool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
