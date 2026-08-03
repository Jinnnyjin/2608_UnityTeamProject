using UnityEditor.Build.Content;
using UnityEngine;

public class MonsterAIMove : MonoBehaviour
{
    // 주변 타 몬스터와의 반경 
    // 테스트 기준 0.2에서 잘 작동, 추후 플레이어 및 몬스터 에셋 적용 후 다시 테스트 필요
    [SerializeField] private float m_checkRadius = 0.2f;

    private Rigidbody2D m_monsterRb;
    private Collider2D m_myCollider;
    private Transform m_player;
    private Collider2D[] m_overlapBuffer = new Collider2D[10];
    private Monster m_monster;
    private ContactFilter2D m_contactFilter;

    private void Awake()
    {
        m_monsterRb = GetComponent<Rigidbody2D>();
        m_myCollider = GetComponent<Collider2D>();
        m_monster = GetComponent<Monster>();

        m_contactFilter = ContactFilter2D.noFilter;
        m_contactFilter.useTriggers = true;
    }

    
    
    private void OnEnable()
    {
        m_player = GameManager.m_Instance.Player.transform;
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

    private Vector2 GetSeparateDir()
    {
        Vector2 separation = Vector2.zero;

        // 
        int count = Physics2D.OverlapCircle(transform.position, m_checkRadius, m_contactFilter ,m_overlapBuffer);

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
