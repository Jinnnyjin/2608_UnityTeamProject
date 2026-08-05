using UnityEngine;

public class SM_MoveForward : SkillMovement
{
    public override void Update()
    {
        base.Update();
        var s = SkillObject.Skill;
        Position += this.transform.right.normalized * s.GetFinalStat(s.ProjSpeed,(i)=>s.ProjSpeed*i.ProjSpeedMult) * Time.deltaTime;
    }
}