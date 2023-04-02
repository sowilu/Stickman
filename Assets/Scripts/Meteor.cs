using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Meteor : NetworkBehaviour
{
    public int damage = 30;
    public List<NetworkObject> explosionParticles;
    public float speed = 10;
    public GameObject explosion;
    
    Rigidbody2D rb;

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("Player") && IsServer)
            other.transform.GetComponent<Health>().hp.Value -= damage;
        
        if(explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);
        
        CameraShake.inst.Shake();
        
        if (IsServer)
        {
            foreach (var particle in explosionParticles)
            {
                var position = transform.position + Random.Range(-1f, 1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.up;
                
                var p = Instantiate(particle, position, Quaternion.identity);
                p.Spawn(true);
                
                var direction = Random.Range(-1f, 1f) * Vector2.right + Vector2.up;
                //print(direction);
                p.GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);
            }
            
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
