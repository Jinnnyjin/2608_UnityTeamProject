using UnityEngine;
using TMPro; // 🌟 TextMeshPro를 제어하기 위해 반드시 필요한 치트키 네임스페이스
using UnityEngine.UI;

public class RewardChoiceButton : MonoBehaviour
{
    // [인스펙터 칸 생성] 내 자식에 있는 진짜 텍스트 글자들을 연결할 변수들
    [Header("[ 버튼 내부 컴포넌트들 ]")]
    [SerializeField] private TextMeshProUGUI m_titleText; // 제목 텍스트 칸
    [SerializeField] private TextMeshProUGUI m_descText;  // 설명 텍스트 칸
    [SerializeField] private Image m_iconImage;          // 아이콘 이미지 칸

    private int m_skillId;
    private SkillSelectUIManager m_uiManager;

    /// <summary>
    /// 🌟 [핵심] UI 매니저가 랜덤으로 뽑아낸 3개의 예시 데이터를 이 버튼에 찔러넣어 주는 함수
    /// </summary>
    public void InitButton(int _id, Sprite _icon, string _title, string _desc, SkillSelectUIManager _manager)
    {
        m_skillId = _id;
        m_uiManager = _manager;

        // ----------------------------------------------------------------------
        // 🛠️ [최종 픽스] 이 코드가 있어야 고정된 글자가 아니라 실시간 데이터로 갈아끼워집니다!
        // ----------------------------------------------------------------------
        if (m_titleText != null) m_titleText.text = _title; // 인스펙터에 적은 제목 주입
        if (m_descText != null) m_descText.text = _desc;   // 인스펙터에 적은 설명 주입
        if (m_iconImage != null)
        {
            if (_icon != null)
            {
                m_iconImage.sprite = _icon;
                m_iconImage.gameObject.SetActive(true);
            }
            else
            {
                m_iconImage.gameObject.SetActive(false); // 가짜 아이콘이 없으면 숨김
            }
        }
    }

    // 마우스로 이 보상 버튼을 클릭했을 때 실행되는 함수 (기존 연동용)
    public void OnClickButton()
    {
        if (m_uiManager != null)
        {
            m_uiManager.OnSkillSelected(m_skillId);
        }
    }
}
