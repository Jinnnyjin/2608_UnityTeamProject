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

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
    }

    [SerializeField] private Player m_Player;
    public Player Player => m_Player;

    private void Start()
    {
        //플레이어 Dead이벤트 구독
    }

    public void TakeDamage(float _damage)
    {
        m_currentHP -= _damage;
        if (m_currentHP <= 0)
        {
            m_currentHP = 0;
            //플레이어 사망 처리
        }
        m_hPSlider.value = m_currentHP / m_maxHP;
    }

    private void AddExp(float _amount)
    {
        
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
