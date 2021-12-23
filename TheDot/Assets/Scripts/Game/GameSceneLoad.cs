using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameSceneLoad : MonoBehaviourPunCallbacks
{
    public GameObject bg;

    public Camera mainCam;

    public Sprite board10x10;
    public Sprite board9x9;
    public Sprite board8x8;
    public Sprite board7x7;
    public Sprite board6x6;
    public Sprite board5x5;
    public Sprite board4x4;
    public Sprite board3x3;
    public Sprite board2x2;

    void Start()
    {
        SetBoard();
        scaleBackgroundImageFitScreenSize();
    }

    private void SetBoard()
    {
        if (SceneManager.GetActiveScene().name == "MPGameOD")
        {
            if (PlayerPrefs.GetInt("GridSize") == 10)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board10x10;
            else if (PlayerPrefs.GetInt("GridSize") == 9)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board9x9;
            else if (PlayerPrefs.GetInt("GridSize") == 8)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board8x8;
            else if (PlayerPrefs.GetInt("GridSize") == 7)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board7x7;
            else if (PlayerPrefs.GetInt("GridSize") == 6)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board6x6;
            else if (PlayerPrefs.GetInt("GridSize") == 5)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board5x5;
            else if (PlayerPrefs.GetInt("GridSize") == 4)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board4x4;
            else if (PlayerPrefs.GetInt("GridSize") == 3)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board3x3;
            else if (PlayerPrefs.GetInt("GridSize") == 2)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board2x2;
        }
        else if (SceneManager.GetActiveScene().name == "MPGameMD")
        {
            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 10)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board10x10;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 9)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board9x9;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 8)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board8x8;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 7)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board7x7;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 6)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board6x6;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 5)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board5x5;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 4)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board4x4;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 3)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board3x3;
            else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GridSize"] == 2)
                GameObject.Find("board").GetComponent<SpriteRenderer>().sprite = board2x2;
        }
    }

    private void scaleBackgroundImageFitScreenSize()
    {
        Vector2 deviceScreenResolution = new Vector2(Screen.width, Screen.height);

        float srcHeight = Screen.height;
        float srcWidth = Screen.width;

        float DEVICE_SCREEN_ASPECT = srcWidth / srcHeight;

        mainCam.aspect = DEVICE_SCREEN_ASPECT;

        float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
        float camWidth = camHeight * DEVICE_SCREEN_ASPECT;

        SpriteRenderer bgSR = bg.GetComponent<SpriteRenderer>();
        float bgHeight = bgSR.sprite.rect.height;
        float bgWidth = bgSR.sprite.rect.width;

        float bgScaleRatioHeight = camHeight / bgHeight;
        float bgScaleRatioWidth = camWidth / bgWidth;

        float x = bgWidth / (bgHeight/33);

        bg.transform.localScale = new Vector3(x, x, 1);
    }
}
