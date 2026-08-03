using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    private Monster m_monster;
    //private SOMonsterSkillData m_SOMonsterSkillData;

    private int m_currentSkillIndex;
    private float m_cooltimer;

    private void Awake()
    {
        m_monster = GetComponent<Monster>();
        //m_SOmonsterSkillData[] = 
    }

    private void Update()
    {
        m_cooltimer += Time.deltaTime;

        
        if (m_cooltimer >= m_monster.BaseInfo.Skills[m_currentSkillIndex].SkillCoolTime)
        {
            DoAction(m_currentSkillIndex);
            m_cooltimer -= m_monster.BaseInfo.Skills[m_currentSkillIndex].SkillCoolTime;
        }
        
    }

    public void DoAction(int _skillIndex)
    {
        SOMonsterSkillData skill = m_monster.BaseInfo.Skills[_skillIndex];
        // 스킬 로직
        Debug.Log($"{_skillIndex}번째 스킬 사용");
        Debug.Log($"{gameObject.name}의 스킬 발동! 공격력: {skill.SkillAttackPower} / 공격범위: {skill.SkillRange} / 쿨타임: {skill.SkillCoolTime} ");
        // 인덱스 번호 추가
        m_currentSkillIndex = (m_currentSkillIndex + 1) % m_monster.BaseInfo.Skills.Count;

    }
}
