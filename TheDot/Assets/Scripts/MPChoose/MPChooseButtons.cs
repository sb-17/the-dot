using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class MPChooseButtons : MonoBehaviourPunCallbacks
{
    public TMP_Text onlinePlayersCount;
    public Dropdown gamemodeSelect;
    public Slider gridSizeSlider;
    public Text gridSizeText;

    private void Start()
    {
        if (PlayerPrefs.GetInt("GridSize") < 2 || PlayerPrefs.GetInt("GridSize") > 10)
        {
            PlayerPrefs.SetInt("GridSize", 10);
        }

        gamemodeSelect.value = PlayerPrefs.GetInt("Gamemode");
        gridSizeSlider.value = PlayerPrefs.GetInt("GridSize");
        gridSizeText.text = "Grid Size: " + gridSizeSlider.value.ToString();
    }

    private void FixedUpdate()
    {
        onlinePlayersCount.text = "Online Players: " + PhotonNetwork.CountOfPlayers.ToString();
    }

    public void OD()
    {
        SceneManager.LoadScene("MPGameOD");
    }

    public void MPPrivate()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MPPrivateSetup");
    }

    public void OnGamemodeChange()
    {
        PlayerPrefs.SetInt("Gamemode", gamemodeSelect.value);

        if (gamemodeSelect.value == 0)
        {
            gridSizeSlider.gameObject.SetActive(true);
            gridSizeText.gameObject.SetActive(true);
        }
        else
        {
            gridSizeSlider.gameObject.SetActive(false);
            gridSizeText.gameObject.SetActive(false);
        }
    }

    public void OnGridSizeSliderChange()
    {
        PlayerPrefs.SetInt("GridSize", (int)gridSizeSlider.value);
        gridSizeText.text = "Grid Size: " + gridSizeSlider.value.ToString();
    }
}
