using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

using OfmSDK.Common;
using OfmSDK.ThirdParty.LitJson;

namespace OfmSDK.Api
{
	public class OfmInterstitialAd
	{
		private static readonly OfmInterstitialAd instance = new OfmInterstitialAd();
		private IOfmInterstitialAdClient client;

		private OfmInterstitialAd()
		{
            client = GetOfmInterstitialAdClient();
		}

		public static OfmInterstitialAd Instance 
		{
			get
			{
				return instance;
			}
		}

		public void loadInterstitialAd(string placementId, Dictionary<string,string> pairs, Dictionary<string, object> customRules)
        {
            client.loadInterstitialAd(placementId, JsonMapper.ToJson(pairs), JsonMapper.ToJson(customRules));
        }

		public void setListener(OfmInterstitialAdListener listener)
        {
            client.setListener(listener);
        }

        public bool hasInterstitialAdReady(string placementId)
        {
            return client.hasInterstitialAdReady(placementId);
        }

        public string checkAdStatus(string placementId)
        {
            return client.checkAdStatus(placementId);
        }

        public void showInterstitialAd(string placementId)
        {
            client.showInterstitialAd(placementId, JsonMapper.ToJson(new Dictionary<string, string>()));
        }

        public void showInterstitialAd(string placementId, Dictionary<string, string> pairs)
        {
            client.showInterstitialAd(placementId, JsonMapper.ToJson(pairs));
        }

        public IOfmInterstitialAdClient GetOfmInterstitialAdClient()
        {
            return OfmSDK.OfmAdsClientFactory.BuildInterstitialAdClient();
        }
	}
}
