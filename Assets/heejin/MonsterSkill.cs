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
        // 스킬 로직

        // 인덱스 번호 추가
        m_currentSkillIndex = (m_currentSkillIndex + 1) % m_monster.BaseInfo.Skills.Count;

    }
}
