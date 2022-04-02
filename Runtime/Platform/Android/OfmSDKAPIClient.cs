using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Common;
using OfmSDK.Api;

namespace OfmSDK.Android
{
    public class OfmSDKAPIClient : AndroidJavaProxy, IOfmSDKAPIClient
    {
		private AndroidJavaObject sdkInitHelper;
        private OfmSDKInitListener sdkInitListener;
        public OfmSDKAPIClient () : base("com.ofm.unitybridge.sdkinit.SDKInitListener")
        {
            this.sdkInitHelper = new AndroidJavaObject(
                "com.ofm.unitybridge.sdkinit.SDKInitHelper", this);
		}

        public void initSDK(string appId, string appKey, string defaultConfig)
        {
            this.initSDK(appId, appKey, defaultConfig, null);
        }

        public void initSDK(string appId, string appKey, string defaultConfig, OfmSDKInitListener listener)
        {
            Debug.Log("initSDK....");
            sdkInitListener = listener;
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("initAppliction", appId, appKey, defaultConfig);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmSDKAPIClient :  error."+e.Message);
            }
        }


        public void initCustomMap(string jsonMap)
        {
            Debug.Log("initCustomMap....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("initCustomMap", jsonMap);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }

       

        public void setLogDebug(bool isDebug)
        {
            Debug.Log("setLogDebug....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("setDebugLogOpen", isDebug);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }


        public void setPersonalizedAdState(int personalizedAdState)
        {
            Debug.Log("setPersonalizedAdState....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("setPersonalizedAdState", personalizedAdState);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }

        public void setHasUserConsent(bool hasUserConsent)
        {
            Debug.Log("setHasUserConsent....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("setHasUserConsent", hasUserConsent);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }

        public void setIsAgeRestrictedUser(bool isAgeRestrictedUser)
        {
            Debug.Log("setIsAgeRestrictedUser....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("setIsAgeRestrictedUser", isAgeRestrictedUser);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }

        public void setDoNotSell(bool doNotSell)
        {
            Debug.Log("setDoNotSell....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("setDoNotSell", doNotSell);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }

        public void setTimeoutForWaitingSetting(int millisecond)
        {
            Debug.Log("setTimeoutForWaitingSetting....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("setTimeoutForWaitingSetting", millisecond);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }

        public int getMediationId()
        {
            int id = 0;
            Debug.Log("getCurrentMediationId....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    id = this.sdkInitHelper.Call<int>("getCurrentMediationId");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
            return id;
        }

        public string getMediationConfig()
        {
            string config = "";
            Debug.Log("getCurrentMediationConfig....");
            try
            {
                if (this.sdkInitHelper != null)
                {
                    config = this.sdkInitHelper.Call<string>("getCurrentMediationConfig");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
            return config;
        }

        public void setMediationSwitchListener(OfmMediationSwitchListener listener)
        {
            Debug.Log("setMediationSwitchListener....");

            OfmOnMediationSwitchListener mediationSwitchListener = new OfmOnMediationSwitchListener(listener);

            try
            {
                if (this.sdkInitHelper != null)
                {
                    this.sdkInitHelper.Call("setMediationSwitchListener", mediationSwitchListener);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("OfmSDKAPIClient :  error." + e.Message);
            }
        }


        public void initSDKSuccess(string appid)
        {
            Debug.Log("initSDKSuccess...unity3d.");
            if(sdkInitListener != null){
                sdkInitListener.initSuccess();
            }
        }

        public void initSDKError(string appid, string message)
        {
            Debug.Log("initSDKError..unity3d..");
            if (sdkInitListener != null)
            {
                sdkInitListener.initFail(message);
            }
        }

    }
}
