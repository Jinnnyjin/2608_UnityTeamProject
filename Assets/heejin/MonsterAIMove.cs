using UnityEngine;

public class MonsterAIMove : MonoBehaviour
{
    // 주변 타 몬스터와의 반경 
    [SerializeField] private float m_checkRadius = 1.0f;

    private Rigidbody2D m_monsterRb;
    private Collider2D m_myCollider;
    private Transform m_player;
    private float m_moveSpeed;
    private Collider2D[] m_overlapBuffer = new Collider2D[10];
    private Monster m_monster;

    private void Awake()
    {
        m_monsterRb = GetComponent<Rigidbody2D>();
        m_myCollider = GetComponent<Collider2D>();
        m_monster = GetComponent<Monster>();
    }

    private void FixedUpdate()
    {
        if( m_player == null) return;

        Vector2 chaseDir = GetChaseDIr();
        Vector2 separateDir = GetSeparateDir();

        Vector2 finalDir = (chaseDir + separateDir).normalized;

        // 스피드 직접 받아옴, 스피드 디버프 혹시모르니..
        m_monsterRb.MovePosition(m_monsterRb.position + finalDir * m_monster.Info.Speed * Time.fixedDeltaTime);

    }

    // 생성될때 플레이어 위치와 데이터 받아옴
    public void SetTarget(Transform _playerTransform)
    {
        m_player = _playerTransform;
    }

    private Vector2 GetSeparateDir()
    {
        Vector2 separation = Vector2.zero;

        // 
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, m_checkRadius, m_overlapBuffer);

        for (int i = 0; i< count; i++)
        {
            Collider2D other = m_overlapBuffer[i];

            if (other == m_myCollider) continue;

            separation += new Vector2
                (m_monsterRb.position.x - other.attachedRigidbody.position.x,
                m_monsterRb.position.y - other.attachedRigidbody.position.y);
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
