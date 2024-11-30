using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Slider")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    //[SerializeField] private AudioMixer _masterVolumMixer; 
    //[SerializeField] private Slider _masterVolumSlider;

    public Sound[] _themeAudio, _sfxAudio;
    public AudioSource _themeAudioSource, _sfxAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _masterVolumeSlider.value = _themeAudioSource.volume = 0.5f;
        _sfxVolumeSlider.value = _sfxAudioSource.volume = 0.5f;
    }

    public void PlayThemeMusic(string audioName)
    {
        Sound sounds = Array.Find(_themeAudio, x => x._audioName == audioName);
        if (sounds == null)
        {
            Debug.Log("Theme Audio Not Found!");
        }
        else
        {
            _themeAudioSource.clip = sounds._audioClip;
            _themeAudioSource.Play();
        }
    }

    public void PlaySFX(string audioName)
    {
        Sound sounds = Array.Find(_sfxAudio, x => x._audioName == audioName);
        if (sounds == null)
        {
            Debug.Log("Theme Audio Not Found!");
        }
        else
        {
            _sfxAudioSource.clip = sounds._audioClip;
            _sfxAudioSource.PlayOneShot(_sfxAudioSource.clip);
        }
    }

    public void MasterVolume(float volume)
    {
        _themeAudioSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        _sfxAudioSource.volume = volume;
    }
}
