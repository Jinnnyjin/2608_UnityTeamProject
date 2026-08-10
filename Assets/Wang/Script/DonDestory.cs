using UnityEngine;

public class DonDestory : MonoBehaviour
{
    private static DonDestory m_Instance = null;

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
}
