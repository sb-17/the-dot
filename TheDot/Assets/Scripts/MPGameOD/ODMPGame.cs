using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ODMPGame : MonoBehaviour
{
    public Text player1DotCountText;
    public Text player2DotCountText;

    public Sprite dot;
    public Sprite red;

    public Text turnText;

    public GameObject fieldObject;
    public Transform board;

    public List<Image> fields = new List<Image>();

    public List<List<int>> squares = new List<List<int>>();

    public List<int> sq = new List<int>();

    public int turn;

    public float x;
    public float y;

    static float time = 0;

    public int[,] grid;

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

        x = PlayerPrefs.GetInt("GridSize");
        y = PlayerPrefs.GetInt("GridSize");

        grid = new int[(int)x, (int)y];

        time = 0;

        StartCoroutine(GameTime());

        AddToGrid();

        FindAllSquares();

        player1DotCountText.text = "Player1's dots: " + Mathf.RoundToInt(x * y / 4).ToString();
        player2DotCountText.text = "Player2's dots: " + Mathf.RoundToInt(x * y / 4).ToString();

        turn = 1;

        turnText.text = "Turn: Player 1";

        float space = 0.06f;

        float posX = -0.27f + 0.03f * (10 - x) ;
        float posY = 0.27f - 0.03f * (10 - y);

        float posX1 = posX;
        float posY1 = posY;

        for (float row = 0; row < x; row++)
        {
            for (float col = 0; col < y; col++)
            {
                GameObject field = new GameObject();
                Destroy(field);

                field = Instantiate(fieldObject, transform);

                field.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX1, posY1);

                fields.Add(field.GetComponent<Image>());

                posX1 += space;
            }

            posX1 = posX;
            posY1 -= space;
        }
    }

    public void CheckWin()
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
                StartCoroutine(endMatch(lu, ru, ld, rd));
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

    public IEnumerator endMatch(int lu, int ru, int ld, int rd)
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

    public void DeleteAll()
    {
        for (int i = 0; i < x * y; i++)
        {
            Destroy(fields[i]);
        }
    }

    void AddToGrid()
    {
        int count = 0;

        for (int i = 1; i < x + 1; i++)
        {
            for (int j = 1; j < y + 1; j++)
            {
                count++;

                grid[i - 1, j - 1] = count;
            }
        }
    }

    void FindAllSquares()
    {
        for (int v = 2; v < x + 1; v++)
        {
            for (int i = 0; i < x - v + 1; i++)
            {
                for (int j = 0; j < y - v + 1; j++)
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
