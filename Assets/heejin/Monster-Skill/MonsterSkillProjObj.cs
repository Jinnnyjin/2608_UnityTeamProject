using UnityEngine;

public class MonsterSkillProjObj : ProjectTileSkillObj
{
    public override void Init(Skill skill)
    {
        base.Init(skill);

        var m = skill.Owner as Monster;
        var p = GameManager.m_Instance.Player;
        var vec = p.Position - m.Position;

        transform.SetAngle(vec);

    }

    public override void Update()
    {
        base.Update();
    }


}
