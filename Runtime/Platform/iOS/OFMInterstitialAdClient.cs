using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Common;
using OfmSDK.Api;

namespace OfmSDK.iOS {
	public class OfmInterstitialAdClient : IOfmInterstitialAdClient {
		private  OfmInterstitialAdListener OfmSDKListener;

		public void addsetting(string placementId,string json){
			//todo...
		}

		public void setListener(OfmInterstitialAdListener listener) {
			Debug.Log("Unity: OfmInterstitialAdClient::setListener()");
	        OfmSDKListener = listener;
	    }

	    public void loadInterstitialAd(string placementId, string mapJson, string customRulesJson) {
			Debug.Log("Unity: OfmInterstitialAdClient::loadInterstitialAd()");
            OfmInterstitialAdWrapper.setClientForPlacementID(placementId, this);
			OfmInterstitialAdWrapper.loadInterstitialAd(placementId, mapJson, customRulesJson);
		}

		public bool hasInterstitialAdReady(string placementId) {
			Debug.Log("Unity: OfmInterstitialAdClient::hasInterstitialAdReady()");
			return OfmInterstitialAdWrapper.hasInterstitialAdReady(placementId);
		}

		public void showInterstitialAd(string placementId, string mapJson) {
			Debug.Log("Unity: OfmInterstitialAdClient::showInterstitialAd()");
			OfmInterstitialAdWrapper.showInterstitialAd(placementId, mapJson);
		}

		public void cleanCache(string placementId) {
			Debug.Log("Unity: OfmInterstitialAdClient::cleanCache()");
			OfmInterstitialAdWrapper.clearCache(placementId);
		}

		public string checkAdStatus(string placementId) {
			Debug.Log("Unity: OfmInterstitialAdClient::checkAdStatus()");
			return OfmInterstitialAdWrapper.checkAdStatus(placementId);
		}

		//Callbacks
	    public void OnInterstitialAdLoaded(string placementID, string callbackJson) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdLoaded()");
	        if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdLoad(placementID, new OfmCallbackInfo(callbackJson));
	    }

	    public void OnInterstitialAdLoadFailure(string placementID, string code, string error) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdLoadFailure()");
	        if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdLoadFail(placementID, code, error);
	    }

	     public void OnInterstitialAdVideoPlayFailure(string placementID, string code, string error) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdVideoPlayFailure()");
	        if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdFailedToPlayVideo(placementID, code, error);
	    }

	    public void OnInterstitialAdVideoPlayStart(string placementID, string callbackJson) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdPlayStart()");
	        if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdStartPlayingVideo(placementID, new OfmCallbackInfo(callbackJson));
	    }

	    public void OnInterstitialAdVideoPlayEnd(string placementID, string callbackJson) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdVideoPlayEnd()");
	        if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdEndPlayingVideo(placementID, new OfmCallbackInfo(callbackJson));
	    }

        public void OnInterstitialAdShow(string placementID, string callbackJson) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdShow()");
            if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdShow(placementID, new OfmCallbackInfo(callbackJson));
	    }

        public void OnInterstitialAdFailedToShow(string placementID) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdFailedToShow()");
	        if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdFailedToShow(placementID);
	    }

        public void OnInterstitialAdClick(string placementID, string callbackJson) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdClick()");
            if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdClick(placementID, new OfmCallbackInfo(callbackJson));
	    }

        public void OnInterstitialAdClose(string placementID, string callbackJson) {
	    	Debug.Log("Unity: OfmInterstitialAdClient::OnInterstitialAdClose()");
            if (OfmSDKListener != null) OfmSDKListener.onInterstitialAdClose(placementID, new OfmCallbackInfo(callbackJson));
	    }
	}
}
