using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [Header("GameObjects")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject matchPanel;

    [Header("Player")]

    [SerializeField] private TMP_InputField playerNameInput;
    private string tempPlayerName = "Player";
    [SerializeField] private GameObject player;

    [Header("Room")]
    [SerializeField] private TMP_InputField roomNameInputField;



    void Start()
    {
        // PhotonNetwork.ConnectUsingSettings();
        //tempPlayerName = "Player" + Random.Range(1, 1000);
        // playerNameInput.text = tempPlayerName;
        ChangePanelsStates(true, false);
    }

    public void Login()
    {
        if (playerNameInput.text != "")
        {
            PhotonNetwork.NickName = playerNameInput.text;
        }
        else
        {
            tempPlayerName = "Player" + Random.Range(1, 1000);
            PhotonNetwork.NickName = tempPlayerName;
        }
        PhotonNetwork.ConnectUsingSettings();
        ChangePanelsStates(false, true);
    }

    private void ChangePanelsStates(bool login, bool match)
    {
        loginPanel.SetActive(login);
        matchPanel.SetActive(match);
    }

    public void QuickFindMatch() 
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom() 
    {
        string tempRoomName = roomNameInputField.text;
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 6 };
        PhotonNetwork.JoinOrCreateRoom(tempRoomName, roomOptions, TypedLobby.Default);
    }

    #region PunCallbacks
    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        Debug.Log("Server: " + PhotonNetwork.CloudRegion + " - Ping: " + PhotonNetwork.GetPing());
       // PhotonNetwork.JoinLobby();
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
        Debug.Log("OnJoinedRoom" );
        Debug.Log("Current room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Current player in room: " + PhotonNetwork.CurrentRoom.PlayerCount);

        ChangePanelsStates(false, false);
       // Instantiate(player);
        PhotonNetwork.Instantiate(player.name, player.transform.position, player.transform.rotation);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected cause: " + cause);
    }
    #endregion
}
