﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OfmSDK.Common;
using OfmSDK.Api;
namespace OfmSDK.Android
{
    public class OfmRewardedVideoAdClient : AndroidJavaProxy,IOfmRewardedVideoAdClient
    {

        private Dictionary<string, AndroidJavaObject> videoHelperMap = new Dictionary<string, AndroidJavaObject>();

		//private  AndroidJavaObject videoHelper;
        private  OfmRewardedVideoListener anyThinkListener;

        public OfmRewardedVideoAdClient() : base("com.ofm.unitybridge.videoad.VideoListener")
        {
            
        }


        public void loadVideoAd(string placementId, string mapJson, string customRulesJson)
        {

            //如果不存在则直接创建对应广告位的helper
            if(!videoHelperMap.ContainsKey(placementId))
            {
                AndroidJavaObject videoHelper = new AndroidJavaObject(
                    "com.ofm.unitybridge.videoad.VideoHelper", this);
                videoHelper.Call("initVideo", placementId);
                videoHelperMap.Add(placementId, videoHelper);
                Debug.Log("OfmRewardedVideoAdClient : no exit helper ,create helper ");
            }

            try
            {
                Debug.Log("OfmRewardedVideoAdClient : loadVideoAd ");
                videoHelperMap[placementId].Call("fillVideo", mapJson, customRulesJson);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);
            }


        }

        public void setListener(OfmRewardedVideoListener listener)
        {
            anyThinkListener = listener;
        }

        public bool hasAdReady(string placementId)
        {
			bool isready = false;
			Debug.Log ("OfmRewardedVideoAdClient : hasAdReady....");
			try{
                if (videoHelperMap.ContainsKey(placementId)) {
                    isready = videoHelperMap[placementId].Call<bool> ("isAdReady");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);
			}
			return isready; 
        }

        public string checkAdStatus(string placementId)
        {
            string adStatusJsonString = "";
            Debug.Log("OfmRewardedVideoAdClient : checkAdStatus....");
            try
            {
                if (videoHelperMap.ContainsKey(placementId))
                {
                    adStatusJsonString = videoHelperMap[placementId].Call<string>("checkAdStatus");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmRewardedVideoAdClient :  error." + e.Message);
            }

            return adStatusJsonString;
        }

        public void setUserData(string placementId, string userId, string customData)
        {
			Debug.Log("OfmRewardedVideoAdClient : setUserData  " );

			try{
                if (videoHelperMap.ContainsKey(placementId)) {
                    this.videoHelperMap[placementId].Call ("setUserData",userId,customData);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);
			}
        }

        public void showAd(string placementId, string scenario)
        {
			Debug.Log("OfmRewardedVideoAdClient : showAd " );

			try{
                if (videoHelperMap.ContainsKey(placementId)) {
                    this.videoHelperMap[placementId].Call ("showVideo", scenario);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);

			}
        }

		public void addsetting (string placementId,string json){
			Debug.Log("OfmRewardedVideoAdClient : addsetting" );

			try{
				if (videoHelperMap.ContainsKey(placementId)) {
					this.videoHelperMap[placementId].Call ("addsetting",json);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);
			}
		}

        public void cleanAd(string placementId)
        {
			
			Debug.Log("OfmRewardedVideoAdClient : clean" );

			try{
                if (videoHelperMap.ContainsKey(placementId)) {
                    this.videoHelperMap[placementId].Call ("clean");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);
			}
        }

        public void onApplicationForces(string placementId)
        {
			Debug.Log ("onApplicationForces.... ");
			try{
				if (videoHelperMap.ContainsKey(placementId)) {
					this.videoHelperMap[placementId].Call ("onResume");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);
			}
        }

        public void onApplicationPasue(string placementId)
        {
			Debug.Log ("onApplicationPasue.... ");
			try{
				if (videoHelperMap.ContainsKey(placementId)) {
					this.videoHelperMap[placementId].Call ("onPause");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmRewardedVideoAdClient :  error."+e.Message);
			}
        }

        //广告加载成功
        public void onRewardedVideoAdLoaded(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdLoaded...unity3d.");
            if(anyThinkListener != null){
                anyThinkListener.onRewardedVideoAdLoaded(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        //广告加载失败
        public void onRewardedVideoAdFailed(string placementId,string code, string error)
        {
            Debug.Log("onRewardedVideoAdFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdLoadFail(placementId, code, error);
            }
        }

        //开始播放
        public void onRewardedVideoAdPlayStart(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdPlayStart...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayStart(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        //结束播放
        public void onRewardedVideoAdPlayEnd(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdPlayEnd...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayEnd(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        //播放失败
        public void onRewardedVideoAdPlayFailed(string placementId,string code, string error)
        {
            Debug.Log("onRewardedVideoAdPlayFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayFail(placementId, code, error);
            }
        }
        //广告关闭
        public void onRewardedVideoAdClosed(string placementId,bool isRewarded, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdClosed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayClosed(placementId,isRewarded, new OfmCallbackInfo(callbackJson));
            }
        }
        //广告点击
        public void onRewardedVideoAdPlayClicked(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdPlayClicked...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayClicked(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        //广告激励下发
        public void onReward(string placementId, string callbackJson)
        {
            Debug.Log("onReward...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onReward(placementId, new OfmCallbackInfo(callbackJson));
            }
        }
       
    }
}
