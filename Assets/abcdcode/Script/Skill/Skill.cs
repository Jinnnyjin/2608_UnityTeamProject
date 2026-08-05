using UnityEngine;
public class Skill
{
    public void Init(ISkillOwner owner,SkillData data)
    {
        CoolTimer = new CoolTimer();
        Owner = owner;
        Data = data;
        //Data.Init(this);
    }
    public void GameUpdate()
    {
        CoolTimer.Update();
        Data.GameUpdate(this);
    }
    public CoolTimer CoolTimer;
    public ISkillOwner Owner{get;private set;}
    public SkillData Data{get;private set;}

    
}