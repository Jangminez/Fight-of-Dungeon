using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public enum SkillType {Attack, Buff};
    public SkillType _skillType;

    public void UseSkill()
    {
        StartCoroutine(SkillProcess());
    }

    public abstract IEnumerator SkillProcess();

}
