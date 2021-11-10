using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    static public Game instance;

    public Sprite dot1;
    public Sprite red1;
    public Text turnText1;

    public Text playerDotCountText1;
    public Text botDotCountText1;

    public static Text botDotCountText;
    public static Text playerDotCountText;

    public static Sprite dot;
    public static Sprite red;

    public static Text turnText;

    public GameObject field;
    public Transform board;

    public static List<Image> fields = new List<Image>();

    public static List<List<int>> squares = new List<List<int>>();

    public static List<int> sq = new List<int>();

    public static int turn;

    static int playerLevel;

    public static int botDotCount;

    public static int x = 10;
    public static int y = 10;

    static float time = 0;

    public static int[,] grid = new int[x, y];

    public static bool canDoMove;

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

        canDoMove = true;

        time = 0;

        AddToGrid();

        FindAllSquares();

        StartCoroutine(GameTime());

        dot = dot1;
        red = red1;
        turnText = turnText1;

        botDotCount = 25;

        playerDotCountText = playerDotCountText1;
        botDotCountText = botDotCountText1;

        botDotCountText.text = "Bot's dots: 25";
        playerDotCountText.text = "Player's dots: 25";

        if (PlayerPrefs.GetInt("offline") == 1)
        {
            playerLevel = 2;
        }
        else
        {
            playerLevel = PlayerPrefs.GetInt("Level");
        }

        turn = Random.Range(1, 3);

        if (turn == 1)
        {
            turnText.text = "Turn: You";
        }
        else if (turn == 2)
        {
            turnText.text = "Turn: Bot";
            StartCoroutine(FirstBot());
        }

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

    public static IEnumerator FirstBot()
    {
        yield return new WaitForSeconds(2f);

        int random = Random.Range(1, 101);

        fields[random - 1].sprite = dot;

        botDotCount--;

        botDotCountText.text = "Bot's dots: " + botDotCount.ToString();

        turn = 1;
        turnText.text = "Turn: You";
    }

    public static IEnumerator BotLogic()
    {
        if (turn == 2 && canDoMove)
        {
            yield return new WaitForSeconds(1.5f);

            int defend = CheckSquares();

            int chance = Random.Range(1, playerLevel + 1);

            List<int> currentPos = new List<int>();
            List<int> possible = new List<int>();

            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].sprite == dot)
                    currentPos.Add(1);
                if (fields[i].sprite != dot)
                    currentPos.Add(0);

                possible.Add(i);
            }

            if (chance == 1)
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    int randomIndex = Random.Range(0, possible.Count);
                    int currentPick = possible[randomIndex];

                    possible.RemoveAt(randomIndex);

                    if (currentPos[currentPick] != 0)
                        continue;

                    currentPos[currentPick] = 1;

                    bool goodMove = true;

                    for (int y = 0; y < squares.Count; y++)
                    {
                        int lu = squares[y][0];
                        int ru = squares[y][1];
                        int ld = squares[y][2];
                        int rd = squares[y][3];

                        int count = 0;

                        if (currentPos[lu - 1] == 1)
                            count++;
                        if (currentPos[ru - 1] == 1)
                            count++;
                        if (currentPos[ld - 1] == 1)
                            count++;
                        if (currentPos[rd - 1] == 1)
                            count++;

                        if (lu - 1 == currentPick || ru - 1 == currentPick || ld - 1 == currentPick || rd - 1 == currentPick)
                        {
                            if (count == 4)
                            {
                                currentPos[currentPick] = 0;

                                goodMove = false;

                                break;
                            }
                        }
                    }

                    if (goodMove)
                    {
                        fields[currentPick].sprite = dot;
                        currentPos.Clear();
                        possible.Clear();

                        botDotCount--;

                        botDotCountText.text = "Bot's dots: " + botDotCount.ToString();

                        CheckWin();

                        break;
                    }
                }
            }
            else if (chance != 1)
            {
                if (defend == 0)
                {
                    for (int i = 0; i < fields.Count; i++)
                    {
                        int randomIndex = Random.Range(0, possible.Count);
                        int currentPick = possible[randomIndex];

                        possible.RemoveAt(randomIndex);

                        if (currentPos[currentPick] != 0)
                            continue;

                        currentPos[currentPick] = 1;

                        bool goodMove = false;

                        for (int y = 0; y < squares.Count; y++)
                        {
                            int lu = squares[y][0];
                            int ru = squares[y][1];
                            int ld = squares[y][2];
                            int rd = squares[y][3];

                            int count = 0;

                            if (currentPos[lu - 1] == 1)
                                count++;
                            if (currentPos[ru - 1] == 1)
                                count++;
                            if (currentPos[ld - 1] == 1)
                                count++;
                            if (currentPos[rd - 1] == 1)
                                count++;

                            if (count == 3 || possible.Count == 0)
                            {
                                currentPos[currentPick] = 0;

                                break;
                            }

                            if (y == squares.Count - 1)
                                goodMove = true;
                        }

                        if (goodMove)
                        {
                            fields[currentPick].sprite = dot;
                            currentPos.Clear();
                            possible.Clear();

                            botDotCount--;

                            botDotCountText.text = "Bot's dots: " + botDotCount.ToString();

                            CheckWin();

                            break;
                        }
                    }
                }
                else
                {
                    fields[defend - 1].sprite = dot;

                    botDotCount--;

                    botDotCountText.text = "Bot's dots: " + botDotCount.ToString();

                    CheckWin();
                }
            }
        }
    }

    static int CheckSquares()
    {
        int i = 0;

        while (i < squares.Count)
        {
            int lu = squares[i][0];
            int ru = squares[i][1];
            int ld = squares[i][2];
            int rd = squares[i][3];

            int lu1 = 0;
            int ru1 = 0;
            int ld1 = 0;
            int rd1 = 0;

            int count = 0;

            if (fields[lu - 1].sprite == dot)
            {
                lu1 = 1;
                count++;
            }
            if (fields[ru - 1].sprite == dot)
            {
                ru1 = 1;
                count++;
            }
            if (fields[ld - 1].sprite == dot)
            {
                ld1 = 1;
                count++;
            }
            if (fields[rd - 1].sprite == dot)
            {
                rd1 = 1;
                count++;
            }

            if (count == 3)
            {
                if (lu1 == 0)
                {
                    return lu;
                }
                else if (ru1 == 0)
                {
                    return ru;
                }
                else if (ld1 == 0)
                {
                    return ld;
                }
                else if (rd1 == 0)
                {
                    return rd;
                }
            }

            i++;
        }

        return 0;
    }

    public static void DeleteAll()
    {
        for (int i = 0; i < 100; i++)
        {
            Destroy(fields[i]);
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
                if(turn == 2)
                {
                    turn = 1;
                    turnText.text = "Turn: You";
                }
                else if(turn == 1)
                {
                    turn = 2;
                    turnText.text = "Turn: Bot";
                }
            }

            i++;
        }
    }

    public static IEnumerator endMatch(int lu, int ru, int ld, int rd)
    {
        canDoMove = false;

        fields[lu - 1].sprite = red;
        fields[ru - 1].sprite = red;
        fields[ld - 1].sprite = red;
        fields[rd - 1].sprite = red;

        yield return new WaitForSeconds(3f);

        if (turn == 1)
        {
            PlayerPrefs.SetFloat("GameTime", time);
            PlayerPrefs.SetInt("ResultSP", 1);

            DeleteAll();

            SceneManager.LoadScene("EndSP");
        }
        else if (turn == 2)
        {
            DeleteAll();

            PlayerPrefs.SetFloat("GameTime", time);
            PlayerPrefs.SetInt("ResultSP", 2);
            SceneManager.LoadScene("EndSP");
        }
    }

    private void Awake()
    {
        instance = this;
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



/*if (defend == 0 || chance == 1)
            {
                int i = 0;

                while (i < squares.Count)
                {
                    int random = Random.Range(1, 4);

                    int lu = squares[i][0];
                    int ru = squares[i][1];
                    int ld = squares[i][2];
                    int rd = squares[i][3];

                    int lu1 = 0;
                    int ru1 = 0;
                    int ld1 = 0;
                    int rd1 = 0;

                    if (random != 1 && rd != 100)
                    {
                        i++;
                        continue;
                    }

                    int count = 0;

                    if (fields[lu - 1].sprite == dot)
                    {
                        lu1 = 1;
                        count++;
                    }
                    if (fields[ru - 1].sprite == dot)
                    {
                        ru1 = 1;
                        count++;
                    }
                    if (fields[ld - 1].sprite == dot)
                    {
                        ld1 = 1;
                        count++;
                    }
                    if (fields[rd - 1].sprite == dot)
                    {
                        rd1 = 1;
                        count++;
                    }

                    if (count == 1)
                    {
                        Debug.Log("1: " + i);
                        if (lu1 == 0)
                        {
                            fields[lu - 1].sprite = dot;
                            break;
                        }
                        else if (ru1 == 0)
                        {
                            fields[ru - 1].sprite = dot;
                            break;
                        }
                        else if (ld1 == 0)
                        {
                            fields[ld - 1].sprite = dot;
                            break;
                        }
                        else if (rd1 == 0)
                        {
                            fields[rd - 1].sprite = dot;
                            break;
                        }
                    }
                    else if (count == 0)
                    {
                        Debug.Log("2: " + i);
                        int rnd = Random.Range(0, 4);

                        if (rnd == 0)
                        {
                            fields[lu - 1].sprite = dot;
                            break;
                        }
                        else if (rnd == 1)
                        {
                            fields[ru - 1].sprite = dot;
                            break;
                        }
                        else if (rnd == 2)
                        {
                            fields[ld - 1].sprite = dot;
                            break;
                        }
                        else if (rnd == 3)
                        {
                            fields[rd - 1].sprite = dot;
                            break;
                        }
                    }

                    i++;
                }

                botDotCount--;

                botDotCountText.text = "Bot's dots: " + botDotCount.ToString();

                CheckWin();
            }
            else if (defend != 0 && chance != 1)
            {
                fields[defend - 1].sprite = dot;

                botDotCount--;

                botDotCountText.text = "Bot's dots: " + botDotCount.ToString();

                CheckWin();
            }
            else if (chance == 1)
            {
                int f = 0;
                while (f == 0)
                {
                    int rndField = Random.Range(1, 101);

                    if (fields[rndField - 1].sprite != dot)
                        f = rndField;
                }

                fields[f - 1].sprite = dot;

                botDotCount--;

                botDotCountText.text = "Bot's dots: " + botDotCount.ToString();

                CheckWin();
            }*/