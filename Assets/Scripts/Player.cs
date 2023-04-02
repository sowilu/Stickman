using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    public static Player localPlayer;
    
    public GameObject crown;
    private Health health;
    SpriteRenderer spriteRenderer;

    public NetworkVariable<Color> color = new(readPerm:NetworkVariableReadPermission.Everyone, writePerm:NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> roundWinner = new();


    public NetworkVariable<bool> isVisible = new();

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
            localPlayer = this;
        
        if(IsServer)
            color.Value = Random.ColorHSV(0, 1, 0.7f, 0.7f, 1, 1);
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        spriteRenderer.color = color.Value;
        transform.name = NetworkObjectId.ToString();
        
        roundWinner.OnValueChanged += (old, current) =>
        {
            crown.SetActive(current);
        };
        
        isVisible.OnValueChanged += (old, current) =>
        {
            //spriteRenderer.enabled = current;
            gameObject.SetActive(current);
        };
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Lava") && IsServer)
        {
            health.hp.Value -= 10;
        }
    }

    public void Die()
    {
        PlayerManager.inst.PlayerDied(this);
    }
}
