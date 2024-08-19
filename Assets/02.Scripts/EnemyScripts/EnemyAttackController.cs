using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    Enemy _enemy;
    Animator _anim;
    private bool _isAttack;

    Transform _attackEffect;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _anim = GetComponent<Animator>(); 
        _isAttack = false;
    }

    private void LateUpdate()
    {
        if(!_isAttack && _enemy.state == Enemy.States.Attack)
        {
            _isAttack = true;
            _anim.SetFloat("AttackSpeed", _enemy.stat.attackSpeed);
            StartCoroutine("EnemyAttack");
        }        
    }

    IEnumerator EnemyAttack()
    {
        while(_isAttack)
        {
            yield return new WaitForSeconds(1 / _enemy.stat.attackSpeed);

            if (_enemy.state != Enemy.States.Attack) 
            {
                _isAttack = false;
                yield break;
            }
            
            GameManager.Instance.player.Hit(damage: _enemy.stat.attack);
        }

    }
}
