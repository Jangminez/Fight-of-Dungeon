using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    private CameraFollow _cameraFollow;
    private Animator _anim;

    void Awake()
    {
        _cameraFollow = GetComponent<CameraFollow>();
        _anim = GetComponent<Animator>();
    }

    public void MoveCam()
    {
        _cameraFollow.enabled = false;
        _anim.enabled = true;
        _anim.SetTrigger("Move");
    }

    public void NormalCam()
    {
        _cameraFollow.enabled = true;
        _anim.enabled = false;
    }
}
