using UnityEngine;
using UnityEngine.Rendering;

public class DashSkill : SkillData
{
    private const string SKILL_ID = "Dash_Skill";

    [SerializeField] private float m_moveweight = 1.0f;

   
    public override void Init(Skill _skill)
    {
        base.Init(_skill);
        _skill.CoolTimer.SetCool(SKILL_ID, _skill.Data.CoolTime, 0, false, null);
       
    }

    public override void GameUpdate(Skill _skill)
    {
        if (_skill.CoolTimer.IsCoolComp(SKILL_ID))
        {

            Monster monster = _skill.Owner as Monster;
            if (monster == null) return;

            if (!monster.TryStartSkill()) return;

            _skill.CoolTimer.RefreshCool(SKILL_ID);

            monster.MoveToPlayer(m_moveweight);
            
        }
    }

    
}
