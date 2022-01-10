using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using OfmSDK.Common;
using OfmSDK.ThirdParty.LitJson;


namespace OfmSDK.Api
{
    public class OfmRewardedVideo
    {
        private static readonly OfmRewardedVideo instance = new OfmRewardedVideo();
        private IOfmRewardedVideoAdClient client;

        private OfmRewardedVideo()
        {
            client = GetOfmRewardedClient();
        }

        public static OfmRewardedVideo Instance
        {
            get
            {
                return instance;
            }
        }


		/***
		 * 
		 */
        public void loadVideoAd(string placementId, Dictionary<string,string> pairs, Dictionary<string, object> customRules)
        {
            
            client.loadVideoAd(placementId, JsonMapper.ToJson(pairs), JsonMapper.ToJson(customRules));

        }

		public void setListener(OfmRewardedVideoListener listener)
        {
            client.setListener(listener);
        }

		public void addsetting(string placementId,Dictionary<string,object> pairs){
			client.addsetting (placementId,JsonMapper.ToJson(pairs));
		}
        public bool hasAdReady(string placementId)
        {
            return client.hasAdReady(placementId);

        }

        public string checkAdStatus(string placementId)
        {
            return client.checkAdStatus(placementId);

        }

        public void setUserData(string placementId, string userId, string customData)
        {
            client.setUserData(placementId, userId, customData);

        }

        public void showAd(string placementId)
        {
            client.showAd(placementId, JsonMapper.ToJson(new Dictionary<string, string>()));
        }

        public void showAd(string placementId, Dictionary<string, string> pairs)
        {
            client.showAd(placementId, JsonMapper.ToJson(pairs));
        }

        public void cleanAd(string placementId)
        {
            client.cleanAd(placementId);
        }

        public void onApplicationForces(string placementId)
        {
            client.onApplicationForces(placementId);
        }

        public void onApplicationPasue(string placementId)
        {
            client.onApplicationPasue(placementId);
        }

        public IOfmRewardedVideoAdClient GetOfmRewardedClient()
        {
            return OfmSDK.OfmAdsClientFactory.BuildRewardedVideoAdClient();
        }

    }
}