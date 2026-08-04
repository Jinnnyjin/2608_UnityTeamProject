using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUIManager : MonoBehaviour
{
    // [실무 팁] 다른 스크립트(GameManager 등)에서 이 UI 매니저를 쉽게 호출할 수 있도록 싱글톤 인스턴스 개방
    public static SkillSelectUIManager Instance { get; private set; }

    [SerializeField] private GameObject m_rewardPanelObject;
    [SerializeField] private RewardChoiceButton[] m_choiceButtons;
    [SerializeField] private Sprite[] m_dummyIcons;

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        //else Destroy(gameObject);
    }

    // --------------------------------------------------
    // [ 정식 공용(public) 함수: 나중에 GameManager가 호출할 구역 ]
    // --------------------------------------------------

    /// <summary>
    /// 플레이어 레벨업 시 호출 (항상 3개 슬롯)
    /// </summary>
    public void OpenLevelUpPanel()
    {
        Time.timeScale = 0f;
        m_rewardPanelObject.SetActive(true);
        SetButtonChoices(3);
    }

    /// <summary>
    /// 보물상자 획득 시 호출 (10% 확률로 5개, 기본 3개 슬롯)
    /// </summary>
    public void OpenBoxPanel()
    {
        Time.timeScale = 0f;
        m_rewardPanelObject.SetActive(true);

        int randomSlotCount = Random.Range(0, 100) < 10 ? 5 : 3;
        SetButtonChoices(randomSlotCount);
    }

    private void SetButtonChoices(int _choiceCount)
    {
        for (int i = 0; i < m_choiceButtons.Length; i++)
        {
            if (i < _choiceCount && i < m_choiceButtons.Length)
            {
                m_choiceButtons[i].gameObject.SetActive(true);

                int randomSkillId = Random.Range(100, 200);
                Sprite randomIcon = m_dummyIcons[Random.Range(0, m_dummyIcons.Length)];
                string dummyName = $"스킬 보상 {i + 1}";
                string dummyDesc = $"공격력을 강화합니다. (ID: {randomSkillId})";

                m_choiceButtons[i].InitButton(randomSkillId, randomIcon, dummyName, dummyDesc, this);
            }
            else
            {
                m_choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnSkillSelected(int _selectedSkillId)
    {
        Debug.Log($"보상을 선택했습니다: {_selectedSkillId}");
        m_rewardPanelObject.SetActive(false);
        Time.timeScale = 1f;

        // 🔗 [추가 예정인 구역]
        // 나중에 GameManager가 완성되면 여기에 대략 이런 식으로 알려주면 됩니다:
        // GameManager.Instance.ApplySkill(_selectedSkillId);
    }

    // --------------------------------------------------
    // [ 질문자님만의 비밀 임시 테스트 구역 ]
    // --------------------------------------------------
    private void Update()
    {
        // ⚠️ 주의: 마우스 왼쪽 클릭(0) 시 보물상자 패널(확률 슬롯) 테스트
        if (Input.GetMouseButtonDown(0))
        {
            if (m_rewardPanelObject.activeSelf == false)
            {
                Debug.Log("[임시 테스트] 마우스 왼쪽 클릭 감지 -> 보물상자 패널 오픈!");
                OpenBoxPanel();
            }
        }

        // ⚠️ 추가 팁: 키보드 L키를 누르면 레벨업 패널(무조건 3개 슬롯) 테스트도 가능하게 배치
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (m_rewardPanelObject.activeSelf == false)
            {
                Debug.Log("[임시 테스트] L 키 감지 -> 레벨업 패널 오픈!");
                OpenLevelUpPanel();
            }
        }
    }
}
