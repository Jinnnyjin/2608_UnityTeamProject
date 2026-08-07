using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObject : BSObj
{
    public virtual void Init(Skill skill)
    {
        Skill = skill;
    }
    public override void Update()
    {
        base.Update();
    }
    public virtual void Delete()
    {
        UnityEngine.Debug.Log("Delete SkillObject");
        if(GetComponent<PoolObject>() != null)
        {
            ObjectPoolManager.m_Instance.PushObject(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public virtual void EventListen(string name)
    {
        if(name == "Delete")
        {
            Delete();
        }
    }
    public virtual void PlaySound(string id)
    {
        var s = m_skillSoundDic.Find(x => x.Id == id);
        if(s == null)
        {
            Debug.Log($"No Sound : {id}");
            return;
        }
        SoundManager.m_Instance.PlaySfx(s.Sound);
    }
    [SerializeField]protected List<SkillSoundInfo> m_skillSoundDic = new List<SkillSoundInfo>();

    public Skill Skill{get;private set;}
}
[Serializable]
public class SkillSoundInfo
{
    public string Id;
    public SOAudio Sound;
}