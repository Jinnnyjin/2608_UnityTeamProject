using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 🌟 이미지 컴포넌트 제어를 위해 추가


[Serializable]
public class SkillSlot
{
    public SkillType Type;
    public Image Icon;
}

public class SkillSelectUIManager : MonoBehaviour
{
    public static SkillSelectUIManager Instance { get; private set; }

    [Header("[ UI 컴포넌트 연결 ]")]
    [SerializeField] private GameObject m_rewardPanelObject;
    [SerializeField] private RewardChoiceButton[] m_choiceButtons;

    [Header("[ 🌟 추가: 메인 화면의 스킬 패널 슬롯들 ]")]
    [SerializeField] private SkillSlot[] m_mainSkillSlots;
    private int m_equippedSkillCount = 0; // 현재 장착된 스킬 개수를 세는 카운터 변수


    [SerializeField] private List<SkillData> m_preLoadSkill;

    private List<int> randomIndices = new List<int>();

    private List<SkillData> m_selectSkillData = new List<SkillData>();

    private void Awake()
    {
        Instance = this;
    }

    public void OpenLevelUpPanel()
    {
        Time.timeScale = 0f;
        m_rewardPanelObject.SetActive(true);
        SetButtonChoices(3);
    }

    private void SetButtonChoices(int _choiceCount)
    {
        if (m_choiceButtons == null || m_choiceButtons.Length == 0) return;

        ISkillOwner skillOwner = GameManager.m_Instance.Player;
        List<SkillData> candidatePool = BuildCandidatePool(skillOwner);

        randomIndices.Clear();

        _choiceCount = _choiceCount < candidatePool.Count ? _choiceCount : candidatePool.Count;

        while (randomIndices.Count < _choiceCount)
        {
            int randIndex = UnityEngine.Random.Range(0, candidatePool.Count);
            if (!randomIndices.Contains(randIndex))
            {
                randomIndices.Add(randIndex);
            }
        }

        for (int i = 0; i< randomIndices.Count; ++i)
        {
            int randomIdx = randomIndices[i];

            m_choiceButtons[i].gameObject.SetActive(true);

            SkillData selectSkill = candidatePool[randomIdx];
            Skill mySkill = skillOwner.GetSkillBySkillData(selectSkill);

            int skillLevel = mySkill != null ? mySkill.SkillLevel : 0;
            m_choiceButtons[i].InitButton(selectSkill, skillLevel);
        }
    }

    //맥스 레벨이거나, 타입별 슬롯이 꽉 찼는데 아직 안 가진 스킬이면 후보에서 제외
    private List<SkillData> BuildCandidatePool(ISkillOwner _skillOwner)
    {
        List<SkillData> pool = new List<SkillData>();
        for (int i = 0; i < m_preLoadSkill.Count; ++i)
        {
            SkillData data = m_preLoadSkill[i];
            Skill curSkill = _skillOwner.GetSkillBySkillData(data);

            if (curSkill != null && curSkill.SkillLevel >= Skill.MaxSkillLevel)
                continue;

            if (curSkill == null && IsTypeSlotFull(data.SkillType))
                continue;

            pool.Add(data);
        }
        return pool;
    }

    private bool IsTypeSlotFull(SkillType _type)
    {
        int total = 0;
        int filled = 0;
        for (int i = 0; i < m_mainSkillSlots.Length; ++i)
        {
            if (m_mainSkillSlots[i].Type != _type) 
                continue;

            ++total;
            if (m_mainSkillSlots[i].Icon.sprite != null)
                ++filled;
        }
        return total > 0 && filled >= total;
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

        SkillType type = _selectSkill.SkillType;

        for (int i = 0; i< m_mainSkillSlots.Length; ++i)
        {
            SkillType slotType = m_mainSkillSlots[i].Type;
            Image iconImage = m_mainSkillSlots[i].Icon;

            if (type == slotType && iconImage.sprite == null)
            {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = _selectSkill.Icon;

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
