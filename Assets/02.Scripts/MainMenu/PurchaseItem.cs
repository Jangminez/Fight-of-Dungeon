using UnityEngine;
using UnityEngine.UI;

public class PurchaseItem : MonoBehaviour
{
    private Button myBtn;
    public GameObject purchaseInfo;
    public Button pButton;
    public enum ItemType { Gold, Dia, Relic}
    public ItemType itemType;
    public int itemValue;
    public enum CostType { Gold, Dia, Money}
    public CostType costType;
    public int itemCost;

    void Awake()
    {
        myBtn = GetComponent<Button>();
        if(myBtn != null)
        {
            myBtn.onClick.AddListener(ClickBtn);
        }
    }

    private void ClickBtn()
    {
        purchaseInfo.SetActive(true);
        UISoundManager.Instance.PlayClickSound();

        if (pButton != null)
        {
            pButton.onClick.RemoveAllListeners();
            pButton.onClick.AddListener(BuyItem);
        }
    }

    private void BuyItem()
    {
        UISoundManager.Instance.PlayBuySound();

        switch (costType)
        {
            case CostType.Gold:
                if (GameManager.Instance.Gold >= itemCost)
                {
                    GameManager.Instance.Gold -= itemCost;
                }
                break;

            case CostType.Dia:
                if (GameManager.Instance.Dia >= itemCost)
                {
                    GameManager.Instance.Dia -= itemCost;
                }
                break;
        }

        switch (itemType)
        {
            case ItemType.Gold:
                GameManager.Instance.Gold += itemValue;
                break;

            case ItemType.Dia:
                GameManager.Instance.Gold += itemValue;
                break;

            case ItemType.Relic:
                RelicManager.Instance.GetRelic(itemValue).r_Count ++;
                break;
        }
        purchaseInfo.SetActive(false);
    }
}


