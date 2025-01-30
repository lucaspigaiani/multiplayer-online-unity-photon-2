using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    private Hashtable gameMode = new Hashtable();
    [SerializeField] private byte gameMaxPlayer = 4;
    private string gameModeKey = "gamemode";


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
        string[] gameModeRandom = new string[]
        {
            "PVP",
            "PVE"
        };

        gameMode.Add(gameModeKey, gameModeRandom[Random.Range(0, gameModeRandom.Length)]);
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom() 
    {
        string tempRoomName = roomNameInputField.text;
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom(tempRoomName, roomOptions, TypedLobby.Default);
    }

    public void PvpMatch() 
    {
        gameMode.Add(gameModeKey, "PVP");
        PhotonNetwork.JoinLobby();
    }

    public void PveMatch ()
    {
        gameMode.Add(gameModeKey, "PVE");
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ShowRoomsList(roomList);
    }

    private void ShowRoomsList(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
            Debug.Log($"Room name: {room.Name}, IsOpen: {room.IsOpen}, IsVisible: {room.IsVisible}, " +
                $"MaxPlayers: {room.MaxPlayers}, PlayerCount: {room.PlayerCount}, CustomProperties:" +
                $" {(room.CustomProperties.TryGetValue(gameModeKey, out object temp) ? temp.ToString() : "N/A")}");
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
        PhotonNetwork.JoinRandomRoom(gameMode, gameMaxPlayer);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomTemp = "Room" + Random.Range(100, 10000);
        
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = gameMaxPlayer;
        options.CustomRoomProperties = gameMode;
        options.CustomRoomPropertiesForLobby = new string[] { gameModeKey };
        PhotonNetwork.CreateRoom(roomTemp, options);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom" );
        Debug.Log("Current room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Current player in room: " + PhotonNetwork.CurrentRoom.PlayerCount);

        ChangePanelsStates(false, false);

        object gameValueType;

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(gameModeKey, out gameValueType))
        {
            Debug.Log("gameValueType: " + gameValueType.ToString());
        }

        foreach (var item in PhotonNetwork.PlayerList)
        {
            Debug.Log("Name: " + item.NickName);
            Debug.Log("IsMaster: " + item.IsMasterClient);

            Hashtable playerCustom = new Hashtable();
            playerCustom.Add("lives", 3);
            playerCustom.Add("score", 0);

            item.SetCustomProperties(playerCustom);
        }

        PhotonNetwork.Instantiate(player.name, player.transform.position, player.transform.rotation);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected cause: " + cause);
    }
    #endregion
}
