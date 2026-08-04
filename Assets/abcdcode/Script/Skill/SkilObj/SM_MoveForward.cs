using UnityEngine;

public class SM_MoveForward : SkillMovement
{
    public override void Update()
    {
        base.Update();
        Position += this.transform.right.normalized * SkillObject.Skill.Data.ProjSpeed * Time.deltaTime;
    }
}