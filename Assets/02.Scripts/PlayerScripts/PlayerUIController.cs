using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : NetworkBehaviour
{
    [SerializeField]private Player _player;

    [Header("HP & MP")]
    [SerializeField] private Image _hpBar;
    [SerializeField] private Image _mpBar;
    private Transform _canvas;
    private Vector3 _initScale;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            this.enabled = false;
            return;
        }
        _player = GetComponent<Player>();
        _canvas = _hpBar.transform.parent;
        _initScale = _canvas.localScale;
    }

    void Update()
    {
        if(_canvas == null) return;

        _canvas.localScale = _initScale;
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
