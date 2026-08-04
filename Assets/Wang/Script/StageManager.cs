using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using static MonsterStageInfo;

/*/////////////////////////////////////////
 *            MonsterStageInfo
 *기능 : 어떤 스테이지(시간 기반에 따라 생성되는 스테이지)에 따라 어떤 몬스터가 몇 초마다 생성되는지 정보를 담는 데이터
 */////////////////////////////////////////
[Serializable]
public class MonsterStageInfo
{
    [Serializable]
    public struct WeightedMonster
    {
        public PoolObject Prefab;
        [Range(0, 100)] public int Weight;
    }


    [Serializable]
    public class SpawnMonsterData
    {
        //해당 step에 맞게 몬스터 랜덤 스폰
        [SerializeField] private List<WeightedMonster> m_monsterPrefabs;
        public IReadOnlyList<WeightedMonster> MonsterPrefabs => m_monsterPrefabs;

        [Min(0.5f)][SerializeField] private float m_spawnStep = 0.5f;
        public float SpawnStep => m_spawnStep;
    }

    [SerializeField] private float m_stageStep = 300.0f;
    public float StageStep => m_stageStep;


    [SerializeField] private List<SpawnMonsterData> m_spawnMonsterDatas;
    
    public SpawnMonsterData SpawnDatas 
    { 
        get
        {
            int maxIdx = m_spawnMonsterDatas.Count;
            if (m_StageLevel >= maxIdx)
                return m_spawnMonsterDatas[maxIdx - 1];

            return m_spawnMonsterDatas[m_StageLevel];
        } 
    }

    public int m_StageLevel = 0;
    public int StageLevel => m_StageLevel;
    public bool IsLastStage => m_StageLevel >= m_spawnMonsterDatas.Count - 1;

    public int MaxStageLevel => m_spawnMonsterDatas.Count;  
}

public class StageManager : MonoBehaviour
{
    //public static StageManager m_Instance = null;

    [SerializeField] private ObjectSpawner m_spawner;

    //[SerializeField] private List<SOStage> m_listStage = new List<SOStage>();

    //Camera.main을 매번 호출 부담
    [SerializeField] private Camera m_mainCam;

    [SerializeField] private int m_LimitSpanwer = 100;
    private int m_SpawnCount = 0;
    [SerializeField] private MonsterStageInfo m_stageInfo;
    [SerializeField] private PoolObject m_StargeBoss;

    private Coroutine m_spawnCoroutine = null;
    private Coroutine m_stageCoroutine = null;

    private WaitForSeconds m_waitStageTime = null;   //다음 스테이지까지 대기 시간
    private WaitForSeconds m_waitSpawnTime = null;  //다음 스폰까지 대기 시간

    private bool m_bossSpawned = false; // 보스는 한 번만 로드
    //private void Awake()
    //{
    //    if (m_Instance != null)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //
    //    m_Instance = this;
    //
    //    DontDestroyOnLoad(gameObject);
    //}

    private void Start()
    {
        StartStage();
    }

    //private void OnDestroy()
    //{
    //    if (m_Instance == this)
    //        m_Instance = null;
    //}

    private void OnEnable()
    {
        Monster.onMonsterDied += MonsterDead;
    }

    private void OnDisable()
    {
        Monster.onMonsterDied -= MonsterDead;
    }

    private void StartStage()
    {
        m_stageCoroutine = StartCoroutine(MoveNextStageCoroutine());
        m_spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator MoveNextStageCoroutine()
    {
        //다음으로 넘길 스테이지 시간
        while(m_stageInfo.MaxStageLevel > m_stageInfo.m_StageLevel)
        {
            float fStageStepTime = m_stageInfo.StageStep;

            m_waitSpawnTime = new WaitForSeconds(m_stageInfo.SpawnDatas.SpawnStep);
            m_waitStageTime = new WaitForSeconds(fStageStepTime);

            yield return m_waitStageTime;

            //시간이 끝났다면 다음 스테이지로
            ++m_stageInfo.m_StageLevel;
            if (m_stageInfo.IsLastStage)
            {
                // 마지막 스테이지 도달 → 보스 한 번만
                if (!m_bossSpawned)
                {
                    m_bossSpawned = true;
                    SpawnBoss();
                }
            }
        }

        m_stageCoroutine = null;
    }
    private IEnumerator SpawnCoroutine()
    {
        while (m_stageInfo.MaxStageLevel > m_stageInfo.m_StageLevel)
        {
            if (m_SpawnCount >= m_LimitSpanwer)
            {
                yield return null;
                continue;
            }

            //선 예약
            ++m_SpawnCount;
            var spawnDatas = m_stageInfo.SpawnDatas;
            var prefabs = spawnDatas.MonsterPrefabs;
            int idx = UnityEngine.Random.Range(0, prefabs.Count);

            m_spawner.AddSpawnObject(spawnDatas.SpawnStep, GetWeightedMonster(spawnDatas), GetSpawnWrold());

            yield return m_waitSpawnTime;
        }

        m_spawnCoroutine = null;
    }


    private PoolObject GetWeightedMonster(SpawnMonsterData _spawnData)
    {
        var monsters = _spawnData.MonsterPrefabs;
        if (monsters.Count == 0) return null;   // 빈 리스트 방어

        int total = 0;
        for (int i = 0; i < monsters.Count; i++)
            total += monsters[i].Weight;

        if (total <= 0)                          // 가중치 전부 0/음수 방어
            return monsters[0].Prefab;

        int roll = UnityEngine.Random.Range(0, total);
        int acc = 0;
        for (int i = 0; i < monsters.Count; i++)
        {
            acc += monsters[i].Weight;
            if (roll < acc)
            {
                return monsters[i].Prefab;
            }
        }

        return monsters[monsters.Count - 1].Prefab;   // 부동소수 아닌 int라 사실상 안 탐
    }

    private Vector3 GetSpawnWrold()
    {
        float RandomX = UnityEngine.Random.Range(-0.1f, 1.1f);
        float RandomY = (UnityEngine.Random.value > 0.5f) ? 1.1f : -0.1f;
        Vector3 viewportPoint = new Vector3(RandomX, RandomY, m_mainCam.nearClipPlane);

        //Viewport 좌표를 World 좌표로 변환
        Vector3 worldPos = m_mainCam.ViewportToWorldPoint(viewportPoint);

        worldPos.z = 0f;
        return worldPos;
    }

    private void MonsterDead(Monster _monster)
    {
        --m_SpawnCount;
    }

    // 그 스테이지의 일반 몬스터를 다 처치했을 때 호출: 마지막 스테이지면 보스 등장, 아니면 다음 스테이지로

    private void SpawnBoss()
    {
        if (m_StargeBoss == null)
            return;
        m_spawner.AddSpawnObject(0.0f, m_StargeBoss, GetSpawnWrold());
    }
}
