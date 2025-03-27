using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    private Button _tutorialBtn;
    public string loadSceneName;

    void Start()
    {
        _tutorialBtn = GetComponent<Button>();
        _tutorialBtn.onClick.AddListener(ClickSceneStartButton);
    }

    private void ClickSceneStartButton()
    {
        GameManager.Instance.StartScene(loadSceneName);
    }
}
