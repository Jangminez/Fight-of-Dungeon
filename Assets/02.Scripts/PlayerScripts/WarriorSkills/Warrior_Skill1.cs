using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_Skill1 : Skill
{
    struct SkillInfo
    {
        public float attackUp;
        public float asUp;
        public float criUp;
        public float coolDown;
        public float duration;
    }
    SkillInfo _info;
    void Awake()
    {
        _isCoolDown = false;
        _info.attackUp = 30f;
        _info.asUp = 30f;
        _info.criUp = 10;
        _info.coolDown = 30f;
        _info.duration = 20f;
    }

    public override IEnumerator SkillProcess()
    {
        _isCoolDown = true;

        GameManager.Instance.player.AttackBonus += _info.attackUp;
        GameManager.Instance.player.AsBonus += _info.asUp;
        GameManager.Instance.player.Critical += _info.criUp;

        float timer = 0;

        while(timer <= _info.duration)
        {
            timer += 1;
        }

        GameManager.Instance.player.AttackBonus -= _info.attackUp;
        GameManager.Instance.player.AsBonus -= _info.asUp;
        GameManager.Instance.player.Critical -= _info.criUp;

        yield return new WaitForSeconds(_info.coolDown - _info.duration);
        _isCoolDown = false;
    }
}
