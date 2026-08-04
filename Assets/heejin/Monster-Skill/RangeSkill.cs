using System.Collections;
using UnityEngine;

public class RangeSkill : SkillData
{
    [SerializeField] private float m_Range;
    [SerializeField] private float m_Damage;
    public override float Damage => m_Damage;

    public float m_WarningDelay;

    

    public override void Init(Skill _skill)
    {
        _skill.CoolTimer.SetCool("Range_Skill", _skill.Data.CoolTime,0,false,null);
    }

    public override void GameUpdate(Skill _skill)
    {
        Monster monster = _skill.Owner as Monster;
        if (monster == null) return;

        // 0804 GameManager 내에서 수정해야함, 현재 테스트파일에 연결
        float distance = (monster.Position - GameManager.m_Instance.Player.Position).magnitude;
        
        if(_skill.CoolTimer.IsCoolComp("Range_Skill") && distance <= m_Range)
        {
                monster.StartCoroutine(SkillCoroutine(monster));
                _skill.CoolTimer.RefreshCool("Range_Skill");
        }
    }

    private IEnumerator SkillCoroutine(Monster monster)
    {
        Debug.Log("전조 이펙트");

        yield return new WaitForSeconds(m_WarningDelay);

        // 0804 GameManager 내에서 수정해야함, 현재 테스트파일에 연결
        float distance = (monster.Position - GameManager.m_Instance.Player.Position).magnitude;
        
        if(distance <= m_Range)
        {
            DamageInfo info = new DamageInfo { Dmg = Damage };
            // 0804 아직 TakeDamage 확인 안됨
            GameManager.m_Instance.Player.TakeDamage(info);
        }
    }
}
