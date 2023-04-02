using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bounds : NetworkBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && IsServer)
        {
            other.gameObject.GetComponent<Health>().hp.Value = 0;
        }
    }
}
