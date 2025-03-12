using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BuyRelic : MonoBehaviour
{
    public ScriptableRelic myRelic; // 유물 SO
    private Button myBtn; // 슬롯 버튼
    public GameObject relic_Description; // 유물 구매창
    public Image relic_Icon; // 유물 아이콘
    public Text relic_Name; // 유물 이름
    public Text relic_Information; // 유물 정보
    public Button buyBtn; // 유물 구매 버튼
    public int relicCost; // 유물 가격
    public int costType; // 0 골드. 1 다이아

    void Awake()
    {
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(ShowDescription);
    }

    void ShowDescription()
    {
        // 구매창 초기화 및 버튼 이벤트 연결
        relic_Icon.sprite = myRelic.r_Icon;
        relic_Name.text = myRelic.r_Name;
        relic_Information.text = myRelic.r_Description;
        relic_Description.SetActive(true);

        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(ClickBuyButton);
    }

    void ClickBuyButton()
    {
        // 유물 구매 
        if(costType == 0)
        {
            if(GameManager.Instance.Gold >= relicCost)
            {
                GameManager.Instance.Gold -= relicCost;
                myRelic.r_Count ++;
            }
        }

        else
        {
            if(GameManager.Instance.Dia >= relicCost)
            {
                GameManager.Instance.Dia -= relicCost;
                myRelic.r_Count ++;
            }
        }

        myRelic.isDraw = true;

        ExitUI();
    }

    private void ExitUI()
    {
        UISoundManager.Instance.PlayBuySound();
        
        HideUI(relic_Description.transform);
    }

    public void HideUI(Transform obj)
    {
        obj.DOScale(0, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => obj.gameObject.SetActive(false)); 
    }
}
