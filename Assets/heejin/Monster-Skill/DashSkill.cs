using UnityEngine;

public class DashSkill : SkillData
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_duration;

    public override void Init(Skill _skill)
    {
        base.Init(_skill);
        _skill.CoolTimer.SetCool("Dash_Skill", _skill.Data.CoolTime, 0, false, null);
    }

    public override void GameUpdate(Skill _skill)
    {
        Debug.Log("DashSkill GameUpdate 호출됨");

        if (_skill.CoolTimer.IsCoolComp("Dash_Skill"))
        {
            Monster monster = _skill.Owner as Monster;
            if (monster == null)  return;

            // 몬스터 -> 플레이어 방향
            Vector2 direction = (GameManager.m_Instance.Player.Position - monster.Position).normalized;

            // 대시스킬 함수 호출
            monster.StartCoroutine(monster.GetComponent<MonsterAIMove>().DoDash(direction, m_speed, m_duration));

            _skill.CoolTimer.RefreshCool("Dash_Skill");
        }
    }
}
