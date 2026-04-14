using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private UnityEngine.Audio.AudioMixer _audioMixer;

    [SerializeField] private string MasterVolumeKey = "Master";
    [SerializeField] private string MusicVolumeKey = "Music";
    [SerializeField] private string SfxVolumeKey = "Effects";

    [Header("Sliders")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundEffectsSlider;

    [Header("Windows")]
    [SerializeField] private Button _closeButton;

    [SerializeField] private GameObject _optionsPanel;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);

        _audioMixer.GetFloat(MasterVolumeKey, out float master);
        _audioMixer.GetFloat(MusicVolumeKey, out float music);
        _audioMixer.GetFloat(SfxVolumeKey, out float effects);

        _masterSlider.value = master;
        _musicSlider.value = music;
        _soundEffectsSlider.value = effects;

        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _soundEffectsSlider.onValueChanged.AddListener(SetEffectsVolume);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);

        _masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        _soundEffectsSlider.onValueChanged.RemoveListener(SetEffectsVolume);
    }

    public void Open()
    {
        if (_optionsPanel != null)
            _optionsPanel.SetActive(true);
    }

    public void Close()
    {
        if (_optionsPanel != null)
            _optionsPanel.SetActive(false);
    }

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat(MasterVolumeKey, level);
    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat(MusicVolumeKey, level);
    }

    public void SetEffectsVolume(float level)
    {
        _audioMixer.SetFloat(SfxVolumeKey, level);
    }
}