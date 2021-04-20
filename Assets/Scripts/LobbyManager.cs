using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField createRoomInput, joinRoomInput, nameInput;
    public string saveName;
    public Text feedbackText, feedbackNameText;
    public GameObject MainMenu, LobbyMenu;

    public void CreateRoom()
    {
        if (createRoomInput.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions() { MaxPlayers = 4 }, null);
            feedbackText.text = "Created a room";
        }
    }

    public void JoinRoom()
    {
        feedbackText.text = "Trying to join a Room";

        if (joinRoomInput.text.Length > 0)
        {
            PhotonNetwork.JoinRoom(joinRoomInput.text);
        }
    }

    public void SetName()
    {
        //setabove name box to set name
        saveName = nameInput.text;
        PhotonNetwork.NickName = saveName;
        feedbackNameText.text = "Name: " + saveName;
    }

    public override void OnJoinedRoom()
    {
        feedbackText.text = "Joined the room!";
        MainMenu.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        feedbackText.text = "Join room failed! / no room exist with this name?";
    }

}
