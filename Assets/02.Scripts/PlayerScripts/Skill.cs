using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public abstract class Skill : MonoBehaviour
{
    //protected enum SkillType {Attack, Buff};
    protected List<Animator> _anims = new List<Animator>();
    protected bool _isCoolDown;
    public Sprite _icon;
    public Image _CD;
    Text _cdText;

    void Start()
    {
        _anims.Add(this.GetComponent<Animator>());

        if(transform.childCount != 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                _anims.Add(transform.GetChild(i).GetComponent<Animator>());
            }
        }

        _cdText = _CD.transform.GetChild(0).GetComponent<Text>();
    }

    public void UseSkill()
    {
        if(!_isCoolDown)
        {
            foreach(var anim in _anims)
            {
                anim.SetTrigger("UseSkill");
            }

            StartCoroutine(SkillProcess());
        }
    }

    public abstract IEnumerator SkillProcess();

    public virtual IEnumerator CoolDown(float cd)
    {
        _isCoolDown = true;
        _cdText.gameObject.SetActive(true);
        
        float timer = 0f;
    
        while(timer < cd)
        {
            timer += 0.1f;
            _CD.fillAmount = (cd - timer) / cd;
            _cdText.text = (cd - timer).ToString();
            yield return new WaitForSeconds(0.1f);
        }

        _isCoolDown = false;
        _cdText.gameObject.SetActive(false);
    }
}
