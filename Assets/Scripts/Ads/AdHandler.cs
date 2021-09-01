using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Advertisements;

public class AdHandler : MonoBehaviour
{
    public AdsManager adsManager;

    // Start is called before the first frame update
    void Start()
    {
        adsManager.OnAdDone += OnAdDone;
    }

    public void OnAdDone(object sender, AdFinishEventArgs args)
    {
        if(args.PlacementID == AdsManager.AndroidRewarded)
        {
            switch(args.AdResult)
            {
                case ShowResult.Failed: Debug.Log("Ad Failed"); break;
                case ShowResult.Skipped: Debug.Log("Ad Skipped"); break;
                case ShowResult.Finished: Debug.Log("Ad Finished");
                    //Reward here
                    break;

            }
        }
    }
}
