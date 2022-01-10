using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Common;
using OfmSDK.Api;

namespace OfmSDK.iOS {
	public class OfmRewardedVideoAdClient : IOfmRewardedVideoAdClient {
		private  OfmRewardedVideoListener OfmSDKListener;

		public void addsetting (string placementId,string json){
			//todo...
		}
		public void setListener(OfmRewardedVideoListener listener) {
			Debug.Log("Unity: OfmRewardedVideoAdClient::setListener()");
	        OfmSDKListener = listener;
	    }

		public void loadVideoAd(string placementId, string mapJson, string customRulesJson) {
			Debug.Log("Unity: OfmRewardedVideoAdClient::loadVideoAd()");
			OfmRewardedVideoWrapper.setClientForPlacementID(placementId, this);
			OfmRewardedVideoWrapper.loadRewardedVideo(placementId, mapJson, customRulesJson);
		}

		public bool hasAdReady(string placementId) {
			Debug.Log("Unity: OfmRewardedVideoAdClient::hasAdReady()");
			return OfmRewardedVideoWrapper.isRewardedVideoReady(placementId);
		}

		//To be implemented
		public void setUserData(string placementId, string userId, string customData) {
			Debug.Log("Unity: OfmRewardedVideoAdClient::setUserData()");
	    }

	    public void showAd(string placementId, string mapJson) {
	    	Debug.Log("Unity: OfmRewardedVideoAdClient::showAd()");
	    	OfmRewardedVideoWrapper.showRewardedVideo(placementId, mapJson);
	    }

	    public void cleanAd(string placementId) {
	    	Debug.Log("Unity: OfmRewardedVideoAdClient::cleanAd()");
	    	OfmRewardedVideoWrapper.clearCache();
	    }

	    public void onApplicationForces(string placementId) {
			Debug.Log("Unity: OfmRewardedVideoAdClient::onApplicationForces()");
	    }

	    public void onApplicationPasue(string placementId) {
			Debug.Log("Unity: OfmRewardedVideoAdClient::onApplicationPasue()");
	    }

	    public string checkAdStatus(string placementId) {
	    	Debug.Log("Unity: OfmRewardedVideoAdClient::checkAdStatus()");
	    	return OfmRewardedVideoWrapper.checkAdStatus(placementId);
	    }

		//Callbacks
	    public void onRewardedVideoAdLoaded(string placementId, string callbackJson) {
	        Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoAdLoaded()");
	        if(OfmSDKListener != null) OfmSDKListener.onRewardedVideoAdLoaded(placementId, new OfmCallbackInfo(callbackJson));
	    }

	    public void onRewardedVideoAdFailed(string placementId, string code, string error) {
	        Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoAdFailed()");
	        if (OfmSDKListener != null) OfmSDKListener.onRewardedVideoAdLoadFail(placementId, code, error);
	    }

        public void onRewardedVideoAdPlayStart(string placementId, string callbackJson) {
	        Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoAdPlayStart()");
            if (OfmSDKListener != null) OfmSDKListener.onRewardedVideoAdPlayStart(placementId, new OfmCallbackInfo(callbackJson));
	    }

        public void onRewardedVideoAdPlayEnd(string placementId, string callbackJson) {
	        Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoAdPlayEnd()");
            if (OfmSDKListener != null) OfmSDKListener.onRewardedVideoAdPlayEnd(placementId, new OfmCallbackInfo(callbackJson));
	    }

	    public void onRewardedVideoAdPlayFailed(string placementId, string code, string error) {
	        Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoAdPlayFailed()");
	        if (OfmSDKListener != null) OfmSDKListener.onRewardedVideoAdPlayFail(placementId, code, error);
	    }

        public void onRewardedVideoAdClosed(string placementId, bool isRewarded, string callbackJson) {
	        Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoAdClosed()");
            if (OfmSDKListener != null) OfmSDKListener.onRewardedVideoAdPlayClosed(placementId, isRewarded, new OfmCallbackInfo(callbackJson));
	    }

        public void onRewardedVideoAdPlayClicked(string placementId, string callbackJson) {
	        Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoAdPlayClicked()");
            if (OfmSDKListener != null) OfmSDKListener.onRewardedVideoAdPlayClicked(placementId, new OfmCallbackInfo(callbackJson));
	    }

        public void onRewardedVideoReward(string placementId, string callbackJson) {
            Debug.Log("Unity: OfmRewardedVideoAdClient::onRewardedVideoReward()");
            if (OfmSDKListener != null) OfmSDKListener.onReward(placementId, new OfmCallbackInfo(callbackJson));
        }
	}
}
