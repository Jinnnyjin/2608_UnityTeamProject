using System.Collections;
using UnityEngine;

public class MonsterAIMove : MonoBehaviour
{
    // ======== 이동 관련 ========
    private Rigidbody2D m_monsterRb;
    public Rigidbody2D Rigidbody => m_monsterRb;
    private Collider2D m_myCollider;
    private Transform m_player;
    private Collider2D[] m_overlapBuffer = new Collider2D[10];
    private Monster m_monster;
    private ContactFilter2D m_contactFilter;
    [SerializeField] private float m_checkRadius = 0.5f;   // 주변 타 몬스터와의 반경 

    // ======== 스킬 관련 ========
    public float MoveWeight { get; set; } = 1.0f;   //이동속도 가중치
    public bool LockDir { get; set; } = false;
    private Vector2 m_prevDirection;

    // ======== 애니메이션 관련 ========
    private AnimTable m_animTable;
    private Animator m_animator;


    // ======== 초기화 ========
    private void Awake()
    {
        m_monsterRb = GetComponent<Rigidbody2D>();
        m_myCollider = GetComponent<Collider2D>();
        m_monster = GetComponent<Monster>();
        m_animTable = GetComponent<AnimTable>();
        m_animator = GetComponent<Animator>();

        m_contactFilter = ContactFilter2D.noFilter;
        m_contactFilter.useTriggers = true;
        m_contactFilter.useLayerMask = true;
        m_contactFilter.layerMask = LayerMask.GetMask("Monster");
    }

    private void Start()
    {
        m_player = GameManager.m_Instance.Player.transform;
        m_prevDirection = GetChaseDIr();
    }

    private void OnEnable()
    {
        LockDir = false;
        MoveWeight = 1.0f;
    }

    // 플레이어 따라가는 움직임 + 대쉬스킬 움직임
    private void FixedUpdate()
    {
        if( m_player == null) return;

        // 분리로직 방향 (주변 몬스터로부터 밀려나는 방향)
        Vector2 separateDir = GetSeparateDir();

        Vector2 chaseDir = Vector2.zero;
        if (LockDir == false)
            // 평소: 실시간 플레이어 추적
            chaseDir = GetChaseDIr();
        else
            // 대쉬스킬 중: 고정된 방향 유지
            chaseDir = m_prevDirection;

        // 플레이어 추적 + 타 몬스터 밀려남
        Vector2 finalDir = (chaseDir + separateDir).normalized;

        // 움직임 최종 로직
        m_monsterRb.MovePosition(m_monsterRb.position + finalDir * m_monster.Info.Speed * MoveWeight * Time.fixedDeltaTime);

        bool isMoving = finalDir.sqrMagnitude > 0.01f;

        // ======== 애니메이션 ========
        if (m_animTable != null)
        {
            m_animTable.SetBool(eEntityState.Run, isMoving);
        }

        if (m_animator != null)
        {
            m_animator.SetFloat("Horizontal", chaseDir.x);
            m_animator.SetFloat("Vertical", chaseDir.y);
        }

        // 
        m_prevDirection = chaseDir;
    }
 
    // 분리 로직 -> 나 제외 주변 몬스터 확인 후 겹치지 않도록 방향 제어
    private Vector2 GetSeparateDir()
    {
        Vector2 separation = Vector2.zero;

        // 반경 내 몬스터 탐색
        int count = Physics2D.OverlapCircle(transform.position, m_checkRadius, m_contactFilter ,m_overlapBuffer);

        for (int i = 0; i< count; i++)  
        {
            Collider2D other = m_overlapBuffer[i];

            // 자기 자신 제외
            if (other == m_myCollider) continue;

            // 타 몬스터 -> 나 방향 벡터 누적
            separation += m_monsterRb.position - other.attachedRigidbody.position;
        }

        return separation.normalized;
    }

    // 플레이어 따라가는 방향 추적 로직
    private Vector2 GetChaseDIr()
    {
        Vector2 move = new Vector2
            (m_player.position.x - m_monsterRb.position.x,
            m_player.position.y - m_monsterRb.position.y).normalized;

        return move;
    }

    // ======== (공격) 애니메이션 ========
    public void MonsterAttackSkill()
    {
        if (m_animTable != null)
        {
            m_animTable.SetTrigger(eEntityState.Attack);
        }
    }

}
