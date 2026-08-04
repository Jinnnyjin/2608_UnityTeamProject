using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Public 변수는 PascalCase
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 0, -10f);

    // FixedUpdate에서 플레이어의 물리 이동 직후 좌표를 바로 흡수하여 동기화
    public void LateUpdate()
    {
        if (Target == null) return;

        // 지연 시간(Lerp) 없이 플레이어 좌표에 오프셋만 더해 바로 고정
        transform.position = Target.position + Offset;
    }
}
