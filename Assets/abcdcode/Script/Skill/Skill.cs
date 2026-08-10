using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Skill : IStat
{
    public void Init(ISkillOwner owner,SkillData data)
    {
        SkillLevel = 1;
        CoolTimer = new CoolTimer();
        m_objDic = new Dictionary<string, object>();
        Owner = owner;
        Data = data;
        Data.Init(this);
    }
    public void GameUpdate()
    {
        CoolTimer.Update();
        Data.GameUpdate(this);
    }
    public void Delete()
    {
        this.Owner.UnRegisterSkill(this);
    }
    public CoolTimer CoolTimer;
    private Dictionary<string,object> m_objDic;
    public T GetSkillObject<T>(string id)
    {
        if(!m_objDic.ContainsKey(id)) return default(T);
        return (T)m_objDic[id];
    }
    public void AddSkillObject(string id, object obj)
    {
        m_objDic[id] = obj;
    }
    public void RemoveSkillObject(string id)
    {
        m_objDic.Remove(id);
    }
    public ISkillOwner Owner{get;private set;}
    public SkillData Data{get;private set;}
    public int SkillLevel{
        get
        {
            return m_skillLevel;
        }
        set
        {
            var prev = m_skillLevel;
            m_skillLevel = Mathf.Clamp(value,1,MaxSkillLevel);
            if(m_skillLevel > prev)
            {
                Data.OnLevelUp(this);
            }
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

    public float HPGen => Data.HPGen + Data.GetSkillLevelDataValue(SkillLevel,0,(l) => l.HPGen);
}