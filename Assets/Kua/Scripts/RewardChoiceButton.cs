using UnityEngine;
using UnityEngine.UI;
using TMPro; // 인스펙터에서 텍스트 컴포넌트 제어를 위해 네임스페이스 추가

public class RewardChoiceButton : MonoBehaviour
{
    // private 멤버 변수는 m_ 접두사 + 카멜 케이스 규칙 준수
    [SerializeField] private Image m_skillIconImage;
    [SerializeField] private TextMeshProUGUI m_skillNameText;
    [SerializeField] private TextMeshProUGUI m_skillDescText;

    private int m_assignedSkillId;
    private SkillSelectUIManager m_panelManager;

    // 총관리자(매니저)가 버튼을 활성화할 때 스킬 정보를 주입해주는 함수
    // 매개 변수는 무조건 _ 접두사 사용 규칙 반영 (_skillId, _icon, _name, _desc, _manager)
    public void InitButton(int _skillId, Sprite _icon, string _name, string _desc, SkillSelectUIManager _manager)
    {
        m_assignedSkillId = _skillId;
        m_panelManager = _manager;

        // 🔗 [완벽 해결] 슬롯이 인스펙터에서 비어있으면(null) 글자를 채우지 말고 그냥 통과시킵니다.
        // 이렇게 예외 처리를 걸어두면 인스펙터가 비어있어도 절대 빨간 줄 에러가 나지 않습니다!
        if (m_skillIconImage != null && _icon != null) m_skillIconImage.sprite = _icon;
        if (m_skillNameText != null) m_skillNameText.text = _name;
        if (m_skillDescText != null) m_skillDescText.text = _desc;
    }


    // 유니티 기본 Button 컴포넌트의 OnClick() 이벤트에 연결할 public 함수
    // 함수명은 무조건 파스칼 케이스 규칙 준수
    public void OnClickSelect()
    {
        if (m_panelManager != null)
        {
            // 총관리자(백화점 점장님)에게 내가 몇 번 스킬 보상인지 최종 보고합니다.
            m_panelManager.OnSkillSelected(m_assignedSkillId);
        }
    }
}
