using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // 한 번 부딪힐 때 깎을 체력

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 부딪힌 대상이 플레이어인지 확인
        if (collision.name.Contains("Player"))
        {
            // 2. 씬에 있는 TempPlayerUIController를 찾아서 데미지 주기
            TempPlayerUIController uiController = FindFirstObjectByType<TempPlayerUIController>();
            if (uiController != null)
            {
                uiController.TakeDamage(damage);
            }
        }
    }
}
