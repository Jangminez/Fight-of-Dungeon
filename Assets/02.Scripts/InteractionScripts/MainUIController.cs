using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public Button startButton;
    public Text nameText;
    public Text goldText;
    public Text diaText;
    public Text levelText;
    public Slider expSlider;

    void Start()
    {
        startButton.onClick.AddListener(GameLobby.Instance.QuickJoinLobby);
        GameManager.Instance.mainUI = this;
    }

    public void SetNickName(string name)
    {
        nameText.text = name;
    }

    public void SetGold(int gold)
    {
        goldText.text = gold.ToString();
    }

    public void SetDia(int dia)
    {
        diaText.text = dia.ToString();
    }

    public void SetLevel(int level)
    {
        levelText.text = $"Lv. {level}";
    }

    public void SetExpBar(float exp, float nextExp)
    {
        expSlider.value = exp / nextExp;
    }
}
