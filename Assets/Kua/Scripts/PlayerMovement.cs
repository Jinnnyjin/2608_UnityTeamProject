using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public 대신 [SerializeField]를 사용하여 인스펙터에서 조절 가능하게 설정
    [SerializeField] private float m_moveSpeed = 5f;

    // Private 멤버 변수
    private Vector2 m_movementInput;

    public void Update()
    {
        // WASD 및 방향키 입력 받기
       // m_movementInput.x = Input.GetAxis("Horizontal");
       // m_movementInput.y = Input.GetAxis("Vertical");
        m_movementInput = InputManager.m_Instance.InputInfo.MoveDir;
        Debug.Log(m_movementInput);
        // 대각선 이동 시 빨라지지 않도록 벡터 정규화(Normalize)
        if (m_movementInput.sqrMagnitude > 1f)
        {
            m_movementInput.Normalize();
        }
    }

    public void FixedUpdate()
    {
        // 물리 연산이 아닌 단순 뼈대 이동이므로 Translate 사용 (알파 테스트용으로 가장 빠름)
        // 2D 탑다운 게임이므로 X, Y 축으로 이동
        Vector3 moveDirection = new Vector3(m_movementInput.x, m_movementInput.y, 0f);
        transform.Translate(moveDirection * m_moveSpeed * Time.fixedDeltaTime);
    }
}
