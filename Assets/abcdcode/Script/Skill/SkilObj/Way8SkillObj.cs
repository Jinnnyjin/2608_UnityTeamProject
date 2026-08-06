using UnityEngine;

public class Way8SkillObj : ProjectTileSkillObj
{
    public override void Init(Skill skill)
    {
        base.Init(skill);
        for(int i = 0 ; i < 8; i++)
        {
            Shoot(i*45);
        }
        Delete();
    }
    public virtual void Shoot(float angle)
    {
        var ptsd = ObjectPoolManager.m_Instance.GetObject(m_Prefab).GetComponent<ProjectTileSkillObj>();
        ptsd.Position = Position;
        ptsd.Init(Skill);
        ptsd.transform.SetAngle(angle);
    }
    [SerializeField]protected GameObject m_Prefab;
}