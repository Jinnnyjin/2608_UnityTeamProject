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

    [SerializeField] private PlayerMovement m_Player;
    public PlayerMovement Player => m_Player;

    

    //나중에 콜백으로 연결해두기
    public void EndGame()
    {

    }
}
