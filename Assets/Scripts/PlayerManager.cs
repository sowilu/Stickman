using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (!IsServer)
             enabled = false;

         NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
         {
             var player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id).GetComponent<Player>();
             players.Add(player);
             activePlayerCount++;
         };
         
        NetworkManager.OnServerStarted += () =>
        {
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
        };
    }

    void OnSceneEvent(SceneEvent sceneEvent)
    {
        print(sceneEvent.SceneEventType);

        if (sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted)
        {
            //find object named spawnpoint
            var spawnPoint = GameObject.Find("SpawnPoint");
            
            if (spawnPoint == null)
            {
                Debug.LogError("No spawnpoint found");
                return;
            }
            
            var pos = spawnPoint.transform.position;
            foreach (var player in players)
            {
                player.transform.position = pos;
                pos += Vector3.right;

                var health = player.GetComponent<Health>();
                health.hp.Value = health.maxHp;
                
                //player.gameObject.SetActive(true);
                player.isVisible.Value = true;
            }
        }
    }

    public void PlayerDied(Player player)
    {
        activePlayerCount--;
        
        //move inactive player to end of list
        players.Remove(player);
        player.gameObject.SetActive(false);
        player.roundWinner.Value = false; 
        players.Add(player);

        if (activePlayerCount == 1)
        {
            players[0].roundWinner.Value = true;
            
            Invoke(nameof(LoadNextScene), 3f);
        }
            
    }
    
    void LoadNextScene()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    
}
