using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Warrior_Skill3 : Skill
{
    [System.Serializable]
    struct SkillInfo
    {
        public float damage;
        public float coolDown;
        public float duration;
        public Collider2D collider;
        public bool isAttack;
        public List<Collider2D> montsterInRage;
    }
    [SerializeField]SkillInfo _info;

    void Awake()
    {
        _info.damage = 0.5f;
        _info.coolDown = 30f;
        _info.duration = 10f;
        _info.collider = GetComponent<Collider2D>();
        _info.collider.enabled = false;
        _info.montsterInRage = new List<Collider2D>();
    }
    void Update()
    {
        
    }
    public override IEnumerator SkillProcess()
    {
        StartCoroutine(CoolDown(_info.coolDown));

        _info.collider.enabled = true;
        yield return new WaitForSeconds(_info.duration);
        _info.collider.enabled = false;
        foreach(var anim in _anims)
        {
                anim.SetTrigger("StopSkill");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!_info.montsterInRage.Contains(other))
        {
            _info.montsterInRage.Add(other);
            StartCoroutine(SkillDamage(other));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _info.montsterInRage.Remove(other);
        //StopCoroutine(SkillDamage(other));
    }

    IEnumerator SkillDamage(Collider2D other)
    {
        var monster = other.GetComponent<Enemy>();
        while(_info.montsterInRage.Contains(other) && !GameManager.Instance.player.Die)
        {
            if(monster != null)
                monster.Hit(damage: GameManager.Instance.player.FinalAttack * _info.damage);
            yield return new WaitForSeconds(0.5f);
        }
        StopCoroutine(SkillDamage(other));
    }
}
