using System;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void Awake()
    {
        m_monsterInfo = new MonsterInfo();
        m_monsterInfo.Attack = m_SOMonsterInfo.BaseAttack;
        m_monsterInfo.Speed = m_SOMonsterInfo.BaseSpeed;
        m_monsterInfo.HP = m_SOMonsterInfo.Max_HP;

        m_monsterMove = GetComponent<MonsterAIMove>();
        
    }

    public void TakeDamage(DamageInfo _damage)
    {
        m_monsterInfo.HP -= _damage.Dmg;

        if(IsDead())
        {
            Die();
        }
    }

    private bool CheckAttack()
    {
        Player target = GameManager.m_Instance.Player;
        Vector3 fiff = target.Position - transform.position;
        float len = fiff.magnitude;



        return false;
    }
    public bool IsDead()
    {
        return m_monsterInfo.HP <= 0; 
    }

    private void Die()
    {
        // 테스트용 경험치
        onMonsterDied?.Invoke(this);

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
}
