using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public Button startButton;
    public Text goldText;
    public Text diaText;
    public Text levelText;
    public Slider expSlider;

    void Start()
    {
        startButton.onClick.AddListener(GameLobby.Instance.QuickJoinLobby);
        GameManager.Instance.mainUI = this;

        SetAllUI();
    }

    void SetAllUI()
    {
        SetGold();
        SetDia();
        SetLevel();
        SetExpBar();
    }
    public void SetGold()
    {
        goldText.text = GameManager.Instance.Gold.ToString();
    }

    public void SetDia()
    {
        diaText.text = GameManager.Instance.Dia.ToString();
    }

    public void SetLevel()
    {
        levelText.text = $"Lv. {GameManager.Instance.Level}";
    }

    public void SetExpBar()
    {
        expSlider.value = GameManager.Instance.Exp / GameManager.Instance.NextExp;
    }
}
