using System;
using UnityEngine;
[Serializable]
public class Skill : IStat
{
    public void Init(ISkillOwner owner,SkillData data)
    {
        SkillLevel = 3;
        CoolTimer = new CoolTimer();
        Owner = owner;
        Data = data;
        Data.Init(this);
    }
    public void GameUpdate()
    {
        CoolTimer.Update();
        Data.GameUpdate(this);
    }
    public CoolTimer CoolTimer;
    public ISkillOwner Owner{get;private set;}
    public SkillData Data{get;private set;}
    public int SkillLevel{
        get
        {
            return m_skillLevel;
        }
        set
        {
            m_skillLevel = Mathf.Clamp(value,1,MaxSkillLevel);
        }
    }

    private int m_skillLevel = 1;
    public const int MaxSkillLevel = 5;
    public T GetFinalStat<T>(T s, Func<IStat,T> o)
    {
        if(Owner is IStat i)
        {
            return o(i);
        }
        return s;
    }

    public float Hp => Data.Hp + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.Hp);

    public float HpMult => Data.HpMult + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.HpMult);

    public float Damage => Data.Damage + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.Damage);

    public float DmgMult => Data.DmgMult + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.DmgMult);

    public float Speed => Data.Speed + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.Speed);

    public float SpeedMult => Data.SpeedMult + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.SpeedMult);

    public float ProjSpeed => Data.ProjSpeed + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.ProjSpeed);

    public float ProjSpeedMult => Data.ProjSpeedMult + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.ProjSpeedMult);

    public int ProjCount => Data.ProjCount + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.ProjCount);

    public float CoolTime => Data.CoolTime + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.CoolTime);

    public float Def => Data.Def + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.Def);

    public float ReduceDmg => Data.ReduceDmg + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.ReduceDmg);
}