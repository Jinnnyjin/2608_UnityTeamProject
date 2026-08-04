using UnityEngine;

public enum SkillType
{
    RangeAttack,
    Dash
}

public class MonsterSkill : MonoBehaviour
{
    private Monster m_monster;

    private int m_currentSkillIndex;
    private float m_cooltimer;

    private void Awake()
    {
        m_monster = GetComponent<Monster>();
    }

    private void Update()
    {
        // 예외처리
        if (m_monster.BaseInfo.Skills.Count == 0) return;
        
        // 쿨타임 타이머
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
        
        switch(skill.Type)
        {
            case SkillType.RangeAttack:
                DoRangeAttack(skill);
                break;
            case SkillType.Dash:
                DoDashAttack(skill);
                break;
        }

        // 인덱스 번호 추가
        m_currentSkillIndex = (m_currentSkillIndex + 1) % m_monster.BaseInfo.Skills.Count;

    }

    private void DoRangeAttack(SOMonsterSkillData _skill)
    {
        float distance = (transform.position - GameManager.m_Instance.Player.transform.position).magnitude;

        if(distance <= _skill.SkillRange)
        {
            // 데미지

            // 테스트용 디버그 로그
            Debug.Log($"데미지: {_skill.SkillAttackPower}");
        }
    }

    private void DoDashAttack(SOMonsterSkillData _skill)
    {
        // 돌진 스킬 구현 예정
    }
}
