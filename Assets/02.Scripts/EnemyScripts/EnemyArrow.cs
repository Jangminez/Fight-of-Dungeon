using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public Enemy _enemy;
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>() != null )
        {  
            GameManager.Instance.player.Hit(damage: _enemy.stat.attack);
            Destroy(gameObject);
        }
    }
}
