using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FieldClick : MonoBehaviour
{
    public Sprite dot;
    public RectTransform rt;

    public static int playerDotCount;

    public static List<int> placed = new List<int>();

    void Start()
    {
        playerDotCount = 25;
    }

    public void OnClick()
    {
        if (Game.turn == 1 && playerDotCount > 0 && gameObject.GetComponent<Image>().sprite != dot && Game.canDoMove)
        {
            gameObject.GetComponent<Image>().sprite = dot;

            placed.Add(CheckMyPos());

            playerDotCount -= 1;

            Game.playerDotCountText.text = "Player's dots: " + playerDotCount.ToString();

            Game.CheckWin();

            StartCoroutine(Game.BotLogic());
        }
    }

    void Update()
    {
        if (playerDotCount == 0 && Game.botDotCount == 0)
        {
            Game.DeleteAll();
            PlayerPrefs.SetInt("ResultSP", 0);
            int random = Random.Range(1, 7);
            PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP") + random);
            SceneManager.LoadScene("EndSP");
        }
    }

    int CheckMyPos()
    {
        return Game.fields.IndexOf(this.gameObject.GetComponent<Image>()) + 1;
    }
}
