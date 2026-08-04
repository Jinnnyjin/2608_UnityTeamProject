using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5f;

    private Vector2 m_movementInput;
    private Rigidbody2D m_rigidbody; // 리지드바디 참조 변수 추가

    void Start()
    {
        // 시작할 때 오브젝트에 붙어있는 Rigidbody2D를 자동으로 가져옴
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        m_movementInput.x = Input.GetAxisRaw("Horizontal");
        m_movementInput.y = Input.GetAxisRaw("Vertical");

        if (m_movementInput.sqrMagnitude > 1f)
        {
            m_movementInput.Normalize();
        }
    }

    public void FixedUpdate()
    {
        // [수정 완료] transform.Translate 대신 Rigidbody2D의 속도(velocity)를 직접 제어합니다.
        // 이 방식을 써야 유니티 물리 엔진이 실시간으로 부딪힘을 감지합니다.
        if (m_rigidbody != null)
        {
            m_rigidbody.linearVelocity = m_movementInput * m_moveSpeed;
        }
    }
}
