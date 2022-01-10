using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AOT;
using OfmSDK.ThirdParty.LitJson;
using OfmSDK.iOS;

public class OfmRewardedVideoWrapper:OfmAdWrapper {
    static private Dictionary<string, OfmRewardedVideoAdClient> clients;
	static private string CMessageReceiverClass = "OFMRewardedVideoWrapper";

    static public new void InvokeCallback(JsonData jsonData) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::InvokeCallback()");
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
        
        if (callback.Equals("OnRewardedVideoLoaded")) {
            OnRewardedVideoLoaded((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnRewardedVideoLoadFailure")) {
            Dictionary<string, object> errorDict = new Dictionary<string, object>();
            Dictionary<string, object> errorMsg = JsonMapper.ToObject<Dictionary<string, object>>(msgJsonData["error"].ToJson());
            if (errorMsg["code"] != null) { errorDict.Add("code", errorMsg["code"]); }
            if (errorMsg["reason"] != null) { errorDict.Add("message", errorMsg["reason"]); }
            OnRewardedVideoLoadFailure((string)msgDict["placement_id"], errorDict);
        } else if (callback.Equals("OnRewardedVideoPlayFailure")) {
            Dictionary<string, object> errorDict = new Dictionary<string, object>();
            Dictionary<string, object> errorMsg = JsonMapper.ToObject<Dictionary<string, object>>(msgJsonData["error"].ToJson());
            if (errorMsg.ContainsKey("code")) { errorDict.Add("code", errorMsg["code"]); }
            if (errorMsg.ContainsKey("reason")) { errorDict.Add("message", errorMsg["reason"]); }
            OnRewardedVideoPlayFailure((string)msgDict["placement_id"], errorDict);
        } else if (callback.Equals("OnRewardedVideoPlayStart")) {
            OnRewardedVideoPlayStart((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnRewardedVideoPlayEnd")) {
            OnRewardedVideoPlayEnd((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnRewardedVideoClick")) {
            OnRewardedVideoClick((string)msgDict["placement_id"], extraJson);
        } else if (callback.Equals("OnRewardedVideoClose")) {
            OnRewardedVideoClose((string)msgDict["placement_id"], (bool)msgDict["rewarded"], extraJson);
        } else if (callback.Equals("OnRewardedVideoReward")) {
            OnRewardedVideoReward((string)msgDict["placement_id"], extraJson);
        }
    }

    //Public method(s)
    static public void setClientForPlacementID(string placementID, OfmRewardedVideoAdClient client) {
        if (clients == null) clients = new Dictionary<string, OfmRewardedVideoAdClient>();
        if (clients.ContainsKey(placementID)) clients.Remove(placementID);
        clients.Add(placementID, client);
    }

    static public void setExtra(Dictionary<string, object> extra) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::setExtra()");
        OfmUnityCBridge.SendMessageToC(CMessageReceiverClass, "setExtra:", new object[]{extra});
    }

    static public void loadRewardedVideo(string placementID, string extraData, string customRulesJson) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::loadRewardedVideo(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessageReceiverClass, "loadRewardedVideoWithPlacementID:extraJSONString:customDataJSONString:callback:", new object[]{placementID, extraData != null ? extraData : "", customRulesJson != null ? customRulesJson : ""}, true);
    }

    static public bool isRewardedVideoReady(string placementID) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::isRewardedVideoReady(" + placementID + ")");
        return OfmUnityCBridge.SendMessageToC(CMessageReceiverClass, "rewardedVideoReadyForPlacementID:", new object[]{placementID});
    }

    static public void showRewardedVideo(string placementID, string mapJson) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::showRewardedVideo(" + placementID + ")");
        OfmUnityCBridge.SendMessageToC(CMessageReceiverClass, "showRewardedVideoWithPlacementID:extraJsonString:", new object[]{placementID, mapJson});
    }

    static public void clearCache() {
        Debug.Log("Unity: OfmRewardedVideoWrapper::clearCache()");
        OfmUnityCBridge.SendMessageToC(CMessageReceiverClass, "clearCache", null);
    }

    static public string checkAdStatus(string placementID) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::checkAdStatus(" + placementID + ")");
        return OfmUnityCBridge.GetStringMessageFromC(CMessageReceiverClass, "checkAdStatus:", new object[]{placementID});
    }

    static public string getValidAdCaches(string placementID)
    {
        Debug.Log("Unity: OfmRewardedVideoWrapper::getValidAdCaches(" + placementID + ")");
        return OfmUnityCBridge.GetStringMessageFromC(CMessageReceiverClass, "getValidAdCaches:", new object[] { placementID });
    }

    //Callbacks
    static public void OnRewardedVideoLoaded(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoLoaded()");
        if (clients[placementID] != null) clients[placementID].onRewardedVideoAdLoaded(placementID, callbackJson);
    }

    static public void OnRewardedVideoLoadFailure(string placementID, Dictionary<string, object> errorDict) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoLoadFailure()");
        Debug.Log("placementID = " + placementID + "errorDict = " + JsonMapper.ToJson(errorDict));
        if (clients[placementID] != null) clients[placementID].onRewardedVideoAdFailed(placementID, (string)errorDict["code"], (string)errorDict["message"]);
    }

     static public void OnRewardedVideoPlayFailure(string placementID, Dictionary<string, object> errorDict) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoPlayFailure()");
        if (clients[placementID] != null) clients[placementID].onRewardedVideoAdPlayFailed(placementID, (string)errorDict["code"], (string)errorDict["message"]);

    }

    static public void OnRewardedVideoPlayStart(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoPlayStart()");
        if (clients[placementID] != null) clients[placementID].onRewardedVideoAdPlayStart(placementID, callbackJson);
    }

    static public void OnRewardedVideoPlayEnd(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoPlayEnd()");
        if (clients[placementID] != null) clients[placementID].onRewardedVideoAdPlayEnd(placementID, callbackJson);
    }

    static public void OnRewardedVideoClick(string placementID, string callbackJson) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoClick()");
        if (clients[placementID] != null) clients[placementID].onRewardedVideoAdPlayClicked(placementID, callbackJson);
    }

    static public void OnRewardedVideoClose(string placementID, bool rewarded, string callbackJson) {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoClose()");
        if (clients[placementID] != null) clients[placementID].onRewardedVideoAdClosed(placementID, rewarded, callbackJson);
    }
    static public void OnRewardedVideoReward(string placementID, string callbackJson)
    {
        Debug.Log("Unity: OfmRewardedVideoWrapper::OnRewardedVideoReward()");
        if (clients[placementID] != null) clients[placementID].onRewardedVideoReward(placementID, callbackJson);
    }

}


