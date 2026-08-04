using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Update()
    {
        var input = InputManager.m_Instance.InputInfo;
        this.transform.Translate(input.MoveDir.normalized*MAGICSPEED*Time.deltaTime);
    }
    private const float MAGICSPEED = 5;
    private Animator animator;
}