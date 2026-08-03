using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_rewardPanelObject;
    [SerializeField] private RewardChoiceButton[] m_choiceButtons;  // 통일된 새 변수명
    [SerializeField] private Sprite[] m_dummyIcons;

    public void OpenLevelUpPanel()
    {
        Time.timeScale = 0f;
        m_rewardPanelObject.SetActive(true);

        SetButtonChoices(3);
    }

    public void OpenBoxPanel()
    {
        Time.timeScale = 0f;
        m_rewardPanelObject.SetActive(true);

        int randomSlotCount = Random.Range(0, 100) < 10 ? 5 : 3;

        SetButtonChoices(randomSlotCount);
    }

    private void SetButtonChoices(int _choiceCount)
    {
        // [완벽 해결] 반복문 자체의 최대 한계를 인스펙터에 등록된 버튼 개수(m_choiceButtons.Length)로 꽉 잠가버립니다!
        for (int i = 0; i < m_choiceButtons.Length; i++)
        {
            // 🔗 안전장치를 추가하여, 요청된 개수와 실제 보관함 개수 둘 다 만족할 때만 버튼을 켭니다.
            if (i < _choiceCount && i < m_choiceButtons.Length)
            {
                m_choiceButtons[i].gameObject.SetActive(true);

                // 임시 데이터 주입 코드 (기존 코드 그대로 유지)
                int randomSkillId = Random.Range(100, 200);
                Sprite randomIcon = m_dummyIcons[Random.Range(0, m_dummyIcons.Length)];
                string dummyName = $"스킬 보상 {i + 1}";
                string dummyDesc = $"공격력을 강화합니다. (ID: {randomSkillId})";

                // 아래 식은 버튼 이미지, 타이틀, 설명 기입식 입니다. 가동 준비 중입니다.
                m_choiceButtons[i].InitButton(randomSkillId, randomIcon, dummyName, dummyDesc, this);
            }
            else
            {
                // 범위를 벗어나는 나머지 칸은 안전하게 꺼줍니다.
                m_choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnSkillSelected(int _selectedSkillId)
    {
        Debug.Log($"보상을 선택했습니다: {_selectedSkillId}");
        m_rewardPanelObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // [우회 테스트 코드] 신형 인풋 시스템에서도 무조건 마우스 왼쪽 클릭을 감지합니다.
        if (Input.GetMouseButtonDown(0))
        {
            // ⚠️ 단, 보상 창이 이미 켜진 상태에서 또 클릭하면 중복 실행되므로, 
            // 평소에 꺼져 있을 때(false)만 마우스 클릭으로 열리도록 안전장치를 걸어줍니다.
            if (m_rewardPanelObject.activeSelf == false)
            {
                Debug.Log("테스트: 마우스 왼쪽 클릭 감지! 보상 창을 엽니다.");
                OpenBoxPanel();
            }
        }
    }

}
