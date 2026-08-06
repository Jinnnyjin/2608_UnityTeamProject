using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 🌟 이미지 컴포넌트 제어를 위해 추가


public class SkillSelectUIManager : MonoBehaviour
{
    public static SkillSelectUIManager Instance { get; private set; }

    [Header("[ UI 컴포넌트 연결 ]")]
    [SerializeField] private GameObject m_rewardPanelObject;
    [SerializeField] private RewardChoiceButton[] m_choiceButtons;

    [Header("[ 🌟 추가: 메인 화면의 스킬 패널 슬롯들 ]")]
    // 📌 여기에 메인 화면에 상시 떠 있는 스킬 슬롯 이미지들을 연결할 겁니다! (예: 3~4개 개수만큼)
    [SerializeField] private Image[] m_mainSkillSlots;
    private int m_equippedSkillCount = 0; // 현재 장착된 스킬 개수를 세는 카운터 변수


    [SerializeField] private List<SkillData> m_preLoadSkill;

    private List<int> randomIndices = new List<int>();

    private List<SkillData> m_selectSkillData = new List<SkillData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenLevelUpPanel()
    {
        Time.timeScale = 0f;
        m_rewardPanelObject.SetActive(true);
        SetButtonChoices(3);
    }

    private void SetButtonChoices(int _choiceCount)
    {
        //GameManager.Player.GetSkillBySkillData(null);
        if (m_choiceButtons == null || m_choiceButtons.Length == 0) return;

        randomIndices.Clear();

        _choiceCount = _choiceCount < m_preLoadSkill.Count ? _choiceCount : m_preLoadSkill.Count;

        while (randomIndices.Count < _choiceCount)
        {
            int randIndex = Random.Range(0, m_preLoadSkill.Count);
            if (!randomIndices.Contains(randIndex))
            {
                randomIndices.Add(randIndex);
            }
        }


        ISkillOwner skillOwner = GameManager.m_Instance.Player;
        for (int i = 0; i< randomIndices.Count; ++i)
        {
            int randomIdx = randomIndices[i];

            m_choiceButtons[i].gameObject.SetActive(true);

            SkillData selectSkill = m_preLoadSkill[randomIdx];
            Skill mySkill = skillOwner.GetSkillBySkillData(selectSkill);

            int skillLevel = 0;
            if (mySkill != null)
                skillLevel = mySkill.SkillLevel;
            m_choiceButtons[i].InitButton(m_preLoadSkill[randomIdx], skillLevel);

            if (skillLevel >= Skill.MaxSkillLevel)
                m_preLoadSkill.Remove(selectSkill);
        }
    }

    /// <summary>
    /// 🌟 [최종 연계 핵심] 보상 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    /// 
    public void OnSkillSelected(SkillData _selectSkill)
    {
        Time.timeScale = 1.0f;
        m_rewardPanelObject.SetActive(false);

        SkillData preSKill = FindData(_selectSkill);
        if (preSKill != null)
            return;

        for (int i = 0; i< m_mainSkillSlots.Length; ++i)
        {
            if (m_mainSkillSlots[i].sprite == null)
            {
                m_mainSkillSlots[i].gameObject.SetActive(true);
                m_mainSkillSlots[i].sprite = _selectSkill.Icon;

                m_selectSkillData.Add(_selectSkill);
                break;
            }
        }
    }

    private SkillData FindData(SkillData _selectSkill)
    {
        for (int i = 0; i<m_selectSkillData.Count; ++i)
        {
            if (m_selectSkillData[i] == _selectSkill)
                return _selectSkill;
        }
        return null;
    }
  
}
