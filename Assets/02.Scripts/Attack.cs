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
            cri = Random.Range(1f, 101f); // 1 ~ 100 ������ �Ǽ� �̱�

            if (cri <= GameManager.Instance.player.Critical) // ũ��Ƽ�� Ȯ���� ���ϸ� ������ 1.5��
            {
                collision.GetComponent<Enemy>().Hit(damage: GameManager.Instance.player.Attack * 1.5f);

                Debug.Log("ũ��Ƽ��!");
            }

            else // �ƴ� �� �׳� ����
            {
                collision.GetComponent<Enemy>().Hit(damage: GameManager.Instance.player.Attack);
                Debug.Log("�Ϲ� ����");
            }
        }


    }
}
