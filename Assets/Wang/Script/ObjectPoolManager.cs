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
        if (m_Instance != null)
            Destroy(this);


        m_Instance = this;
        DontDestroyOnLoad(this);
    }

  

    public GameObject GetObject(GameObject _gaemObject)
    {
        if (m_hashPool.TryGetValue(_gaemObject, out var queValue) == false)
        {
            queValue = new Queue<GameObject>();
            m_hashPool.Add(_gaemObject, queValue);
        }

        GameObject retValue = null;
        if (queValue.TryPeek(out retValue) == false)
        {
            retValue = GameObject.Instantiate(_gaemObject);
            retValue.GetComponent<PoolObject>().SetOriginKey(_gaemObject);
        }
        else
            queValue.Dequeue();

        //처음 Push를 할 때
        PoolObject PoolObject = retValue.GetComponent<PoolObject>();
        if (PoolObject == null)
        {
            Debug.Log("오브젝트 풀에 이상한 오류 있음");
            return null;
        }

        retValue.transform.SetParent(null);
        PoolObject.Pop();
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