using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager inst;

    [HideInInspector]
    public List<Player> players = new();
    
    private int activePlayerCount = 0;
    
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            enabled = false;
        }
    }

    void Start()
    {
        if(!IsServer)
            enabled = false;

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            var player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id).GetComponent<Player>();
            players.Add(player);
            activePlayerCount++;
        };
    }

    public void PlayerDied(Player player)
    {
        activePlayerCount--;
        
        //move inactive player to end of list
        players.Remove(player);
        player.gameObject.SetActive(false);
        players.Add(player);
        
        if(activePlayerCount == 1)
            players[0].roundWinner.Value = true;
    }
    
}
