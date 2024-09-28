using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    bool isActive = false;
    public UnityEvent OnClick;

    public void ButtonClick()
    {
        if(!isActive){
            isActive = true;
            OnClick.Invoke();
        }
        else
            return;
    }
}
