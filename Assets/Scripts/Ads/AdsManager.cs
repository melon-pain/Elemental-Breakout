using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public EventHandler<AdFinishEventArgs> OnAdDone;

    public string GameID
    {
        get
        {
#if UNITY_ANDROID
            return "4290327";
#elif UNITY_IOS
            return "4290326";
#else
            return "";
#endif
        }
    }

    public const string AndroidRewarded = "Rewarded_Android";
    public const string AndroidInterstitial = "Interstitial_Android";
    public const string AndroidBanner = "Banner_Android";
    private void Awake()
    {
        //Set flag to false when uploading to play store, else just true for testing
        Advertisement.Initialize(GameID, true);
    }

    public void ShowInterstitialAd()
    {
        if(Advertisement.IsReady(AndroidInterstitial))
        {
            Advertisement.Show(AndroidInterstitial);
        }
        else
        {
            Debug.Log("No Ads: " + AndroidInterstitial);
        }
    }
    public void ShowBannerAd()
    {
        StartCoroutine(ShowBannerRoutine());
    }

    public void HideBannerAd()
    {
        if(Advertisement.Banner.isLoaded)
        {
            Advertisement.Banner.Hide();
        }
    }

    IEnumerator ShowBannerRoutine()
    {
        while(!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Show(AndroidBanner);
    }

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log($"Loading done {placementId}");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log($"Ad error: {message}");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log($"Ad shown: {placementId}");
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(AndroidRewarded))
        {
            Advertisement.Show(AndroidRewarded);
        }
        else
        {
            Debug.Log("No Ads: " + AndroidRewarded);
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(OnAdDone != null)
        {
            AdFinishEventArgs args = new AdFinishEventArgs(placementId, showResult);
            OnAdDone(this, args);
        }
    }
}
