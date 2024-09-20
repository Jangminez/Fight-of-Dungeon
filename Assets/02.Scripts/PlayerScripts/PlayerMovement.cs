using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick _joystickMovement;
    float _speed;
    Rigidbody2D _playerRb;
    Animator _anim;

    void Start()
    {
        _joystickMovement = GameObject.FindWithTag("JoyStick").GetComponent<Joystick>();
        _playerRb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _speed = GameManager.Instance.player.Speed;
    }

    void FixedUpdate()
    {
        Movement();
    }

    void LateUpdate()
    {
        Movement_Anim();
    }
    
    public virtual void Movement()
    {
        if(_joystickMovement.Direction.y != 0)
        {
            Vector2 nextVec = new Vector2(_joystickMovement.Direction.x * _speed, _joystickMovement.Direction.y * _speed); 
            _playerRb.velocity = nextVec;

            SetDirection();

            if(nextVec ==  Vector2.zero)
            {
                _anim.SetFloat("RunState", 0f);
            }
        }

        else
        {
            _playerRb.velocity = Vector2.zero;
        }
    }

    // 플레이어 이동 애니메이션
    public virtual void Movement_Anim()
    {
        if(_joystickMovement.Direction.x !=0  || _joystickMovement.Direction.y !=0)
        {
            _anim.SetFloat("RunState", 0.5f);
        }

        else
        {
            _anim.SetFloat("RunState", 0f);
        }
    }

    void SetDirection()
    {
        if(_joystickMovement.Direction.x > 0)
        {
            _anim.transform.localScale = new Vector3(-1, 1, 1);
        }

        else if (_joystickMovement.Direction.x < 0)
        {
            _anim.transform.localScale = new Vector3(1, 1, 1);
        }
    }

}
