using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Common;
using OfmSDK.Api;

namespace OfmSDK.iOS {
	public class OfmBannerAdClient : IOfmBannerAdClient {
		private  OfmBannerAdListener OfmSDKListener;

		public void addsetting(string placementId,string json){
			//todo...
		}

		public void setListener(OfmBannerAdListener listener) {
			Debug.Log("Unity: OfmBannerAdClient::setListener()");
	        OfmSDKListener = listener;
	    }

	    public void loadBannerAd(string placementId, string mapJson, string customRulesJson) {
			Debug.Log("Unity: OfmBannerAdClient::loadBannerAd()");
			OfmBannerAdWrapper.setClientForPlacementID(placementId, this);
			OfmBannerAdWrapper.loadBannerAd(placementId, mapJson, customRulesJson);
	    }

	    public void showBannerAd(string placementId, OfmRect rect) {
			Debug.Log("Unity: OfmBannerAdClient::showBannerAd()");
			OfmBannerAdWrapper.showBannerAd(placementId, rect);
	    }

        public void showBannerAd(string placementId, string position)
        {
            Debug.Log("Unity: OfmBannerAdClient::showBannerAd()");
            OfmBannerAdWrapper.showBannerAd(placementId, position);
        }

        public void cleanBannerAd(string placementId) {
			Debug.Log("Unity: OfmBannerAdClient::cleanBannerAd()");	
			OfmBannerAdWrapper.cleanBannerAd(placementId);	
	    }

	    public void hideBannerAd(string placementId) {
	    	Debug.Log("Unity: OfmBannerAdClient::hideBannerAd()");	
			OfmBannerAdWrapper.hideBannerAd(placementId);
	    }

	    public void showBannerAd(string placementId) {
	    	Debug.Log("Unity: OfmBannerAdClient::showBannerAd()");	
			OfmBannerAdWrapper.showBannerAd(placementId);
	    }

        public void cleanCache(string placementId) {
			Debug.Log("Unity: OfmBannerAdClient::cleanCache()");
			OfmBannerAdWrapper.clearCache();
        }

        public string checkAdStatus(string placementId) {
            Debug.Log("Unity: OfmBannerAdClient::checkAdStatus()");
            return OfmBannerAdWrapper.checkAdStatus(placementId);
        }

        public void OnBannerAdLoad(string placementId, string callbackJson) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdLoad()");
	        if (OfmSDKListener != null) OfmSDKListener.onAdLoad(placementId, new OfmCallbackInfo(callbackJson));
	    }
	    
	    public void OnBannerAdLoadFail(string placementId, string code, string message) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdLoadFail()");
	        if (OfmSDKListener != null) OfmSDKListener.onAdLoadFail(placementId, code, message);
	    }
	    
	    public void OnBannerAdImpress(string placementId, string callbackJson) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdImpress()");
            if (OfmSDKListener != null) OfmSDKListener.onAdImpress(placementId, new OfmCallbackInfo(callbackJson));
	    }
	    
        public void OnBannerAdClick(string placementId, string callbackJson) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdClick()");
            if (OfmSDKListener != null) OfmSDKListener.onAdClick(placementId, new OfmCallbackInfo(callbackJson));
	    }
	    
        public void OnBannerAdAutoRefresh(string placementId, string callbackJson) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdAutoRefresh()");
            if (OfmSDKListener != null) OfmSDKListener.onAdAutoRefresh(placementId, new OfmCallbackInfo(callbackJson));
	    }
	    
	    public void OnBannerAdAutoRefreshFail(string placementId, string code, string message) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdAutoRefreshFail()");
	        if (OfmSDKListener != null) OfmSDKListener.onAdAutoRefreshFail(placementId, code, message);
	    }

	    public void OnBannerAdClose(string placementId) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdClose()");
	        if (OfmSDKListener != null) OfmSDKListener.onAdClose(placementId);
	    }

	    public void OnBannerAdCloseButtonTapped(string placementId, string callbackJson) {
			Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdCloseButton()");
	        if (OfmSDKListener != null) OfmSDKListener.onAdCloseButtonTapped(placementId, new OfmCallbackInfo(callbackJson));
	    }
	}
}