using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Attack : NetworkBehaviour
{
    public enum attackType { BasicAttack, Projectile, Skill }
    public attackType type;
    private Player player;
    float cri;
    bool isAttack = false;
    public float skillDamage;

    void Awake()
    {
        skillDamage = 1f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!IsOwner) return;

        if (col.GetComponent<IDamgeable>() != null && !isAttack)
        {
            player = GameManager.Instance.player;

            if (type != attackType.Skill)
            {
                isAttack = true;
            }
            
            cri = Random.Range(1f, 101f); // 1 ~ 100 확률 지정

            // cri의 값이 크리티컬 범위 안에 존재한다면 크리티컬 공격
            if (col.tag == "Player")
            {
                player.AttackPlayerServerRpc(damage:
                cri <= player.Critical ?
                player.FinalAttack * 1.5f * skillDamage :
                player.FinalAttack * skillDamage);
            }
            else
            {
                col.GetComponent<IDamgeable>().Hit(damage:
                cri <= player.Critical ?
                player.FinalAttack * 1.5f * skillDamage :
                player.FinalAttack * skillDamage);
            }

            if (type == attackType.Projectile)
            {
                GetComponent<Animator>().SetTrigger("Hit");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(this.gameObject, 1f);
            }
        }
    }
}
