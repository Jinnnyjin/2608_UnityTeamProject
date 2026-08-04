using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance { get; private set; }

    [SerializeField] private Slider m_expSlider;
    [SerializeField] private Slider m_hPSlider;

    [SerializeField] private float m_currentExp = 0.0f;
    [SerializeField] private float m_maxExp = 0.0f;
    private float m_currentHP = 0.0f;
    [SerializeField] private float m_maxHP = 0.0f;
    private int m_iCurrentLevel = 0;

    //이건 플레이어 쪽에서 만들어주셨음 좋겠어요
    public event Action<float> m_onDamaged; //현제 내가 받은 데이미 양
    public event Action<float> m_onRecvExp; //현제 내가 받은 경험치 양

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

    private void AddExp(float _amount)
    {
        if (_amount <= 0)
            return;

        m_currentExp += _amount;
        if(m_currentExp <= m_maxExp)
        {
            m_currentExp -= m_maxExp;
            LevelUp();
        }
    }

    // ExSlider의 채우기 연출이 실제로 Max에 도달했을 때 Player가 호출
    private void LevelUp()
    {
        m_iCurrentLevel += 1;

        //OnLevelUp?.Invoke(m_iCurrentLevel);
        //m_refCardCreator?.ShowChoices();
    }

    private void AddHP()
    {

    }

    //나중에 콜백으로 연결해두기
    public void EndGame()
    {

    }
}
