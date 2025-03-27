using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnEnable : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if(anim != null)
            anim.SetTrigger("Active");
    }
}
