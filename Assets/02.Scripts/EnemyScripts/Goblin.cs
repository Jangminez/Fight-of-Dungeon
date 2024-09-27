using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goblin : Enemy
{
    public GameObject _arrow;
    public Transform _tip;
    void Start()
    {
        InitMonster();
    }

    // 몬스터 초기화
    public override void InitMonster()
    {
        if (!stat.isDie)
            _initTransform = this.transform.position;

        else
        {
            _isAttack = false;
            transform.position = _initTransform;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = true;
            state = States.Idle;
            StartCoroutine("HitEffect");
            anim.SetTrigger("Respawn");
        }

        stat.maxHp = 1000f;
        Hp = stat.maxHp;

        stat.attack = 300f;
        stat.attackRange = 7f;
        stat.attackSpeed = 0.7f;

        stat.defense = 100f;

        stat.speed = 1.3f;
        stat.chaseRange = 10f;

        stat.exp = 1000f;
        stat.gold = 1300;

        stat.isDie = false;

        // 원거리 애니메이션
        anim.SetFloat("NormalState", 0.5f);

        StartCoroutine("MonsterState");
    }

    #region 피격 및 사망 처리

    public override void Hit(float damage)
    {
        float finalDamage = damage - stat.defense;
        if (finalDamage < 0f)
        {
            finalDamage = 1f;
        }

        Hp -= finalDamage;

        if(FloatingDamagePrefab != null && stat.hp > 0){
            ShowFloatingDamage(finalDamage);
        }

        StartCoroutine("HitEffect");

        if (stat.hp <= 0)
        {
            StopAllCoroutines();

            anim.SetTrigger("Die");
            Die();
        }
    }

    public override IEnumerator HitEffect()
    {
        SPUM_SpriteList spumList = transform.GetChild(0).GetComponent<SPUM_SpriteList>();
        List<SpriteRenderer> itemList = spumList._itemList;
        List<SpriteRenderer> armorList = spumList._armorList;
        List<SpriteRenderer> bodyList = spumList._bodyList;

        // 캐릭터의 Hair 색은 변경하지않음
        var filterItemList = itemList.Skip(2).ToList();

        foreach(var item in filterItemList)
        {
            item.color = Color.gray;
        }

        foreach(var armor in armorList)
        {
            armor.color = Color.gray;
        }

        foreach(var body in bodyList)
        {
            body.color = Color.gray;
        }

        yield return new WaitForSeconds(0.2f);

        foreach(var item in filterItemList)
        {
            item.color = Color.white;
        } 

        foreach(var armor in armorList)
        {
            armor.color = Color.white;
        }

        foreach(var body in bodyList)
        {
            body.color = Color.white;
        }
    }

    public override void Die()
    {
        Hp = 0f;
        stat.isDie = true;

        state = States.Die;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        GiveExpGold(GameManager.Instance.player);

        Invoke("InitMonster", 10f);
    }
    #endregion
    // 이동 애니메이션
    public override void Movement_Anim()
    {
        if(state == States.Chase || state == States.Return)
        {
            anim.SetFloat("RunState", 0.5f);
        }

        else
        {
            anim.SetFloat("RunState", 0f);
        }
    }

    public override IEnumerator EnemyAttack()
    {
        while(_isAttack)
        {
            // 공격시 방향 전환 및 애니메이션 실행
            SetDirection();
            anim.SetTrigger("Attack");

            // 공격속도 지연
            yield return new WaitForSeconds(1 / stat.attackSpeed);

            if(state != States.Attack) 
            {
                _isAttack = false;
                _target = null;
                yield break;
            }

            // 화살 생성 후 타겟 방향으로 회전 및 발사
            GameObject arrow = Instantiate(_arrow, _tip.transform.position, Quaternion.identity);
            arrow.GetComponent<EnemyArrow>()._enemy = this;
            Vector3 direction = (_target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            arrow.GetComponent<Rigidbody2D>().velocity = direction * 10f;
            Destroy(arrow, 1f);


        }
    }
}
