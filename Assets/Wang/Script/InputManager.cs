using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*///////////////////////////////////////////
                InputManager
기능 : 연결된 액션의 값을 읽어 tInputInfo에 반영
 *///////////////////////////////////////////
public struct tInputInfo
{
    public Vector2 MoveDir;    // 이동 방향 (아날로그 크기 유지, 최대 1)
    public Vector2 ScreenPos;  // 포인터 스크린 좌표
    public Vector2 Delta;      // 포인터 이동량
    public bool OnSpace;       // 스페이스 눌림 상태
    public bool OnLButton;     // 좌클릭 눌림 상태
}

public class InputManager : MonoBehaviour
{
    public static InputManager m_Instance { get; private set; }

    [SerializeField] private List<InputActionReference> m_MoveAction;
    [SerializeField] private List<InputActionReference> m_ScreenAction;
    [SerializeField] private List<InputActionReference> m_DeltaAction;
    [SerializeField] private List<InputActionReference> m_SpaceAction;

    private tInputInfo m_tInputInfo;
    public tInputInfo InputInfo => m_tInputInfo;

    public event Action OnSpaceDown;
    public event Action OnClickDown;

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(gameObject);

        EnableAll(m_MoveAction);
        EnableAll(m_ScreenAction);
        EnableAll(m_DeltaAction);
        EnableAll(m_SpaceAction);

        // 버튼류는 이벤트 콜백으로
        Subscribe(m_SpaceAction, ctx => OnSpaceDown?.Invoke());
    }

    private void OnDestroy()
    {
        // 이 오브젝트가 실제 인스턴스일 때만 정리
        if (m_Instance != this)
            return;

        DisableAll(m_MoveAction);
        DisableAll(m_ScreenAction);
        DisableAll(m_DeltaAction);
        DisableAll(m_SpaceAction);
    }

    private void Update()
    {
        m_tInputInfo.MoveDir = ReadMove();
        m_tInputInfo.ScreenPos = ReadVector2(m_ScreenAction);
        m_tInputInfo.Delta = ReadVector2(m_DeltaAction);
        m_tInputInfo.OnSpace = ReadIsPressed(m_SpaceAction);


    }

    // 여러 이동 소스 중 크기가 가장 큰 입력을 선택 (덮어쓰기 버그 방지)
    private Vector2 ReadMove()
    {
        Vector2 Best = Vector2.zero;
        float BestSqr = 0f;

        for (int i = 0; i < m_MoveAction.Count; ++i)
        {
            Vector2 CurVector = m_MoveAction[i].action.ReadValue<Vector2>();
            float Sqr = CurVector.sqrMagnitude;
            if (Sqr > BestSqr)
            {
                BestSqr = Sqr;
                Best = CurVector;
            }
        }
        return Vector2.ClampMagnitude(Best, 1f);
    }

    private Vector2 ReadVector2(List<InputActionReference> list)
    {
        Vector2 Value = Vector2.zero;
        for (int i = 0; i < list.Count; ++i)
            Value = list[i].action.ReadValue<Vector2>();
        return Value;
    }

    private bool ReadIsPressed(List<InputActionReference> list)
    {
        for (int i = 0; i < list.Count; ++i)
            if (list[i].action.IsPressed())
                return true;
        return false;
    }

    private static void EnableAll(List<InputActionReference> list)
    {
        for (int i = 0; i < list.Count; ++i)
            list[i].action.Enable();
    }

    private static void DisableAll(List<InputActionReference> list)
    {
        for (int i = 0; i < list.Count; ++i)
            list[i].action.Disable();
    }

    private static void Subscribe(List<InputActionReference> list, Action<InputAction.CallbackContext> CallBack)
    {
        for (int i = 0; i < list.Count; ++i)
            list[i].action.performed += CallBack;
    }
}
