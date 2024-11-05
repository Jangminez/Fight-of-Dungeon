using Unity.Netcode;
using UnityEngine;

public class EnemyArrow : NetworkBehaviour
{
    public Enemy _enemy;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>() != null )
        {  
            other.GetComponent<Player>().Hit(damage: _enemy.stat.attack);
            Destroy(gameObject);
        }

        else if(other.gameObject.layer == 22)
        {
            Destroy(gameObject);
        }
    }
}
