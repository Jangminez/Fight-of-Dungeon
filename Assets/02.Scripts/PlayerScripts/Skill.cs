using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    //protected enum SkillType {Attack, Buff};
    protected bool _isCoolDown;

    public void UseSkill()
    {
        if(!_isCoolDown)
            StartCoroutine(SkillProcess());
    }

    public abstract IEnumerator SkillProcess();
}
