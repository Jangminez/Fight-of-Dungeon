using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class InteractionObject : MonoBehaviour
{
    private enum objectName { Shop, Upgrade }
    [SerializeField]
    private objectName _openUI;

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
        if(_openUI == objectName.Upgrade)
        {
            _upgradeUI.gameObject.SetActive(true);
        }

        else if(_openUI == objectName.Shop)
        {
            _shopUI.gameObject.SetActive(true);
        }
    }
}
