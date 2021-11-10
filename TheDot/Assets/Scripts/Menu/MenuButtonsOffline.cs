using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtonsOffline : MonoBehaviour
{
    public void PlaySP()
    {
        SceneManager.LoadScene("SPGame");
    }

    public void Help()
    {
        SceneManager.LoadScene("Help");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        PlayerPrefs.SetInt("offline", 0);

        SceneManager.LoadScene("FirebaseINIT");
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
