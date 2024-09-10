using System.Collections;
using UnityEngine;

public abstract class PlayerAttackController : MonoBehaviour
{
    protected bool _isAttack;
    protected Animator _anim;
    public Transform _basicAttack;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        _isAttack = false;
    }

    public virtual void Attack()
    {
        if(GameManager.Instance.player._target != null & !_isAttack)
        {
            _isAttack = true;
            _anim.SetFloat("AttackSpeed", GameManager.Instance.player.FinalAS);
            StartCoroutine("BasicAttack");
        }
    }

    public abstract IEnumerator BasicAttack();
}
