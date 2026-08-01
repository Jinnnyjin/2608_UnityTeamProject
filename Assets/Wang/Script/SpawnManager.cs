using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// /
/// </summary>

public class MonsterManager : MonoBehaviour
{

    //[SerializeField] private List<SOMonsterInfo> m_listMonsterInfo;
    [SerializeField] private List<PoolObject> m_Monsters;


    [SerializeField] private float m_fSpawnTime = 2.0f;

    private float m_fCurTime = 0.0f;
    private void Update()
    {
        m_fCurTime += Time.deltaTime;

        if (m_fCurTime >= m_fSpawnTime)
        {
            m_fCurTime = 0.0f;
            int idx = UnityEngine.Random.Range(0, m_Monsters.Count);
            Spawn(m_Monsters[idx]);
        }


    }

    private void Spawn(PoolObject _Monster)
    {
        float RandomX = UnityEngine.Random.Range(-0.1f, 1.1f);
        float RandomY = (UnityEngine.Random.value > 0.5f) ? 1.1f : -0.1f;
        Vector3 viewportPoint = new Vector3(RandomX, RandomY, Camera.main.nearClipPlane);

        // 2. Viewport 좌표를 World 좌표로 변환
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPoint);

        GameObject Monster = ObjectPoolManager.m_Instance.GetObject(_Monster.PoolKey);
        Monster.transform.position = worldPos;
    }



}
