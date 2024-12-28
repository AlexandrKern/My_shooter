using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����� ��� ���������� UI ���������� ������������������ �� ������
/// </summary>
public class UIAudioController : MonoBehaviour
{
    [SerializeField]private Slider _musicSlider, _sfxSlider, _masterSlider;

    [SerializeField] private Toggle _audioToggle;

    private void Start()
    {
        InitializeSliders();
    }

    /// <summary>
    /// ��������/��������� ����
    /// </summary>
    public void AudioToggle()
    {
        AudioManager.Instance.AudioToggle(_audioToggle.isOn);

        ///��������/��������� ���������� ���������
        _musicSlider.interactable = !_audioToggle.isOn;
        _sfxSlider.interactable = !_audioToggle.isOn;
        _masterSlider.interactable = !_audioToggle.isOn;
    }

    /// <summary>
    /// ������������� ��������� ������� ������
    /// </summary>
    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    /// <summary>
    /// ������������� ��������� �������� ��������
    /// </summary>
    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    /// <summary>
    /// ������������� ����� ���������
    /// </summary>
    public void MasterVolume()
    {
        AudioManager.Instance.MasterVolume(_masterSlider.value);
    }

    /// <summary>
    /// ������������� ��������� ��������� ��� ��������
    /// </summary>
    private void InitializeSliders()
    {
        if (AudioManager.Instance != null)
        {
            if (_musicSlider != null)
                _musicSlider.value = AudioManager.Instance.LoadAllVolumes().musicVolume;

            if (_sfxSlider != null)
                _sfxSlider.value = AudioManager.Instance.LoadAllVolumes().sfxVolume;

            if (_masterSlider != null)
                _masterSlider.value = AudioManager.Instance.LoadAllVolumes().masterVolume;

            if (_audioToggle != null)
            {
                _audioToggle.isOn = AudioManager.Instance.LoadAllVolumes().isMuted;
            }
        }
        _musicSlider.interactable = !_audioToggle.isOn;
        _sfxSlider.interactable = !_audioToggle.isOn;
        _masterSlider.interactable = !_audioToggle.isOn;
    }
}
