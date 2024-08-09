using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private bool _isAttack;

    public Transform _basicAttack;

    private void Start()
    {
        _isAttack = false;
    }

    void Update()
    {
        if(GameManager.Instance.player._target != null & !_isAttack)
        {
            _isAttack = true;
            StartCoroutine("BasicAttack");
        }

    }

    IEnumerator BasicAttack()
    {
        while (_isAttack)
        {
            yield return new WaitForSeconds(1 / GameManager.Instance.player.AttackSpeed);

            if (GameManager.Instance.player._target == null) // 타겟이 존재하지않으면 공격 X
            {
                _isAttack= false;
                yield break;
            }

            // 공격 이펙트 생성 및 위치 지정
            GameObject attack = Instantiate(_basicAttack.gameObject);
            attack.transform.position = GameManager.Instance.player._target.transform.position;
            attack.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
            Destroy(attack, 0.5f);


        }

    }
}
