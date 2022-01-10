using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.ThirdParty.LitJson;

namespace OfmSDK.Api
{
    public class OfmCallbackInfo
    {
        public readonly int mediation_id;
        public readonly int network_firm_id;
        public readonly string network_firm_name;
//        public readonly string adsource_id;
//        public readonly int adsource_index;
//        public readonly double adsource_price;
//        public readonly int adsource_isheaderbidding;

//        public readonly string id;
        public readonly double publisher_revenue;
        public readonly string currency;
        public readonly string country;
        public readonly string adunit_id;

        public readonly string adunit_format;
//        public readonly string precision;
//        public readonly string network_type;
        public readonly string network_placement_id;
        public readonly string mediation_placement_id;
//        public readonly int ecpm_level;

//        public readonly int segment_id;
        public readonly string scenario_id;
//        public readonly string scenario_reward_name;
//        public readonly int scenario_reward_number;
//        public readonly string placement_reward_name;
//        public readonly int placement_reward_number;

//        public readonly string sub_channel;
//        public readonly string channel;
//        public readonly Dictionary<string, object> custom_rule;

        public readonly string ilrd;
        private string callbackJson;

        public OfmCallbackInfo(string callbackJson)
        {
            try
            {
                this.callbackJson = callbackJson;

                JsonData jsonData = JsonMapper.ToObject(callbackJson);
                mediation_id = int.Parse(jsonData.ContainsKey("mediation_id") ? jsonData["mediation_id"].ToString() : "0");
                network_firm_id = int.Parse(jsonData.ContainsKey("network_firm_id") ? jsonData["network_firm_id"].ToString() : "0");
                network_firm_name = jsonData.ContainsKey("network_firm_name") ? (string)jsonData["network_firm_name"] : "";

//                adsource_id = jsonData.ContainsKey("adsource_id") ? (string)jsonData["adsource_id"] : "";
//                adsource_index = int.Parse(jsonData.ContainsKey("adsource_index") ? jsonData["adsource_index"].ToString() : "-1");
//                adsource_price = double.Parse(jsonData.ContainsKey("adsource_price") ? jsonData["adsource_price"].ToString() : "0");

//                adsource_isheaderbidding = 0;
//                if (jsonData.ContainsKey("adsource_isheaderbidding")) {
//                    adsource_isheaderbidding = int.Parse(jsonData.ContainsKey("adsource_isheaderbidding") ? jsonData["adsource_isheaderbidding"].ToString() : "0");
//                } else if (jsonData.ContainsKey("adsource_isHeaderBidding")) {
//                    adsource_isheaderbidding = int.Parse(jsonData.ContainsKey("adsource_isHeaderBidding") ? jsonData["adsource_isHeaderBidding"].ToString() : "0");
//                }

//                id = jsonData.ContainsKey("id") ? (string)jsonData["id"] : "";
                publisher_revenue = double.Parse(jsonData.ContainsKey("publisher_revenue") ? jsonData["publisher_revenue"].ToString() : "0");
                currency = jsonData.ContainsKey("currency") ? (string)jsonData["currency"] : "";
                country = jsonData.ContainsKey("country") ? (string)jsonData["country"] : "";

                adunit_format = jsonData.ContainsKey("adunit_format") ? (string)jsonData["adunit_format"] : "";
                adunit_id = jsonData.ContainsKey("adunit_id") ? (string)jsonData["adunit_id"] : "";

//                precision = jsonData.ContainsKey("precision") ? (string)jsonData["precision"] : "";

//                network_type = jsonData.ContainsKey("network_type") ? (string)jsonData["network_type"] : "";

                network_placement_id = jsonData.ContainsKey("network_placement_id") ? (string)jsonData["network_placement_id"] : "";
                mediation_placement_id = jsonData.ContainsKey("mediation_placement_id") ? (string)jsonData["mediation_placement_id"] : "";
//                ecpm_level = int.Parse(jsonData.ContainsKey("ecpm_level") ? jsonData["ecpm_level"].ToString() : "0");
//                segment_id = int.Parse(jsonData.ContainsKey("segment_id") ? jsonData["segment_id"].ToString() : "0");
                scenario_id = jsonData.ContainsKey("scenario_id") ? (string)jsonData["scenario_id"] : "";// RewardVideo & Interstitial

//                scenario_reward_name = jsonData.ContainsKey("scenario_reward_name") ? (string)jsonData["scenario_reward_name"] : "";
//                scenario_reward_number = int.Parse(jsonData.ContainsKey("scenario_reward_number") ? jsonData["scenario_reward_number"].ToString() : "0");

//                channel = jsonData.ContainsKey("channel") ? (string)jsonData["channel"] : "";
//                sub_channel = jsonData.ContainsKey("sub_channel") ? (string)jsonData["sub_channel"] : "";
//                custom_rule = jsonData.ContainsKey("custom_rule") ? JsonMapper.ToObject<Dictionary<string, object>>(jsonData["custom_rule"].ToJson()) : null;

//                reward_custom_data = jsonData.ContainsKey("reward_custom_data") ? (string)jsonData["reward_custom_data"] : "";
                ilrd = jsonData.ContainsKey("ilrd") ? (string)jsonData["ilrd"] : "";
            }
            catch (System.Exception e) {
                System.Console.WriteLine("Exception caught: {0}", e);
            }
        }

        public string getOriginJSONString()
        {
            return callbackJson;
        }

        public Dictionary<string, object> toDictionary()
        {
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

            dataDictionary.Add("mediation_id", mediation_id);
            if (network_firm_id != 0)
            {
                dataDictionary.Add("network_firm_id", network_firm_id);
            }
            if (!string.IsNullOrEmpty(network_firm_name))
            {
                dataDictionary.Add("network_firm_name", network_firm_name);
            }

//            dataDictionary.Add("adsource_id", adsource_id);
//            dataDictionary.Add("adsource_index", adsource_index);
//            dataDictionary.Add("adsource_price", adsource_price);
//            dataDictionary.Add("adsource_isheaderbidding", adsource_isheaderbidding);
//            dataDictionary.Add("id", id);
            if (publisher_revenue != 0)
            {
                dataDictionary.Add("publisher_revenue", publisher_revenue);
            }
            if (!string.IsNullOrEmpty(currency))
            {
                dataDictionary.Add("currency", currency);
            }
            if (!string.IsNullOrEmpty(country))
            {
                dataDictionary.Add("country", country);
            }
            if (!string.IsNullOrEmpty(adunit_id))
            {
                dataDictionary.Add("adunit_id", adunit_id);
            }
            if (!string.IsNullOrEmpty(adunit_format))
            {
                dataDictionary.Add("adunit_format", adunit_format);
            }
            if (!string.IsNullOrEmpty(network_placement_id))
            {
                dataDictionary.Add("network_placement_id", network_placement_id);
            }
            if (!string.IsNullOrEmpty(mediation_placement_id))
            {
                dataDictionary.Add("mediation_placement_id", mediation_placement_id);
            }
            if (!string.IsNullOrEmpty(scenario_id))
            {
                dataDictionary.Add("scenario_id", scenario_id);
            }
            if (!string.IsNullOrEmpty(ilrd))
            {
                dataDictionary.Add("ilrd", ilrd);
            }


            //            dataDictionary.Add("precision", precision);
            //            dataDictionary.Add("network_type", network_type);


            //            dataDictionary.Add("ecpm_level", ecpm_level);
            //            dataDictionary.Add("segment_id", segment_id);

            //            dataDictionary.Add("scenario_reward_name", scenario_reward_name);
            //            dataDictionary.Add("scenario_reward_number", scenario_reward_number);

            //            dataDictionary.Add("placement_reward_name", placement_reward_name);
            //            dataDictionary.Add("placement_reward_number", placement_reward_number);
            //            dataDictionary.Add("sub_channel", sub_channel);
            //            dataDictionary.Add("channel", channel);
            //            dataDictionary.Add("custom_rule", custom_rule);
            
            return dataDictionary;
        }


    }
}
