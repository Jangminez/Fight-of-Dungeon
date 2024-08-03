using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _mpBar;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _mpText;

    [SerializeField] private Transform _RespawnUI;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void LateUpdate()
    {
        _hpBar.fillAmount = _player.Hp / _player.MaxHp;
        _hpText.text = _player.Hp.ToString() + " / " + _player.MaxHp.ToString();

        _mpBar.fillAmount = _player.Mp / _player.MaxMp;
        _mpText.text = _player.Mp.ToString() + " / " + _player.MaxMp.ToString();

        if (_player._isDie && !_RespawnUI.gameObject.activeSelf)
        {
            _RespawnUI.gameObject.SetActive(true);
        }
    }
}
