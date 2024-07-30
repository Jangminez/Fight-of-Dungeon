using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionObject : MonoBehaviour
{
    [Tooltip("Upgrade or Shop")]
    [SerializeField]private string _objectName;

    public Transform _interactionCanvas;
    public Transform _interactionButton;

    public Transform _upgradeUI;
    public Transform _shopUI;

    private void Awake()
    {
        _interactionCanvas.gameObject.SetActive(false);
        _interactionButton.gameObject.SetActive(false);

        _interactionButton.GetComponent<Button>().onClick.AddListener(OpenUI);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(true);
            _interactionButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(false);
            _interactionButton.gameObject.SetActive(false);
        }
    }

    private void OpenUI()
    {
        switch( _objectName )
        {
            case "Upgrade":
                _upgradeUI.gameObject.SetActive(true);
                break;

            case "Shop":
                _shopUI.gameObject.SetActive(true);
                break;
        }
    }
}
