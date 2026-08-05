using UnityEngine;

public class TempExpGem : MonoBehaviour
{
    [Header("[ 경험치 설정 ]")]
    [SerializeField] private float expValue = 10f; // 10으로 설정

    // [추가] 이미 먹힌 보석인지 체크하는 방어벽 변수
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 만약 이미 부딪힌 보석이라면, 뒤의 코드를 실행하지 않고 즉시 리턴(탈출)합니다.
        if (isCollected) return;

        // 2. 부딪힌 대상을 확인 (플레이어 이름 검사 추가로 더 안전하게)
        if (collision.name.Contains("Player"))
        {
            // 3. 들어오자마자 바로 '이미 먹힘' 상태로 잠가버려서 중복 입력을 원천 차단합니다.
            isCollected = true;

            TempPlayerUIController uiController = FindFirstObjectByType<TempPlayerUIController>();

            if (uiController != null)
            {
                uiController.GainExp(expValue);

                // 화면에서 제거
                Destroy(gameObject);

                Debug.Log($"[정밀 보석 획득] 경험치 {expValue} 추가 완료!");
            }
        }
    }
}
