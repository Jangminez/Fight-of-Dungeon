using UnityEngine;

public class PlayerFindTarget : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Collider2D[] enemys;
    [SerializeField] private Collider2D _target;


    private void Update()
    {
        enemys = Physics2D.OverlapCircleAll(transform.position, GameManager.Instance.player.AttackRange, layer);

        if (enemys.Length > 0)
        {
            float closeEnemy1 = Vector2.Distance(transform.position, enemys[0].transform.position);

            foreach (Collider2D coll in enemys)
            {
                float closeEnemy2 = Vector2.Distance(transform.position, coll.transform.position);

                if (closeEnemy1 >= closeEnemy2)
                {
                    closeEnemy1 = closeEnemy2;
                    GameManager.Instance.player._target = coll.transform;
                }
            }
        }

        else
        {
            GameManager.Instance.player._target = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, GameManager.Instance.player.AttackRange);
    }
}

