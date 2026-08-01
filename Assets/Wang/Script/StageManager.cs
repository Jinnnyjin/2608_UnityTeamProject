using System.Collections.Generic;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager m_Instance = null;

    [SerializeField] private ObjectSpawner m_Spawner;

    private int m_CurrentStageIdx = 0;
    private int m_RemainMonsterCount = 0;
    private bool m_BossSpawned = false;

    //[SerializeField] private List<SOStage> m_listStage = new List<SOStage>();


    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (m_Instance == this)
            m_Instance = null;
    }

    private void OnEnable()
    {
        //Monster.OnMonsterDied += MonsterDead;
    }

    private void OnDisable()
    {
        //Monster.OnMonsterDied -= MonsterDead;
    }

    //public void StartStage(int _StageIdx)
    //{
    //    if (_StageIdx >= m_listStage.Count)
    //    {
    //        Debug.Log("던전 클리어 : DungeonManager");
    //        GameSceneManager.m_Instance.LoadFirstScene();
    //        return;
    //    }

    //    m_CurrentStageIdx = _StageIdx;
    //    m_BossSpawned = false;

    //    SOStage refStage = m_listStage[_StageIdx];
    //    m_RemainMonsterCount = refStage.ListSpawnEntry.Count;

    //    for (int i = 0; i < refStage.ListSpawnEntry.Count; ++i)
    //    {
    //        var tEntry = refStage.ListSpawnEntry[i];
    //        m_Spawner.AddSpawnObject(tEntry.fSpawnTime, tEntry.MonsterPrefab, tEntry.vPosition);
    //    }
    //}

    public void StartStage()
    {
        float RandomX = UnityEngine.Random.Range(0f, 1f);
        float RandomY = (float)UnityEngine.Random.Range(-1, 2);
        Vector3 ViewportPoint = new Vector3(RandomX, RandomY, Camera.main.nearClipPlane);

        //Viewport 좌표를 World 좌표로 변환
        Vector3 WorldPos = Camera.main.ViewportToWorldPoint(ViewportPoint);
        

        //PoolObject refMon = ObjectPoolManager.m_Instance.GetObject(_refMon, WorldPos, Quaternion.identity);
        //int idx = UnityEngine.Random.Range(0, m_listMonsterInfo.Count);
    }

    private void MonsterDead(int _iExpReward)
    {
        if (m_BossSpawned == true)
        {
            // 보스는 마지막 스테이지에서만 등장하므로, 보스가 죽었다는 건 곧 던전 클리어
            //StartStage(m_CurrentStageIdx + 1);
            return;
        }

        --m_RemainMonsterCount;
        if (m_RemainMonsterCount <= 0 && m_Spawner.RemainObject <= 0)
            SpawnBoss();
    }

    // 그 스테이지의 일반 몬스터를 다 처치했을 때 호출: 마지막 스테이지면 보스 등장, 아니면 다음 스테이지로

    private void SpawnBoss()
    {
        m_BossSpawned = true;

        //SOStage refStage = m_listStage[m_CurrentStageIdx];
        //m_Spawner.AddSpawnObject(0.0f, refStage.BossPrefab, refStage.BossSpawnPosition);
    }
}
