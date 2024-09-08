using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public Skill[] _skills;
    public Button[] _buttons;

    void Start()
    {
        for (int i = 0; i < _skills.Length; i++)
        {
            _buttons[i].onClick.AddListener(() => _skills[i].UseSkill());
        }
    }
}
