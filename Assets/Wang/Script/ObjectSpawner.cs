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
    private PriorityQueue<tSpawnData> m_PQObject;

    public int RemainObject => m_PQObject.Count;
    private struct tSpawnData
    {
        public float fSpawnTime;
        public GameObject refSpawnObject; //Prefab
        public Vector3 vPosition;

        public tSpawnData(float _fTime, GameObject _refSpawnObj, Vector3 _vPosition)
        {
            fSpawnTime = _fTime;
            refSpawnObject = _refSpawnObj;
            vPosition = _vPosition;
        }
    }
    private struct tSpawnTimeComparer : IComparer<tSpawnData>
    {
        public int Compare(tSpawnData x, tSpawnData y)
        {
            return x.fSpawnTime.CompareTo(y.fSpawnTime);
        }
    }

    private void Awake()
    {
        m_PQObject = new PriorityQueue<tSpawnData>(new tSpawnTimeComparer());
    }

    private void Update()
    {
        if (m_PQObject.Count <= 0)
            return;

        var tSpawn = m_PQObject.Peek();
        if (tSpawn.fSpawnTime - Time.time > 0.0f)
            return;
           

        m_PQObject.Dequeue();
        //SpawnObject(tSpawn.refSpawnObject, tSpawn.vPosition);
    }

   
    public void AddSpawnObject(float _fNextSpawnTime, GameObject _refPoolData, Vector3 _vPosition = default)//default를 쓰면 컴파일 타임 상수로
    {
        tSpawnData tData = new tSpawnData(Time.time + _fNextSpawnTime, _refPoolData, _vPosition);
        m_PQObject.Enqueue(tData);
    }

    //private void SpawnObject(GameObject _refSpawnObject, Vector3 _vPosition)
    //{
    //    GameObject refGameObject = ObjectPoolManager.m_Instance.GetObject(_refSpawnObject);
    //    if (refGameObject == null)
    //        return;
    //
    //    refGameObject.transform.position = _vPosition;
    //}
}
