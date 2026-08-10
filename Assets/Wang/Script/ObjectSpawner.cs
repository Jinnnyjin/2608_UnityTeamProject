using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

/*///////////////////////////////////////////
                ObjectSpawner
기능 : (시간, 오브젝트, 위치) 스폰 예약을 우선순위 큐(PQ)로 관리하는 범용 스폰 엔진.
       매 프레임 큐 최상단(가장 이른 예약)만 확인해서, 예약 시각이 지나면 그 자리에서
       Dequeue 후 ObjectPool에서 꺼내 배치한다. 
 *///////////////////////////////////////////
public class ObjectSpawner : MonoBehaviour
{
    private PriorityQueue<SpawnData> m_PQObject;

    public int RemainObject => m_PQObject.Count;

    public event Action<GameObject> OnSpawn;
    private struct SpawnData
    {
        public float SpawnTime;
        public PoolObject SpawnObject; //Prefab
        public Vector3 Position;

        public SpawnData(float _Time, PoolObject _SpawnObj, Vector3 _Position)
        {
            SpawnTime = _Time;
            SpawnObject = _SpawnObj;
            Position = _Position;
        }
    }
    private struct SpawnTimeComparer : IComparer<SpawnData>
    {
        public int Compare(SpawnData x, SpawnData y)
        {
            return x.SpawnTime.CompareTo(y.SpawnTime);
        }
    }

    private void Awake()
    {
        m_PQObject = new PriorityQueue<SpawnData>(new SpawnTimeComparer());
    }

    private void Update()
    {
        while(m_PQObject.Count > 0)
        {
            var SpawnOption = m_PQObject.Peek();
            if (SpawnOption.SpawnTime - Time.time > 0.0f)
                return;

            m_PQObject.Dequeue();
            SpawnObject(SpawnOption.SpawnObject, SpawnOption.Position);
        }
    }

   
    public void AddSpawnObject(float _fNextSpawnTime, PoolObject _PoolData, Vector3 _vPosition = default)//default를 쓰면 컴파일 타임 상수로
    {
        SpawnData spawnData = new SpawnData(Time.time + _fNextSpawnTime, _PoolData, _vPosition);
        m_PQObject.Enqueue(spawnData);
    }

    private void SpawnObject(PoolObject _SpawnObject, Vector3 _vPosition)
    {
        GameObject refGameObject = ObjectPoolManager.m_Instance.GetObject(_SpawnObject.gameObject);
        if (refGameObject == null)
            return;
    
        refGameObject.transform.position = _vPosition;
        OnSpawn?.Invoke(refGameObject);
    }
}
