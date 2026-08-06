using System.Collections.Generic;

public interface ISkillOwner
{
    public BSObj Obj{get;}
    /// <summary>
    /// 보유한 스킬 목록
    /// </summary>
    public List<Skill> SkillList{get;}
    /// <summary>
    /// 스킬 등록
    /// </summary>
    public void RegisterSkill(Skill skill);
    public void RegisterSkill(SkillData skillData);
    public void RegisterSkill(string skillId);
    /// <summary>
    /// 스킬 해제
    /// </summary>
    public void UnRegisterSkill(Skill skill);
    public void UnRegisterSkill(SkillData skill);
    public void UnRegisterSkill(string skillId);
    public Skill GetSkillBySkillData(SkillData data)
    {
        return SkillList.Find(x => x.Data == data);
    }
}