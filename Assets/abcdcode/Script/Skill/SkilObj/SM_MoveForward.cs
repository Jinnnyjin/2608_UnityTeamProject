using UnityEngine;

public class SM_MoveForward : SkillMovement
{
    public override void Update()
    {
        Position += this.transform.right.normalized * SkillObject.Skill.Speed * Time.deltaTime;
    }
}