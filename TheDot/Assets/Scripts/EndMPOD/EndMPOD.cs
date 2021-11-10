using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMPOD : MonoBehaviour
{
    public Text winText;
    public Text timeText;

    void Start()
    {
        if(PlayerPrefs.GetInt("ResultMPOD") == 1)
        {
            winText.text = "Player 1 won!";
        }
        else if(PlayerPrefs.GetInt("ResultMPOD") == 2)
        {
            winText.text = "Player 2 won!";
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
        SceneManager.LoadScene("MPGameOD");
    }

    public void Back()
    {
        BannerAd.HideBanner();
        SceneManager.LoadScene("Menu");
    }
}