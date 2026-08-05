using UnityEngine;
using UnityEngine.SceneManagement; // 🌟 씬 전환 기능을 쓰기 위한 필수 치트키!

public class TitleSceneController : MonoBehaviour
{
    [Header("[ 이동할 진짜 게임 씬 이름 ]")]
    // 📌 유니티 인스펙터 창에서 이동하고 싶은 방의 이름(예: InGame, MainStage 등)을 받아옵니다.
    [SerializeField] private string m_targetSceneName;

    /// <summary>
    /// 🌟 유니티 버튼 컴포넌트(On Click)에 연결할 씬 전환 실행 함수!
    /// </summary>
    public void ClickToStartGame()
    {
        if (string.IsNullOrEmpty(m_targetSceneName) == false)
        {
            // 타임스케일이 혹시 0으로 굳어있을지 모르니 1로 안전하게 풀고 넘어갑니다.
            Time.timeScale = 1f;

            // 지정한 이름의 진짜 씬방으로 유저를 순간이동 시킵니다!
            SceneManager.LoadScene(m_targetSceneName);
        }
        else
        {
            Debug.LogError("[경고] 이동할 씬 이름이 인스펙터 창에서 비어있습니다!");
        }
    }
}
