using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultShootSkillData : SkillData
{
    public float m_Damage;
    public float m_Speed;
    public int m_ProjCount;
    public override float Damage => m_Damage;
    public override float ProjSpeed => m_Speed;
    public override int ProjCount => m_ProjCount;
    public override void Init(Skill skill)
    {
        base.Init(skill);
        skill.CoolTimer.SetCool(Cool,CoolTime,0,true,() =>Shoot(skill));
    }
    public override void GameUpdate(Skill skill)
    {
        base.GameUpdate(skill);
    }
    public virtual void Shoot(Skill skill)
    {
        //var ptsd = Instantiate(Prefab).GetComponent<ProjectTileSkillObj>();
        if(skill.Owner is MonoBehaviour m)
        {
           m.StartCoroutine(ShootCoroutine(skill));
        }
        else
        {
            Debug.Log("What?? ISkillOwner Must be MonoBehaviour!!!");
        }
    }
    IEnumerator ShootCoroutine(Skill skill)
    {
        var projCount = skill.GetFinalStat(skill.ProjCount,(i)=>skill.ProjCount+i.ProjCount);
        for(int i = 0; i < projCount;i++)
        {
            var ptsd = ObjectPoolManager.m_Instance.GetObject(Prefab).GetComponent<ProjectTileSkillObj>();
            ptsd.Position = skill.Owner.Obj.Position;
            ptsd.Init(skill);
            skill.CoolTimer.SetCool(Cool,CoolTime,0,true,() =>Shoot(skill));
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
    private const string Cool = "Cool";
}