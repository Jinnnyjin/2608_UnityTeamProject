using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    // 🌟 [요구사항 1] 싱글톤 디자인 패턴
    public static LoadingSceneController Instance { get; private set; }

    [Header("[ 로딩 UI 패널 자체 오브젝트 ]")]
    [SerializeField] private GameObject m_loadingPanelObject;


    [SerializeField] private GameObject m_TitlePanelObject;

    [Header("[ 로딩 바 슬라이더 연결 ]")]
    public Slider m_loadingSlider;

    private void Awake()
    {
        // 🌟 [요구사항 2] 온로드(DontDestroyOnLoad) 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (m_loadingPanelObject != null)
        {
            m_loadingPanelObject.SetActive(false);
        }
    }

    /// <summary>
    /// 🌟 [핵심 입구] 외부 버튼들에서 이 함수를 누르고 괄호 안에 "목적지 이름"만 다르게 써주면 됩니다!
    /// </summary>
    public void TriggerLoading(string targetSceneName)
    {
        m_TitlePanelObject.SetActive(false);
        StartCoroutine(LoadMapSceneProcess(targetSceneName));
    }

    public void TriggerTitle()
    {
        m_TitlePanelObject.SetActive(true);
        StartCoroutine(LoadMapSceneProcess("TitleScene"));
    }

    // 🌟 직접 작성하신 '비동기 슬라이더 연산 공식' 100% 완벽 재탕 구역
    private IEnumerator LoadMapSceneProcess(string targetSceneName)
    {
        // 로딩 중에는 사운드 재생 중지하도록 설정.
        GlobalAudioSource globalAudio = Object.FindFirstObjectByType<GlobalAudioSource>();
        if (globalAudio != null) globalAudio.StopBgmForLoading();

        if (m_loadingPanelObject != null) m_loadingPanelObject.SetActive(true);
        m_loadingSlider.value = 0f;

        // 📝 고정된 이름 대신, 버튼이 던져준 '목적지 씬 이름'을 비동기로 로드합니다!
        AsyncOperation op = SceneManager.LoadSceneAsync(targetSceneName);

        while (!op.isDone)
        {
            m_loadingSlider.value = op.progress;
            yield return null;
        }

        if (m_loadingPanelObject != null)
        {
            m_loadingPanelObject.SetActive(false);
        }
    }
}
