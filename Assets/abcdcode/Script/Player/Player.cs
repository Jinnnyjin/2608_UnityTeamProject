using System.Collections.Generic;
using UnityEngine;

public class Player : BSObj, IDamageable,ISkillOwner
{
    public void Start()
    {
        SkillList = new List<Skill>();
    } 
    public List<Skill> SkillList { get; private set; }

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

    [SerializeField]private PlayerController m_Controller;
}