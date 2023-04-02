using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    private bool destructOnInpact = true;

    public int damage = 5;
    public float lifeTime = 15;
    public float speed = 10;
    
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        
        Invoke(nameof(SelfDestruct), lifeTime);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && IsServer)
        {
            other.gameObject.GetComponent<Health>().hp.Value -= damage;

            if (destructOnInpact)
            {
                SelfDestruct();
            }
        }
            
    }
    
    void SelfDestruct()
    {
        //TODO: add effects
        if(IsServer)
            GetComponent<NetworkObject>().Despawn(true);
    }
    
}
