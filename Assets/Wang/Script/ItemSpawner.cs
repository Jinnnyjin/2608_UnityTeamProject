using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemWorldObject m_itmePrefab;
    [SerializeField] private List<SOItemData> m_ItemDatas;

    private List<ItemWorldObject> m_spawnItems = new List<ItemWorldObject>();
    public IReadOnlyList<ItemWorldObject> SpawnItme => m_spawnItems;


    [Header("Test")]
    [SerializeField] private ObjectSpawner m_spawner; 
    [SerializeField] private Camera m_mainCam;

    [SerializeField] private int m_testSpawnCount = 50;
    [SerializeField] private float m_testSpawnDelay = 1f;

    private void Start()
    {
        //만약 없거나, 컴포넌트가 없다면 강제로 터지게
        PoolObject itemPoolObject = m_itmePrefab.GetComponent<PoolObject>();

        //테스트용 : 뷰포트 내 랜덤 위치에 아이템 스폰을 미리 예약
        for (int i = 0; i < m_testSpawnCount; ++i)
        {
            m_spawner.AddSpawnObject(m_testSpawnDelay, itemPoolObject, GetRandomPositionInViewport());
        }
    }

    private Vector3 GetRandomPositionInViewport()
    {
        float randomX = UnityEngine.Random.Range(0f, 1f);
        float randomY = UnityEngine.Random.Range(0f, 1f);
        Vector3 viewportPoint = new Vector3(randomX, randomY, m_mainCam.nearClipPlane);

        //Viewport 좌표를 World 좌표로 변환
        Vector3 worldPos = m_mainCam.ViewportToWorldPoint(viewportPoint);

        worldPos.z = 0f;
        return worldPos;
    }

    private void OnEnable()
    {
        Monster.OnMonsterDied += SpawnItemObject;
    }


    private void OnDisable()
    {
        Monster.OnMonsterDied -= SpawnItemObject;
    }

    private void SpawnItemObject(Monster _monster)
    {
        Vector3 vMonsterPos = _monster.transform.position;

        int itemIdx = 0;
        SOItemData itemData = m_ItemDatas[itemIdx];
        /*
        list에 있는 아이템 오브젝트 중 하나를 생성 (현재는 아이템이 하나기 때문에 Idx = 0으로 고정)
         */
        
        GameObject spawnObj = ObjectPoolManager.m_Instance.GetObject(m_itmePrefab.gameObject);
        ItemWorldObject itemCom = spawnObj.GetComponent<ItemWorldObject>();
        itemCom.Init(itemData, _monster.BaseInfo.ExpReward, OnExecute);
        spawnObj.transform.position = vMonsterPos;

        m_spawnItems.Add(itemCom);
    }

    private void OnExecute(ItemWorldObject _executeItem)
    {
        int idx = -1;
        for (int i = 0; i < m_spawnItems.Count; ++i)
        {
            if (m_spawnItems[i] == _executeItem)
                idx = i;
        }
        if (idx == -1)
            return;


        //Swap and remove
        int last = m_spawnItems.Count - 1;
        ItemWorldObject temItem = m_spawnItems[idx];
        m_spawnItems[idx] = m_spawnItems[last];
        m_spawnItems[last] = temItem;
        m_spawnItems.RemoveAt(last);

    }
}
