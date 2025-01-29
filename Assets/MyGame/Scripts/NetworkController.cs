using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    bool isConnected = false;

    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected) 
        {
            if (Input.GetKeyDown(KeyCode.Q) )
            {
                PhotonNetwork.Disconnect();
            }
        }
        if (!isConnected)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                PhotonNetwork.ConnectToRegion("eu");
            }
        }
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        Debug.Log("Server: " + PhotonNetwork.CloudRegion + " - Ping: " + PhotonNetwork.GetPing());
        isConnected = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected cause: " + cause);
        isConnected = false;

    }
}
