using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        Debug.Log("Server: " + PhotonNetwork.CloudRegion + " - Ping: " + PhotonNetwork.GetPing());
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomTemp = "Room" + Random.Range(100, 10000);
        PhotonNetwork.CreateRoom(roomTemp);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected cause: " + cause);
    }
}
