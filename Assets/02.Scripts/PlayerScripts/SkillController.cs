using Unity.Netcode;
using UnityEngine.UI;

public class SkillController : NetworkBehaviour
{
    public Skill[] _skills;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
            return;
        }

        for (int i = 0; i < _skills.Length; i++)
        {
            UIManager.Instance.skillButtons[i].onClick.AddListener(_skills[i].UseSkill);
            UIManager.Instance.skillButtons[i].image.sprite = _skills[i]._icon;
            _skills[i]._CD = UIManager.Instance.skillButtons[i].transform.parent.GetChild(1).GetComponent<Image>();
        }
    }
}
