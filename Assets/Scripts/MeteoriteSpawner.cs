using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class MeteoriteSpawner : NetworkBehaviour
{
    public float waitTime = 3f;
    
    public NetworkObject meteoritePrefab;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            if (!IsServer) enabled = false;
            
            Spawn();
        };
    }

    void Spawn(int count = 5)
    {
        //pass parameter to coroutine
        StartCoroutine(SpawnCoroutine(count));
    }
    
    IEnumerator SpawnCoroutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(Random.Range(waitTime / 3f, waitTime));
            
            //random position
            var position = new Vector3(Random.Range(-8f, 8f), transform.position.y, 0);
            
            var meteorite = Instantiate(meteoritePrefab, position, Quaternion.identity);
            meteorite.Spawn(true);
        }
    }
    
    
    
}
