using UnityEngine;

public class FieldItem : MonoBehaviour
{
    // 인스펙터에서 이 아이템이 가질 고유 스프라이트를 지정합니다.
    [SerializeField] private Sprite m_itemIconSprite;

    private UIManager m_uiManager;

    public void Start()
    {
        // 씬에 있는 UIManager를 자동으로 찾아 연결
        m_uiManager = FindFirstObjectByType<UIManager>();
    }

    // 플레이어와 부딪혔을 때 작동 (물리적 밀침이 없도록 Is Trigger 체크 필수)
    public void OnTriggerEnter2D(Collider2D _other)
    {
        
        // 부딪힌 대상의 태그가 Player인 경우에만 작동
        if (_other.CompareTag("Player"))
        {
            if (m_uiManager != null)
            {
                // UI 매니저에게 내 아이콘 이미지를 전달하며 슬롯을 채우라고 명령
                m_uiManager.AcquireItem(m_itemIconSprite);
            }

            // 먹었으므로 필드에서 삭제
            Destroy(gameObject);
        }
    }
}
