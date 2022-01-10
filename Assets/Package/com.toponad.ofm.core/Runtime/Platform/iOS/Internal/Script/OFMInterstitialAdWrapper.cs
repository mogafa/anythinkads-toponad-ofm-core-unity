using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AOT;
using OfmSDK.ThirdParty.LitJson;
using OfmSDK.iOS;

public class OfmInterstitialAdWrapper:OfmAdWrapper {
	static private Dictionary<string, OfmInterstitialAdClient> clients;
	static private string CMessaageReceiverClass = "OFMInterstitialAdWrapper";

	static public new void InvokeCallback(JsonData jsonData) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::InvokeCallback()");
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
        
        if (callback.Equals("OnInterstitialAdLoaded")) {
            OnInterstitialAdLoaded((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnInterstitialAdLoadFailure")) {
            Dictionary<string, object> errorDict = new Dictionary<string, object>();
            Dictionary<string, object> errorMsg = JsonMapper.ToObject<Dictionary<string, object>>(msgJsonData["error"].ToJson());
            if (errorMsg.ContainsKey("code")) { errorDict.Add("code", errorMsg["code"]); }
            if (errorMsg.ContainsKey("reason")) { errorDict.Add("message", errorMsg["reason"]); }
            OnInterstitialAdLoadFailure((string)msgDict["placement_id"], errorDict);
        } else if (callback.Equals("OnInterstitialAdVideoPlayFailure")) {
            Dictionary<string, object> errorDict = new Dictionary<string, object>();
            Dictionary<string, object> errorMsg = JsonMapper.ToObject<Dictionary<string, object>>(msgJsonData["error"].ToJson());
            if (errorMsg.ContainsKey("code")) { errorDict.Add("code", errorMsg["code"]); }
            if (errorMsg.ContainsKey("reason")) { errorDict.Add("message", errorMsg["reason"]); }
            OnInterstitialAdVideoPlayFailure((string)msgDict["placement_id"], errorDict);
        } else if (callback.Equals("OnInterstitialAdVideoPlayStart")) {
            OnInterstitialAdVideoPlayStart((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnInterstitialAdVideoPlayEnd")) {
            OnInterstitialAdVideoPlayEnd((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnInterstitialAdShow")) {
            OnInterstitialAdShow((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnInterstitialAdClick")) {
            OnInterstitialAdClick((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnInterstitialAdClose")) {
            OnInterstitialAdClose((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnInterstitialAdFailedToShow")) {
            OnInterstitialAdFailedToShow((string)msgDict["placement_id"]);
        }
    }

    static public void setClientForPlacementID(string placementID, OfmInterstitialAdClient client) {
        if (clients == null) clients = new Dictionary<string, OfmInterstitialAdClient>();
        if (clients.ContainsKey(placementID)) clients.Remove(placementID);
        clients.Add(placementID, client);
    }

    static public void loadInterstitialAd(string placementID, string extraData, string customRulesJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::loadInterstitialAd(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "loadInterstitialAdWithPlacementID:extraJSONString:customDataJSONString:callback:", new object[]{placementID, extraData != null ? extraData : "", customRulesJson != null ? customRulesJson : ""}, true);
    }

    static public bool hasInterstitialAdReady(string placementID) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::isInterstitialAdReady(" + placementID + ")");
        return OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "interstitialAdReadyForPlacementID:", new object[]{placementID});
    }

    static public void showInterstitialAd(string placementID, string mapJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::showInterstitialAd(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "showInterstitialAdWithPlacementID:extraJsonString:", new object[]{placementID, mapJson});
    }

    static public void clearCache(string placementID) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::clearCache()");
        OfmUnityCBridge.SendMessageToC(CMessaageReceiverClass, "clearCache", null);
    }

    static public string checkAdStatus(string placementID) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::checkAdStatus(" + placementID + ")");
        return OfmUnityCBridge.GetStringMessageFromC(CMessaageReceiverClass, "checkAdStatus:", new object[]{placementID});
    }

    static public string getValidAdCaches(string placementID)
    {
        Debug.Log("Unity: OfmInterstitialAdWrapper::checkAdStatus(" + placementID + ")");
        return OfmUnityCBridge.GetStringMessageFromC(CMessaageReceiverClass, "getValidAdCaches:", new object[] { placementID });
    }

    //Callbacks
    static private void OnInterstitialAdLoaded(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdLoaded()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdLoaded(placementID, callbackJson);
    }

    static private void OnInterstitialAdLoadFailure(string placementID, Dictionary<string, object> errorDict) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdLoadFailure()");
        Debug.Log("placementID = " + placementID + "errorDict = " + JsonMapper.ToJson(errorDict));
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdLoadFailure(placementID, (string)errorDict["code"], (string)errorDict["message"]);
    }

     static private void OnInterstitialAdVideoPlayFailure(string placementID, Dictionary<string, object> errorDict) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdVideoPlayFailure()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdVideoPlayFailure(placementID, (string)errorDict["code"], (string)errorDict["message"]);
    }

    static private void OnInterstitialAdVideoPlayStart(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdPlayStart()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdVideoPlayStart(placementID, callbackJson);
    }

    static private void OnInterstitialAdVideoPlayEnd(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdVideoPlayEnd()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdVideoPlayEnd(placementID, callbackJson);
    }

    static private void OnInterstitialAdShow(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdShow()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdShow(placementID, callbackJson);
    }

    static private void OnInterstitialAdFailedToShow(string placementID) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdFailedToShow()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdFailedToShow(placementID);
    }

    static private void OnInterstitialAdClick(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdClick()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdClick(placementID, callbackJson);
    }

    static private void OnInterstitialAdClose(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmInterstitialAdWrapper::OnInterstitialAdClose()");
        if (clients[placementID] != null) clients[placementID].OnInterstitialAdClose(placementID, callbackJson);
    }
}



