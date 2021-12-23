using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FieldClickMPOD : MonoBehaviour
{
    ODMPGame game;

    public Sprite dot;
    public RectTransform rt;

    public int playerDotCount1;
    public int playerDotCount2;

    public List<int> placed = new List<int>();

    public static bool canDoClick;

    void Start()
    {
        game = GameObject.Find("board").GetComponent<ODMPGame>();

        playerDotCount1 = Mathf.RoundToInt(game.x * game.y / 4);
        playerDotCount2 = Mathf.RoundToInt(game.x * game.y / 4);

        canDoClick = true;
    }

    public void OnClick()
    {
        if (game.turn == 1 && canDoClick)
        {
            if (playerDotCount1 > 0 && gameObject.GetComponent<Image>().sprite != dot)
            {
                gameObject.GetComponent<Image>().sprite = dot;

                playerDotCount1 -= 1;

                game.player1DotCountText.text = "Player1's dots: " + playerDotCount1.ToString();

                placed.Add(CheckMyPos());

                game.CheckWin();
            }
        }
        else if (game.turn == 2 && canDoClick)
        {
            if (playerDotCount2 > 0 && gameObject.GetComponent<Image>().sprite != dot)
            {
                gameObject.GetComponent<Image>().sprite = dot;

                playerDotCount2 -= 1;

                game.player2DotCountText.text = "Player2's dots: " + playerDotCount2.ToString();

                placed.Add(CheckMyPos());

                game.CheckWin();
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
        return Game.fields.IndexOf(gameObject.GetComponent<Image>()) + 1;
    }
}
