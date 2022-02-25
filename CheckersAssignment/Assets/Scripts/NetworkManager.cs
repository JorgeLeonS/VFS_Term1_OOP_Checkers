using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public NetworkBoard networkBoard;

    int playerCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connecting");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        //Player Counter variable is not being taken by the server, only locally
        networkBoard.CreateBoardSections();
        if (playerCounter == 0)
        {
            networkBoard.GenerateRedPieces();
        }
        else
        {
            networkBoard.GenerateBlackPieces();
        }
        playerCounter++;
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
