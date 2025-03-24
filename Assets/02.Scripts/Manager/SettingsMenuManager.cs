using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public Dropdown _graphicsDropdown;
    public Slider _musicVol, _sfxVol;
    public AudioMixer _mainAudioMixer;

    void OnEnable()
    {
        InitializeSetting();
    }

    void Start()
    {
        InitializeSetting();
    }

    public void InitializeSetting()
    {
        _mainAudioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol", 0f));
        _mainAudioMixer.SetFloat("SfxVol", PlayerPrefs.GetFloat("SfxVol", 0f));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualitySet", 1));

        _musicVol.value = PlayerPrefs.GetFloat("MusicVol", 0f);
        _sfxVol.value = PlayerPrefs.GetFloat("SfxVol", 0f);
        _graphicsDropdown.value = PlayerPrefs.GetInt("QualitySet", 1);
    }

    public void ChangeGraphicsQuality()
    {
        UISoundManager.Instance.PlayClickSound();
        QualitySettings.SetQualityLevel(_graphicsDropdown.value);

        PlayerPrefs.SetInt("QualitySet", _graphicsDropdown.value);
        PlayerPrefs.Save();
    }

    public void ChangeMusicVolume()
    {
        _mainAudioMixer.SetFloat("MusicVol", _musicVol.value);

        PlayerPrefs.SetFloat("MusicVol", _musicVol.value);
        PlayerPrefs.Save();
    }

    public void ChangeSfxVolume()
    {
        _mainAudioMixer.SetFloat("SfxVol", _sfxVol.value);

        PlayerPrefs.SetFloat("SfxVol", _sfxVol.value);
        PlayerPrefs.Save();
    }

    public void GotoMain()
    {
        UISoundManager.Instance.PlayClickSound();
        GameManager.Instance.BackToScene();
    }
}
