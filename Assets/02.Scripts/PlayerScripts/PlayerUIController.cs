using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]private Player _player;

    [Header("HP & MP")]
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _mpBar;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    // HP의 값이 변경될 때  UI 변경
    public void HpChanged()
    {
        if (_player != null)
            _hpBar.fillAmount = _player.Hp / _player.FinalHp;
    }

    // MP의 값이 변경될 때 UI 변경
    public void MpChanged()
    {
        if (_player != null)
            _mpBar.fillAmount = _player.Mp / _player.FinalMp;
    }
}
