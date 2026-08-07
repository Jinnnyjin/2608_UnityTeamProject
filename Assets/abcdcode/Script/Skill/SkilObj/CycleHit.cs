using System.Collections.Generic;
using UnityEngine;

public class CycleHit : SkillHit
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_owner == null) return;
        var target = collision.gameObject.GetComponent<IDamageable>();
        if(target.Faction == m_owner.Faction || target.IsDead()) return;
        if(cycleDic.ContainsKey(target))
        {
            if(cycleDic[target]+m_cycle > m_time) return;
        }
        cycleDic[target] = m_time;
        target.TakeDamage(new (){Dmg = m_parent.Skill.FinalDamage()});
    }
    public override void Update()
    {
        base.Update();
        m_time += Time.deltaTime;
    }
    private float m_time;
    [SerializeField]protected float m_cycle;
    private Dictionary<IDamageable,float> cycleDic = new Dictionary<IDamageable, float>();
}