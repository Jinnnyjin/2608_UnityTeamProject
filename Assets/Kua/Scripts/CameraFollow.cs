using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 0, -10f);

    public void LateUpdate()
    {
        if (Target == null) 
            return;

        transform.position = Target.position + Offset;
    }
}
