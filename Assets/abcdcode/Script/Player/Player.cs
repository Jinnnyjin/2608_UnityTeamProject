using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : BSObj, IDamageable,ISkillOwner,IStat
{
    public void Start()
    {
        SkillList = new List<Skill>();
        TestList = SkillList;
        CurrentHp = Hp;
        RegisterSkill(TestSkill);
    }
    public void Update()
    {
        SkillList.ForEach(x => x.GameUpdate());
    }

    public bool IsDead()
    {
        return CurrentHp == 0;
    }
    public void TakeDamage(DamageInfo info)
    {
        CurrentHp -= info.Dmg;
        GameManager.m_Instance.TakeDamage(info.Dmg);
        if(IsDead())
        {
        }
    }
    public void RegisterSkill(Skill skill)
    {
        SkillList.Add(skill);
    }
    public void RegisterSkill(SkillData skillData)
    {
        Skill s = new Skill();
        s.Init(this,skillData);
        RegisterSkill(s);
    }
    public void RegisterSkill(string skillId)
    {
        throw new NotImplementedException();
    }
    public void UnRegisterSkill(Skill skill)
    {
        SkillList.Remove(skill);
        skill.Data.UnRegisterSkill(skill);
    }
    public void UnRegisterSkill(SkillData skill)
    {
        var s = SkillList.Find(x => x.Data == skill);
        if(s != null)
        {
            UnRegisterSkill(s);
        }
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
    [SerializeField]private SkillData TestSkill;
    public List<Skill> TestList;
}