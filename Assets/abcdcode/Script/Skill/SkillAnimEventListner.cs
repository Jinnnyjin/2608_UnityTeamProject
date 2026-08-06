using UnityEngine;

public class SkillAnimEventListner : MonoBehaviour
{
    public void Awake()
    {
        Transform p = this.transform;
        while(true)
        {
            p = p.parent;
            if(p == null) return;
            var so = p.GetComponent<SkillObject>();
            if(so != null) 
            {
                m_parent = so; 
                return;
            }
        }
    }
    public void EventListen(string name)
    {
        if(m_parent != null)
        {
            m_parent.EventListen(name);
        }
    }
    private SkillObject m_parent;
}