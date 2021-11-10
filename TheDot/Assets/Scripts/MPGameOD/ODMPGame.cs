using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ODMPGame : MonoBehaviour
{
    static public ODMPGame instance;

    public Sprite dot1;
    public Sprite red1;
    public Text turnText1;

    public Text player1DotCountText1;
    public Text player2DotCountText1;

    public static Text player1DotCountText;
    public static Text player2DotCountText;

    public static Sprite dot;
    public static Sprite red;

    public static Text turnText;

    public GameObject field;
    public Transform board;

    public static List<Image> fields = new List<Image>();

    public static List<List<int>> squares = new List<List<int>>();

    public static List<int> sq = new List<int>();

    public static int turn;

    public static int x = 10;
    public static int y = 10;

    static float time = 0;

    public static int[,] grid = new int[x, y];

    private void Awake()
    {
        instance = this;
    }

    IEnumerator GameTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            time += 0.01f;
        }
    }

    void Start()
    {
        squares.Clear();
        fields.Clear();

        time = 0;

        StartCoroutine(GameTime());

        AddToGrid();

        FindAllSquares();

        player1DotCountText = player1DotCountText1;
        player2DotCountText = player2DotCountText1;

        player1DotCountText.text = "Player1's dots: 25";
        player2DotCountText.text = "Player2's dots: 25";

        dot = dot1;
        red = red1;
        turnText = turnText1;

        turn = 1;

        turnText.text = "Turn: Player 1";

        float x = -0.27f;
        float y = 0.27f;

        for (int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(field, new Vector3(0, 0, 0), Quaternion.identity, board);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            fields.Add(obj.GetComponent<Image>());

            if (x == -0.27f)
            {
                x = -0.21f;
            }
            else if (x == -0.21f)
            {
                x = -0.15f;
            }
            else if (x == -0.15f)
            {
                x = -0.09f;
            }
            else if (x == -0.09f)
            {
                x = -0.03f;
            }
            else if (x == -0.03f)
            {
                x = 0.03f;
            }
            else if (x == 0.03f)
            {
                x = 0.09f;
            }
            else if (x == 0.09f)
            {
                x = 0.15f;
            }
            else if (x == 0.15f)
            {
                x = 0.21f;
            }
            else if (x == 0.21f)
            {
                x = 0.27f;
            }
            else if (x == 0.27f)
            {
                x = -0.27f;

                y -= 0.06f;
            }
        }
    }

    public static void CheckWin()
    {
        int i = 0;

        while (i < squares.Count)
        {
            int lu = squares[i][0];
            int ru = squares[i][1];
            int ld = squares[i][2];
            int rd = squares[i][3];

            int count = 0;

            if (fields[lu - 1].sprite == dot)
            {
                count++;
            }
            if (fields[ru - 1].sprite == dot)
            {
                count++;
            }
            if (fields[ld - 1].sprite == dot)
            {
                count++;
            }
            if (fields[rd - 1].sprite == dot)
            {
                count++;
            }

            if (count == 4)
            {
                instance.StartCoroutine(endMatch(lu, ru, ld, rd));
            }
            else
            {
                if(turn == 1)
                {
                    turn = 2;
                    turnText.text = "Turn: Player 2";
                }
                else if(turn == 2)
                {
                    turn = 1;
                    turnText.text = "Turn: Player 1";
                }
            }

            i++;
        }
    }

    public static IEnumerator endMatch(int lu, int ru, int ld, int rd)
    {
        FieldClickMPOD.canDoClick = false;

        fields[lu - 1].sprite = red;
        fields[ru - 1].sprite = red;
        fields[ld - 1].sprite = red;
        fields[rd - 1].sprite = red;

        yield return new WaitForSeconds(3f);

        if (turn == 1)
        {
            PlayerPrefs.SetFloat("GameTime", time);
            PlayerPrefs.SetInt("ResultMPOD", 1);
            int random = Random.Range(8, 17);
            PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP") + random);
            SceneManager.LoadScene("EndMPOD");
        }
        else if (turn == 2)
        {
            PlayerPrefs.SetFloat("GameTime", time);
            PlayerPrefs.SetInt("ResultMPOD", 2);
            SceneManager.LoadScene("EndMPOD");
        }
    }

    public static void DeleteAll()
    {
        for (int i = 0; i < 100; i++)
        {
            Destroy(fields[i]);
        }
    }

    void AddToGrid()
    {
        int count = 0;

        for (int i = 1; i < 11; i++)
        {
            for (int j = 1; j < 11; j++)
            {
                count++;

                grid[i - 1, j - 1] = count;
            }
        }
    }

    void FindAllSquares()
    {
        for (int v = 2; v < 11; v++)
        {
            for (int i = 0; i < 10 - v + 1; i++)
            {
                for (int j = 0; j < 10 - v + 1; j++)
                {
                    int lu = grid[i, j];
                    int ru = grid[i + v - 1, j];
                    int ld = grid[i, j + v - 1];
                    int rd = grid[i + v - 1, j + v - 1];

                    sq = new List<int>();

                    sq.Add(lu);
                    sq.Add(ru);
                    sq.Add(ld);
                    sq.Add(rd);

                    squares.Add(sq);
                }
            }
        }
    }
}
