using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Shooter : NetworkBehaviour
{
    private Gun gun;

    private List<Transform> gunsAround = new();

    private void Update()
    {
        //if e is pressed pick nearest gun
        if (Input.GetKeyDown(KeyCode.E) && gun == null && IsLocalPlayer)
        {
            PickUpServerRpc();
        }
        
        //if mouse pressed - shoot
        if (Input.GetMouseButtonDown(0) && IsLocalPlayer)
        {
            ShootServerRpc();
        }
        
        //if q is pressed - drop gun
        if (Input.GetKeyDown(KeyCode.Q) && gun != null && IsLocalPlayer)
        {
            DropServerRpc();
        }

        if (IsLocalPlayer)
        {
            LookServerRpc(Input.mousePosition);
        }
            
    }

    [ServerRpc]
    void DropServerRpc()
    {
        if (gun.ammo.Value <= 0)
        {
            gun.GetComponent<NetworkObject>().Despawn(true);
            return;
        }
            
        gun.name = "Gun:";
        gun.rb.simulated = true;
        gun.target = null;
        gun = null;
    }

    [ServerRpc]
    void ShootServerRpc()
    {
        if (gun == null || gun.ammo.Value <= 0) return;
        
        Instantiate(gun.bulletPrefab, gun.transform.position + gun.transform.right, gun.transform.rotation).Spawn(true);
        
        gun.ammo.Value--;
    }

    [ServerRpc]
    void LookServerRpc(Vector3 mousePos)
    {
        if (!gun) return;
        
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        
        var lookDir = worldPos - transform.position;

        gun.transform.right = lookDir;
    }

    [ServerRpc]
    void PickUpServerRpc()
    {
        if(gunsAround.Count == 0) return;
        
        var minDistance = float.MaxValue;
        foreach (var gunTransform in gunsAround)
        {
            var distance = Vector2.Distance(transform.position, gunTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                gun = gunTransform.GetComponent<Gun>();
            }
        }

        gun.name = "Picked Gun:" + transform.name;
        gun.rb.simulated = false;
        gun.target = transform;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gun"))
        {
            gunsAround.Add(other.transform);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Gun"))
        {
            gunsAround.Remove(other.transform);
        }
    }
}
