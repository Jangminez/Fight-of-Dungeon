using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class EnemyHp : NetworkBehaviour
{
    private Enemy _enemy;


    [SerializeField] private Transform _canvas;
    [SerializeField] private Image _hpBar;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void ChangeHp()
    {
        if (_enemy != null)
        {
            if (_enemy.Hp == 0)
            {
                _canvas.gameObject.SetActive(false);
            }

            else
            {
                _canvas.gameObject.SetActive(true);
            }

            if (_hpBar != null)
            {
                _hpBar.fillAmount = _enemy.Hp / _enemy.MaxHp;
            }
        }
    }
}
