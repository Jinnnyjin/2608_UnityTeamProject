using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;


/*///////////////////////////////////////////
               ObjectPoolManager
기능 : 오브젝트를 미리 로드해두고 필요할 때 꺼내어 쓰면 반납할 수 있게 하는 클래스
 *///////////////////////////////////////////

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager m_Instance = null;
    private Dictionary<GameObject, Queue<GameObject>> m_hashPool = new Dictionary<GameObject, Queue<GameObject>>();
    //Prefab을 Key로

    private void Awake()
    {
        m_Instance = this;

        //if (m_Instance != null)
        //{
        //    Destroy(this);
        //    return;
        //}
        //
        //m_Instance = this;
        //DontDestroyOnLoad(this);
    }

  

    public GameObject GetObject(GameObject _gameObject)
    {
        //오리진 (에디터) 프리팹을 꼭 가지고 있어야 합니다
        PoolObject poolObj = _gameObject.GetComponent<PoolObject>();
        if (poolObj == null || poolObj.OriginPrefab == null)
        {
            Debug.Log("풀 오브젝트 컴포넌트나 키 값이 설정되지 않음");
            Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
            return null;
        }

        if (m_hashPool.TryGetValue(poolObj.OriginPrefab, out var queValue) == false)
        {
            queValue = new Queue<GameObject>();
            m_hashPool.Add(poolObj.OriginPrefab, queValue);
        }

        GameObject retValue = null;
        if (queValue.TryPeek(out retValue) == false)
            retValue = GameObject.Instantiate(_gameObject);
        else
            queValue.Dequeue();

        //Instantiate 시 자기 참조 필드가 클론 쪽으로 리매핑되므로 원본을 다시 못박아줍니다
        PoolObject retPoolObj = retValue.GetComponent<PoolObject>();
        retPoolObj.SetOriginKey(poolObj.OriginPrefab);

        retValue.transform.SetParent(null);
        retPoolObj.Pop();
        retValue.gameObject.SetActive(true);
        return retValue;
    }

    
    public void PushObject(GameObject _refGameObj)
    {
        PoolObject refPoolObj = _refGameObj.GetComponent<PoolObject>();
        if (refPoolObj == null)
        {
            Debug.Log("오브젝트에 PoolObject 컴포넌트가 없음");
            Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
            return;
        }

        if (refPoolObj.PushFlag > 0)
            return;

        if (refPoolObj.OriginPrefab == null)
        {
            Debug.Log("PoolObject의 OriginPrefab이 설정되지 않음 (GetObject를 거치지 않고 생성된 오브젝트)");
            Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
            return;
        }

        if (m_hashPool.TryGetValue(refPoolObj.OriginPrefab, out var queValue) == false)
        {
            queValue = new Queue<GameObject>();
            m_hashPool.Add(refPoolObj.OriginPrefab, queValue);
        }

        refPoolObj.Push();
        _refGameObj.transform.SetParent(transform);
        _refGameObj.gameObject.SetActive(false);

        queValue.Enqueue(_refGameObj);
    }

    public int GetObjectCount(GameObject _gaemObject)
    {
        if (m_hashPool.TryGetValue(_gaemObject, out var queValue) == false)
            return -1;

        return queValue.Count;
    }
}