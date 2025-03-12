using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomRelicShop : MonoBehaviour
{
    public Sprite[] price_Icons;    // 재화 아이콘 (골드, 다이아)
    public Transform[] slots; // 상점 슬롯
    public List<ScriptableRelic> shop_relics = new List<ScriptableRelic>(); // 슬롯에 존재하는 유물들
    public int[] random_goldCost; // 골드 가격 배열
    public int[] random_diaCost; // 다이아 가격 배열

    void Start()
    {
        SetRelicsToSlot();   
    }

    private void SetRelicsToSlot()
    {
        // 리스트 초기화    
        shop_relics.Clear();

        foreach(Transform slot in slots)
        {
            // 중복되지않는 유물 하나 뽑아서 slot에 설정
            ScriptableRelic relic = GetRandomRelic();
            int random_icon = Random.Range(0, 2);
            int random_cost = RandomCost(random_icon);

            slot.GetChild(0).GetComponent<Image>().sprite = relic.r_Icon;
            slot.GetChild(1).GetChild(0).GetComponent<Image>().sprite = price_Icons[random_icon];
            slot.GetChild(1).GetChild(1).GetComponent<Text>().text = $"{random_cost}";

            //리스트에 뽑은 항목 추가
            shop_relics.Add(relic);
            
            BuyRelic buyRelic = slot.GetComponent<BuyRelic>();

            buyRelic.myRelic = relic;
            buyRelic.relicCost = random_cost;
            buyRelic.costType = random_icon;
        }
    }

    private ScriptableRelic GetRandomRelic()
    {
        // 겹치지 않은 유물이 나오면 반환
        ScriptableRelic relic = RelicManager.Instance.GetRelic(Random.Range(101, 110));

        if(!shop_relics.Contains(relic))
            return relic;

        else
            return GetRandomRelic(); // 겹치면 재귀
    }

    private int RandomCost(int n)
    {
        // 각 재화에 맞는 금액 랜덤 설정
        if(n == 0)
        {
            return random_goldCost[Random.Range(0, random_goldCost.Length)];
        }

        else
        {
            return random_diaCost[Random.Range(0, random_goldCost.Length)];
        }
    }

    
}
