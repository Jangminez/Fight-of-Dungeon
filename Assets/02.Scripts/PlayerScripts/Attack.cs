using Unity.Netcode;
using UnityEngine;

public class Attack : NetworkBehaviour
{
    public Player player;
    float cri;
    bool isAttack = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null && !isAttack)
        {
            isAttack = true;
            cri = Random.Range(1f, 101f); // 1 ~ 100 확률 지정

            // cri의 값이 크리티컬 범위 안에 존재한다면 크리티컬 공격

            collision.GetComponent<Enemy>().Hit(damage:
            cri <= player.Critical ?
            player.FinalAttack * 1.5f :
            player.FinalAttack);
        }
    }
}
