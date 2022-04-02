using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AOT;
using OfmSDK.ThirdParty.LitJson;
using System;

public class OfmUnityCBridge {
	public delegate void CCallBack(string wrapperClass, string msg);
	
    #if UNITY_IOS || UNITY_IPHONE
	[DllImport("__Internal")]
    extern static bool message_from_unity(string msg, CCallBack callback);

    [DllImport("__Internal")]
    extern static int get_message_for_unity(string msg, CCallBack callback);

    [DllImport("__Internal")]
    extern static string get_string_message_for_unity(string msg, CCallBack callback);
    #endif

    [MonoPInvokeCallback(typeof(CCallBack))]
    static public void MessageFromC(string wrapperClass, string msg) {
        Debug.Log("Unity: OfmUnityCBridge::MessageFromC(" + wrapperClass + "," + msg + ")");
        JsonData jsonData = JsonMapper.ToObject(msg);
        if (wrapperClass.Equals("OfmRewardedVideoWrapper"))
        {
            Debug.Log("Unity: OfmUnityCBridge::MessageFromC(), hit rv");
            OfmRewardedVideoWrapper.InvokeCallback(jsonData);
        }
        else if (wrapperClass.Equals("OfmInterstitialAdWrapper"))
        {
            OfmInterstitialAdWrapper.InvokeCallback(jsonData);
        }
        else if (wrapperClass.Equals("OfmBannerAdWrapper"))
        {
            OfmBannerAdWrapper.InvokeCallback(jsonData);
        }
    }

    static public bool SendMessageToC(string className, string selector, object[] arguments) {
        return SendMessageToC(className, selector, arguments, false);
    }

    static public int GetMessageFromC(string className, string selector, object[] arguments) {
        Debug.Log("Unity: OfmUnityCBridge::GetMessageFromC()");
        Dictionary<string, object> msgDict = new Dictionary<string, object>();
        msgDict.Add("class", className);
        msgDict.Add("selector", selector);
        msgDict.Add("arguments", arguments);
        #if UNITY_IOS || UNITY_IPHONE
        return get_message_for_unity(JsonMapper.ToJson(msgDict), null);
        #else
        return 0;
        #endif
    }

    static public string GetStringMessageFromC(string className, string selector, object[] arguments) {
        Debug.Log("Unity: OfmUnityCBridge::GetStringMessageFromC()");
        Dictionary<string, object> msgDict = new Dictionary<string, object>();
        msgDict.Add("class", className);
        msgDict.Add("selector", selector);
        msgDict.Add("arguments", arguments);
        #if UNITY_IOS || UNITY_IPHONE
        return get_string_message_for_unity(JsonMapper.ToJson(msgDict), null);
        #else 
        return "";
        #endif
    }

    static public bool SendMessageToC(string className, string selector, object[] arguments, bool carryCallback) {
        Debug.Log("Unity: OfmUnityCBridge::SendMessageToC()");
        Dictionary<string, object> msgDict = new Dictionary<string, object>();
    	msgDict.Add("class", className);
    	msgDict.Add("selector", selector);
    	msgDict.Add("arguments", arguments);
        CCallBack callback = null;
        if (carryCallback) callback = MessageFromC;
        #if UNITY_IOS || UNITY_IPHONE
        return message_from_unity(JsonMapper.ToJson(msgDict), callback);
        #else
        return false;
        #endif
    }

#if UNITY_IOS || UNITY_IPHONE
    [DllImport("__Internal")]
    extern static bool message_from_unity(string msg, Func<string, int> callback);
#endif
    static public void SendMessageToCWithCallBack(string className, string selector, object[] arguments, Func<string, int> callback)
    {
        Debug.Log("Unity: OfmUnityCBridge::SendMessageToCWithCallBack()");
        Dictionary<string, object> msgDict = new Dictionary<string, object>();
        msgDict.Add("class", className);
        msgDict.Add("selector", selector);
        msgDict.Add("arguments", arguments);
#if UNITY_IOS || UNITY_IPHONE
        message_from_unity(JsonMapper.ToJson(msgDict), callback);
#endif
    }
}
