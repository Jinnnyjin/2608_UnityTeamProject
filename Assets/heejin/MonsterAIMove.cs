using System.Collections;
using UnityEngine;

public class MonsterAIMove : MonoBehaviour
{
    private Rigidbody2D m_monsterRb;
    public Rigidbody2D Rigidbody => m_monsterRb;
    private Collider2D m_myCollider;
    private Transform m_player;
    private Collider2D[] m_overlapBuffer = new Collider2D[10];
    private Monster m_monster;
    private ContactFilter2D m_contactFilter;

    private AnimTable m_animTable;
    private Animator m_animator;

    private TrailRenderer m_trailRenderer;
    
    // 주변 타 몬스터와의 반경 
    // 테스트 기준 0.5에서 잘 작동, 추후 플레이어 및 몬스터 에셋 적용 후 다시 테스트 필요
    [SerializeField] private float m_checkRadius = 0.5f;

    //이동속도 가중치
    public float MoveWeight { get; set; } = 1.0f;
    public bool LockDir { get; set; } = false;
    // 대쉬스킬 관련
    /*/////////////////////////////////////
     *              Test
     */////////////////////////////////////
    //public bool m_IsDashing;
    private Vector2 m_dashDirection;
    private Vector2 m_prevDirection;
    private float m_dashSpeed;

    private bool m_IsTargetDir;
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
        m_trailRenderer = GetComponent<TrailRenderer>();

        m_trailRenderer.emitting = false;
    }

    //private void OnEnable()
    //{
    //    m_player = GameManager.m_Instance.Player.transform;
    //    m_prevDirection = GetChaseDIr();
    //}
    private void Start()
    {
        m_player = GameManager.m_Instance.Player.transform;
        m_prevDirection = GetChaseDIr();

    }

    private void FixedUpdate()
    {
        if( m_player == null) return;

        // 분리로직 방향
        Vector2 separateDir = GetSeparateDir();
        Vector2 finalDir;
        float speed;


        // 스킬 사용하지 않을때, 평소 움직임
        Vector2 chaseDir = Vector2.zero;

        if (LockDir == false)
            chaseDir = GetChaseDIr();
        else
            chaseDir = m_prevDirection;

        finalDir = (chaseDir + separateDir).normalized;
        speed = m_monster.Info.Speed;

        // 움직임 최종 로직
        m_monsterRb.MovePosition(m_monsterRb.position + finalDir * speed * MoveWeight * Time.fixedDeltaTime);

        bool isMoving = finalDir.sqrMagnitude > 0.01f;
        if (m_animTable != null)
        {
            m_animTable.SetBool(eEntityState.Run, isMoving);
        }

        if (m_animator != null)
        {
            m_animator.SetFloat("Horizontal", finalDir.x);
            m_animator.SetFloat("Vertical", finalDir.y);
        }

        m_prevDirection = chaseDir;
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

    public void MonsterAttackSkill()
    {
        Debug.Log("MonsterAttackSkill 호출");
        if (m_animTable != null)
        {
            m_animTable.SetTrigger(eEntityState.Attack);
        }

        else Debug.Log("m_animTable이 null");
    }

    public void StartTrail()
    {
        m_trailRenderer.emitting = true;
    }

    public void StopTrail()
    {
        m_trailRenderer.emitting = false;
    }

//    public IEnumerator DoDash(Vector2 direction, float speed, float duration)
//    {

//        Debug.Log("대쉬!");

//        float time = 0f;
//        // 스킬 지속시간동안에만 유지 (여러 프레임에 걸쳐서)
//        while (time < duration)
//        {
//            m_monsterRb.MovePosition(m_monsterRb.position + direction * speed * Time.fixedDeltaTime);
//            time += Time.fixedDeltaTime;
//            yield return new WaitForFixedUpdate();

//        }

//        // 스킬 사용 종료 후 
//        IsOverridden = false;
//    }
}
