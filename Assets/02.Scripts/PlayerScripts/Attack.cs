using UnityEngine;

public class Attack : MonoBehaviour
{
    float cri;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            cri = Random.Range(1f, 101f); // 1 ~ 100 확률 지정

            collision.GetComponent<Enemy>().Hit(damage:
            cri <= GameManager.Instance.player.Critical ?
            GameManager.Instance.player.FinalAttack * 1.5f :
            GameManager.Instance.player.FinalAttack);
        }
    }
}
