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
    }
    public override void Update()
    {
        base.Update();
    }
    public virtual void Delete()
    {
        UnityEngine.Debug.Log("Delete Projectile");
        ObjectPoolManager.m_Instance.PushObject(this.gameObject);
    }

    protected const string Cool = "Cool";
    [SerializeField]protected float m_duration;
    [SerializeField]protected ProjectTileInitType m_InitType;
    [SerializeField]protected SkillMovement skillMovement;
}
public enum ProjectTileInitType
{
    None,
    Forward,
    NearestEnemy,
    Random
}