using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    private Monster m_monster;

    private void Awake()
    {
        m_monster = GetComponent<Monster>();

        // 스킬 등록
        foreach (SkillData data in m_monster.BaseInfo.Skills)
        {
            Skill skill = new Skill();
            skill.Init(m_monster, data);
            data.Init(skill);
            m_monster.RegisterSkill(skill);
        }
    }

    private void Update()
    {
        foreach (Skill skill in m_monster.SkillList)
        {
            skill.GameUpdate();
        }
        
    }
}
