using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OfmManager {
	private static bool SDKStarted;
	public static bool StartSDK(string appID, string appKey, string defaultConfig) {
		Debug.Log("Unity: OfmManager::StartSDK(" + appID + "," + appKey + "," + defaultConfig +")");
		if (!SDKStarted) {
			Debug.Log("Has not been started before, will starting SDK");
			SDKStarted = true;
			return OfmUnityCBridge.SendMessageToC("OFMUnityManager", "startSDKWithAppID:appKey:defaultConfig:", new object[]{appID, appKey, defaultConfig});
		} else {
			Debug.Log("SDK has been started already, ignore this call");
            return false;
		}
	}

	public static void setPurchaseFlag() {
		OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setPurchaseFlag", null);
	}

	public static void clearPurchaseFlag() {
		OfmUnityCBridge.SendMessageToC("OFMUnityManager", "clearPurchaseFlag", null);
	}

	public static bool purchaseFlag() {
		return OfmUnityCBridge.SendMessageToC("OFMUnityManager", "clearPurchaseFlag", null);
	}

	public static bool isEUTraffic() {
		return OfmUnityCBridge.SendMessageToC("OFMUnityManager", "inDataProtectionArea", null);
	}

    public static void getUserLocation(Func<string, int> callback)
    {
        Debug.Log("Unity:OfmManager::getUserLocation()");
        OfmUnityCBridge.SendMessageToCWithCallBack("OFMUnityManager", "getUserLocation:", new object[] { }, callback);
    }

	public static void ShowGDPRAuthDialog() {
		OfmUnityCBridge.SendMessageToC("OFMUnityManager", "presentDataConsentDialog", null);
	}

	public static int GetDataConsent() {
		return OfmUnityCBridge.GetMessageFromC("OFMUnityManager", "getDataConsent", null);
	}

	public static void SetDataConsent(int consent) {
		OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setDataConsent:", new object[]{consent});
	}

	public static void SetNetworkGDPRInfo(int network, string mapJson) {
		OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setDataConsent:network:", new object[]{mapJson, network});
	}

    public static void setChannel(string channel)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setChannel:", new object[] {channel});
    }

    public static void setSubChannel(string subchannel)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setSubChannel:", new object[] {subchannel});
    }

    public static void setCustomMap(string jsonMap)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setCustomData:", new object[] { jsonMap });
    }

    public static void setCustomDataForPlacementID(string customData, string placementID)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setCustomData:forPlacementID:", new object[] {customData, placementID});
    }

    public static void setLogDebug(bool isDebug)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setDebugLog:", new object[] { isDebug ? "true" : "false" });
    }

    public static void deniedUploadDeviceInfo(string deniedInfo)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "deniedUploadDeviceInfo:", new object[] {deniedInfo});
    }

    public static void setPersonalizedAdState(int personalizedAdState)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setPersonalizedAdState:", new object[] {personalizedAdState});
    }

    public static void setHasUserConsent(bool hasUserConsent)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setHasUserConsent:", new object[] {hasUserConsent});
    }

    public static void setIsAgeRestrictedUser(bool isAgeRestrictedUser)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setIsAgeRestrictedUser:", new object[] {isAgeRestrictedUser});
    }

    public static void setDoNotSell(bool doNotSell)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setDoNotSell:", new object[] {doNotSell});
    }

    public static void setTimeoutForWaitingSetting(int millisecond)
    {
        OfmUnityCBridge.SendMessageToC("OFMUnityManager", "setTimeoutForWaitingSetting:", new object[] {millisecond});
    }

    public static int getMediationId()
    {
        return OfmUnityCBridge.GetMessageFromC("OFMUnityManager", "getMediationId", null);
    }

    public static string getMediationConfig()
    {
        return OfmUnityCBridge.GetStringMessageFromC("OFMUnityManager", "getMediationConfig", null);
    }

    public static void setMediationSwitchListener(Func<string, int> callback)
    {
        Debug.Log("Unity:OfmManager::setMediationSwitchListener()");
        OfmUnityCBridge.SendMessageToCWithCallBack("OFMUnityManager", "setMediationSwitchListener:", new object[] { }, callback);
    }
}
