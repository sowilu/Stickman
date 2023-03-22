using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking.Transport;

public class Gun : NetworkBehaviour
{
    public NetworkVariable<int> ammo = new();
    public int maxAmmo = 15;
    public NetworkObject bulletPrefab;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform target;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
            ammo.Value = maxAmmo;
        else
            rb.simulated = false;
    }

    private void LateUpdate()
    {
        if (target != null && IsServer)
        {
            transform.position = target.position;
        }
    }
}
