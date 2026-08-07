using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(PoolObject))]
public class Monster : BSObj, IDamageable, ISkillOwner, IStat
{
    // ======== 식별 정보 ========
    public FactionEnum Faction => FactionEnum.Enemy;

    // ======== 스탯 관련 ========
    private MonsterInfo m_monsterInfo = null;
    private MonsterAIMove m_monsterMove = null;
    [SerializeField] private SOMonsterInfo m_SOMonsterInfo = null;
    public static event Action<Monster> OnMonsterDied;
    private List<Skill> m_skillList = new List<Skill>();

    public BSObj Obj => this;
    public SOMonsterInfo BaseInfo => m_SOMonsterInfo;
    public MonsterInfo Info => m_monsterInfo;
    public List<Skill> SkillList { get => m_skillList;}
    public MonsterAIMove MonsterMove => m_monsterMove;

    // ======== 근접 공격 관련 ========
    [SerializeField]private float m_baseAttackTime = 1.0f;
    private float m_curAttackTime = 0.0f;
    private DamageInfo m_baseDamageInfo = new DamageInfo();
    private Coroutine m_hitCoroutine;

    // ======== 피격 이벤트 관련 ========
    private WaitForSeconds m_waitTime;
    [SerializeField] private float m_hitEffectTime = 0.3f;
    private SpriteRenderer m_renderer;
    [SerializeField] private Color m_changeColor;
    private Color m_originColor;

    // ======== 스킬 관련 ========
    private TrailRenderer m_trailRenderer;
    private GameObject m_activeIndicator;
    private Coroutine m_dashCoroutine;
    private bool m_isUsingSkill = false;

    // ======== 오디오 관련 ========
    [SerializeField] private SOAudio m_audioDied;
    [SerializeField] private SOAudio m_audioDamaged;
    [SerializeField] private SOAudio m_audioDashSkill;


    // ======== 초기화 ========
    private void Awake()
    {
        m_monsterInfo = new MonsterInfo();

        m_monsterMove = GetComponent<MonsterAIMove>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_waitTime = new WaitForSeconds(m_hitEffectTime);

        m_originColor = m_renderer.color;

        m_trailRenderer = GetComponent<TrailRenderer>();
        if (m_trailRenderer != null)
        {
            m_trailRenderer.emitting = false;
        }
    }

    private void OnEnable()
    {
        m_monsterInfo.Attack = m_SOMonsterInfo.BaseAttack;
        m_monsterInfo.Speed = m_SOMonsterInfo.BaseSpeed;
        m_monsterInfo.HP = m_SOMonsterInfo.Max_HP;

        m_curAttackTime = 0.0f;
        m_isUsingSkill = false;
    }

    public void UpdateStat(float _weight)
    {
        m_monsterInfo.Attack = m_SOMonsterInfo.BaseAttack * _weight;
        m_monsterInfo.Speed = m_SOMonsterInfo.BaseSpeed * _weight;
        m_monsterInfo.HP = m_SOMonsterInfo.Max_HP * _weight;
    }

    // ======== 데미지 ========
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
            SoundManager.m_Instance.PlaySfx(m_audioDamaged);
        }
    }

    private IEnumerator HitEffect()
    {
        m_renderer.color = m_changeColor;
        yield return m_waitTime;
        m_renderer.color = m_originColor;

        m_hitCoroutine = null;
    }

    public bool IsDead()
    {
        return m_monsterInfo.HP <= 0;
    }

    private void Die()
    {
        OnMonsterDied?.Invoke(this);
        SoundManager.m_Instance.PlaySfx(m_audioDied);
        m_renderer.color = m_originColor;

        // 죽었을때 인디케이터도 반납
        if (m_activeIndicator != null)
        {
            ObjectPoolManager.m_Instance.PushObject(m_activeIndicator);
            m_activeIndicator = null;
        }

        ObjectPoolManager.m_Instance.PushObject(gameObject);
    }

    public override void Update()
    {
        base.Update();
        CheckAttack();
    }

    // ======== 기본 근접 공격 ========
    private void CheckAttack()
    {
        Player target = GameManager.m_Instance.Player;
        Vector2 fiff = target.Position - transform.position;
        float len = fiff.magnitude;
        if(len < m_SOMonsterInfo.BaseAttackRange)
        {
            m_curAttackTime += Time.deltaTime;
            if (m_curAttackTime >= m_baseAttackTime)
            {
                m_baseDamageInfo.Dmg = m_monsterInfo.Attack;
                GameManager.m_Instance.Player.TakeDamage(m_baseDamageInfo);
                m_curAttackTime = 0.0f;
            }
        }
        else
        {
            m_curAttackTime = 0.0f;
        }
    }

    // ======== 대쉬스킬 ========
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

        StartTrail();
        SoundManager.m_Instance.PlaySfx(m_audioDashSkill);

        float time = 0f;
        float prevDist = (transform.position - GameManager.m_Instance.Player.Position).magnitude;

        // 지속시간동안 스킬 
        while (time < _fWeight)
        {
            float currentDist = (transform.position - GameManager.m_Instance.Player.Position).magnitude;

            // 직전 거리보다 멀어졌다면?
            if(currentDist > prevDist)
            {
                // 0.3f만큼만 더 가고 멈춤
                yield return new WaitForSeconds(0.3f);
                // 대쉬스킬 중단
                break;
            }

            prevDist = currentDist;
            time += Time.deltaTime;

            // 1 프레임 쉬고 다음프레임에 while문 이어서
            yield return null;
        }

        // 가중치 원래 값으로
        MonsterMove.MoveWeight = startWeight;
        MonsterMove.LockDir = false;

        StopTrail();

        EndSkill();

        m_dashCoroutine = null;
    }

    public void StartTrail()
    {
        if (m_trailRenderer != null)
        {
            m_trailRenderer.emitting = true;
        }
    }

    public void StopTrail()
    {
        if (m_trailRenderer != null)
        {
            m_trailRenderer.emitting = false;
        }
    }

    // ======== 범위스킬 (인디케이터 및 움직임멈춤) ========
    public void SetActiveIndicator(GameObject indicator)
    {
        m_activeIndicator = indicator;
    }

    public void ClearActiveIndicator()
    {
        m_activeIndicator = null;
    }

    public void StopMoving()
    {
        MonsterMove.MoveWeight = 0f;
    }

    public void ResumeMoving()
    {
        MonsterMove.MoveWeight = 1f;
    }

    // ======== 다중스킬 시 중복동작 제어 ========
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

    // ======== IDamageable INTERFACE ========
    public void Heal(float value)
    {
        throw new NotImplementedException();
    }


    // ======== ISTAT INTERFACE ========
    public float Hp => Mathf.Clamp((m_SOMonsterInfo.Max_HP + this.GetSKillStatValue<float>(0, (a, b) => a += b.Data.Hp)) * HpMult, 1, 9999);
    public float HpMult => 1 * this.GetSKillStatValue<float>(1, (a, b) => a *= b.Data.HpMult);
    public float Damage => m_monsterInfo.Attack + this.GetSKillStatValue<float>(0, (a, b) => a += b.Data.Damage);
    public float Speed => m_monsterInfo.Speed + this.GetSKillStatValue<float>(0, (a, b) => a += b.Data.Speed);
    public float SpeedMult => 1 * this.GetSKillStatValue<float>(1, (a, b) => a *= b.Data.SpeedMult);
    public float CoolTime => 1 * this.GetSKillStatValue<float>(1, (a, b) => a *= b.Data.CoolTime);
    public float Def => 0 + this.GetSKillStatValue<float>(0, (a, b) => a += b.Data.Def);
    public float ReduceDmg => 1 * this.GetSKillStatValue<float>(1, (a, b) => a *= b.Data.ReduceDmg);
    public float DmgMult => 1 * this.GetSKillStatValue<float>(1, (a, b) => a *= b.Data.DmgMult);
    public float ProjSpeed => 0 + this.GetSKillStatValue<float>(0, (a, b) => a += b.Data.ProjSpeed);
    public float ProjSpeedMult => 1 * this.GetSKillStatValue<float>(1, (a, b) => a *= b.Data.ProjSpeedMult);
    public int ProjCount => 0 + this.GetSKillStatValue<int>(0, (a, b) => a += b.Data.ProjCount);
    public float HPGen => 0 + this.GetSKillStatValue<float>(0, (a, b) => a += b.HPGen);

    // ======== ISkillOwner Interface ========

    public void RegisterSkill(Skill _skill)
    {
        SkillList.Add(_skill);
    }

    public void UnRegisterSkill(Skill _skill)
    {
        SkillList.Remove(_skill);
    }

    public void UnRegisterSkill(string _skillId)
    {
        m_skillList.RemoveAll(skill => skill.Data.Name == _skillId);
    }

    public void RegisterSkill(SkillData _skillData)
    {
        Skill skill = new Skill();
        skill.Init(this, _skillData);
        _skillData.Init(skill);
        this.RegisterSkill(skill);
    }

    public void RegisterSkill(string skillId)
    {
        throw new NotImplementedException();
    }

    public void UnRegisterSkill(SkillData _skill)
    {
        m_skillList.RemoveAll(skill => skill.Data.Name == _skill.Name);
    }

}
