using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class CalcUtil
{
    public static R GetSKillStatValue<R>(this ISkillOwner owner, R start,Func<R,Skill,R> func)
    {
        return owner.SkillList.FindAll(x => x.Data.SkillType == SkillType.Passive || x.Data.SkillType == SkillType.Buff).GetEach<R,Skill>(func,start);
    }
    public static List<T> ConvertGameObjectListToComp<T>(this List<GameObject> list)
    {
        if(list[0].GetComponent<T>() == null) return null;
        List<T> result = new List<T>();
        foreach(var o in list)
        {
            result.Add(o.GetComponent<T>());
        }
        return result;
    }
    public static T GetNearestInList<T>(this List<T> list,Vector3 pos) where T : BSObj
    {
        return FindObjective(list,(a,b) =>Vector3.Distance(a.Position,pos) > Vector3.Distance(b.Position,pos) ? b : a);
    }
    public static T FindObjective<T>(List<T> list, Func<T,T,T> func)
    {
        T result = default(T);
        foreach(var t in list)
        {
            if(result == null)
            {
                result = t;
                continue;
            }
            result = func(result,t);
        }
        return result;
    }
    public static float FinalDamage(this Skill s)
    {
        if(s.Owner is IStat o)
        {
            return (s.Damage + o.Damage) * o.DmgMult;
        }
        return s.Damage;
    }
    public static R GetEach<R,T>(this List<T> list, Func<R,T,R> func, R start)
    {
        list.ForEach((v) => start = func(start,v));
        return start;
    }
    /// <summary>
    /// 2D 회전 코드. 우측(+x)을 바라보는게 0도. 위를 바라보는게 90도, 좌측이 180도, 아래가 270(-90)도
    /// </summary>
    /// <param name="t"></param>
    /// <param name="angle"></param>
    public static void SetAngle(this Transform t, float angle)
    {
        t.eulerAngles = new Vector3(0,0,angle);
    }
    /// <summary>
    /// looAt 방향을 바라봄.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="lookAt"></param>
    public static void SetAngle(this Transform t, Vector2 lookAt)
    {
        float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg;
        SetAngle(t,angle);
    }
}