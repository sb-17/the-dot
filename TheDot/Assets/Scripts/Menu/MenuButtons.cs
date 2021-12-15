using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    public GameObject playMenu;

    public Text pLevel;
    public Text xp;
    public Text rating;

    public TMP_InputField nick;

    void Start()
    {
        StartCoroutine(AuthManager.CheckXP());
    }

    public void Play()
    {
        playMenu.SetActive(true);
    }
    public void Tournaments()
    {
        SceneManager.LoadScene("Tournaments");
    }

    public void PlaySP()
    {
        SceneManager.LoadScene("SPGame");
    }

    public void PlayMP()
    {
        SceneManager.LoadScene("MPChoose");
    }

    public void ClosePlayMenu()
    {
        playMenu.SetActive(false);
    }

    public void Help()
    {
        SceneManager.LoadScene("Help");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetNickname()
    {
        if(nick.text != "" && nick.text.Length < 9 && nick.text != " " && nick.text != "  " && nick.text != "   " && nick.text != "    " && nick.text != "     " && nick.text != "      " && nick.text != "       " && nick.text != "        ")
        {
            PlayerPrefs.SetString("Nick", nick.text);
            PhotonNetwork.NickName = nick.text;
        }
        nick.text = PlayerPrefs.GetString("Nick");
    }

    public void DiscordButton()
    {
        Application.OpenURL("https://discord.gg/9ERdXUBwEZ");
    }

    public void TwitterButton()
    {
        Application.OpenURL("https://twitter.com/lostin_games");
    }
}
