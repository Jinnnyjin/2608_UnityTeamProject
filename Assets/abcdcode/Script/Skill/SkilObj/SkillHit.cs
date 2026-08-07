using System.Collections.Generic;
using UnityEngine;
public class SkillHit : BSObj
{
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_owner == null || m_isDeleted) return;
        var target = collision.gameObject.GetComponent<IDamageable>();
        if(target.Faction == m_owner.Faction || target.IsDead()) return;
        if(m_damagedTargets.Contains(target)) return;
        target.TakeDamage(new (){Dmg = m_parent.Skill.FinalDamage()});
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
    protected bool m_isDeleted = false;
    protected List<IDamageable> m_damagedTargets;
    protected SkillObject m_parent;
    protected IDamageable m_owner;
    [SerializeField]protected HitType h_Type;
}
public enum HitType
{
    Once,
    Infinity
}