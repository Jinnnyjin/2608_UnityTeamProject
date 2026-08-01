using UnityEngine;

public class MonsterAIMove : MonoBehaviour
{
    // 주변 타 몬스터와의 반경 
    [SerializeField] private float m_checkRadius = 1.0f;

    private Rigidbody2D m_monsterRb;
    private Collider2D m_myCollider;
    private Transform m_player;
    private float m_moveSpeed;

    private void Awake()
    {
        m_monsterRb = GetComponent<Rigidbody2D>();
        m_myCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if( m_player == null) return;

        Vector2 chaseDir = GetChaseDIr();
        Vector2 separateDir = GetSeparateDir();

        Vector2 finalDir = (chaseDir + separateDir).normalized;

        m_monsterRb.MovePosition(m_monsterRb.position + finalDir * m_moveSpeed * Time.fixedDeltaTime);

    }

    // 생성될때 플레이어 위치와 데이터 받아옴
    public void SetTarget(Transform _playerTransform)
    {
        m_player = _playerTransform;
    }

    public void SetData(MonsterData _data)
    {
        m_moveSpeed = _data.MoveSpeed;
    }

    private Vector2 GetSeparateDir()
    {
        Vector2 separation = Vector2.zero;

        // 주변 타 몬스터와 겹치지 않도록
        Collider2D[] nearbyMonsters = Physics2D.OverlapCircleAll(transform.position, m_checkRadius);

        // 자신 제외
        foreach (Collider2D other in nearbyMonsters)
        {
            if (other == m_myCollider)
            {
                continue;
            }

            separation += new Vector2
                (m_monsterRb.position.x - other.transform.position.x,
                m_monsterRb.position.y - other.transform.position.y);
        }

        return separation.normalized;
    }

    private Vector2 GetChaseDIr()
    {
        Vector2 move = new Vector2
            (m_player.position.x - m_monsterRb.position.x,
            m_player.position.y - m_monsterRb.position.y).normalized;

        return move;
    }
}
