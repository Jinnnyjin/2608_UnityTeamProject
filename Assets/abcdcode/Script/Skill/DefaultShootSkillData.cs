public class DefaultShootSkillData : SkillData
{
    public float m_Damage;
    public float m_Speed;
    public override float Damage => m_Damage;
    public override float Speed => m_Speed;
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
        var ptsd = Instantiate(Prefab).GetComponent<ProjectTileSkillObj>();
        ptsd.Position = skill.Owner.Obj.Position;
        ptsd.Init(skill);
        skill.CoolTimer.SetCool(Cool,CoolTime,0,true,() =>Shoot(skill));
    }
    private const string Cool = "Cool";
}