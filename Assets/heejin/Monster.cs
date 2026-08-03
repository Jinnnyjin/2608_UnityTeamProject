using System;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class Monster : MonoBehaviour
{
    [Header("몬스터 데이터")]
    private MonsterInfo m_monsterInfo = null;
    [SerializeField] private SOMonsterInfo m_SOMonsterInfo = null;
    public static event Action<int> onMonsterDied;

    public MonsterInfo Info => m_monsterInfo;
    public SOMonsterInfo BaseInfo => m_SOMonsterInfo;

    private void Awake()
    {
        m_monsterInfo = new MonsterInfo();
        m_monsterInfo.Attack = m_SOMonsterInfo.BaseAttack;
        m_monsterInfo.Speed = m_SOMonsterInfo.BaseSpeed;
        m_monsterInfo.HP = m_SOMonsterInfo.Max_HP;
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
        onMonsterDied?.Invoke(m_SOMonsterInfo.ExpReward);

        ObjectPoolManager.m_Instance.PushObject(gameObject);
    }

}
