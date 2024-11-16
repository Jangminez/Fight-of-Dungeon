using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMonsterSpawner : NetworkBehaviour
{
    static NetworkMonsterSpawner _instance;
    public static NetworkMonsterSpawner Instance {get {return _instance;} }
    public List<SpawnArea> spawnAreas;
    private Dictionary<GameObject, List<NetworkObject>> activeMonsters = new Dictionary<GameObject, List<NetworkObject>>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        foreach (var area in spawnAreas)
        {
            activeMonsters[area.monsterPrefab] = new List<NetworkObject>();
            StartCoroutine(SpawnMonstersInArea(area));
        }
    }

    private IEnumerator<WaitForSeconds> SpawnMonstersInArea(SpawnArea area)
    {
        if (!IsServer) yield break;

        while (true)
        {
            if (NetworkObjectPool.Instance.isInitialized && activeMonsters[area.monsterPrefab].Count < area.maxCount)
            {
                Vector3 spawnPosition = GetRandomPositionInRange(area.spawnCenter, area.spawnRadius);
                NetworkObject monster = NetworkObjectPool.Instance.GetNetworkObject(area.monsterPrefab, spawnPosition, Quaternion.identity);

                if (!monster.IsSpawned)
                {
                    monster.GetComponent<Enemy>().prefab = area.monsterPrefab;
                    monster.Spawn();

                    yield return null;
                    monster.transform.SetParent(area.spawnCenter.parent);
                }
                
                monster.GetComponent<Enemy>().InitMonster();
                activeMonsters[area.monsterPrefab].Add(monster);
            }
            yield return new WaitForSeconds(4f);
        }
    }

    private Vector3 GetRandomPositionInRange(Transform center, float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        return new Vector3(center.position.x + randomPoint.x, center.position.y + randomPoint.y, 0f);
    }

    public void DespawnMonster(NetworkObject monster, GameObject prefab)
    {
        if(!IsServer) return;

        if (activeMonsters.ContainsKey(prefab) && activeMonsters[prefab].Contains(monster))
        {
            activeMonsters[prefab].Remove(monster);
            NetworkObjectPool.Instance.ReturnNetworkObject(monster, prefab);
            //monster.Despawn();
        }
    }

    [System.Serializable]
    public class SpawnArea
    {
        public GameObject monsterPrefab;
        public int maxCount;
        public Transform spawnCenter;
        public float spawnRadius;
    }
}

