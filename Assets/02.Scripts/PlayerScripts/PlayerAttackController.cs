using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class PlayerAttackController : NetworkBehaviour
{
    protected bool _isAttack;
    protected Animator _anim;
    public Transform _basicAttack;
    Player player;

    public override void OnNetworkSpawn()
    {
        player = GetComponent<Player>();
        _anim = GetComponent<Animator>();

        _isAttack = false;
    }

    public virtual void Attack()
    {
        if(player._target != null & !_isAttack)
        {
            _isAttack = true;
            _anim.SetFloat("AttackSpeed", player.FinalAS);
            StartCoroutine("BasicAttack");
        }
    }

    public abstract IEnumerator BasicAttack();
}

