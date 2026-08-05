using UnityEngine;

public abstract class SkillObject : BSObj
{
    public virtual void Init(Skill skill)
    {
        Skill = skill;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    public override void Update()
    {
        base.Update();
    }
    public Skill Skill{get;private set;}
}