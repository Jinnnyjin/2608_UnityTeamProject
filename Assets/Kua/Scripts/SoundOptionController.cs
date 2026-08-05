using UnityEngine;
using UnityEngine.UI;

public class SoundOptionController : MonoBehaviour
{
    [Header("[ 사운드 패널 UI 자체 오브젝트 ]")]
    [SerializeField] private GameObject m_soundPanelObject;

    [Header("[ 슬라이더 3세트 연결 ]")]
    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_sfxSlider;

    [Header("[ 📢 누구나 테스트 가능한 BGM 전용 스피커 ]")]
    // 📌 여기에 현재 가지고 계신 mp3 오디오 소스를 연결합니다.
    [SerializeField] private AudioSource m_bgmAudioSource;

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
            // 이렇게 곱해주어야 마스터가 줄어들면 모든 소리가 줄어들고, BGM만 줄여도 따로 줄어듭니다!
            m_bgmAudioSource.volume = m_masterSlider.value * m_bgmSlider.value;
        }
    }

    // 💥 효과음 슬라이더가 움직일 때 실시간 실행되는 함수
    private void OnSfxVolumeChanged(float value)
    {
        // 🚨 효과음 수치(value)가 아무리 변해도 위의 UpdateRealBgmVolume()을 호출하지 않으므로
        // 현재 씬에 나오는 BGM 소리 크기에는 '단 1도' 영향을 주지 않고 완벽하게 독립됩니다!
        // (나중에 사운드 팀원이 오면 이 value 값을 효과음 믹서에 꽂아주기만 하면 끝납니다.)
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
    }
}
