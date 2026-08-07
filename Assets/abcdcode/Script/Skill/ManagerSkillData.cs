using UnityEngine;

/// <summary>
/// 획득 시 영구 유지되는 소환수 같은걸 생성하는 스킬
/// </summary>
public class ManagerSkillData : SkillData
{
    [SerializeField]protected string m_ManagerId = "Manager";
    public override void Init(Skill skill)
    {
        base.Init(skill);
        SummonManager(skill);
    }
    public virtual void SummonManager(Skill skill)
    {
        var s = Instantiate(Prefab).GetComponent<SkillObject>();
        skill.AddSkillObject(m_ManagerId,s);
        s.Position = skill.Owner.Obj.Position;
        s.Init(skill);
    }
    public virtual void DeSpawnManager(Skill skill)
    {
        var d = skill.GetSkillObject<SkillObject>(m_ManagerId);
        if(d != null)
        {
            d.Delete();
            skill.RemoveSkillObject(m_ManagerId);
        }
    }
    public override void OnLevelUp(Skill skill)
    {
        base.OnLevelUp(skill);
        DeSpawnManager(skill);
        SummonManager(skill);
    }
    public override void UnRegisterSkill(Skill skill)
    {
        base.UnRegisterSkill(skill);
        DeSpawnManager(skill);
    }
}