using System;
using UnityEngine;
using UnityEngine.Events;

//public interface IPoolable
//{
//    public string PoolKey { get; }
//    public int PushFlag { get; }

//    public void Push();
//    public void Pop();
//}

public class PoolObject : MonoBehaviour
{

    [SerializeField] private GameObject m_orioginalPrefab = null;
    public GameObject OriginPrefab => m_orioginalPrefab;

    private int m_iPushCount = 0;
    public int PushFlag { get { return m_iPushCount; } }


    public event Action OnPush;
    public event Action OnPop;

    [SerializeField] private float m_fAliveTime = 10.0f;
    private float m_fSettingAliveTime = 0.0f;

    private void Update()
    {
        m_fAliveTime -= Time.deltaTime;
        if (m_fAliveTime <= 0.0f)
            ObjectPoolManager.m_Instance.PushObject(gameObject);
    }

    public virtual void Push()
    {
        m_iPushCount = 1;
        OnPush?.Invoke();
    }

    public virtual void Pop()
    {
        m_iPushCount = 0;
        m_fAliveTime = m_fSettingAliveTime > 0f ? m_fSettingAliveTime : float.MaxValue;
        OnPop?.Invoke();
    }

    public void SetAliveTime(float PushTime)
    {
        m_fAliveTime = PushTime;
        m_fSettingAliveTime = m_fAliveTime;
    }

    public void SetOriginKey(GameObject _orginPrefab)
    {
        m_orioginalPrefab = _orginPrefab;
    }

    //동적으로 키를 바꿀 이유가 있다면 
    //public void SetOriginalKey(string _key)
    //{
    //    m_originalPoolKey = _key;
    //}

}
