using System;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Monster : MonoBehaviour
{
    //[SerializeField] SOMonsterData;
    private float m_Hp;
    // private PoolObject m_poolObject;
    private int m_ExpReward;

    [Header("")]
    private MonsterInfo m_monsterInfo = null;
    [SerializeField] private SOMonsterInfo m_SOMonsterInfo = null;
    public static event Action<int> onMonsterDied;

    private void Awake()
    {
        m_monsterInfo = new MonsterInfo();
        m_monsterInfo.Attack = m_SOMonsterInfo.BaseAttack;
        m_monsterInfo.Speed = m_SOMonsterInfo.BaseSpeed;
        m_monsterInfo.HP = m_SOMonsterInfo.Max_HP;
        // m_poolObject = GetComponent<PoolObject>();
    }

    public void SetData(MonsterData _data)
    {
        m_Hp = _data.Hp;
        m_ExpReward = _data.ExpReward;
    }

    public void TakeDamage(float _damage)
    {
        m_Hp -= _damage;

        if(m_Hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 테스트용 경험치
        onMonsterDied?.Invoke(m_ExpReward);


        ObjectPoolManager.m_Instance.PushObject(gameObject);
    }

}
