using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathTrigger : MonoBehaviour
{
    public BannerView bannerV;
    AdRequest req;
    public GameObject banner;
    private void Start()
    {
        MobileAds.Initialize(initStatus => { });

        bannerV = new BannerView("ca-app-pub-8300493683714143/7244353018", AdSize.Banner, AdPosition.Bottom);
        req = new AdRequest.Builder().Build();
        bannerV.LoadAd(req);
        banner = GameObject.Find("BANNER(Clone)");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            banner.SetActive(true);
            other.GetComponent<player>().play = false;
            if (other.GetComponent<player>().deaths < 2) other.GetComponent<player>().deaths += 1;
            else
            {
                InterstitialAd intersAd = new InterstitialAd("ca-app-pub-8300493683714143/3445736478");
                AdRequest req1 = new AdRequest.Builder().Build();
                intersAd.LoadAd(req1);
                intersAd.Show();
                other.GetComponent<player>().deaths = 0;
            }
            if (Math.Round(transform.position.z, 0) > PlayerPrefs.GetInt("best")) PlayerPrefs.SetInt("best", System.Convert.ToInt32(Math.Round(transform.position.z, 0)));
        }
    }
}
