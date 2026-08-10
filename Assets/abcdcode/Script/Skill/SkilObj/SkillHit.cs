using System.Collections.Generic;
using UnityEngine;
public class SkillHit : BSObj
{
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_owner == null || m_isDeleted) return;
        var target = collision.gameObject.GetComponent<IDamageable>();
        if(!TargetVaild(target)) return;
        if(m_damagedTargets.Contains(target)) return;
        GiveDamage(target);
        m_damagedTargets.Add(target);
        if(h_Type == HitType.Once)
        {
            m_parent.Delete();
            m_isDeleted = true;
        }
    }
    public virtual void Init(SkillObject parent)
    {
        m_isDeleted = false;
        m_parent = parent;
        m_damagedTargets = new List<IDamageable>();
        if(m_parent.Skill.Owner is IDamageable o)
        {
            m_owner = o;
        }
    }
    public virtual bool TargetVaild(IDamageable target)
    {
        if(target == null) return false;
        if(target.Faction == m_owner.Faction || target.IsDead()) return false;
        return true;
    }
    public virtual void GiveDamage(IDamageable target)
    {
        target.TakeDamage(new (){Dmg = m_parent.Skill.FinalDamage()});
        m_parent.PlaySound(m_hitSoundName);
    }
    protected bool m_isDeleted = false;
    protected List<IDamageable> m_damagedTargets;
    protected SkillObject m_parent;
    protected IDamageable m_owner;
    [SerializeField]protected HitType h_Type;
    [SerializeField]protected string m_hitSoundName = "Hit";
}
public enum HitType
{
    Once,
    Infinity
}