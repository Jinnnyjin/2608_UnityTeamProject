using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 🌟 이미지 컴포넌트 제어를 위해 추가

[System.Serializable]
public class MyTempSkillData
{
    public int skillId;
    public Sprite icon;
    public string title;
    [TextArea(2, 5)] public string desc;
}

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

    [Header("[ 예시 스킬 데이터 보관함 (6개) ]")]
    [SerializeField] private List<MyTempSkillData> m_previewSkillList = new List<MyTempSkillData>();

    [SerializeField] private List<SkillData> m_preLoadSkill;
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
        if (m_choiceButtons == null || m_choiceButtons.Length == 0) return;

        List<int> randomIndices = new List<int>();
        if (m_previewSkillList.Count >= _choiceCount)
        {
            while (randomIndices.Count < _choiceCount)
            {
                int randIndex = Random.Range(0, m_previewSkillList.Count);
                if (!randomIndices.Contains(randIndex))
                {
                    randomIndices.Add(randIndex);
                }
            }
        }

        if (m_preLoadSkill.Count >= _choiceCount)
        {
            while (randomIndices.Count < _choiceCount)
            {
                int randIndex = Random.Range(0, m_preLoadSkill.Count);
                if (!randomIndices.Contains(randIndex))
                {
                    randomIndices.Add(randIndex);
                }
            }
        }

        for(int i = 0; i< randomIndices.Count; ++i)
        {
            int randomIdx = randomIndices[i];

            m_choiceButtons[i].gameObject.SetActive(true);
            m_choiceButtons[i].InitButton(m_preLoadSkill[randomIdx]);
        }

        //for (int i = 0; i < m_choiceButtons.Length; i++)
        //{
        //    if (i < _choiceCount && i < m_choiceButtons.Length)
        //    {
        //        m_choiceButtons[i].gameObject.SetActive(true);
        //
        //        //int skillId = Random.Range(100, 200);
        //        //Sprite icon = null;
        //        //string title = $"임시 스킬 {i + 1}";
        //        //string desc = "공격력을 강화합니다.";
        //
        //        if (randomIndices.Count > i)
        //        {
        //            //MyTempSkillData chosenData = m_previewSkillList[randomIndices[i]];
        //            //skillId = chosenData.skillId;
        //            //icon = chosenData.icon;
        //            //title = chosenData.title;
        //            //desc = chosenData.desc;
        //            //Debug.Log(desc);
        //        }
        //
        //        m_choiceButtons[i].InitButton(skillId, icon, title, desc, this);
        //    }
        //    else
        //    {
        //        m_choiceButtons[i].gameObject.SetActive(false);
        //    }
        //}
    }

    /// <summary>
    /// 🌟 [최종 연계 핵심] 보상 버튼을 클릭했을 때 실행되는 함수
    /// </summary>
    /// 
    public void OnSkillSelected(SkillData _selectSkill)
    {
        for(int i = 0; i< m_mainSkillSlots.Length; ++i)
        {
            if (m_mainSkillSlots[i].sprite == null)
                m_mainSkillSlots[i].sprite = _selectSkill.Icon;
        }
    }
    public void OnSkillSelected(int _selectedSkillId)
    {
        Debug.Log($"보상을 선택했습니다 ID: {_selectedSkillId}");

        // 1. 내가 선택한 스킬 ID에 맞는 진짜 아이콘(Sprite)을 6개 보관함에서 역추적해 찾아옵니다.
        Sprite selectedIcon = null;
        foreach (var skill in m_previewSkillList)
        {
            if (skill.skillId == _selectedSkillId)
            {
                selectedIcon = skill.icon;
                break;
            }
        }

        // 2. 화면 구석에 있는 스킬 패널(슬롯)의 빈자리를 찾아 아이콘을 착! 넣어줍니다.
        if (m_mainSkillSlots != null && m_equippedSkillCount < m_mainSkillSlots.Length)
        {
            if (selectedIcon != null)
            {
                // 빈 슬롯의 이미지를 내가 고른 스킬 아이콘으로 변경!
                m_mainSkillSlots[m_equippedSkillCount].sprite = selectedIcon;
                // 투명하게 꺼져있던 슬롯 이미지를 불투명하게 켜기
                m_mainSkillSlots[m_equippedSkillCount].color = Color.white;

                m_equippedSkillCount++; // 장착 개수 증가 (다음 자리에 넣기 위해)
                Debug.Log($"[스킬 패널 연동] 메인 화면 스킬 슬롯 {m_equippedSkillCount}번에 아이콘 장착 완료!");
            }
        }

        // 3. 스킬 담당자 직통 호출 (방어벽 유지)
        Player playerComponent = FindFirstObjectByType<Player>();
        if (playerComponent != null)
        {
            try { playerComponent.RegisterSkill(new Skill()); }
            catch (System.NotImplementedException) { }
        }

        m_rewardPanelObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (m_rewardPanelObject != null && !m_rewardPanelObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                OpenLevelUpPanel();
            }
        }
    }
}
