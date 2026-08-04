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

        // ----------------------------------------------------------------------
        // 🌟 [오후 미션 완료] GameManager를 거치지 않고 Player 컴포넌트를 직접 호출!
        // ----------------------------------------------------------------------

        // 1. 씬에 존재하는 진짜 'Player' 클래스를 직접 찾아옵니다.
        Player playerComponent = FindFirstObjectByType<Player>();

        if (playerComponent != null)
        {
            // 2. [임시 테스트용 가짜 스킬 데이터 생성] 
            // 팀원의 RegisterSkill(Skill skill) 함수가 'Skill' 객체를 요구하므로 형식을 맞춰줍니다.
            Skill dummySkill = new Skill();

            // 3. 팀원의 플레이어 스크립트에 있는 RegisterSkill 함수를 직접 호출!
            // (※ 현재 팀원이 구현을 안 해두어 실행 시 에러가 뜰 수 있으므로, 
            // try-catch 문으로 감싸서 내 UI 테스트가 멈추지 않도록 안전장치를 칩니다.)
            try
            {
                playerComponent.RegisterSkill(dummySkill);
                Debug.Log($"[연동] Player의 RegisterSkill 함수를 직접 호출했습니다! (ID: {_selectedSkillId})");
            }
            catch (System.NotImplementedException)
            {
                // 팀원이 아직 함수 내부를 안 짜두었을 때 예외 처리
                Debug.LogWarning($"[UI 가상 테스트] Player의 RegisterSkill 함수 호출 성공! (단, 스킬 담당자가 아직 함수 내부 기능을 구현하지 않은 상태입니다. UI 연동 자체는 성공!)");
            }
        }
        else
        {
            Debug.LogWarning("[연동 경고] 씬에서 Player 오브젝트를 찾지 못했습니다. 플레이어가 배치되었는지 확인하세요.");
        }

        // 보상 선택 종료 후 패널 끄고 시간 재생 (기존 정석)
        m_rewardPanelObject.SetActive(false);
        Time.timeScale = 1f;
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
