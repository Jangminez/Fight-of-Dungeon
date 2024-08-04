using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Image _hpBar;
    [Space(10f)]
    [SerializeField] private Image _mpBar;
    [Space(10f)]
    [SerializeField] private Slider _expBar;

    [SerializeField] private Transform _RespawnUI;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void LateUpdate()
    {
        _hpBar.fillAmount = _player.Hp / _player.MaxHp;

        _mpBar.fillAmount = _player.Mp / _player.MaxMp;

        _expBar.value = _player.Exp / _player.NextExp;


        // 플레이어 사망 시 리스폰 UI 활성화
        if (_player._isDie && !_RespawnUI.gameObject.activeSelf)
        {
            _RespawnUI.gameObject.SetActive(true);
        }
    }
}
