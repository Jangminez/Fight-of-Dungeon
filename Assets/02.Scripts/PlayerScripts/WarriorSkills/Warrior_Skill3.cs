using System.Collections;
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
    }
    [SerializeField]SkillInfo _info;

    void Awake()
    {
        _info.damage = 0.8f;
        _info.coolDown = 30f;
        _info.duration = 10f;
        _info.collider = GetComponent<Collider2D>();
        _info.collider.enabled = false;
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
    }

    void OnTriggerStay2D(Collider2D other)
    {
        var monster = other.GetComponent<Enemy>();
      

    }
}
