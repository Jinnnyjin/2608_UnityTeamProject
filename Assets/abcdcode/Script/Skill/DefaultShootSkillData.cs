using System.Diagnostics;

public class DefaultShootSkillData : SkillData
{
    public float m_Damage;
    public float m_Speed;
    public int m_ProjCount;
    public override float Damage => m_Damage;
    public override float ProjSpeed => m_Speed;
    public override int ProjCount => m_ProjCount;
    public override void Init(Skill skill)
    {
        base.Init(skill);
        skill.CoolTimer.SetCool(Cool,CoolTime,0,true,() =>Shoot(skill));
    }
    public override void GameUpdate(Skill skill)
    {
        base.GameUpdate(skill);
    }
    public virtual void Shoot(Skill skill)
    {
        UnityEngine.Debug.Log("Shoot");
        //var ptsd = Instantiate(Prefab).GetComponent<ProjectTileSkillObj>();
        var ptsd = ObjectPoolManager.m_Instance.GetObject(Prefab).GetComponent<ProjectTileSkillObj>();
        ptsd.Position = skill.Owner.Obj.Position;
        ptsd.Init(skill);
        skill.CoolTimer.SetCool(Cool,CoolTime,0,true,() =>Shoot(skill));
        UnityEngine.Debug.Log("Shoot End");
    }
    private const string Cool = "Cool";
}