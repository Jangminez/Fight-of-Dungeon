using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public Skill[] _skills;

    void Awake()
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            UIManager.Instance.buttons[i].onClick.AddListener(_skills[i].UseSkill);
            UIManager.Instance.buttons[i].image.sprite = _skills[i]._icon;
            _skills[i]._CD = UIManager.Instance.buttons[i].transform.parent.GetChild(1).GetComponent<Image>();
        }
    }
}
