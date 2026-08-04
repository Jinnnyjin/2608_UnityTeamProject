using UnityEngine;
public abstract class SkillData : SOData, IStat
{
    [SerializeField]protected float m_CoolTime;
    [SerializeField]protected GameObject Prefab;
    [SerializeField]protected SkillType m_type;
    [SerializeField]protected Sprite m_icon;
    [SerializeField]protected string m_title;
    [SerializeField]protected string m_desc;
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

    public virtual float Hp => 0;
    public virtual float HpMult => 1;
    public virtual float Damage => 0;
    public virtual float Speed => 0;
    public virtual float CoolTime => m_CoolTime;
    public virtual float Def => 0;
    public virtual float ReduceDmg => 1;
    public virtual float DmgMult => 1;
    public virtual float SpeedMult => 1;
    public virtual float ProjSpeed => 0;
    public virtual float ProjSpeedMult => 1;
    public int ProjCount => 0;

    
    public virtual Sprite Icon =>m_icon;
    public virtual string Title => m_title;
    public virtual string Desc => m_desc;
    public virtual SkillType SkillType => m_type;

    
}
public enum SkillType
{
    Active,
    Passive
}
