using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class EndMPMD : MonoBehaviourPunCallbacks
{
    public Text winText;
    public Text timeText;
    public Text ratingText;

    void Start()
    {
        string p1 = PhotonNetwork.PlayerList[0].NickName;
        string p2 = PhotonNetwork.PlayerList[1].NickName;

        PhotonNetwork.Disconnect();

        if (PlayerPrefs.GetInt("ResultMPOD") == 1)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(AuthManager.AddRatingAfterGame(p2, ratingText));
            }
            else
            {
                StartCoroutine(AuthManager.SubstractRatingAfterGame(p1, ratingText));
            }

            winText.text = p1 + " won!";
        }
        else if (PlayerPrefs.GetInt("ResultMPOD") == 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(AuthManager.SubstractRatingAfterGame(p2, ratingText));
            }
            else
            {
                StartCoroutine(AuthManager.AddRatingAfterGame(p1, ratingText));
            }

            winText.text = p2 + " won!";
        }
        else if (PlayerPrefs.GetInt("ResultMPOD") == 0)
        {
            winText.text = "Draw!";
        }

        float time = PlayerPrefs.GetFloat("GameTime");

        timeText.text = "Time played: " + time.ToString() + " second(s)";
    }

    public void PlayAgain()
    {
        BannerAd.HideBanner();
        SceneManager.LoadScene("MPChoose");
    }

    public void Back()
    {
        BannerAd.HideBanner();
        SceneManager.LoadScene("Menu");
    }
}
