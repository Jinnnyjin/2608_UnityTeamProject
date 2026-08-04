using Unity.VisualScripting;
using UnityEngine;

public abstract class BSObj : MonoBehaviour
{
    public Vector3 Position
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }
    public Vector3 Scale
    {
        get
        {
            return this.transform.localScale;
        }
        set
        {
            this.transform.localScale = value;
        }
    }
    public virtual void Update()
    {
        m_Timer.Update();
    }
    public CoolTimer m_Timer = new ();
}