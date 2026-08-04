using System;
using System.Collections.Generic;

public static class CalcUtil
{
    public static R GetEach<R,T>(this List<T> list, Func<R,T,R> func, R start)
    {
        list.ForEach((v) => start = func(start,v));
        return start;
    }
}