using UnityEngine;
using UnityEngine.UI;

public class SoundOptionPanelController : MonoBehaviour
{
    [Header("사운드 옵션 UI 등록")]
    [SerializeField] private Slider bgmSlider;
    // [SerializeField] private Slider sfxSlider; // 추후 효과음 추가 시 주석 해제하여 사용
    // [SerializeField] private Toggle muteToggle; // 추후 음소거 추가 시 주석 해제하여 사용

    private AudioSource bgmAudioSource;

    void Start()
    {
        // 1. 씬에 살아남아 있는 'GlobalAudioSource' 찾기
        GlobalAudioSource globalAudio = FindObjectOfType<GlobalAudioSource>();

        if (globalAudio != null)
        {
            // 2. 글로벌 오디오 소스 안의 AudioSource 컴포넌트 가져오기
            bgmAudioSource = globalAudio.GetComponent<AudioSource>();

            // 3. 기존 볼륨 값을 UI 슬라이더에 반영 (이전 씬의 설정 유지)
            if (bgmSlider != null && bgmAudioSource != null)
            {
                bgmSlider.value = bgmAudioSource.volume;

                // 4. 슬라이더 이벤트 연결 (인스펙터 세팅 필요 없음)
                bgmSlider.onValueChanged.AddListener(SetBgmVolume);
            }
        }
        else
        {
            Debug.LogWarning("씬에서 GlobalAudioSource를 찾을 수 없습니다!");
        }
    }

    // 배경음 볼륨 조절 함수
    private void SetBgmVolume(float value)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = value;
        }
    }
}
