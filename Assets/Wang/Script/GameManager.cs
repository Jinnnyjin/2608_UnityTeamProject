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
    [SerializeField] private SkillSelectUIManager m_skillSelectUI;
    private float m_weightExp = 0.5f; 

    private int m_iCurrentLevel = 0;

    [SerializeField] private Player m_player;
    public Player Player => m_player;
    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(10.0f);
        }
    }
    private void Start()
    {
        UpdateSlider(-1);
    }

    public void UpdateSlider(float _damage)
    {
        float currentHP = m_player.CurrentHp;
        float maxHP = m_player.Hp;
        m_hPSlider.value = currentHP / maxHP;
    }

    private void AddExp(float _amount)
    {
        m_currentExp += _amount;
        if(m_currentExp >= m_maxExp)
        {
            m_currentExp -= m_maxExp;
            m_maxExp *= (m_maxExp * m_weightExp);
            LevelUp();
        }

        m_expSlider.value = m_currentExp / m_maxExp;
    }

    // ExSlider의 채우기 연출이 실제로 Max에 도달했을 때 Player가 호출
    private void LevelUp()
    {
        m_iCurrentLevel += 1;
        m_skillSelectUI.OpenLevelUpPanel();

    }

    public void EndGame()
    {

    }
}
