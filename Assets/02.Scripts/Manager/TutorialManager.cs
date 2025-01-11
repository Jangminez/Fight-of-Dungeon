using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public GameObject _upgradeArrow;
    public GameObject _shopArrow;
    public Transform _slot1;
    public UnityEvent CheckItem;
    bool isFirst = false;

    void Awake()
    {
        _upgradeArrow.SetActive(false);
        _shopArrow.SetActive(false);
    }

    void Update()
    {
        if(_slot1.childCount == 1 && !isFirst)
        {
            isFirst = true;
            CheckItem.Invoke();
        }
    }

    public void UpgradeArrow()
    {
        if(_upgradeArrow.activeSelf)
            _upgradeArrow.SetActive(false);

        else 
            _upgradeArrow.SetActive(true);
    }

    public void ShopArrow()
    {
        if(_shopArrow.activeSelf)
            _shopArrow.SetActive(false);

        else 
            _shopArrow.SetActive(true);
    }

    public void GiveGold()
    {
        GameManager.Instance.player.Gold += 300;
    }
}
