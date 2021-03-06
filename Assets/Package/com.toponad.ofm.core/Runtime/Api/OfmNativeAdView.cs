using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

using OfmSDK.ThirdParty.LitJson;

namespace OfmSDK.Api
{
    public class OfmNativeAdView 
    {
        public OfmNativeConfig config;
        public OfmNativeAdView(OfmNativeConfig config)
        {
            this.config = config;
        }


        private string parentKey = "parent";
        private string appIconKey = "appIcon";
        private string mainImageKey = "mainImage";
        private string titleKey = "title";
        private string descKey = "desc";
        private string adLogoKey = "adLogo";
        private string ctaButtonKey = "cta";

        public string toJSON()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            if(config.parentProperty != null)
            {
                builder.Append("\"").Append(parentKey).Append("\"");
                builder.Append(":");
                builder.Append(JsonUtility.ToJson(config.parentProperty));
                builder.Append(",");
            }
            if(config.appIconProperty != null){
                builder.Append("\"").Append(appIconKey).Append("\"");
                builder.Append(":");
                builder.Append(JsonUtility.ToJson(config.appIconProperty));
                builder.Append(",");
            }
           
            if(config.mainImageProperty != null)
            {
                builder.Append("\"").Append(mainImageKey).Append("\"");
                builder.Append(":");
                builder.Append(JsonUtility.ToJson(config.mainImageProperty));
                builder.Append(",");
            }

            if(config.titleProperty != null)
            {
                builder.Append("\"").Append(titleKey).Append("\"");
                builder.Append(":");
                builder.Append(JsonUtility.ToJson(config.titleProperty));
                builder.Append(",");
            }
            if(config.descProperty != null)
            {
                builder.Append("\"").Append(descKey).Append("\"");
                builder.Append(":");
                builder.Append(JsonUtility.ToJson(config.descProperty));
                builder.Append(",");
            }

            if(config.adLogoProperty != null)
            {
                builder.Append("\"").Append(adLogoKey).Append("\"");
                builder.Append(":");
                builder.Append(JsonUtility.ToJson(config.adLogoProperty));
                builder.Append(",");
            }

            if(config.ctaButtonProperty != null)
            {
                builder.Append("\"").Append(ctaButtonKey).Append("\"");
                builder.Append(":");
                builder.Append(JsonUtility.ToJson(config.ctaButtonProperty));
            }

            string temp = builder.ToString();

            if (temp.EndsWith(","))
            {
                temp = temp.Substring(0, temp.Length - 1);
            }

            temp = temp + "}";

            return temp;

        }
    }
}
