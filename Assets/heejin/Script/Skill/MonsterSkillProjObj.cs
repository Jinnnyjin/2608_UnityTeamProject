using Unity.VisualScripting;
using UnityEngine;

public class MonsterSkillProjObj : ProjectTileSkillObj
{
    [SerializeField] private SOAudio m_audioEnergySkill;

    public override void Init(Skill skill)
    {
        base.Init(skill);

        var m = skill.Owner as Monster;
        var p = GameManager.m_Instance.Player;
        var vec = p.Position - m.Position;

        
        transform.SetAngle(vec);
        SoundManager.m_Instance.PlaySfx(m_audioEnergySkill);
    }

    public override void Update()
    {
        base.Update();
    }


}
