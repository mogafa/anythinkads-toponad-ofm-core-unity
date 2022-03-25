using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using OfmSDK.Common;
using OfmSDK.ThirdParty.LitJson;

namespace OfmSDK.Api
{
    public class OfmBannerAdLoadingExtra
    {
        public static readonly string kOfmBannerAdLoadingExtraBannerAdSize = "banner_ad_size";
        public static readonly string kOfmBannerAdLoadingExtraBannerAdSizeStruct = "banner_ad_size_struct";
        public static readonly string kOfmBannerAdSizeUsesPixelFlagKey = "uses_pixel";
        public static readonly string kOfmBannerAdShowingPisitionTop = "top";
        public static readonly string kOfmBannerAdShowingPisitionBottom = "bottom";

        public static readonly string kOfmBannerAdLoadingExtraAdaptiveWidth = "adaptive_width";
        public static readonly string kOfmBannerAdLoadingExtraAdaptiveOrientation = "adaptive_orientation";
        public static readonly int kOfmBannerAdLoadingExtraAdaptiveOrientationCurrent = 0;
        public static readonly int kOfmBannerAdLoadingExtraAdaptiveOrientationPortrait = 1;
        public static readonly int kOfmBannerAdLoadingExtraAdaptiveOrientationLandscape = 2;

    }
    public class OfmBannerAd 
	{
		private static readonly OfmBannerAd instance = new OfmBannerAd();
		private IOfmBannerAdClient client;

		private OfmBannerAd() 
		{
            client = GetOfmBannerAdClient();
		}

		public static OfmBannerAd Instance 
		{
			get 
			{
				return instance;
			}
		}

		/**
		API
		*/
		public void loadBannerAd(string placementId, Dictionary<string,object> pairs, Dictionary<string, object> customRules)
		{   
            if (pairs != null && pairs.ContainsKey(OfmBannerAdLoadingExtra.kOfmBannerAdLoadingExtraBannerAdSize))
            {
                client.loadBannerAd(placementId, JsonMapper.ToJson(pairs), JsonMapper.ToJson(customRules));
            }
            else if (pairs != null && pairs.ContainsKey(OfmBannerAdLoadingExtra.kOfmBannerAdLoadingExtraBannerAdSizeStruct))
            {
                OfmSize size = (OfmSize)(pairs[OfmBannerAdLoadingExtra.kOfmBannerAdLoadingExtraBannerAdSizeStruct]);
                pairs.Add(OfmBannerAdLoadingExtra.kOfmBannerAdLoadingExtraBannerAdSize, size.width + "x" + size.height);
                pairs.Add(OfmBannerAdLoadingExtra.kOfmBannerAdSizeUsesPixelFlagKey, size.usesPixel);

                //Dictionary<string, object> newPaires = new Dictionary<string, object> { { OfmBannerAdLoadingExtra.kOfmBannerAdLoadingExtraBannerAdSize, size.width + "x" + size.height }, { OfmBannerAdLoadingExtra.kOfmBannerAdSizeUsesPixelFlagKey, size.usesPixel } };
                client.loadBannerAd(placementId, JsonMapper.ToJson(pairs), JsonMapper.ToJson(customRules));
            }
            else
            {
                client.loadBannerAd(placementId, JsonMapper.ToJson(pairs), JsonMapper.ToJson(customRules));
            }
			
		}

        public string checkAdStatus(string placementId)
        {
            return client.checkAdStatus(placementId);
        }


        public void setListener(OfmBannerAdListener listener)
        {
            client.setListener(listener);
        }

        public void showBannerAd(string placementId, OfmRect rect)
        {
            client.showBannerAd(placementId, rect);
        }

        public void showBannerAd(string placementId, string position)
        {
            client.showBannerAd(placementId, position);
        }

        public void showBannerAd(string placementId)
        {
            client.showBannerAd(placementId);
        }

        public void hideBannerAd(string placementId)
        {
            client.hideBannerAd(placementId);
        }

        public void cleanBannerAd(string placementId)
        {
            client.cleanBannerAd(placementId);
        }

        public IOfmBannerAdClient GetOfmBannerAdClient()
        {
            return OfmSDK.OfmAdsClientFactory.BuildBannerAdClient();
        }
	}
}
