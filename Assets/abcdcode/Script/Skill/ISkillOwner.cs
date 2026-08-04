using System.Collections.Generic;

public interface ISkillOwner
{
    public BSObj Obj{get;}
    /// <summary>
    /// 보유한 스킬 목록
    /// </summary>
    public List<Skill> SkillList{get;set;}
    /// <summary>
    /// 스킬 등록
    /// </summary>
    public void RegisterSkill(Skill skill);
    /// <summary>
    /// 스킬 해제
    /// </summary>
    public void UnRegisterSkill(Skill skill);
    public void UnRegisterSkill(string skillId);

}