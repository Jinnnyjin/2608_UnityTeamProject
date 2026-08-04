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

        // 레벨업 처리 루프 (경험치가 최대치를 넘기면 이월)
        if (currentEXP >= maxEXP)
        {
            currentEXP -= maxEXP;
            Debug.Log("[UI 반영] ★레벨업!★ 경험치 초기화 및 이월");
        }

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
