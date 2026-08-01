using System;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Monster : MonoBehaviour
{

    private float m_Hp;
    private PoolObject m_poolObject;

    public static event Action<int> onMonsterDied;

    private void Awake()
    {
        m_poolObject = GetComponent<PoolObject>();
    }

    public void TakeDamage(float _damage)
    {
        m_Hp -= _damage;
    }

}
