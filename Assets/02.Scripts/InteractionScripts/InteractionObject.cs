using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class InteractionObject : MonoBehaviour
{
    public Transform _interactionCanvas;
    public Transform _interactionButton;

    public Transform _myUI;

    private void Awake()
    {
        _interactionCanvas.gameObject.SetActive(false);
        _interactionButton.gameObject.SetActive(false);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(true);
            _interactionButton.gameObject.SetActive(true);
            _interactionButton.GetComponent<Button>().onClick.AddListener(OpenUI);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _interactionCanvas.gameObject.SetActive(false);
            _interactionButton.gameObject.SetActive(false);
            _interactionButton.GetComponent<Button>().onClick.RemoveAllListeners();          
        }
    }

    private void OpenUI()
    {
        _myUI.gameObject.SetActive(true);
    }
}
