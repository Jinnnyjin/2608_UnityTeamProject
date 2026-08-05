using System;
using UnityEngine;
[Serializable]
public class Skill : IStat
{
    public void Init(ISkillOwner owner,SkillData data)
    {
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
            Mathf.Clamp(value,1,MaxSkillLevel);
        }
    }
    private int m_skillLevel = 1;
    public const int MaxSkillLevel = 5;

    public float Hp => throw new NotImplementedException();

    public float HpMult => throw new NotImplementedException();

    public float Damage => throw new NotImplementedException();

    public float DmgMult => throw new NotImplementedException();

    public float Speed => throw new NotImplementedException();

    public float SpeedMult => throw new NotImplementedException();

    public float ProjSpeed => throw new NotImplementedException();

    public float ProjSpeedMult => throw new NotImplementedException();

    public int ProjCount => throw new NotImplementedException();

    public float CoolTime => throw new NotImplementedException();

    public float Def => throw new NotImplementedException();

    public float ReduceDmg => throw new NotImplementedException();
}