using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    public string gameId = "3936473";
    public string placementId = "Android_Banner";
    public bool testMode = false;

    void Start()
    {
        // Initialize the SDK if you haven't already done so:
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(placementId))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(placementId);
    }

    public static void HideBanner()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
            Advertisement.Banner.Hide();
    }
}
