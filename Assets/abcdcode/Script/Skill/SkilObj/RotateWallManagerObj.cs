using System.Collections.Generic;
using UnityEngine;

public class RotateWallManagerObj : SkillObject
{
    public override void Init(Skill skill)
    {
        base.Init(skill);
        //var ballCount = skill.GetFinalStat(skill.ProjCount,(i)=>skill.ProjCount+i.ProjCount);
        ballList = new List<SkillObject>();
        var ballCount = skill.ProjCount;
        for(int i = 0 ; i < ballCount; i++)
        {
            var ball = Instantiate(m_Prefab).GetComponent<SkillObject>();
            ball.Init(skill);
            ballList.Add(ball);
        }
    }
    public override void Delete()
    {
        base.Delete();
        foreach(var b in ballList.ToArray())
        {
            b.Delete();
        }
        ballList.Clear();
    }
    public override void Update()
    {
        base.Update();
        this.Position = Skill.Owner.Obj.Position;
        m_curAngle += m_rotateSpeed * Time.deltaTime;
        for(int i = 0; i < ballList.Count; i++)
        {
            var b = ballList[i];
            b.Position = this.Position+GetDirVector(m_curAngle+i*360/ballList.Count).normalized*m_range;
        }
    }
    private Vector3 GetDirVector(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad),0);
    }
    protected float m_curAngle;
    //[SerializeField]protected RotateDir m_rotateDir;
    [SerializeField]protected float m_rotateSpeed;
    [SerializeField]protected float m_range;
    [SerializeField]protected GameObject m_Prefab;
    private List<SkillObject> ballList = new List<SkillObject>();
}