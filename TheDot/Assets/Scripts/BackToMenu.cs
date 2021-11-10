using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BackToMenu : MonoBehaviourPun
{
    public void Back()
    {
        if (PlayerPrefs.GetInt("offline") == 0)
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Menu");
        }
        else if (PlayerPrefs.GetInt("offline") == 1)
        {
            SceneManager.LoadScene("MenuOffline");
        }
    }
}
