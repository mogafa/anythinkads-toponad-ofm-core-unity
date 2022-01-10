using UnityEngine;
using OfmSDK.Api;
using OfmSDK.Common;

using System.Collections;
using System.Collections.Generic;

namespace OfmSDK
{
    public class OfmAdsClientFactory
    {
        public static IOfmBannerAdClient BuildBannerAdClient()
        {
            #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.
            #elif UNITY_ANDROID
                return new OfmSDK.Android.OfmBannerAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new OfmSDK.iOS.OfmBannerAdClient();
            #else
                
            #endif
            return new UnityBannerClient();
        }

        public static IOfmInterstitialAdClient BuildInterstitialAdClient()
        {
            #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.
            #elif UNITY_ANDROID
                return new OfmSDK.Android.OfmInterstitialAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new OfmSDK.iOS.OfmInterstitialAdClient();
            #else

            #endif
            return new UnityInterstitialClient();
        }

  

        public static IOfmRewardedVideoAdClient BuildRewardedVideoAdClient()
        {
            #if UNITY_EDITOR
            // Testing UNITY_EDITOR first because the editor also responds to the currently
            // selected platform.

            #elif UNITY_ANDROID
                return new OfmSDK.Android.OfmRewardedVideoAdClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                return new OfmSDK.iOS.OfmRewardedVideoAdClient();            
            #else
                            
            #endif
            return new UnityRewardedVideoAdClient();
        }

        public static IOfmSDKAPIClient BuildSDKAPIClient()
        {
            Debug.Log("BuildSDKAPIClient");
            #if UNITY_EDITOR
                Debug.Log("Unity Editor");
                        // Testing UNITY_EDITOR first because the editor also responds to the currently
                        // selected platform.

            #elif UNITY_ANDROID
                return new OfmSDK.Android.OfmSDKAPIClient();
            #elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
                 Debug.Log("Unity:OfmAdsClientFactory::Build iOS Client");
                return new OfmSDK.iOS.OfmSDKAPIClient();         
            #else

            #endif
            return new UnitySDKAPIClient();
        }

    }

    class UnitySDKAPIClient:IOfmSDKAPIClient
    {
        public void initSDK(string appId, string appkey, string defaultConfig){}
        public void initSDK(string appId, string appkey, string defaultConfig, OfmSDKInitListener listener){ }     
        public void initCustomMap(string cutomMap){ }       
        public void setLogDebug(bool isDebug){ }
        public void setPersonalizedAdState(int personalizedAdState){ }
        public void setHasUserConsent(bool hasUserConsent){ }
        public void setIsAgeRestrictedUser(bool isAgeRestrictedUser){ }
        public void setDoNotSell(bool doNotSell){ }

        public void setTimeoutForWaitingSetting(int millisecond) { }

        public int getMediationId()
        {
            return 0;
        }

        public string getMediationConfig()
        {
            return "";
        }

        public void setMediationSwitchListener(OfmMediationSwitchListener listener) { }
    }

    class UnityBannerClient:IOfmBannerAdClient
    {
       OfmBannerAdListener listener;
       public void loadBannerAd(string unitId, string mapJson, string customRulesJson){
            if(listener != null)
            {
                listener.onAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }
     
       public void setListener(OfmBannerAdListener listener)
       {
            this.listener = listener;
       }

       public string checkAdStatus(string unitId) { return ""; }

       public void showBannerAd(string unitId, string position){ }
       
       public void showBannerAd(string unitId, OfmRect rect){ }
    
       public  void cleanBannerAd(string unitId){ }
      
       public void hideBannerAd(string unitId){ }
    
       public void showBannerAd(string unitId){ }
      
       public void cleanCache(string unitId){}
   }

    class UnityInterstitialClient : IOfmInterstitialAdClient
    {
       OfmInterstitialAdListener listener;
       public void loadInterstitialAd(string unitId, string mapJson, string customRulesJson){
            if (listener != null)
            {
               listener.onInterstitialAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }
       
       public void setListener(OfmInterstitialAdListener listener){
            this.listener = listener;
       }

       public bool hasInterstitialAdReady(string unitId) { return false; }

        public string checkAdStatus(string unitId) { return ""; }

        public void showInterstitialAd(string unitId, string mapJson){}
        
       public void cleanCache(string unitId){}
    }

 

    class UnityRewardedVideoAdClient : IOfmRewardedVideoAdClient
    {
        OfmRewardedVideoListener listener;
        public void loadVideoAd(string unitId, string mapJson, string customRulesJson){
            if (listener != null)
            {
                listener.onRewardedVideoAdLoadFail(unitId, "-1", "Must run on Android or IOS platform!");
            }
       }

        public void setListener(OfmRewardedVideoListener listener){
            this.listener = listener;
       }

        public bool hasAdReady(string unitId) { return false; }

        public string checkAdStatus(string unitId) { return ""; }

        public void setUserData(string unitId, string userId, string customData){}

        public void showAd(string unitId, string mapJson){}

        public void cleanAd(string unitId){}

        public void onApplicationForces(string unitId){}

        public void onApplicationPasue(string unitId){}

        public void addsetting(string unitId, string json){}
    }
}