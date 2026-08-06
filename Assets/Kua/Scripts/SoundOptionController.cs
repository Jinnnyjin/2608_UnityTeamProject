using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가

public class SoundOptionController : MonoBehaviour
{
    [Header("[ 사운드 패널 UI 자체 오브젝트 ]")]
    [SerializeField] private GameObject m_soundPanelObject;

    [Header("[ 슬라이더 3세트 연결 ]")]
    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_sfxSlider;

    [Header("[ 📢 누구나 테스트 가능한 BGM 전용 스피커 ]")]
    [SerializeField] private AudioSource m_bgmAudioSource;

    // 게임 일시 정지 상태를 체크하기 위한 변수 추가
    private bool isPaused = false;

    void Start()
    {
        // 🚀 [추가] 게임 씬에서 Missing이 나는 문제를 해결하는 자동 검색 기능
        // 인스펙터에 수동으로 연결이 안 되어 있거나(Null), 씬 전환으로 끊겼다면(Missing) 실행
        if (m_bgmAudioSource == null)
        {
            GlobalAudioSource globalAudio = Object.FindFirstObjectByType<GlobalAudioSource>();
            if (globalAudio != null)
            {
                m_bgmAudioSource = globalAudio.GetComponent<AudioSource>();

                // 찾은 오디오 소스를 기준으로 볼륨을 한 번 더 동기화해 줍니다.
                UpdateRealBgmVolume();
            }
        }

        // 게임 시작 시 패널은 자동으로 꺼두기
        if (m_soundPanelObject != null)
        {
            m_soundPanelObject.SetActive(false);
        }
    }

    void Update()
    {
        // 🚀 [추가] 타이틀 씬이 아닐 때만 ESC 키로 일시 정지 및 옵션 창 켜기
        if (SceneManager.GetActiveScene().name == "TitleScene") return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseAndMenu();
        }
    }

    // 🚀 [추가] 일시 정지 및 패널 토글 함수
    public void TogglePauseAndMenu()
    {
        if (m_soundPanelObject == null) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // 게임 세상 일시 정지
            m_soundPanelObject.SetActive(true); // 옵션 패널 강제 활성화
        }
        else
        {
            Time.timeScale = 1f; // 게임 재개
            SaveAndCloseOptions(); // 닫힐 때 기존 저장 및 비활성화 로직 실행
        }
    }

    private void OnEnable()
    {
        // [로드] 기존 저장된 볼륨 값 가져오기 (0.0 ~ 1.0)
        m_masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 1f);
        m_bgmSlider.value = PlayerPrefs.GetFloat("BgmVol", 1f);
        m_sfxSlider.value = PlayerPrefs.GetFloat("SfxVol", 1f);

        // [실시간 리스너] 슬라이더를 바꿀 때마다 실시간으로 오디오 연산 함수 실행
        m_masterSlider.onValueChanged.AddListener(OnVolumeChanged);
        m_bgmSlider.onValueChanged.AddListener(OnVolumeChanged);
        m_sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        // 켜지자마자 현재 저장된 수치대로 소리 크기 셋팅
        UpdateRealBgmVolume();
    }

    private void OnDisable()
    {
        m_masterSlider.onValueChanged.RemoveAllListeners();
        m_bgmSlider.onValueChanged.RemoveAllListeners();
        m_sfxSlider.onValueChanged.RemoveAllListeners();
    }

    // 🔊 마스터 혹은 BGM 슬라이더가 움직일 때 실시간 실행되는 함수
    private void OnVolumeChanged(float value)
    {
        UpdateRealBgmVolume();
    }

    // 🌟 [핵심 공식] 마스터와 BGM의 완벽한 상위-하위 종속 관계 구현
    private void UpdateRealBgmVolume()
    {
        if (m_bgmAudioSource != null)
        {
            // 실제 배경음 스피커 소리 = (마스터 슬라이더 수치 × 배경음 슬라이더 수치)
            m_bgmAudioSource.volume = m_masterSlider.value * m_bgmSlider.value;
        }
    }

    // 💥 효과음 슬라이더가 움직일 때 실시간 실행되는 함수
    private void OnSfxVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("SfxVol", value);
    }

    public void SaveAndCloseOptions()
    {
        PlayerPrefs.SetFloat("MasterVol", m_masterSlider.value);
        PlayerPrefs.SetFloat("BgmVol", m_bgmSlider.value);
        PlayerPrefs.SetFloat("SfxVol", m_sfxSlider.value);
        PlayerPrefs.Save();

        Debug.Log("[사운드 옵션] 마스터/BGM/SFX 구조적 전역 저장 완료!");

        if (m_soundPanelObject != null)
        {
            m_soundPanelObject.SetActive(false);
        }

        // 일시 정지 상태에서 마우스로 직접 닫기 버튼을 눌렀을 때를 대비해 시간 재생 보장
        isPaused = false;
        Time.timeScale = 1f;
    }
}
