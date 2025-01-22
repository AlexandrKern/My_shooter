using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// ����� ��� ���������� ����������� � ���������� �����
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] _musicSounds, _sfxSounds;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    private Dictionary<string, Sound> _musicDictionary, _sfxDictionary;

    private float _masterVolume = 1f; // ����� ������� ���������
    private float _fadeDuration = 0f; // ������������ ��������� ���������������

    private float _currentMusicVolume;
    private float _musicVolume;
    private float _sfxVolume;

    public const string VOLUME_SETTINGS_FILE = "volumeSettings.json"; 

    // ����� ��� �������� ������ � ���������
    [System.Serializable]
    public class VolumeSettings
    {
        public float musicVolume;
        public float sfxVolume;
        public float masterVolume;
        public bool isMuted;
    }

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
        _musicDictionary = _musicSounds.ToDictionary(s => s.name);
        _sfxDictionary = _sfxSounds.ToDictionary(s => s.name);
        LoadAllVolumes();
        PlayMusic("Background");
    }

    private void OnApplicationQuit()
    {
        SaveAllVolumes();
    }

    /// <summary>
    /// ������������� ������� ������
    /// </summary>
    /// <param name="name">�������� �����</param>
    public void PlayMusic(string name)
    {
        if (_musicDictionary.TryGetValue(name, out Sound s))
        {
            _musicSource.clip = s.clip;
            _musicSource.Play();
        }
        else
        {
            Debug.Log("Sound not found");
        }
    }

    /// <summary>
    /// ������������� �������� ������
    /// </summary>
    /// <param name="name">�������� �������</param>
    public void PlaySFX(string name)
    {
        if (_sfxDictionary.TryGetValue(name, out Sound s))
        {
            _sfxSource.PlayOneShot(s.clip);
        }
        else
        {
            Debug.Log("SFX not found");
        }
    }

    /// <summary>
    /// ��������/��������� ����
    /// </summary>
    /// <param name="mute">��������� �����</param>
    public void AudioToggle(bool mute)
    {
        _musicSource.mute = mute;
        _sfxSource.mute = mute;

        if (!mute)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StopAllCoroutines();
        }
        SaveAllVolumes();
    }

    /// <summary>
    /// ������������� ��������� ������� ������
    /// </summary>
    /// <param name="volume">�������� ���������</param>
    public void MusicVolume(float volume)
    {
        _currentMusicVolume = volume;
        _musicSource.volume = volume * _masterVolume;
        _musicVolume = volume;
        SaveAllVolumes();
    }

    /// <summary>
    /// ������������� ��������� �������� ��������
    /// </summary>
    /// <param name="volume">�������� ���������</param>
    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume * _masterVolume;
        _sfxVolume = volume;
        SaveAllVolumes();
    }

    /// <summary>
    /// ������������� ����� ���������
    /// </summary>
    /// <param name="volume">�������� ���������</param>
    public void MasterVolume(float volume)
    {
        _masterVolume = volume;
        ApplyMasterVolume();
        SaveAllVolumes();
    }

    /// <summary>
    /// ��������� ����� ��������� �� ���� ���������� �����
    /// </summary>
    private void ApplyMasterVolume()
    {
        _musicSource.volume = _musicVolume * _masterVolume;
        _sfxSource.volume = _sfxVolume * _masterVolume;
    }

    /// <summary>
    /// �������� ��� �������� ���������� ���������
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeIn()
    {
        float currentTime = 0f;
        _musicSource.volume = 0f;

        while (currentTime < _fadeDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(0f, _currentMusicVolume, currentTime / _fadeDuration) * _masterVolume; // ������ ���������� ���������
            yield return null; // ���� �� ���������� �����
        }

        _musicSource.volume = _currentMusicVolume * _masterVolume;
    }

    /// <summary>
    /// ��������� ��������� ���� ����������
    /// </summary>
    public void SaveAllVolumes()
    {
        VolumeSettings settings = new VolumeSettings
        {
            musicVolume = _musicVolume,
            sfxVolume = _sfxVolume,
            masterVolume = _masterVolume,
            isMuted = _musicSource.mute
        };

        string json = JsonUtility.ToJson(settings,true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, VOLUME_SETTINGS_FILE), json);
    }

    /// <summary>
    /// ��������� ��������� ���� ����������
    /// </summary>
    public VolumeSettings LoadAllVolumes()
    {
        string filePath = Path.Combine(Application.persistentDataPath, VOLUME_SETTINGS_FILE);
        VolumeSettings settings;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<VolumeSettings>(json);

            _musicVolume = settings.musicVolume;
            _sfxVolume = settings.sfxVolume;
            _masterVolume = settings.masterVolume;
            bool isMuted = settings.isMuted;

            _musicSource.mute = isMuted;
            _sfxSource.mute = isMuted;

            if (!isMuted)
            {
                _currentMusicVolume = _musicVolume;
            }
            
            ApplyMasterVolume();
            return settings;
        }
        else
        {
            settings = new VolumeSettings{
                sfxVolume = 1,
                masterVolume = 1,
                isMuted = false,
                musicVolume = 1,
            };

            _musicVolume = 1f;
            _sfxVolume = 1f;
            _masterVolume = 1f;
            _musicSource.mute = false;
            _sfxSource.mute = false;

            _currentMusicVolume = _musicVolume;
            ApplyMasterVolume();
            return settings;
        }
    }
}

