using System.Collections;
using UnityEngine;

public class Warrior_Skill3 : Skill
{
    [System.Serializable]
    struct SkillInfo
    {
        public float damage;
        public float coolDown;
    }
    [SerializeField]SkillInfo _info;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override IEnumerator SkillProcess()
    {
        throw new System.NotImplementedException();
    }
}
