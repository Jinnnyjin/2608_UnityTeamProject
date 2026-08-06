using UnityEngine;
using UnityEngine.Rendering;

public class DashSkill : SkillData
{
    [SerializeField] private float m_speed;

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

            _skill.CoolTimer.RefreshCool("Dash_Skill");

            monster.MoveToPlayer(m_moveweight);
            
        }
    }

    
}
