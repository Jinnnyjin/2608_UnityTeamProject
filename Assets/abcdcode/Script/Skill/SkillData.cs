using UnityEngine;
public abstract class SkillData : SOData
{
    public float CoolTime;
    public GameObject Prefab;
    public SkillType type;
    public virtual void Init(Skill skill)
    {
        //skill.coolTimer.SetCool("Attack",1,0,false,null);
    }
    public virtual void GameUpdate(Skill skill)
    {
        /*
        if(skill.coolTimer.IsCoolComp("Attack"))
        {
            //공격함
            skill.coolTimer.SetCool("Attack",1,0,false,null);
        }
        */
    }
}
public enum SkillType
{
    Active,
    Passive
}