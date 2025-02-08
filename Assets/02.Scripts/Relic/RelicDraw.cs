using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicDraw : MonoBehaviour
{
    private Button myBtn;
    public ScriptableRelic[] relics;
    public GameObject drawEffect;
    public GameObject drawInfo;
    public Text drawText;
    public Image relicIcon;
    public Text relicName;
    public Text relicCount;
    public Text relicLevel;
    public Image relicBar;

    void Awake()
    {
        relics = Resources.LoadAll<ScriptableRelic>("Relics");
        myBtn = GetComponent<Button>();
        
        if(myBtn != null)
        {
            myBtn.onClick.AddListener(DrawRelic);
        }
    }

    private int GetRandomRelic()
    {
        return Random.Range(0, relics.Length);
    }

    public void DrawRelic()
    {
        drawEffect.transform.parent.gameObject.SetActive(true);
        StartCoroutine(StartDrawRelic());
    }

    IEnumerator StartDrawRelic()
    {
        ScriptableRelic drawRelic = relics[GetRandomRelic()];
        Debug.Log(drawRelic.r_Name);

        drawRelic.r_Count += 1;

        drawEffect.SetActive(true);

        drawEffect.GetComponent<Animator>().SetTrigger("Draw");
        drawText.text = "유물 뽑는 중";
        yield return new WaitForSeconds(0.3f);
        drawText.text = "유물 뽑는 중.";
        yield return new WaitForSeconds(0.3f);
        drawText.text = "유물 뽑는 중..";
        yield return new WaitForSeconds(0.2f);
        drawText.text = "유물 뽑는 중...";
        yield return new WaitForSeconds(0.2f);

        drawEffect.SetActive(false);

        relicIcon.sprite = drawRelic.r_Icon;
        relicName.text = drawRelic.r_Name;
        relicCount.text = $"{drawRelic.r_Count} / {drawRelic.r_UpgradeCount}";
        relicLevel.text = $"Lv. {drawRelic.r_Level}";
        relicBar.fillAmount = drawRelic.r_Count / drawRelic.r_UpgradeCount;

        drawInfo.SetActive(true);
        
        yield return null;

        
    }
}
