using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfArea : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layer)
        {
            collision.gameObject.GetComponent<Enemy>().OutofArea();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == layer)
        {
            collision.transform.GetComponent<Enemy>().state = Enemy.States.Return;
        }
    }
}
