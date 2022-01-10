using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AOT;
using OfmSDK.ThirdParty.LitJson;
using OfmSDK.iOS;
using OfmSDK.Api;

public class OfmBannerAdWrapper:OfmAdWrapper {
	static private Dictionary<string, OfmBannerAdClient> clients;
    static private string CMessaageReceiverClass = "OFMBannerAdWrapper";

    static public new void InvokeCallback(JsonData jsonData) {
        Debug.Log("Unity: OfmBannerAdWrapper::InvokeCallback()");
        string extraJson = "";
        string callback = (string)jsonData["callback"];
        Dictionary<string, object> msgDict = JsonMapper.ToObject<Dictionary<string, object>>(jsonData["msg"].ToJson());
        JsonData msgJsonData = jsonData["msg"];
        IDictionary idic = (System.Collections.IDictionary)msgJsonData;

        if (idic.Contains("extra")) { 
            JsonData extraJsonDate = msgJsonData["extra"];
            if (extraJsonDate != null) {
                extraJson = msgJsonData["extra"].ToJson();
            }
        }

        if (callback.Equals("OnBannerAdLoad")) {
            OnBannerAdLoad((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnBannerAdLoadFail")) {
            Dictionary<string, object> errorMsg = JsonMapper.ToObject<Dictionary<string, object>>(msgJsonData["error"].ToJson());
            OnBannerAdLoadFail((string)msgDict["placement_id"], (string)errorMsg["code"], (string)errorMsg["reason"]);
        } else if (callback.Equals("OnBannerAdImpress")) {
            OnBannerAdImpress((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnBannerAdClick")) {
            OnBannerAdClick((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnBannerAdAutoRefresh")) {
            OnBannerAdAutoRefresh((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnBannerAdAutoRefreshFail")) {
            Dictionary<string, object> errorMsg = JsonMapper.ToObject<Dictionary<string, object>>(msgJsonData["error"].ToJson());
            OnBannerAdAutoRefreshFail((string)msgDict["placement_id"], (string)errorMsg["code"], (string)errorMsg["reason"]);
        } else if (callback.Equals("OnBannerAdClose")) {
            OnBannerAdClose((string)msgDict["placement_id"]);
        } else if (callback.Equals("OnBannerAdCloseButtonTapped")) {
            OnBannerAdCloseButtonTapped((string)msgDict["placement_id"], extraJson);
        }
    }

    static public void loadBannerAd(string placementID, string extraData, string customRulesJson) {
        Debug.Log("Unity: OfmBannerAdWrapper::loadBannerAd(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "loadBannerAdWithPlacementID:extraJSONString:customDataJSONString:callback:", new object[]{placementID, extraData != null ? extraData : "", customRulesJson != null ? customRulesJson : ""}, true);
    }

    static public string checkAdStatus(string placementID) {
        Debug.Log("Unity: OfmBannerAdWrapper::checkAdStatus(" + placementID + ")");
        return OfmUnityCBridge.GetStringMessageFromC(CMessaageReceiverClass, "checkAdStatus:", new object[]{placementID});
    }

    static public void hideBannerAd(string placementID) {
        Debug.Log("Unity: OfmBannerAdWrapper::showBannerAd(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "hideBannerAdWithPlacementID:", new object[]{placementID}, false);
    }

    static public void showBannerAd(string placementID) {
        Debug.Log("Unity: OfmBannerAdWrapper::showBannerAd(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "showBannerAdWithPlacementID:", new object[]{placementID}, false);
    }

    static public void showBannerAd(string placementID, string position)
    {
        Debug.Log("Unity: OfmBannerAdWrapper::showBannerAd(" + placementID + "," + position + ")");
        Dictionary<string, object> rectDict = new Dictionary<string, object> { { "position", position } };
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "showBannerAdWithPlacementID:rect:", new object[] { placementID, JsonMapper.ToJson(rectDict) }, false);
    }

    static public void showBannerAd(string placementID, OfmRect rect) {
        Debug.Log("Unity: OfmBannerAdWrapper::showBannerAd(" + placementID + ")");
        Dictionary<string, object> rectDict = new Dictionary<string, object>{ {"x", rect.x},  {"y", rect.y}, {"width", rect.width}, {"height", rect.height}, {"uses_pixel", rect.usesPixel}};
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "showBannerAdWithPlacementID:rect:", new object[]{placementID, JsonMapper.ToJson(rectDict)}, false);
    }

    static public void cleanBannerAd(string placementID) {
        Debug.Log("Unity: OfmBannerAdWrapper::cleanBannerAd(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "removeBannerAdWithPlacementID:", new object[]{placementID}, false);
    }

    static public void clearCache() {
        Debug.Log("Unity: OfmBannerAdWrapper::clearCache()");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "clearCache", null);
    }

    static public void setClientForPlacementID(string placementID, OfmBannerAdClient client) {
        if (clients == null) clients = new Dictionary<string, OfmBannerAdClient>();
        if (clients.ContainsKey(placementID)) clients.Remove(placementID);
        clients.Add(placementID, client);
    }

    static private void OnBannerAdLoad(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdLoad()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdLoad(placementID, callbackJson);
    }
    
    static private void OnBannerAdLoadFail(string placementID, string code, string message) {
        Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdLoadFail()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdLoadFail(placementID, code, message);
    }
    
    static private void OnBannerAdImpress(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdImpress()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdImpress(placementID, callbackJson);
    }
    
    static private void OnBannerAdClick(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdClick()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdClick(placementID, callbackJson);
    }
    
    static private void OnBannerAdAutoRefresh(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdAutoRefresh()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdAutoRefresh(placementID, callbackJson);
    }
    
    static private void OnBannerAdAutoRefreshFail(string placementID, string code, string message) {
        Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdAutoRefreshFail()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdAutoRefreshFail(placementID, code, message);
    }

    static private void OnBannerAdCloseButtonTapped(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmBannerAdWrapper::onAdCloseButtonTapped()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdCloseButtonTapped(placementID, callbackJson);
    }

    static private void OnBannerAdClose(string placementID) {
        Debug.Log("Unity: OfmBannerAdWrapper::OnBannerAdClose()");
        if (clients[placementID] != null) clients[placementID].OnBannerAdClose(placementID);
    }
}
