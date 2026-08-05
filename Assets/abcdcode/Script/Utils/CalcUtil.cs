using System;
using System.Collections.Generic;
using UnityEngine;

public static class CalcUtil
{
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
    public static void LookAt(this Transform t, Vector2 lookAt)
    {
        t.right = lookAt;
    }
}