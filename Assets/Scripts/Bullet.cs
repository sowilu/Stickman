using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    //TODO: damage
    public float lifeTime = 15;
    public float speed = 10;
    
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        
        Invoke(nameof(SelfDestruct), lifeTime);
    }
    
    void SelfDestruct()
    {
        //TODO: add effects
        if(IsServer)
            GetComponent<NetworkObject>().Despawn(true);
    }
    
}
