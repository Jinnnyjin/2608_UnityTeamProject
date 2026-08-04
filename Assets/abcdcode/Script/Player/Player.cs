using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : BSObj, IDamageable,ISkillOwner,IStat
{
    public void Start()
    {
        SkillList = new List<Skill>();
    } 

    public bool IsDead()
    {
        throw new System.NotImplementedException();
    }
    public void TakeDamage(DamageInfo info)
    {
        throw new System.NotImplementedException();
    }
    public void RegisterSkill(Skill skill)
    {
        throw new System.NotImplementedException();
    }

    public void UnRegisterSkill(Skill skill)
    {
        SkillList.Remove(skill);
    }

    public void UnRegisterSkill(string skillId)
    {
        var skill = SkillList.Find(x => x.Data.Name == skillId);
        if(skill != null)
        {
            UnRegisterSkill(skill);
        }
    }
    public PlayerController Controller
    {
        get => m_Controller;
    }
    public BSObj Obj => this;
    public List<Skill> SkillList { get; private set; }
    public float CurrentHp
    {
        get => m_currenHp;
        private set
        {
            m_currenHp = Mathf.Clamp(value,0,Hp);
        }
    }
    private R GetPValue<R>(R start,Func<R,Skill,R> func)
    {
        return SkillList.FindAll(x => x.Data.SkillType == SkillType.Passive).GetEach<R,Skill>(func,start);
    }
    private float m_currenHp;
    public float Hp => Mathf.Clamp((BaseHp + GetPValue<float>(0,(a,b)=> a+=b.Data.Hp)) * HpMult,1,9999);
    public float HpMult => 1 * GetPValue<float>(1,(a,b)=> a*=b.Data.HpMult);
    public float Damage => 0 + GetPValue<float>(0,(a,b)=> a+=b.Data.Damage);
    public float Speed => BaseSpeed + GetPValue<float>(0,(a,b)=> a+=b.Data.Speed);
    public float SpeedMult => 1 * GetPValue<float>(1,(a,b)=> a*=b.Data.SpeedMult);
    public float CoolTime => 1 * GetPValue<float>(1,(a,b)=> a*=b.Data.CoolTime);
    public float Def => 0 + GetPValue<float>(0,(a,b)=> a+=b.Data.Def);
    public float ReduceDmg => 1 * GetPValue<float>(1,(a,b)=> a*=b.Data.ReduceDmg);
    public float DmgMult => 1 * GetPValue<float>(1,(a,b)=> a*=b.Data.DmgMult);
    public float ProjSpeed => 0 + GetPValue<float>(0,(a,b)=> a+=b.Data.ProjSpeed);
    public float ProjSpeedMult => 1 * GetPValue<float>(1,(a,b)=> a*=b.Data.ProjSpeedMult);
    public int ProjCount => 0 + GetPValue<int>(0,(a,b) => a += b.Data.ProjCount);
    



    private const float BaseHp = 100;
    private const float BaseSpeed = 5;
    [SerializeField]private PlayerController m_Controller;
}