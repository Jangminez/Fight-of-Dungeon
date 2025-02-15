using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTutorial : MonoBehaviour
{
    private Button _tutorialBtn;
    
    void Start()
    {
        _tutorialBtn = GetComponent<Button>();
        _tutorialBtn.onClick.AddListener(ClickTutorialButton);
    }

    private void ClickTutorialButton()
    {
        GameManager.Instance.StartTutorial();
    }
}
