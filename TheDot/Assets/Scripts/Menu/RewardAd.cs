using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System;

public class RewardAd : MonoBehaviour, IUnityAdsListener
{
    string gameId = "3936473";
    string myPlacementId = "xpVideo";
    bool testMode = false;

    bool updating = false;

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady(myPlacementId))
        {
            Advertisement.Show(myPlacementId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    private IEnumerator UpdateXP()
    {
        StartCoroutine(AuthManager.AdReward());
        yield return new WaitForSeconds(.5f);
        updating = false;
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if (!updating)
            {
                updating = true;
                StartCoroutine(UpdateXP());
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
