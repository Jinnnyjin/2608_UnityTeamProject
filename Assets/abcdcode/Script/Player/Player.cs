using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable,ISkillOwner
{
    public void Start()
    {
        SkillList = new List<Skill>();
    } 
    public List<Skill> SkillList { get; set; }

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
        throw new System.NotImplementedException();
    }

    public void UnRegisterSkill(string skillId)
    {
        throw new System.NotImplementedException();
    }
}