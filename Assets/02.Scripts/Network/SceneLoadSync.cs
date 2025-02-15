using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SceneLoadSync : NetworkBehaviour
{
    public static SceneLoadSync Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public NetworkVariable<int> playersLoaded = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        if(IsClient)
        {
            SceneLoadedServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SceneLoadedServerRpc()
    {
        playersLoaded.Value++;
    }
}
