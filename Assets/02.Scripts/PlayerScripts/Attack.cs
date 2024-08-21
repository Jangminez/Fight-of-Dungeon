using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    float cri;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            cri = Random.Range(1f, 101f); // 1 ~ 100 확률 지정

            if (cri <= GameManager.Instance.player.Critical) // 공격이 크리티컬 일 때
            {
                collision.GetComponent<Enemy>().Hit(damage: GameManager.Instance.player.FinalAttack * 1.5f);

                Debug.Log("크리티컬!");
            }

            else // 일반 공격
            {
                collision.GetComponent<Enemy>().Hit(damage: GameManager.Instance.player.FinalAttack);
                Debug.Log("일반 공격");
            }
        }
    }
}
