using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GunSpawner : NetworkBehaviour
{
    public NetworkObject gunPrefab;
    
    public Vector2 spawnRange = new Vector2(-8, 8);

    public float waitTime = 1;

    public float spawnRate = 0.1f;
    
    void Start()
    {
        NetworkManager.OnServerStarted += () =>
        {
            if (!IsServer) enabled = false;

            InvokeRepeating(nameof(Spawn), waitTime, 1 / spawnRate);
        };
    }

    void Spawn()
    {
        var pos  = transform.position + Vector3.right * Random.Range(spawnRange.x, spawnRange.y);
        Instantiate(gunPrefab, pos, Quaternion.identity).Spawn(true);
    }
}
