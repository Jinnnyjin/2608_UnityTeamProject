using UnityEngine;
public class ProjectTileSkillObj : SkillObject
{
    public override void Init(Skill skill)
    {
        base.Init(skill);
        m_Timer.SetCool(Cool,m_duration,m_duration,true,Delete);
    }
    public override void Update()
    {
        base.Update();
    }
    public virtual void Delete()
    {
        
    }
    private const string Cool = "Cool";
    [SerializeField]private float m_duration;
    [SerializeField]private ProjectTileInitType m_InitType;
}
public enum ProjectTileInitType
{
    None,
    Forward,
    NearestEnemy,
    Random
}