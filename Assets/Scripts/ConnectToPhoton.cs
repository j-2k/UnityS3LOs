using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectToPhoton : MonoBehaviourPunCallbacks
{
    public GameObject ConnectingMenu;
    public GameObject MainMenu;

    public GameObject playerPrefab;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        print("Connecting...");
    }

    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinLobby(TypedLobby.Default);
        print("Connected");
        ConnectingMenu.SetActive(false);
        MainMenu.SetActive(true);

    }

    public override void OnJoinedLobby()
    {
        print("On Joined Lobby");



    }

    public override void OnJoinedRoom()
    {
        print("On Joined Room");
        gameObject.SetActive(false);
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero + Vector3.up * 3.4f, playerPrefab.transform.rotation);
        playerPrefab.transform.Find("PlayerCamera").gameObject.SetActive(true);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("DisconnectFrom Photon");
        ConnectingMenu.SetActive(true);
        MainMenu.SetActive(false);

    }
}
