using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GoogleAdMobController : MonoBehaviour
{
    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private float deltaTime;
    private bool isShowingAppOpenAd;
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;


    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
        deviceIds.Add("75EF8D155528C04DACBBA6F36F433035");
#endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);

        RequestBannerAd();
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            RequestBannerAd();
        });
    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    public void OnApplicationPause(bool paused)
    {
        // Display the app open ad when the app is foregrounded.
        if (!paused)
        {
            ShowAppOpenAd();
        }
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        #if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
        rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
    }

    public void RequestAndLoadRewardedInterstitialAd()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        RewardedInterstitialAd.LoadAd(adUnitId, CreateAdRequest(), (rewardedInterstitialAd, error) =>
        {
            if (error != null)
            {
                return;
            }
            this.rewardedInterstitialAd = rewardedInterstitialAd;
            // Register for ad events.
            this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
            };
            this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
                this.rewardedInterstitialAd = null;
            };
            this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
                this.rewardedInterstitialAd = null;
            };
        });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((reward) => {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
            });
        }
    }

    #endregion

    #region APPOPEN ADS

    public void RequestAndLoadAppOpenAd()
    {
        string adUnitId = "ca-app-pub-1217351089486176~1439312473";

        // create new app open ad instance
        AppOpenAd.LoadAd(adUnitId, ScreenOrientation.Portrait, CreateAdRequest(), (appOpenAd, error) =>
        {
            if (error != null)
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
                return;
            }
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
            });
            this.appOpenAd = appOpenAd;
        });
    }

    public void ShowAppOpenAd()
    {
        if (isShowingAppOpenAd)
        {
            return;
        }
        if (appOpenAd == null)
        {
            return;
        }
        // Register for ad events.
        this.appOpenAd.OnAdDidDismissFullScreenContent += (sender, args) =>
        {
            isShowingAppOpenAd = false;
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                Debug.Log("AppOpenAd dismissed.");
                if (this.appOpenAd != null)
                {
                    this.appOpenAd.Destroy();
                    this.appOpenAd = null;
                }
            });
        };
        this.appOpenAd.OnAdFailedToPresentFullScreenContent += (sender, args) =>
        {
            isShowingAppOpenAd = false;
            var msg = args.AdError.GetMessage();
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                if (this.appOpenAd != null)
                {
                    this.appOpenAd.Destroy();
                    this.appOpenAd = null;
                }
            });
        };
        this.appOpenAd.OnAdDidPresentFullScreenContent += (sender, args) =>
        {
            isShowingAppOpenAd = true;
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                Debug.Log("AppOpenAd presented.");
            });
        };
        this.appOpenAd.OnAdDidRecordImpression += (sender, args) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                Debug.Log("AppOpenAd recorded an impression.");
            });
        };
        this.appOpenAd.OnPaidEvent += (sender, args) =>
        {
            string currencyCode = args.AdValue.CurrencyCode;
            long adValue = args.AdValue.Value;
            string suffix = "AppOpenAd received a paid event.";
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                string msg = string.Format("{0} (currency: {1}, value: {2}", suffix, currencyCode, adValue);
            });
        };
        appOpenAd.Show();
    }

    #endregion


    #region AD INSPECTOR

    public void OpenAdInspector()
    {
        MobileAds.OpenAdInspector((error) =>
        {
            if (error != null)
            {
                string errorMessage = error.GetMessage();
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
            }
            else
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() => {
                });
            }
        });
    }

    #endregion
}