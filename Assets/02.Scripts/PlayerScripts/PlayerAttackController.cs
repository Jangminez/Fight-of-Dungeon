using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttackController : MonoBehaviour
{
    private bool _isAttack;

    public Transform _basicAttack;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        _isAttack = false;
    }

    void Update()
    {
        if(GameManager.Instance.player._target != null & !_isAttack)
        {
            _isAttack = true;
            _anim.SetFloat("AttackSpeed", GameManager.Instance.player.AttackSpeed);
            StartCoroutine("BasicAttack");
        }

    }

    IEnumerator BasicAttack()
    {
        while (_isAttack)
        {
            yield return new WaitForSeconds(1 / GameManager.Instance.player.AttackSpeed);

            if (GameManager.Instance.player._target == null) // 플레이어의 타겟이 없으면 공격 중지
            {
                _isAttack= false;
                yield break;
            }
            // 공격 애니메이션
            _anim.SetFloat("AttackState", 0f);
            _anim.SetFloat("NormalState", 0f);
            _anim.SetTrigger("Attack");

            // 타겟의 위치에 공격 이펙트 생성
            GameObject attack = Instantiate(_basicAttack.gameObject);
            attack.transform.position = GameManager.Instance.player._target.transform.position;
            attack.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SortingGroup>().sortingLayerName;
            Destroy(attack, 0.5f);
        }
    }
}
