using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]private Player _player;

    [Header("HP & MP")]
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _mpBar;
    [Space(10f)]
    [Header("Level & EXP")]
    [SerializeField] private Slider _expBar;
    [SerializeField] private Text _levelText;
    [Space(10f)]
    [Header("Gold")]
    [SerializeField] private Text _goldText;

    [SerializeField] private Transform _RespawnUI;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    // HP ���� �� ���� �� UI ����
    public void HpChanged()
    {
        if (_player != null)
            _hpBar.fillAmount = _player.Hp / _player.MaxHp;
    }

    // MP ���� �� ���� �� UI ����
    public void MpChanged()
    {
        if (_player != null)
            _mpBar.fillAmount = _player.Mp / _player.MaxMp;
    }

    // EXP �� ���� �� UI ����
    public void ExpChanged()
    {
        if(_player != null)
            _expBar.value = _player.Exp / _player.NextExp;
    }

    // Level �� ���� �� UI ����
    public void LevelChanged()
    {
        if (_player != null)
            _levelText.text = _player.Level.ToString();
    }

    // Gold �� ���� �� UI ����
    public void GoldChanged()
    {
        if (_player != null)
            _goldText.text = _player.Gold.ToString();
    }

    // �÷��̾��� ���°� Die �϶� ������ UI ����
    public void OnRespawn()
    {
        if (_player != null)
            _RespawnUI.gameObject.SetActive(true);
    }
}
