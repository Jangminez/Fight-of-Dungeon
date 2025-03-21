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
        _mainAudioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        _mainAudioMixer.SetFloat("SfxVol", PlayerPrefs.GetFloat("SfxVol"));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualitySet"));

        _musicVol.value = PlayerPrefs.GetFloat("MusicVol");
        _sfxVol.value = PlayerPrefs.GetFloat("SfxVol");
        _graphicsDropdown.value = PlayerPrefs.GetInt("QualitySet");
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
        UISoundManager.Instance.PlayClickSound();
        _mainAudioMixer.SetFloat("MusicVol", _musicVol.value);

        PlayerPrefs.SetFloat("MusicVol", _musicVol.value);
        PlayerPrefs.Save();
    }

    public void ChangeSfxVolume()
    {
        UISoundManager.Instance.PlayClickSound();
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
