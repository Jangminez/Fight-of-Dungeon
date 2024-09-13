using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public Skill[] _skills;
    public Button[] _buttons;
    public GameObject[] _locked;

    void Awake()
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            _buttons[i].onClick.AddListener(_skills[i].UseSkill);
            _buttons[i].image.sprite = _skills[i]._icon;
            _skills[i]._CD = _buttons[i].transform.parent.GetChild(1).GetComponent<Image>();
        }
    }

}
