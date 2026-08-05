using UnityEngine;
public class ProjectTileSkillObj : SkillObject
{
    public override void Init(Skill skill)
    {
        base.Init(skill);
        m_Timer.SetCool(Cool,m_duration,0,true,Delete);
        switch(m_InitType)
        {
            case ProjectTileInitType.Forward:
                var p = skill.Owner as Player;
                transform.SetAngle(p.Controller.LookAt);
                break;
            case ProjectTileInitType.NearestEnemy:
                break;
            case ProjectTileInitType.Random:
                float v = UnityEngine.Random.Range(0,360);
                transform.SetAngle(v);
                break;
            default:
                break;
        }
        skillMovement.Init(this);
        skillHit.Init(this);
    }
    public override void Update()
    {
        base.Update();
    }
    

    protected const string Cool = "Cool";
    [SerializeField]protected float m_duration;
    [SerializeField]protected ProjectTileInitType m_InitType;
    [SerializeField]protected SkillMovement skillMovement;
    [SerializeField]protected SkillHit skillHit;
}
public enum ProjectTileInitType
{
    None,
    Forward,
    NearestEnemy,
    Random
}