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
            cri = Random.Range(1f, 101f); // 1 ~ 100 사이의 실수 뽑기

            if (cri <= GameManager.Instance.player.Critical) // 크리티컬 확률에 속하면 데미지 1.5배
            {
                collision.GetComponent<Enemy>().Hit(damage: GameManager.Instance.player.Attack * 1.5f);

                Debug.Log("크리티컬!");
            }

            else // 아닐 때 그냥 공격
            {
                collision.GetComponent<Enemy>().Hit(damage: GameManager.Instance.player.Attack);
                Debug.Log("일반 공격");
            }
        }


    }
}
