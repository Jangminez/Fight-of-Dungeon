using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HideUIEffect : MonoBehaviour
{
    private Button exitBtn;
    public GameObject obj;

    void Awake()
    {
        exitBtn = GetComponent<Button>();

        if(exitBtn != null)
            exitBtn.onClick.AddListener(ExitUI);
    }

    private void ExitUI()
    {
        HideUI(obj.transform);
    }

    public void HideUI(Transform obj)
    {
        obj.DOScale(0, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => obj.gameObject.SetActive(false)); 
    }
}
