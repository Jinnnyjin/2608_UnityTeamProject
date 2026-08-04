using UnityEngine;
using UnityEngine.UI;

public class TempPlayerUIController : MonoBehaviour
{
    [Header("[ UI 컴포넌트 연결 ]")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider expSlider;

    [Header("[ 임시 플레이어 스탯 설정 ]")]
    public float maxHP = 100f;
    public float currentHP = 100f;

    public float maxEXP = 100f;
    public float currentEXP = 0f;

    void Start()
    {
        // 시작할 때 슬라이더를 현재 수치(100%)로 가득 채우기
        UpdateHPSlider();
        UpdateEXPSlider();
    }

    // --------------------------------------------------
    // [ 외부 충돌 및 연동용 공용(public) 함수 ]
    // --------------------------------------------------

    // 적과 충돌했을 때 다른 스크립트에서 호출할 함수 (예: TakeDamage(10f);)
    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        UpdateHPSlider();
        Debug.Log($"[UI 반영] 체력 감소! 현재 체력: {currentHP}/{maxHP}");
    }

    // 경험치 오브젝트를 먹었을 때 다른 스크립트에서 호출할 함수 (예: GainExp(15f);)
    public void GainExp(float expAmount)
    {
        currentEXP += expAmount;

        // 🌟 경험치가 최대치를 넘기는 순간 (레벨업 달성!)
        if (currentEXP >= maxEXP)
        {
            // 1. 경험치 초기화 및 남은 수치 이월
            currentEXP -= maxEXP;

            // 2. 레벨 수치 증가 (이후 밸런스를 위해 요구량도 증가)
            // currentLevel++; // 레벨 변수가 있다면 주석 해제
            // maxEXP = currentLevel * 50f; 

            Debug.Log("[시스템] ★레벨업 달성!★ 경험치 게이지 충족 완료.");

            // 3. 🔗 [최종 연계 핵심] 아까 만든 스킬 보상 선택 UI 매니저를 직접 찌릅니다!
            if (SkillSelectUIManager.Instance != null)
            {
                // 레벨업 전용 패널(항상 3개 슬롯 뜨는 정석 함수)을 자동으로 호출합니다.
                SkillSelectUIManager.Instance.OpenLevelUpPanel();
                Debug.Log("[연계 성공] SkillSelectUIManager의 OpenLevelUpPanel() 자동 호출 완료!");
            }
            else
            {
                Debug.LogWarning("[연계 경고] 씬에 SkillSelectUIManager(Canvas)가 비활성화되어 있거나 찾을 수 없습니다.");
            }
        }

        // 슬라이더 바 화면 갱신
        UpdateEXPSlider();
        Debug.Log($"[UI 반영] 경험치 획득! 현재 경험치: {currentEXP}/{maxEXP}");
    }


    // --------------------------------------------------
    // [ 내부 UI 슬라이더 갱신 함수 ]
    // --------------------------------------------------
    private void UpdateHPSlider()
    {
        if (hpSlider != null) hpSlider.value = currentHP / maxHP;
    }

    private void UpdateEXPSlider()
    {
        if (expSlider != null) expSlider.value = currentEXP / maxEXP;
    }
}
