using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public Dropdown _graphicsDropdown;
    public Slider _musicVol, _sfxVol;
    public AudioMixer _mainAudioMixer;


    public void ChangeGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(_graphicsDropdown.value);
    }

    public void ChangeMusicVolume()
    {
        _mainAudioMixer.SetFloat("MusicVol", _musicVol.value);
    }

    public void ChangeSfxVolume()
    {
        _mainAudioMixer.SetFloat("SfxVol", _sfxVol.value);
    }

    public void GotoMain()
    {
        GameManager.Instance.BackToScene();
    }
}
