using UnityEngine;
public class Skill : IStat
{
    public void Init(ISkillOwner owner,SkillData data)
    {
        CoolTimer = new CoolTimer();
        Owner = owner;
        Data = data;
    }
    public void GameUpdate()
    {
        CoolTimer.Update();
        Data.GameUpdate(this);
    }
    public float Hp => 0;

    public float Damage => Data.Damage;

    public float Speed => Data.Speed;

    public float CoolTime => Data.CoolTime;
    public CoolTimer CoolTimer;
    public ISkillOwner Owner{get;private set;}
    public SkillData Data{get;private set;}

    
}