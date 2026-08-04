using System.Collections;
using UnityEngine;

public class SkillRange : SkillData
{
    public float m_Atk;
    public float m_Range;

    public float m_WarningDelay;

    

    public override void Init(Skill _skill)
    {
        _skill.CoolTimer.SetCool("Range_attack",_skill.Data.CoolTime,0,false,null);
    }

    public override void GameUpdate(Skill _skill)
    {
        Monster monster = _skill.Owner as Monster;
        if (monster == null) return;

        float distance = (monster.transform.position - GameManager.m_Instance.Player.transform.position).magnitude;
        
        if(_skill.CoolTimer.IsCoolComp("Range_attack") && distance <= m_Range)
        {
                monster.StartCoroutine(SkillCoroutine(monster));
                _skill.CoolTimer.RefreshCool("Range_attack");
        }
    }

    private IEnumerator SkillCoroutine(Monster monster)
    {
        Debug.Log("전조 이펙트");

        yield return new WaitForSeconds(m_WarningDelay);

        float distance = (monster.transform.position - GameManager.m_Instance.Player.transform.position).magnitude;
        
        if(distance <= m_Range)
        {
            DamageInfo info = new DamageInfo { Dmg = m_Atk };
            GameManager.m_Instance.Player.TakeDamage(info);
        }
    }
}
