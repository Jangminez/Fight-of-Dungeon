using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 _inputVec;
    public float _speed;

    private Animator _animator;
    
    Rigidbody2D _playerRig;

    void Awake()
    {
        _playerRig = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
        _playerRig.MovePosition(_playerRig.position + nextVec);


        // 플레이어 이동 애니메이션
        if (_inputVec.x < 0)
        {
            _animator.SetInteger("Direction", 3);
        }
        else if (_inputVec.x > 0)
        {
            _animator.SetInteger("Direction", 2);
        }

        if (_inputVec.y > 0)
        {
            _animator.SetInteger("Direction", 1);
        }
        else if (_inputVec.y < 0)
        {
            _animator.SetInteger("Direction", 0);
        }
    }

    void OnMove(InputValue value)
    {
        _inputVec = value.Get<Vector2>();
    }
}
