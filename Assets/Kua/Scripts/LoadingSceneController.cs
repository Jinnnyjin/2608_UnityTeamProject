using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    [Header("[ 로딩 바 슬라이더 연결 ]")]
    // 🌟 이 변수 앞에 public을 붙여서 유니티 인스펙터 창에 칸이 무조건 확실하게 보이도록 만들었습니다!
    public Slider m_loadingSlider;

    private void Start()
    {
        // 시작하자마자 백그라운드에서 Map 씬 비동기 로딩 개시
        StartCoroutine(LoadMapSceneProcess());
    }

    private IEnumerator LoadMapSceneProcess()
    {
        // 1. 이미 세팅 완료된 슬라이더의 시작 값을 0으로 리셋
        m_loadingSlider.value = 0f;

        // 2. 비동기로 Map 씬 불러오기 시작
        AsyncOperation op = SceneManager.LoadSceneAsync("Map");

        // 3. 로딩이 진행되는 동안 실제 진행도를 슬라이더 가치에 그대로 대입 (0 ~ 1)
        while (!op.isDone)
        {
            // 유니티 내부 비동기 값(op.progress)을 그대로 슬라이더에 꽂아 넣습니다.
            // 이미 0 세팅을 마쳐두셨으니 이 한 줄로 100%까지 칼같이 차오릅니다.
            m_loadingSlider.value = op.progress;

            yield return null;
        }
    }
}
