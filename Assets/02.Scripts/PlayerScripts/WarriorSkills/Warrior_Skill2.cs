using System.Collections;
using UnityEngine;

public class Warrior_Skill2 : Skill
{
    [System.Serializable]
    struct SkillInfo
    {
        public float damage;
        public float coolDown;
    }
    [SerializeField]SkillInfo _info;

    void Awake()
    {
        _info.damage = 2f;
        _info.coolDown = 10f;
    }
    public override IEnumerator SkillProcess()
    {
        StartCoroutine(CoolDown(_info.coolDown));

        yield return null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var monster = other.GetComponent<Enemy>();

        if(monster != null)
        {
            monster.Hit(damage: GameManager.Instance.player.FinalAttack * _info.damage);
        }
    }

}
