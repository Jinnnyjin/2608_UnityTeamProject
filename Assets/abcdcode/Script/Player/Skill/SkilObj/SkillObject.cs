using UnityEngine;

public abstract class SkillObject : BSObj
{
    public virtual void Init(Skill skill)
    {
        Skill = skill;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
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
    public Skill Skill{get;private set;}
}