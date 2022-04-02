using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using OfmSDK.Common;
using OfmSDK.ThirdParty.LitJson;


namespace OfmSDK.Api
{
    public class OfmNativeAdLoadingExtra
    {
        public static readonly string kOfmNativeAdLoadingExtraNativeAdSizeStruct = "native_ad_size_struct";
        public static readonly string kOfmNativeAdLoadingExtraNativeAdSize = "native_ad_size";
        public static readonly string kOfmNativeAdSizeUsesPixelFlagKey = "uses_pixel";
    }

    public class OfmNativeAd
    {

        private static readonly OfmNativeAd instance = new OfmNativeAd();
        private IOfmNativeAdClient client;

        public OfmNativeAd(){
            client = GetOfmNativeAdClient();
        }

        public static OfmNativeAd Instance
        {
            get
            {
                return instance;
            }
        }


        public void loadNativeAd(string placementId, Dictionary<String,object> pairs, Dictionary<string, object> customRules)
        {
            if (pairs != null && pairs.ContainsKey(OfmNativeAdLoadingExtra.kOfmNativeAdLoadingExtraNativeAdSizeStruct))
            {
                OfmSize size = (OfmSize)(pairs[OfmNativeAdLoadingExtra.kOfmNativeAdLoadingExtraNativeAdSizeStruct]);
                pairs.Add(OfmNativeAdLoadingExtra.kOfmNativeAdLoadingExtraNativeAdSize, size.width + "x" + size.height);
                pairs.Add(OfmNativeAdLoadingExtra.kOfmNativeAdSizeUsesPixelFlagKey, size.usesPixel);
            }
            client.loadNativeAd(placementId,JsonMapper.ToJson(pairs), JsonMapper.ToJson(customRules));
        }

        public bool hasAdReady(string placementId){
            return client.hasAdReady(placementId);
        }

        public void setListener(OfmNativeAdListener listener){
            client.setListener(listener);
        }

        public void renderAdToScene(string placementId, OfmNativeAdView anyThinkNativeAdView){
            client.renderAdToScene(placementId, anyThinkNativeAdView);
        }

        public void cleanAdView(string placementId, OfmNativeAdView anyThinkNativeAdView){
            client.cleanAdView(placementId, anyThinkNativeAdView);
        }

        public void onApplicationForces(string placementId, OfmNativeAdView anyThinkNativeAdView){
            client.onApplicationForces(placementId, anyThinkNativeAdView);
        }

        public void onApplicationPasue(string placementId, OfmNativeAdView anyThinkNativeAdView){
            client.onApplicationPasue(placementId, anyThinkNativeAdView);
        }

        public void cleanCache(string placementId){
            client.cleanCache(placementId);
        }



        public IOfmNativeAdClient GetOfmNativeAdClient()
        {
            return OfmSDK.OfmAdsClientFactory.BuildNativeAdClient();
        }

    }
}