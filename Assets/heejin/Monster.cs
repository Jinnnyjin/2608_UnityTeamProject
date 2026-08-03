using System;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Monster : MonoBehaviour
{

    // private PoolObject m_poolObject;
    private int m_ExpReward;

    [Header("몬스터 데이터")]
    private MonsterInfo m_monsterInfo = null;
    [SerializeField] private SOMonsterInfo m_SOMonsterInfo = null;
    public static event Action<int> onMonsterDied;

    public MonsterInfo Info => m_monsterInfo;

    private void Awake()
    {
        m_monsterInfo = new MonsterInfo();
        m_monsterInfo.Attack = m_SOMonsterInfo.BaseAttack;
        m_monsterInfo.Speed = m_SOMonsterInfo.BaseSpeed;
        m_monsterInfo.HP = m_SOMonsterInfo.Max_HP;
        // m_poolObject = GetComponent<PoolObject>();
    }

    public void TakeDamage(float _damage)
    {
        m_monsterInfo.HP -= _damage;

        if(m_monsterInfo.HP <= 0)
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
