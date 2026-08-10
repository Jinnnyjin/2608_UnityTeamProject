using UnityEngine;

public class AreaSkillObj_Random : ProjectTileSkillObj
{
    public override void Init(Skill skill)
    {
        base.Init(skill);
        var v = new Vector3(UnityEngine.Random.Range(-5f,5f),UnityEngine.Random.Range(-3f,3f));
        Position = skill.Owner.Obj.Position + v;
    }
}