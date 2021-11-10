using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAsGuest : MonoBehaviour
{
    public void OfflineMode()
    {
        PlayerPrefs.SetInt("offline", 1);
        SceneManager.LoadScene("MenuOffline");
    }
}
