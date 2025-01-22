using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс для управления изменениями и настройкой звука
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] _musicSounds, _sfxSounds;
    [SerializeField] private AudioSource _musicSource, _sfxSource;

    private Dictionary<string, Sound> _musicDictionary, _sfxDictionary;

    private float _masterVolume = 1f; // Общий уровень громкости
    private float _fadeDuration = 0f; // Длительность плавности воспроизведения

    private float _currentMusicVolume;
    private float _musicVolume;
    private float _sfxVolume;

    public const string VOLUME_SETTINGS_FILE = "volumeSettings.json"; 

    // Класс для хранения данных о громкости
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
    /// Воспроизводит фоновую музыку
    /// </summary>
    /// <param name="name">Назавние трека</param>
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
    /// Воспроизводит звуковой эффект
    /// </summary>
    /// <param name="name">Название эффекта</param>
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
    /// Включает/выключает звук
    /// </summary>
    /// <param name="mute">Состояние звука</param>
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
    /// Устанавливает громкость фонофой музыки
    /// </summary>
    /// <param name="volume">Значение громкости</param>
    public void MusicVolume(float volume)
    {
        _currentMusicVolume = volume;
        _musicSource.volume = volume * _masterVolume;
        _musicVolume = volume;
        SaveAllVolumes();
    }

    /// <summary>
    /// Устанавливает громкость звуковых эффектов
    /// </summary>
    /// <param name="volume">Значение громкости</param>
    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume * _masterVolume;
        _sfxVolume = volume;
        SaveAllVolumes();
    }

    /// <summary>
    /// Устанавливает общую громкость
    /// </summary>
    /// <param name="volume">Значение громкости</param>
    public void MasterVolume(float volume)
    {
        _masterVolume = volume;
        ApplyMasterVolume();
        SaveAllVolumes();
    }

    /// <summary>
    /// Применяет общую громкость ко всем источникам звука
    /// </summary>
    private void ApplyMasterVolume()
    {
        _musicSource.volume = _musicVolume * _masterVolume;
        _sfxSource.volume = _sfxVolume * _masterVolume;
    }

    /// <summary>
    /// Корутина для плавного увеличения громкости
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeIn()
    {
        float currentTime = 0f;
        _musicSource.volume = 0f;

        while (currentTime < _fadeDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(0f, _currentMusicVolume, currentTime / _fadeDuration) * _masterVolume; // Плавно увеличивае громкость
            yield return null; // Ждет до следующего кадра
        }

        _musicSource.volume = _currentMusicVolume * _masterVolume;
    }

    /// <summary>
    /// Сохраняет громкость всех источников
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
    /// Загружает громкость всех источников
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

