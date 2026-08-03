using UnityEngine;
using UnityEngine.Pool;

public class MapTile : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private IObjectPool<GameObject> m_managedPool;

    // public 변수/프로퍼티는 파스칼 케이스
    public Vector2Int GridPosition { get; private set; }

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 매개 변수는 _ 접두사 사용 (_gridPos, _sprite, _pool)
    public void InitTile(Vector2Int _gridPos, Sprite _sprite, IObjectPool<GameObject> _pool)
    {
        GridPosition = _gridPos;
        m_managedPool = _pool;
        m_spriteRenderer.sprite = _sprite;
    }

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
