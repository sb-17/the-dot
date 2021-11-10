using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FieldClickMPOD : MonoBehaviour
{
    public Sprite dot;
    public RectTransform rt;

    public static int playerDotCount1;
    public static int playerDotCount2;

    public static List<int> placed = new List<int>();

    public static bool canDoClick;

    void Start()
    {
        playerDotCount1 = 25;
        playerDotCount2 = 25;

        canDoClick = true;
    }

    public void OnClick()
    {
        if (ODMPGame.turn == 1 && canDoClick)
        {
            if (playerDotCount1 > 0 && gameObject.GetComponent<Image>().sprite != dot)
            {
                gameObject.GetComponent<Image>().sprite = dot;

                playerDotCount1 -= 1;

                ODMPGame.player1DotCountText.text = "Player1's dots: " + playerDotCount1.ToString();

                placed.Add(CheckMyPos());

                ODMPGame.CheckWin();
            }
        }
        else if (ODMPGame.turn == 2 && canDoClick)
        {
            if (playerDotCount2 > 0 && gameObject.GetComponent<Image>().sprite != dot)
            {
                gameObject.GetComponent<Image>().sprite = dot;

                playerDotCount2 -= 1;

                ODMPGame.player2DotCountText.text = "Player2's dots: " + playerDotCount2.ToString();

                placed.Add(CheckMyPos());

                ODMPGame.CheckWin();
            }
        }
    }

    void Update()
    {
        if (playerDotCount1 == 0 && playerDotCount2 == 0)
        {
            PlayerPrefs.SetInt("ResultMPOD", 0);
            int random = Random.Range(1, 5);
            PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP") + random);
            SceneManager.LoadScene("EndMPOD");
        }
    }

    int CheckMyPos()
    {
        return Game.fields.IndexOf(this.gameObject.GetComponent<Image>()) + 1;
    }
}
