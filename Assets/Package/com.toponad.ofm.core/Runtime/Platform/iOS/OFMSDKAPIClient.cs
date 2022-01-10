using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Common;
using OfmSDK.Api;
using AOT;
using System;
using OfmSDK.ThirdParty.LitJson;

namespace OfmSDK.iOS {
	public class OfmSDKAPIClient : IOfmSDKAPIClient {
        static private OfmGetUserLocationListener locationListener;
        static private OfmMediationSwitchListener mediationSwitchListener;
        public OfmSDKAPIClient () {
            Debug.Log("Unity:OfmSDKAPIClient::OfmSDKAPIClient()");
		}
		public void initSDK(string appId, string appKey, string defaultConfig) {
			Debug.Log("Unity:OfmSDKAPIClient::initSDK(string, string)");
			initSDK(appId, appKey, defaultConfig, null);
	    }

	    public void initSDK(string appId, string appKey, string defaultConfig, OfmSDKInitListener listener) {
	    	Debug.Log("Unity:OfmSDKAPIClient::initSDK(string, string, string, OfmSDKInitListener)");
	    	bool started = OfmManager.StartSDK(appId, appKey, defaultConfig);
            if (listener != null)
            {
                if (started)
                {
                    listener.initSuccess();
                }
                else
                {
                    listener.initFail("Failed to init.");
                }
            }
	    }

        [MonoPInvokeCallback(typeof(Func<string, int>))]
       static public int DidGetUserLocation(string location)
        {
            if (locationListener != null) { locationListener.didGetUserLocation(Int32.Parse(location)); }
            return 0;
        }

        public void getUserLocation(OfmGetUserLocationListener listener)
        {
            Debug.Log("Unity:OfmSDKAPIClient::getUserLocation()");
            OfmSDKAPIClient.locationListener = listener;
            OfmManager.getUserLocation(DidGetUserLocation);
        }

        public void setGDPRLevel(int level) {
	    	Debug.Log("Unity:OfmSDKAPIClient::setGDPRLevel()");
	    	OfmManager.SetDataConsent(level);
	    }

	    public void showGDPRAuth() {
	    	Debug.Log("Unity:OfmSDKAPIClient::showGDPRAuth()");
	    	OfmManager.ShowGDPRAuthDialog();
	    }

	    public void setPurchaseFlag() {
			OfmManager.setPurchaseFlag();
		}

		public void clearPurchaseFlag() {
			OfmManager.clearPurchaseFlag();
		}

		public bool purchaseFlag() {
			return OfmManager.purchaseFlag();
		}

	    public void addNetworkGDPRInfo(int networkType, string mapJson) {
	    	Debug.Log("Unity:OfmSDKAPIClient::addNetworkGDPRInfo()");
	    	OfmManager.SetNetworkGDPRInfo(networkType, mapJson);
	    }

        public void setChannel(string channel)
        {
            OfmManager.setChannel(channel);
        }

        public void setSubChannel(string subchannel)
        {
            OfmManager.setSubChannel(subchannel);
        }

        public void initCustomMap(string jsonMap)
        {
            OfmManager.setCustomMap(jsonMap);
        }

        public void setCustomDataForPlacementID(string customData, string placementID)
        {
            OfmManager.setCustomDataForPlacementID(customData, placementID);
        }

        public void setLogDebug(bool isDebug)
        {
            OfmManager.setLogDebug(isDebug);
        }

        public int getGDPRLevel()
        {
            return OfmManager.GetDataConsent();
        }

        public bool isEUTraffic()
        {
            return OfmManager.isEUTraffic();
        }

        public void deniedUploadDeviceInfo(string deniedInfo)
        {
            OfmManager.deniedUploadDeviceInfo(deniedInfo);
        }

        public void setPersonalizedAdState(int personalizedAdState)
        {
            OfmManager.setPersonalizedAdState(personalizedAdState);
        }

        public void setHasUserConsent(bool hasUserConsent)
        {
            OfmManager.setHasUserConsent(hasUserConsent);
        }

        public void setIsAgeRestrictedUser(bool isAgeRestrictedUser)
        {
            OfmManager.setIsAgeRestrictedUser(isAgeRestrictedUser);
        }

        public void setDoNotSell(bool doNotSell)
        {
            OfmManager.setDoNotSell(doNotSell);
        }

        public void setTimeoutForWaitingSetting(int millisecond)
        {
            OfmManager.setTimeoutForWaitingSetting(millisecond);
        }

        public int getMediationId()
        {
            return OfmManager.getMediationId();
        }

        public string getMediationConfig()
        {
            return OfmManager.getMediationConfig();
        }

        [MonoPInvokeCallback(typeof(Func<string, int>))]
       static public int MediationSwitch(string mediationConfig)
        {
            Debug.Log("Unity:OfmSDKAPIClient::MediationSwitch(" + mediationConfig + ")");
            if (mediationSwitchListener != null) { 
                JsonData jsonData = JsonMapper.ToObject(mediationConfig);
                IDictionary msgDict = (System.Collections.IDictionary)jsonData;
                mediationSwitchListener.onMediationSwitch((string)jsonData["oldConfig"],(string)jsonData["newConfig"]);
            }
            return 0;
        }
        public void setMediationSwitchListener(OfmMediationSwitchListener listener)
        {
            OfmSDKAPIClient.mediationSwitchListener = listener;
            OfmManager.setMediationSwitchListener(MediationSwitch);
        }
	}
}
