using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Slider = UnityEngine.UI.Slider;

[Serializable]
public enum eAudioChannelType
{
    MASTER,
    BGM,
    SFX,
    UI
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager m_Instance = null;

    [Header("BGM")]
    [SerializeField] private AudioSource m_BgmSource;
    [SerializeField] private SOAudio m_BgmData;

    [Header("SFX Pool")]
    [SerializeField] private int m_SfxPoolCount = 10;
    [SerializeField] private AudioSource m_SfxPrefab;

    [SerializeField] public AudioMixerGroup m_AudioMixerGroup = null; // private에서 public으로 임시 전환
    private List<AudioSource> m_SfxSources = new List<AudioSource>();
    private int m_SfxIndex = 0;

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;

        BuildSfxPool();

        PlayBgm(m_BgmData);
    }


    private void BuildSfxPool()
    {
        m_SfxSources.Clear();

        for (int i = 0; i < m_SfxPoolCount; ++i)
        {
            AudioSource pSrc;
            if (m_SfxPrefab != null)
            {
                pSrc = Instantiate(m_SfxPrefab, transform);
            }
            else
            {
                GameObject Instance = new GameObject("SFX_" + i);
                Instance.transform.SetParent(transform);
                pSrc = Instance.AddComponent<AudioSource>();
            }

            pSrc.playOnAwake = false;
            pSrc.loop = false;
            m_SfxSources.Add(pSrc);
        }
    }


    public void PlayBgm(SOAudio _pAudio)
    {
        if (_pAudio == null || _pAudio.Clips.Count == 0)
            return;

        AudioClip pClip = _pAudio.Clips[0];
        m_BgmSource.outputAudioMixerGroup = _pAudio.OutputGroup;
        m_BgmSource.clip = pClip;
        m_BgmSource.loop = true;
        m_BgmSource.volume = _pAudio.Volume;
        m_BgmSource.pitch = 1.0f;
        m_BgmSource.spatialBlend = 0.0f;
        m_BgmSource.Play();
    }

    public void StopBgm()
    {
        m_BgmSource.Stop();
        m_BgmSource.clip = null;
    }

    public int PlaySfx(SOAudio _Audio)
    {
        if (_Audio == null || _Audio.Clips.Count == 0)
            return -1;

        int iSrcIdx = GetNextSfxSourceIdx();

        AudioSource pSrc = m_SfxSources[iSrcIdx];

        int iClipIdx = UnityEngine.Random.Range(0, _Audio.Clips.Count);
        pSrc.clip = _Audio.Clips[iClipIdx];
        pSrc.outputAudioMixerGroup = _Audio.OutputGroup;

        pSrc.volume = _Audio.Volume;
        pSrc.pitch = UnityEngine.Random.Range(_Audio.PitchMin, _Audio.PitchMax);


        pSrc.Play();

        return iSrcIdx;
    }
    public void StopSfx(int _SrcIdx)
    {
        if (_SrcIdx >= m_SfxPoolCount || _SrcIdx < 0)
            return;

        m_SfxSources[_SrcIdx].Stop();

    }

    private int GetNextSfxSourceIdx()
    {
        if (m_SfxSources.Count == 0)
            return -1;

        int iClipIdx = m_SfxIndex;
        ++m_SfxIndex;
        if (m_SfxIndex >= m_SfxSources.Count)
            m_SfxIndex = 0;

        return iClipIdx;
    }


    public void UpdateSound(Slider _Slider, AudioMixerGroup _Group, eAudioChannelType _Type)
    {
        float fDB = _Slider.value <= 0.0001f ? -80f : Mathf.Log10(_Slider.value) * 20f;

        switch (_Type)
        {
            case eAudioChannelType.MASTER:
                _Group.audioMixer.SetFloat("MasterVolume", fDB);
                break;
            case eAudioChannelType.BGM:
                _Group.audioMixer.SetFloat("BGMVolume", fDB);
                break;

            case eAudioChannelType.SFX:
                _Group.audioMixer.SetFloat("SFXVolume", fDB);
                break;

            case eAudioChannelType.UI:
                _Group.audioMixer.SetFloat("UIVolume", fDB);
                break;
        }
    }
}