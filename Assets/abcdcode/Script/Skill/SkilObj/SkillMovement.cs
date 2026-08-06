using UnityEngine;

public abstract class SkillMovement : BSObj
{
    public virtual void Init(SkillObject obj)
    {
        SkillObject = obj;
    }
    public override void Update()
    {
        base.Update();
    }
    public SkillObject SkillObject{get;private set;}
}