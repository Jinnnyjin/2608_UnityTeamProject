using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class Player : BSObj, IDamageable,ISkillOwner,IStat
{
    public void Awake()
    {
        SkillList = new List<Skill>();
        CurrentHp = Hp;
        RegisterSkill(TestSkill);
    }
    public void Update()
    {
        if(IsDead()) return;
        SkillList.ForEach(x => x.GameUpdate());
    }

    public bool IsDead()
    {
        return CurrentHp == 0;
    }
    private void Player_Dead()
    {
        this.gameObject.SetActive(false);
        GameManager.m_Instance.EndGame();
    }
    public FactionEnum Faction => FactionEnum.Player;
    public void TakeDamage(DamageInfo info)
    {
        if(IsDead()) return;
        var finalDmg = Mathf.Max(1,(info.Dmg-Def) * ReduceDmg);
        CurrentHp -= finalDmg;
        GameManager.m_Instance.TakeDamage(finalDmg);
        if(IsDead())
        {
            m_Controller.PlayDead();
        }
    }
    public void RegisterSkill(Skill skill)
    {
        SkillList.Add(skill);
    }
    public void RegisterSkill(SkillData skillData)
    {
        var cs = SkillList.Find(x => x.Data == skillData);
        if(cs != null)
        {
            cs.SkillLevel += 1;
            return;
        }
        Skill s = new Skill();
        s.Init(this,skillData);
        RegisterSkill(s);
    }
    public void RegisterSkill(string skillId)
    {
        Debug.Log("Not Implement Api");
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

    

    

    private float m_currenHp;
    public float Hp => Mathf.Clamp((BaseHp + this.GetSKillStatValue<float>(0,(a,b)=> a+=b.Hp)) * HpMult,1,9999);
    public float HpMult => 1 * this.GetSKillStatValue<float>(1,(a,b)=> a*=b.HpMult);
    public float Damage => 0 + this.GetSKillStatValue<float>(0,(a,b)=> a+=b.Damage);
    public float Speed => (BaseSpeed + this.GetSKillStatValue<float>(0,(a,b)=> a+=b.Speed))*SpeedMult;
    public float SpeedMult => 1 * this.GetSKillStatValue<float>(1,(a,b)=> a*=b.SpeedMult);
    public float CoolTime => 1 * this.GetSKillStatValue<float>(1,(a,b)=> a*=b.CoolTime);
    public float Def => 0 + this.GetSKillStatValue<float>(0,(a,b)=> a+=b.Def);
    public float ReduceDmg => 1 * this.GetSKillStatValue<float>(1,(a,b)=> a*=b.ReduceDmg);
    public float DmgMult => 1 * this.GetSKillStatValue<float>(1,(a,b)=> a*=b.DmgMult);
    public float ProjSpeed => 0 + this.GetSKillStatValue<float>(0,(a,b)=> a+=b.ProjSpeed);
    public float ProjSpeedMult => 1 * this.GetSKillStatValue<float>(1,(a,b)=> a*=b.ProjSpeedMult);
    public int ProjCount => 0 + this.GetSKillStatValue<int>(0,(a,b) => a += b.ProjCount);

    

    private const float BaseHp = 100;
    private const float BaseSpeed = 5;
    [SerializeField]private PlayerController m_Controller;
    [SerializeField]private SkillData TestSkill;
}