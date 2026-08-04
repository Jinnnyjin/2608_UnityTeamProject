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
        // [수정] m_uiButtons를 전부 m_choiceButtons로 변경 완료했습니다.
        for (int i = 0; i < m_choiceButtons.Length; i++)
        {
            if (i < _choiceCount)
            {
                m_choiceButtons[i].gameObject.SetActive(true);

                // 임시로 무작위 스프라이트와 ID를 주입하는 로직 예시 (에러 방지용)
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
    }
}
