using System.Collections.Generic;
using UnityEngine;
public class SkillHit : BSObj
{
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_owner == null) return;
        var target = collision.gameObject.GetComponent<IDamageable>();
        if(target.Faction == m_owner.Faction) return;
        if(m_damagedTargets.Contains(target)) return;
        target.TakeDamage(new (){Dmg = m_parent.Skill.FinalDamage()});
    }
    public virtual void Init(SkillObject parent)
    {
        m_parent = parent;
        m_damagedTargets = new List<IDamageable>();
        if(m_parent.Skill.Owner is IDamageable o)
        {
            m_owner = o;
        }
    }
    protected List<IDamageable> m_damagedTargets;
    protected SkillObject m_parent;
    protected IDamageable m_owner;
}
public enum HitType
{
    Once,
    Infinity
}