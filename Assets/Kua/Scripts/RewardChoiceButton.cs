using UnityEngine;
using TMPro; // 🌟 TextMeshPro를 제어하기 위해 반드시 필요한 치트키 네임스페이스
using UnityEngine.UI;

public class RewardChoiceButton : MonoBehaviour
{
    // [인스펙터 칸 생성] 내 자식에 있는 진짜 텍스트 글자들을 연결할 변수들
    [Header("[ 버튼 내부 컴포넌트들 ]")]
    [SerializeField] private TextMeshProUGUI m_titleText; // 제목 텍스트 칸
    [SerializeField] private TextMeshProUGUI m_descText;  // 설명 텍스트 칸
    [SerializeField] private TextMeshProUGUI m_levelText;  // 설명 텍스트 칸
    [SerializeField] private Image m_iconImage;          // 아이콘 이미지 칸

    //스크립터블 오브젝트
    private SkillData m_randomSkill = null;
    private int m_skillId;
    private SkillSelectUIManager m_uiManager;

    /// <summary>
    /// 🌟 [핵심] UI 매니저가 랜덤으로 뽑아낸 3개의 예시 데이터를 이 버튼에 찔러넣어 주는 함수
    /// </summary>
    /// 
    int count =0;
    public void InitButton(SkillData _randomSkill)
    {
        m_randomSkill = _randomSkill;
        m_iconImage.sprite = _randomSkill.Icon;
        m_iconImage.gameObject.SetActive(true);
        m_descText.text = _randomSkill.Desc;
        m_titleText.text = _randomSkill.Title;
        //m_levelText.text = 
        Debug.Log($"{count}는 {m_randomSkill.Icon.name}");
        //TODO 스킬 ID에 스킬 설명을 넣는다
    }
    
    // 마우스로 이 보상 버튼을 클릭했을 때 실행되는 함수 (기존 연동용)
    public void OnClickButton()
    {
        Debug.Log($"눌린 얘는 {m_randomSkill.Icon.name}");

        GameManager.m_Instance.Player.RegisterSkill(m_randomSkill);
        SkillSelectUIManager.Instance.OnSkillSelected(m_randomSkill);
    }
}
