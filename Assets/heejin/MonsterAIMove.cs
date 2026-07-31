using UnityEngine;

public class MonsterAIMove : MonoBehaviour
{
    private Rigidbody2D m_monsterRb;
    private Transform m_player;
    private float m_moveSpeed;

    private void Awake()
    {
        m_monsterRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if( m_player == null)
        {
            return;
        }

        Vector2 move = new Vector2(
            m_player.position.x - m_monsterRb.position.x,
            m_player.position.y - m_monsterRb.position.y
            ).normalized;

        m_monsterRb.MovePosition(m_monsterRb.position + move * m_moveSpeed * Time.fixedDeltaTime);
    }

    // 생성될때 플레이어 위치와 데이터 받아옴
    public void SetTarget(Transform _playerTransform)
    {
        m_player = _playerTransform;
    }

    public void SetData(MonsterData _data)
    {
        m_moveSpeed = _data.MoveSpeed;
    }
}
