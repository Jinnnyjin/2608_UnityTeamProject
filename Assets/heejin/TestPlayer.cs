using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public static Transform PlayerTransform;

    private void Awake()
    {
        PlayerTransform = transform;
    }
}
