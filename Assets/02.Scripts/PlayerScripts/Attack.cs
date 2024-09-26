using UnityEngine;

public class Attack : MonoBehaviour
{
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
            cri <= GameManager.Instance.player.Critical ?
            GameManager.Instance.player.FinalAttack * 1.5f :
            GameManager.Instance.player.FinalAttack);
        }
    }
}
