using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalAudioSource : MonoBehaviour
{
    private static GlobalAudioSource instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 🚀 [핵심 수정] 어떤 씬으로 이동하든 로딩 UI의 상태에 맞춰 사운드를 제어합니다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. 타이틀 씬으로 돌아왔을 때
        if (scene.name == "TitleScene")
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        // 2. 게임 씬(인게임)으로 들어왔을 때
        else if (scene.name == "GameScene")
        {
            // 인게임 전용 BGM으로 음원을 바꾸고 싶다면 아래 주석을 풀고 클립을 연결하세요.
            // audioSource.clip = 인게임_BGM_클립; 

            if (!audioSource.isPlaying) audioSource.Play();
        }
    }

    // 🚀 [추가] 로딩 창이 켜질 때 외부(LoadingSceneController)에서 이 함수를 호출해 사운드를 끕니다.
    public void StopBgmForLoading()
    {
        if (audioSource != null) audioSource.Stop();
    }
}
