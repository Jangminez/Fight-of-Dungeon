using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleButton : MonoBehaviour
{
    private Button _myBtn;
    private Button _toggleBtn;
    private GameObject selectOb;
    public string _characterName;
    void Awake()
    {
        _myBtn = this.transform.GetComponent<Button>();
        if(_myBtn != null){
            _myBtn.onClick.AddListener(ClickSlot);   
        }

        _toggleBtn = this.transform.GetChild(2).GetComponent<Button>();
        _toggleBtn.onClick.AddListener(SelectButton);
        
    }
    void FixedUpdate()
    {
        selectOb = EventSystem.current.currentSelectedGameObject;

        if(_toggleBtn.gameObject.activeSelf && selectOb != _myBtn.gameObject && selectOb != _toggleBtn.gameObject ){
            _toggleBtn.gameObject.SetActive(false);
       }
    }

    // 슬롯 버튼 클릭시
    public void ClickSlot()
    {
            _toggleBtn.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_toggleBtn.gameObject);
    }

    public void SelectButton()
    {
        GameManager.Instance.playerPrefabName = _characterName;
        _toggleBtn.gameObject.SetActive(false);
    }
}
