using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class FieldClickMPMD : MonoBehaviourPun
{
    public Sprite dot;
    public RectTransform rt;
    public static List<int> placed = new List<int>();

    public static bool canDoClick;

    void Start()
    {
        canDoClick = true;
    }

    public void OnClick()
    {
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["turn"] == 1 && canDoClick && PhotonNetwork.IsMasterClient && gameObject.GetComponent<Image>().sprite != dot)
        {
            canDoClick = false;
            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["P1Dots"] > 0)
            {
                gameObject.GetComponent<Image>().sprite = dot;

                MDMPGame.player1DotCountText.text = PhotonNetwork.PlayerList[0].NickName + "'s dots: " + PhotonNetwork.CurrentRoom.CustomProperties["P1Dots"].ToString();

                placed.Add(CheckMyPos());

                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "clicked", CheckMyPos() - 1 } });

                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "P1Dots", (int)PhotonNetwork.CurrentRoom.CustomProperties["P1Dots"] - 1 } });

                canDoClick = true;

                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "turn", 2 } });
            }
        }
        else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["turn"] == 2 && canDoClick && !PhotonNetwork.IsMasterClient && gameObject.GetComponent<Image>().sprite != dot)
        {
            canDoClick = false;
            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["P2Dots"] > 0)
            {
                gameObject.GetComponent<Image>().sprite = dot;

                MDMPGame.player2DotCountText.text = PhotonNetwork.PlayerList[1].NickName + "'s dots: " + PhotonNetwork.CurrentRoom.CustomProperties["P2Dots"].ToString();

                placed.Add(CheckMyPos());

                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "clicked", CheckMyPos() - 1 } });

                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "P2Dots", (int)PhotonNetwork.CurrentRoom.CustomProperties["P2Dots"] - 1 } });

                canDoClick = true;

                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "turn", 1 } });
            }
        }
    }

    int CheckMyPos()
    {
        return MDMPGame.fields.IndexOf(gameObject.GetComponent<Image>()) + 1;
    }
}
