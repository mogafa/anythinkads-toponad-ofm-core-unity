using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;



using OfmSDK.Common;
using OfmSDK.ThirdParty.LitJson;


namespace OfmSDK.Api
{
    public interface OfmGetUserLocationListener
    {
        void didGetUserLocation(int location);
    }

    public class OfmSDKAPI
    {
        //public static readonly int kOfmUserLocationUnknown = 0;
        //public static readonly int kOfmUserLocationInEU = 1;
        //public static readonly int kOfmUserLocationOutOfEU = 2;

        //public static readonly int PERSONALIZED = 0;
        //public static readonly int NONPERSONALIZED = 1;
        //public static readonly int UNKNOWN = 2;

        ////for android and ios
        //public static readonly string OS_VERSION_NAME = "os_vn";
        //public static readonly string OS_VERSION_CODE = "os_vc";
        //public static readonly string APP_PACKAGE_NAME = "package_name";
        //public static readonly string APP_VERSION_NAME = "app_vn";
        //public static readonly string APP_VERSION_CODE = "app_vc";

        //public static readonly string BRAND = "brand";
        //public static readonly string MODEL = "model";
        //public static readonly string DEVICE_SCREEN_SIZE = "screen";
        //public static readonly string MNC = "mnc";
        //public static readonly string MCC = "mcc";

        //public static readonly string LANGUAGE = "language";
        //public static readonly string TIMEZONE = "timezone";
        //public static readonly string USER_AGENT = "ua";
        //public static readonly string ORIENTATION = "orient";
        //public static readonly string NETWORK_TYPE = "network_type";

        ////for android
        //public static readonly string INSTALLER = "it_src";
        //public static readonly string ANDROID_ID = "android_id";
        //public static readonly string GAID = "gaid";
        //public static readonly string MAC = "mac";
        //public static readonly string IMEI = "imei";
        //public static readonly string OAID = "oaid";

        ////for ios
        //public static readonly string IDFA = "idfa";
        //public static readonly string IDFV = "idfv";


        public static readonly int PERSONALIZED_NOT_BLOCK = 1;
        public static readonly int PERSONALIZED_BLOCK = 2;



        private static readonly IOfmSDKAPIClient client = GetOfmSDKAPIClient();

        public static void initSDK(string appId, string appKey, string defaultConfig)
        {
            client.initSDK(appId, appKey, defaultConfig);
        }

        public static void initSDK(string appId, string appKey, string defaultConfig, OfmSDKInitListener listener)
        {
            client.initSDK(appId, appKey, defaultConfig, listener);
        }

       
        public static void initCustomMap(Dictionary<string, string> customMap)
        {
            client.initCustomMap(JsonMapper.ToJson(customMap));
        }

       

        public static void setLogDebug(bool isDebug)
        {
            client.setLogDebug(isDebug);
        }

        public static void setPersonalizedAdState(int personalizedAdState)
        {
            client.setPersonalizedAdState(personalizedAdState);
        }
        public static void setHasUserConsent(bool hasUserConsent)
        {
            client.setHasUserConsent(hasUserConsent);
        }
        public static void setIsAgeRestrictedUser(bool isAgeRestrictedUser)
        {
            client.setIsAgeRestrictedUser(isAgeRestrictedUser);
        }
        public static void setDoNotSell(bool doNotSell)
        {
            client.setDoNotSell(doNotSell);
        }


        public static void setTimeoutForWaitingSetting(int millisecond)
        {
            client.setTimeoutForWaitingSetting(millisecond);
        }

        public static int getMediationId()
        {
            return client.getMediationId();
        }

        public static string getMediationConfig()
        {
            return client.getMediationConfig();
        }

        public static void setMediationSwitchListener(OfmMediationSwitchListener listener)
        {
            client.setMediationSwitchListener(listener);
        }



        private static IOfmSDKAPIClient GetOfmSDKAPIClient(){
            Debug.Log("GetOfmSDKAPIClient");
            return OfmSDK.OfmAdsClientFactory.BuildSDKAPIClient();
        }

    }
}

