using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Particle : NetworkBehaviour
{
    public int damage = 10;
    public float maxLifetime = 3f;
    public float minLifetime = 1f;
    
    private void Start()
    {
        //get random lifetime
        var lifetime = Random.Range(minLifetime, maxLifetime);
        
        //call destroy after lifetime
        Invoke(nameof(SelfDestruct), lifetime);
    }

    void SelfDestruct()
    {
        //get network object and despawn it
        if(IsServer)
            GetComponent<NetworkObject>().Despawn(true);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && IsServer)
        {
            other.transform.GetComponent<Health>().hp.Value -= damage;
            SelfDestruct();
        }
            
    }
}
