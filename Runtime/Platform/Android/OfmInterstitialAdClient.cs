using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OfmSDK.Common;
using OfmSDK.Api;
namespace OfmSDK.Android
{
    public class OfmInterstitialAdClient : AndroidJavaProxy,IOfmInterstitialAdClient
    {

        private Dictionary<string, AndroidJavaObject> interstitialHelperMap = new Dictionary<string, AndroidJavaObject>();

		//private  AndroidJavaObject videoHelper;
        private  OfmInterstitialAdListener anyThinkListener;

        public OfmInterstitialAdClient() : base("com.ofm.unitybridge.interstitial.InterstitialListener")
        {
            
        }


        public void loadInterstitialAd(string placementId, string mapJson, string customRulesJson)
        {

            //如果不存在则直接创建对应广告位的helper
            if(!interstitialHelperMap.ContainsKey(placementId))
            {
                AndroidJavaObject videoHelper = new AndroidJavaObject(
                    "com.ofm.unitybridge.interstitial.InterstitialHelper", this);
                videoHelper.Call("initInterstitial", placementId);
                interstitialHelperMap.Add(placementId, videoHelper);
                Debug.Log("OfmInterstitialAdClient : no exit helper ,create helper ");
            }

            try
            {
                Debug.Log("OfmInterstitialAdClient : loadInterstitialAd ");
                interstitialHelperMap[placementId].Call("loadInterstitialAd", mapJson, customRulesJson);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmInterstitialAdClient :  error."+e.Message);
            }


        }

        public void setListener(OfmInterstitialAdListener listener)
        {
            anyThinkListener = listener;
        }

        public bool hasInterstitialAdReady(string placementId)
        {
			bool isready = false;
			Debug.Log ("OfmInterstitialAdClient : hasAdReady....");
			try{
                if (interstitialHelperMap.ContainsKey(placementId)) {
                    isready = interstitialHelperMap[placementId].Call<bool> ("isAdReady");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmInterstitialAdClient :  error."+e.Message);
			}
			return isready; 
        }

        public string checkAdStatus(string placementId)
        {
            string adStatusJsonString = "";
            Debug.Log("OfmInterstitialAdClient : checkAdStatus....");
            try
            {
                if (interstitialHelperMap.ContainsKey(placementId))
                {
                    adStatusJsonString = interstitialHelperMap[placementId].Call<string>("checkAdStatus");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmInterstitialAdClient :  error." + e.Message);
            }

            return adStatusJsonString;
        }

        public void showInterstitialAd(string placementId, string jsonmap)
        {
			Debug.Log("OfmInterstitialAdClient : showAd " );

			try{
                if (interstitialHelperMap.ContainsKey(placementId)) {
                    this.interstitialHelperMap[placementId].Call ("showInterstitialAd", jsonmap);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmInterstitialAdClient :  error."+e.Message);

			}
        }


        public void cleanCache(string placementId)
        {
			
			Debug.Log("OfmInterstitialAdClient : clean" );

			try{
                if (interstitialHelperMap.ContainsKey(placementId)) {
                    this.interstitialHelperMap[placementId].Call ("clean");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmInterstitialAdClient :  error."+e.Message);
			}
        }

        public void onApplicationForces(string placementId)
        {
			Debug.Log ("onApplicationForces.... ");
			try{
				if (interstitialHelperMap.ContainsKey(placementId)) {
					this.interstitialHelperMap[placementId].Call ("onResume");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmInterstitialAdClient :  error."+e.Message);
			}
        }

        public void onApplicationPasue(string placementId)
        {
			Debug.Log ("onApplicationPasue.... ");
			try{
				if (interstitialHelperMap.ContainsKey(placementId)) {
					this.interstitialHelperMap[placementId].Call ("onPause");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmInterstitialAdClient :  error."+e.Message);
			}
        }

        //广告加载成功
        public void onInterstitialAdLoaded(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdLoaded...unity3d.");
            if(anyThinkListener != null){
                anyThinkListener.onInterstitialAdLoad(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        //广告加载失败
        public void onInterstitialAdLoadFail(string placementId,string code, string error)
        {
            Debug.Log("onInterstitialAdFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdLoadFail(placementId, code, error);
            }
        }

        //开始播放
        public void onInterstitialAdVideoStart(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdPlayStart...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdStartPlayingVideo(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        //结束播放
        public void onInterstitialAdVideoEnd(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdPlayEnd...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdEndPlayingVideo(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        //播放失败
        public void onInterstitialAdVideoError(string placementId,string code, string error)
        {
            Debug.Log("onInterstitialAdPlayFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdFailedToPlayVideo(placementId, code, error);
            }
        }
        //广告关闭
        public void onInterstitialAdClose(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdClosed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdClose(placementId, new OfmCallbackInfo(callbackJson));
            }
        }
        //广告点击
        public void onInterstitialAdClicked(string placementId, string callbackJson)
        {
            Debug.Log("onInterstitialAdClicked...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdClick(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        public void onInterstitialAdShow(string placementId, string callbackJson){
            Debug.Log("onInterstitialAdShow...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onInterstitialAdShow(placementId, new OfmCallbackInfo(callbackJson));
            }
        }
       
    }
}
