using Unity.Netcode;
using UnityEngine;

public class PumkinSlash : NetworkBehaviour
{
    public Enemy _enemy;
    public GameObject prefab;
    public Animator anim;

    void OnEnable()
    {
        if(!IsServer) return;

        OnSlashClientRpc();
        anim.SetTrigger("Slash");
        Invoke("OffSlash", 1.5f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(!IsServer) return;

        if(other.GetComponent<Player>() != null)
        {  
            AttackClientRpc(other.GetComponent<NetworkObject>().OwnerClientId, _enemy.stat.attack);
        }
    }

    void OffSlash()
    {
        NetworkObjectPool.Instance.ReturnNetworkObject(GetComponent<NetworkObject>(), prefab);
        OffSlashClientRpc();
    }

    [ClientRpc]
    private void AttackClientRpc(ulong clientId, float damage)
    {
        // 공격 받은 클라이언트라면 Hit() 처리
        if (clientId == NetworkManager.Singleton.LocalClientId)
            GameManager.Instance.player.Hit(damage: damage);
    }

    [ClientRpc]
    private void OnSlashClientRpc()
    {
        this.gameObject.SetActive(true);
    }

    [ClientRpc]
    private void OffSlashClientRpc()
    {
        this.gameObject.SetActive(false);
    }
}
