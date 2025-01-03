using Unity.Netcode;
using UnityEngine;

public class Attack : NetworkBehaviour
{
    private Player player;
    float cri;
    bool isAttack = false;
    [SerializeField] bool isProjectile;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!IsOwner) return;

        if (col.GetComponent<IDamgeable>() != null && !isAttack)
        {
            player = GameManager.Instance.player;
            isAttack = true;
            cri = Random.Range(1f, 101f); // 1 ~ 100 확률 지정

            // cri의 값이 크리티컬 범위 안에 존재한다면 크리티컬 공격
            if(col.tag == "Player")
            {
                player.AttackPlayerServerRpc(damage:
                cri <= player.Critical ?
                player.FinalAttack * 1.5f :
                player.FinalAttack);
            }
            else
            {
                col.GetComponent<IDamgeable>().Hit(damage:
                cri <= player.Critical ?
                player.FinalAttack * 1.5f :
                player.FinalAttack);
            }
        }

        if(isProjectile)
        {
            GetComponent<Animator>().SetTrigger("Hit");
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(this.gameObject, 1f);
        }
    }
}
