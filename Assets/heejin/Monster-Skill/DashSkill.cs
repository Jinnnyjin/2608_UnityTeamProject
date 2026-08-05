using UnityEngine;
using UnityEngine.Rendering;

public class DashSkill : SkillData
{
    [SerializeField] private float m_speed;
    /*
     * 기존 스피드 방식에서 weight방식으로 변경
     */
    [SerializeField] private float m_moveweight = 1.0f;
    [SerializeField] private float m_duration;

   
    public override void Init(Skill _skill)
    {
        base.Init(_skill);
        _skill.CoolTimer.SetCool("Dash_Skill", _skill.Data.CoolTime, 0, false, null);
       
    }

    public override void GameUpdate(Skill _skill)
    {
        if (_skill.CoolTimer.IsCoolComp("Dash_Skill"))
        {

            Monster monster = _skill.Owner as Monster;
            if (monster == null) return;

            if (!monster.TryStartSkill()) return;

            //monster.GetComponent<MonsterAIMove>().m_IsDashing = true;
            //// 몬스터 -> 플레이어 방향
            //Vector2 direction = (GameManager.m_Instance.Player.Position - monster.Position).normalized;

            _skill.CoolTimer.RefreshCool("Dash_Skill");

            // 공격 애니메이션이 필요한가?
            //monster.MonsterMove.MonsterAttackSkill();
            monster.MoveToPlayer(m_moveweight);
            //// 대시스킬 함수 호출
            //monster.StartCoroutine(monster.GetComponent<MonsterAIMove>().DoDash(direction, m_speed, m_duration));

          
            //monster.GetComponent<MonsterAIMove>().m_IsDashing = false;
        }
    }

    
}
