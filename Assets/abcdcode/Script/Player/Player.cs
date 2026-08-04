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
    private float m_currenHp;
    public float Hp => BaseHp;

    public float Damage => 1;

    public float Speed => BaseSpeed;

    public float CoolTime => 1;

    public float Def => 0;

    public float ReduceDmg => 0;

    private const float BaseHp = 100;
    private const float BaseSpeed = 5;
    [SerializeField]private PlayerController m_Controller;
}