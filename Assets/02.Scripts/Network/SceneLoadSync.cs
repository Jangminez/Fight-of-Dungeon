using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneLoadSync : NetworkBehaviour
{
    public NetworkVariable<bool> IsLoaded = new NetworkVariable<bool>(false);

    private void Start()
    {
        if (IsOwner)
        {
            StartCoroutine(CheckLoadingProgress());
        }
    }

    private IEnumerator CheckLoadingProgress()
    {
        while (SceneLoadManager.Instance.loadingProgress.Value < 1f)
        {
            yield return null;
        }

        IsLoaded.Value = true;
    }
}
