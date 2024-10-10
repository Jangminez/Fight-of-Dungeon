using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Warrior_BasicAttack : PlayerAttackController
{
    void Update()
    {
        if (!IsOwner) return;

        Attack();
    }

    public override IEnumerator BasicAttack()
    {
        if (!IsOwner)
        {
            yield break;
        }

        while (_isAttack)
        {
            yield return new WaitForSeconds(1 / player.FinalAS);

            if (player._target == null) // 플레이어의 타겟이 없으면 공격 중지
            {
                _isAttack = false;
                yield break;
            }
            // 공격 애니메이션
            _anim.SetFloat("AttackState", 0f);
            _anim.SetFloat("NormalState", 0f);
            _anim.SetTrigger("Attack");

            // 타겟의 위치에 공격 이펙트 생성
            SpawnAttackServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnAttackServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject attack = Instantiate(_basicAttack.gameObject);
        attack.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        SetAttackClientRpc(attack.GetComponent<NetworkObject>().NetworkObjectId);
        Destroy(attack, 0.5f);
    }

    [ClientRpc]
    private void SetAttackClientRpc(ulong objectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out var attackObject))
        {
            if (attackObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                Attack attack = attackObject.GetComponent<Attack>();
                attack.player = GameManager.Instance.player;
                attack.GetComponent<Animator>().SetFloat("Attack", Random.Range(0, 2)); // 공격 이펙트 랜덤 설정
                attack.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SortingGroup>().sortingLayerName;
                Destroy(attack, 0.5f);
                attack.transform.position = player._target.transform.position;
            }
        }
    }

}
