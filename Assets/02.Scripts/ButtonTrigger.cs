using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    bool isActive = false;
    public string name;
    public UnityEvent OnClick;

    public void ButtonClick()
    {
        if(name == "Shop"){
            GameManager.Instance.player.Gold += 300;
        }
        if(!isActive){
            isActive = true;
            OnClick.Invoke();
        }
        else
            return;
    }
}
