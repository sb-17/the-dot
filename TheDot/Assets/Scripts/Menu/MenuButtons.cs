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

    public Text rewardAdText;

    public TMP_InputField nick;

    void Start()
    {
        StartCoroutine(AuthManager.CheckXP());

        if (Application.isMobilePlatform)
        {
            int random = Random.Range(1, 3);
            if (random == 1)
            {
                GoogleAdMobController gamc = GameObject.Find("GoogleAdMobController").GetComponent<GoogleAdMobController>();

                gamc.RequestAndLoadRewardedAd();

                rewardAdText.gameObject.SetActive(true);
            }
        }
        else
        {
            rewardAdText.gameObject.SetActive(false);
        }
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
