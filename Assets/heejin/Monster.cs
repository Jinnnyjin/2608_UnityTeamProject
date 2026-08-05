using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PoolObject))]
public class Monster : BSObj, IDamageable, ISkillOwner
{
    private MonsterInfo m_monsterInfo = null;
    private MonsterAIMove m_monsterMove = null;
    
    [SerializeField] private SOMonsterInfo m_SOMonsterInfo = null;
    public static event Action<Monster> onMonsterDied;

    public BSObj Obj => this;
    public MonsterInfo Info => m_monsterInfo;
    public SOMonsterInfo BaseInfo => m_SOMonsterInfo;

    private List<Skill> m_skillList = new List<Skill>();
    public List<Skill> SkillList { get => m_skillList;}

    public MonsterAIMove MonsterMove => m_monsterMove;

    public FactionEnum Faction => FactionEnum.Enemy;

    //baseAttack타임동안 Range에 들어오면 공격
    [SerializeField]private float m_baseHitRange = 1.0f;
    [SerializeField]private float m_baseAttackTime = 1.0f;
    private float m_curAttackTime = 0.0f;
    private DamageInfo m_baseDamageInfo = new DamageInfo();

    private Coroutine m_hitCoroutine;
    [SerializeField] private float m_hitEffectTime = 0.3f;
    private WaitForSeconds m_waitTime;
    private SpriteRenderer m_renderer;

    [SerializeField] private Color m_changeColor;
    private Color m_originColor;


    private WaitForSeconds m_waitSecond = null;
    private Coroutine m_dashCoroutine;

    private bool m_isUsingSkill = false;
    
    private void Awake()
    {
        m_monsterInfo = new MonsterInfo();

        m_monsterMove = GetComponent<MonsterAIMove>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_waitTime = new WaitForSeconds(m_hitEffectTime);

        m_originColor = m_renderer.color;
        m_baseDamageInfo.Dmg = 10.0f;
    }

    private void OnEnable()
    {
        m_monsterInfo.Attack = m_SOMonsterInfo.BaseAttack;
        m_monsterInfo.Speed = m_SOMonsterInfo.BaseSpeed;
        m_monsterInfo.HP = m_SOMonsterInfo.Max_HP;

        m_curAttackTime = 0.0f;
    }
    public void TakeDamage(DamageInfo _damage)
    {
        m_monsterInfo.HP -= _damage.Dmg;

        if(IsDead())
        {
            Die();
        }
        else
        {
            if (m_hitCoroutine != null)
                StopCoroutine(m_hitCoroutine);
            m_hitCoroutine = StartCoroutine(HitEffect());
        }
    }
    public override void Update()
    {
        base.Update();
        CheckAttack();
    }
    private void CheckAttack()
    {
        Player target = GameManager.m_Instance.Player;
        Vector2 fiff = target.Position - transform.position;
        float len = fiff.magnitude;
        if(len < m_baseHitRange)
        {
            m_curAttackTime += Time.deltaTime;
            if (m_curAttackTime >= m_baseAttackTime)
            {
                GameManager.m_Instance.Player.TakeDamage(m_baseDamageInfo);
                m_curAttackTime = 0.0f;
            }
        }
        else
        {
            m_curAttackTime = 0.0f;
        }
    }
    public bool IsDead()
    {
        return m_monsterInfo.HP <= 0; 
    }

    private void Die()
    {
        // 테스트용 경험치
        onMonsterDied?.Invoke(this);
        m_renderer.color = m_originColor;
        ObjectPoolManager.m_Instance.PushObject(gameObject);
    }

    public void RegisterSkill(Skill skill)
    {
        SkillList.Add(skill);
    }

    public void UnRegisterSkill(Skill skill)
    {
        SkillList.Remove(skill);
    }

    public void UnRegisterSkill(string skillId)
    {
        m_skillList.RemoveAll(skill => skill.Data.Name == skillId);
    }

    public void RegisterSkill(SkillData skillData)
    {
        throw new NotImplementedException();
    }

    public void RegisterSkill(string skillId)
    {
        throw new NotImplementedException();
    }

    public void UnRegisterSkill(SkillData skill)
    {
        throw new NotImplementedException();
    }

    private IEnumerator HitEffect()
    {
        m_renderer.color = m_changeColor;
        yield return m_waitTime;
        m_renderer.color = m_originColor;

        m_hitCoroutine = null;
    }


    public void MoveToPlayer(float _fWeight)
    {
        if (m_dashCoroutine != null)
            StopCoroutine(m_dashCoroutine);
        m_dashCoroutine = StartCoroutine(DashCoroutine(_fWeight));
    }

    private IEnumerator DashCoroutine(float _fWeight)
    {
        float startWeight = MonsterMove.MoveWeight;
        MonsterMove.MoveWeight = _fWeight;
        MonsterMove.LockDir = true;

        MonsterMove.StartTrail();

        yield return new WaitForSeconds(_fWeight);

        MonsterMove.MoveWeight = startWeight;
        MonsterMove.LockDir = false;

        MonsterMove.StopTrail();

        EndSkill();

        m_dashCoroutine = null;
    }

    public void StopMoving()
    {
        MonsterMove.MoveWeight = 0f;
    }

    public void ResumeMoving()
    {
        MonsterMove.MoveWeight = 1f;
    }

    public bool TryStartSkill()
    {
        if (m_isUsingSkill) return false;

        m_isUsingSkill = true;
        return true;
    }

    public void EndSkill()
    {
        m_isUsingSkill = false;
    }
}
