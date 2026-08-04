using UnityEngine;
public abstract class SkillData : SOData, IStat
{
    public float m_CoolTime;
    public GameObject Prefab;
    public SkillType m_type;

    public virtual float Hp => 0;

    public virtual float Damage => 0;

    public virtual float Speed => 0;

    public virtual float CoolTime => CoolTime;

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
public enum SkillTyp
{
    Active,
    Passive
}
