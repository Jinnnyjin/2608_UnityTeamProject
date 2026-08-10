using System;
using System.Collections;
using Unity.VisualScripting;
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

    //[SerializeField] private GameObject m_refPre;
    private int m_iCurrentLevel = 0;

    [SerializeField] private Player m_player;
    public Player Player => m_player;

    [SerializeField] GameObject m_GameOverView;
    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;


        Monster.OnMonsterDied += MonsterDead;
    }

    private void Start()
    {
        m_expSlider.value = 0.0f;

        var playerSkills = Player.SkillList;
        for(int i = 0; i<playerSkills.Count; ++i)
            SkillSelectUIManager.Instance.OnSkillSelected(playerSkills[i].Data); 

        TakeDamage(-1);
    }
    //private void OnDestroy()
    //{
    //    Monster.onMonsterDied += MonsterDead;
    //
    //}
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //AddExp(10.0f);
        //    ObjectPoolManager.m_Instance.GetObject(m_refPre);
        //}
    }
   

    public void TakeDamage(float _damage)
    {
        float currentHP = m_player.CurrentHp;
        float maxHP = m_player.Hp;
        m_hPSlider.value = currentHP / maxHP;
    }

    private void MonsterDead(Monster _monster)
    {
        if (_monster == null)
            return;

        AddExp(_monster.BaseInfo.ExpReward);
    }

    private void AddExp(float _amount)
    {
        m_currentExp += _amount;
        if(m_currentExp >= m_maxExp)
        {
            m_currentExp -= m_maxExp;
            m_maxExp += (m_maxExp * m_weightExp);
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
        Time.timeScale = 0.0f;
        m_GameOverView.SetActive(true);
    }

    public void ReturnLoby()
    {
        Time.timeScale = 1.0f;
        LoadingSceneController.Instance.TriggerTitle();
    }
}
