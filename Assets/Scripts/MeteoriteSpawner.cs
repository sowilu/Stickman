using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MeteoriteSpawner : NetworkBehaviour
{
    public float waitTime = 3f;
    
    public NetworkObject meteoritePrefab;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            if (!IsServer) enabled = false;
            
            Invoke(nameof(Spawn), waitTime);
        };
    }

    void Spawn()
    {
        var meteorite = Instantiate(meteoritePrefab, transform.position, Quaternion.identity);
        meteorite.Spawn(true);
    }
}
