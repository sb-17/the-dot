using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSP : MonoBehaviour
{
    public Text winText;
    public Text timeText;

    void Start()
    {
        if (PlayerPrefs.GetInt("ResultSP") == 1)
        {
            winText.text = "You won!";

            if (PlayerPrefs.GetInt("offline") != 1)
                StartCoroutine(AuthManager.AddXPAfterSPGame());
        }
        else if (PlayerPrefs.GetInt("ResultSP") == 2)
        {
            winText.text = "You lose!";

            if (PlayerPrefs.GetInt("offline") != 1)
                StartCoroutine(AuthManager.SubstractXPAfterSPGame());
        }
        else if (PlayerPrefs.GetInt("ResultSP") == 0)
        {
            winText.text = "Draw!";
        }

        float time = PlayerPrefs.GetFloat("GameTime");

        timeText.text = "Time played: " + time.ToString() + " second(s)";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SPGame");
    }

    public void Back()
    {
        if (PlayerPrefs.GetInt("offline") != 1)
            SceneManager.LoadScene("Menu");
        else
            SceneManager.LoadScene("MenuOffline");
    }
}
