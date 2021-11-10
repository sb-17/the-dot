using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PrivateMatchLobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Button createMatchBtn;
    [SerializeField]
    Button joinMatchBtn;
    [SerializeField]
    GameObject setupPanel;
    [SerializeField]
    GameObject controlPanel;
    [SerializeField]
    GameObject joinPanel;
    [SerializeField]
    GameObject progressLabel;
    [SerializeField]
    TMP_InputField nameInput;
    [SerializeField]
    TMP_InputField nameInput1;

    TypedLobby privateLobby = new TypedLobby("privateLobby", LobbyType.SqlLobby);

    void Start()
    {
        setupPanel.SetActive(false);
        createMatchBtn.interactable = false;
        joinMatchBtn.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        createMatchBtn.interactable = true;
        joinMatchBtn.interactable = true;
    }

    public void CreateMatch()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("Nick");

        if (nameInput.text.Length > 0)
        {
            MakeRoom();

            controlPanel.SetActive(false);
            setupPanel.SetActive(false);
            progressLabel.SetActive(true);
        }
    }

    void MakeRoom()
    {
        RoomOptions roomOptions =
        new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };

        Hashtable RoomCustomProps = new Hashtable();
        RoomCustomProps.Add("P1Dots", 25);
        RoomCustomProps.Add("P2Dots", 25);
        RoomCustomProps.Add("turn", 1);
        RoomCustomProps.Add("clicked", null);
        roomOptions.CustomRoomProperties = RoomCustomProps;

        PhotonNetwork.CreateRoom(nameInput.text, roomOptions, privateLobby);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MPGameMD");
        }
    }

    public void StopSearch()
    {
        setupPanel.SetActive(false);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom()
    {
        if (nameInput1.text.Length > 0)
        {
            PhotonNetwork.JoinRoom(nameInput1.text);
        }
    }
}
