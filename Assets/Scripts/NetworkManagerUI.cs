using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking.Types;

public class NetworkManagerUI : MonoBehaviour
{
    public void Host() => NetworkManager.Singleton.StartHost();

    public void Join() => NetworkManager.Singleton.StartClient();
}
