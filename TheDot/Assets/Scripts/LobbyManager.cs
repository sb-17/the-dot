using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Button findMatchBtn;
    [SerializeField]
    GameObject searchingPanel;
    [SerializeField]
    GameObject controlPanel;

    void Start()
    {
        searchingPanel.SetActive(false);
        findMatchBtn.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        findMatchBtn.interactable = true;
    }

    public void FindMatch()
    {
        searchingPanel.SetActive(true);
        controlPanel.SetActive(false);

        PhotonNetwork.NickName = PlayerPrefs.GetString("Nick");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        MakeRoom();
    }

    void MakeRoom()
    {
        int randomRoomName = Random.Range(0, 5000);
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
        RoomCustomProps.Add("GridSize", 10);
        RoomCustomProps.Add("turn", 1);
        RoomCustomProps.Add("clicked", null);
        roomOptions.CustomRoomProperties = RoomCustomProps;

        PhotonNetwork.CreateRoom("Room_" + randomRoomName.ToString(), roomOptions);
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
        searchingPanel.SetActive(false);
        controlPanel.SetActive(true);

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

        SceneManager.LoadScene("MPChoose");
    }
}
