using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void Start()
    {
        animator = GetComponent<Animator>();
        m_player = GetComponent<Player>();
    }
    public void Update()
    {
        //InputManager에서 인풋 가져오기
        var input = InputManager.m_Instance.InputInfo;
        //방향키 인풋
        var MoveDir = input.MoveDir;
        
        //이동 방향에 따라 플레이어 방향 변경
        if(MoveDir.x > 0)
        {
            this.transform.localScale = new Vector3(1,1);
        }
        else if(MoveDir.x < 0)
        {
            this.transform.localScale = new Vector3(-1,1);
        }
        //이동했다면 Animation 적용
        if(MoveDir.x != 0 || MoveDir.y != 0)
        {
            animator.SetFloat(MoveFloat,1);
            LookAt = MoveDir;
        } else
        {
            animator.SetFloat(MoveFloat,0);
        }
        //이동
        this.transform.Translate(MoveDir.normalized*m_player.Speed*Time.deltaTime);
    }
    public void PlayDead()
    {
        animator.SetTrigger("Dead");
    }
    public Vector2 LookAt{get;private set;}
    private const string MoveFloat = "Move";
    private const float MAGICSPEED = 5;
    private Animator animator;
    private Player m_player;
}