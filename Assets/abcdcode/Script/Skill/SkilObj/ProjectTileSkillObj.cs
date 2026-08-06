using System.Linq;
using UnityEngine;
[RequireComponent(typeof(SkillMovement),typeof(SkillHit))]
public class ProjectTileSkillObj : SkillObject
{
    public override void Init(Skill skill)
    {
        base.Init(skill);
        var p = skill.Owner as Player;
        skillMovement = GetComponent<SkillMovement>();
        skillHit = GetComponent<SkillHit>();
        m_Timer.SetCool(Cool,m_duration,0,true,Delete);
        switch(m_InitType)
        {
            case ProjectTileInitType.Forward:
                transform.SetAngle(p.Controller.LookAt);
                break;
            case ProjectTileInitType.NearestEnemy:
                var m = StageManager.m_Instance.SpawnGameObject.ToList().ConvertGameObjectListToComp<Monster>().GetNearestInList(p.Position);
                if(m == null)
                {
                    transform.SetAngle(0);
                }
                else
                {
                    var vec = m.Position - p.Position;
                    transform.SetAngle(vec);
                }
                
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