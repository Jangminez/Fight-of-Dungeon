using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float _destroyTime = 3f;
    public Vector3 _offset = new Vector3(0, 2f, 0);

    void Start() 
    {
        Destroy(gameObject, _destroyTime);

        transform.localPosition += _offset;
    }
}
