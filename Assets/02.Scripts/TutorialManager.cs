using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject _upgradeArrow;
    public GameObject _shopArrow;

    void Awake()
    {
        _upgradeArrow.SetActive(false);
        _shopArrow.SetActive(false);
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
}
