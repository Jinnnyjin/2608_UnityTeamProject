using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance { get; private set; }

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
    }




}
