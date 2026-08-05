using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Public 변수는 PascalCase
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 0, -10f);

    public void LateUpdate()
    {
        if (Target == null) return;

        // 지연 시간(Lerp) 없이 플레이어 좌표에 오프셋만 더해 바로 고정
        transform.position = Target.position + Offset;
    }
}
