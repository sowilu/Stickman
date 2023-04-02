using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Networking.Types;

public class NetworkManagerUI : MonoBehaviour
{
    public TMP_InputField ip;
    public TMP_InputField port;

    private void Start()
    {
        GetPublicIp();
    }

    public void Host()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Port = ushort.Parse(port.text);
        NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ip.text;
        transport.ConnectionData.Port = ushort.Parse(port.text);
        
        NetworkManager.Singleton.StartClient();
    }

    public void GetPublicIp()
    {
        StartCoroutine(GetPublicIpCoroutine());
    }

    IEnumerator GetPublicIpCoroutine()
    {
        var www = new WWW("https://api.my-ip.io/ip");
        yield return www;
        ip.text = www.text;
    }
}
