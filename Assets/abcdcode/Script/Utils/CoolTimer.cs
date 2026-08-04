using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 쿨타임 관리자
/// </summary>
public class CoolTimer
{
    public CoolTimer()
    {
        coolDic = new Dictionary<string, CoolInfo>();
    }
    /// <summary>
    /// 쿨타임 추가. 이미 같은 id를 추가했었다면 덮어씀
    /// </summary>
    /// <param name="id">추가할 쿨타임의 id</param>
    /// <param name="cool">쿨타임. 1이면 1초</param>
    /// <param name="startCool">시작 쿨타임</param>
    /// <param name="isOnce">한번 호출 후 사라질 지 여부. 단순 쿨타임 체크용이라면 false 권장</param>
    /// <param name="callBack">쿨타임 되었을때 콜백 함수</param>
    public void SetCool(string id, float cool, float startCool, bool isOnce, Action callBack)
    {
        CoolInfo info = new (startCool,cool,callBack,isOnce);
        coolDic[id] = info;
    }
    /// <summary>
    /// 쿨타임 삭제
    /// </summary>
    /// <param name="id"></param>
    public void DeleteCool(string id)
    {
        if(coolDic.ContainsKey(id))
        {
            coolDic.Remove(id);
        }
    }
    /// <summary>
    /// 지정 쿨타임 0으로 초기화
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newCool">해당 쿨타임 정보 새로운 쿨타임 지정</param>
    public void RefreshCool(string id, float newCool = -1)
    {
        if(coolDic.ContainsKey(id))
        {
            coolDic[id].cur = 0;
            if(newCool > 0)
            {
                coolDic[id].cool = newCool;
            }
        }
    }
    /// <summary>
    /// 쿨타임 정보 가져오기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CoolInfo GetCool(string id)
    {
        if(coolDic.ContainsKey(id)) return coolDic[id];
        return null;
    }
    /// <summary>
    /// 쿨타임이 다 되었는지 체크하기
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsCoolComp(string id)
    {
        if(!coolDic.ContainsKey(id)) return true;
        return coolDic[id].cur >= coolDic[id].cool;
    }
    public void Update()
    {
        foreach(var pair in coolDic.ToList())
        {
            var ci = pair.Value;
            ci.cur += Time.deltaTime;
            if(ci.cur >= ci.cool)
            {
                if(ci.callBack != null) ci.callBack();
                if(ci.IsOnce) DeleteCool(pair.Key);
            }
        }
    }
    public Dictionary<string,CoolInfo> coolDic;
    public class CoolInfo
    {
        public CoolInfo(float c, float co, Action action, bool io)
        {
            cur = c;
            cool = co;
            callBack = action;
            IsOnce = io;
        }
        public float cur;
        public float cool;
        public Action callBack;
        public bool IsOnce;
    }
}