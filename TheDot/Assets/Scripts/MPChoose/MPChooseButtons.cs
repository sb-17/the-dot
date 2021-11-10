using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class MPChooseButtons : MonoBehaviourPunCallbacks
{
    public TMP_Text onlinePlayersCount;

    private void Update()
    {
        onlinePlayersCount.text = "Online Players: " + PhotonNetwork.CountOfPlayers.ToString();
    }

    public void OD()
    {
        SceneManager.LoadScene("MPGameOD");
    }

    public void MPPrivate()
    {
        SceneManager.LoadScene("MPPrivateSetup");
    }
}
