using System.Collections;
using UnityEngine;

public class Warrior_Skill2 : Skill
{
    [System.Serializable]
    struct SkillInfo
    {
        public float damage; // 데미지  
        public float coolDown; // 쿨타임
    }
    [SerializeField]SkillInfo _info;

    void Awake()
    {
        // 스킬 정보 초기화
        _info.damage = 2f;
        _info.coolDown = 10f;
    }
    public override IEnumerator SkillProcess()
    {
        // 쿨타임 시작
        StartCoroutine(CoolDown(_info.coolDown));
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 몬스터가 스킬 범위에 들어오면 데미지 적용
        var monster = other.GetComponent<Enemy>();

        if(monster != null)
        {
            monster.Hit(damage: GameManager.Instance.player.FinalAttack * _info.damage);
        }
    }

}
