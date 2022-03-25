using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Api;

namespace OfmSDK.Android
{
    public class OfmOnMediationSwitchListener : AndroidJavaProxy
    {
        OfmMediationSwitchListener mListener;
        public OfmOnMediationSwitchListener(OfmMediationSwitchListener listener): base("com.ofm.unitybridge.sdkinit.SDKMediationSwitchListener")
        {
            mListener = listener;
        }


        public void onMediationSwitch(string oldMediationConfig, string newMediationConfig)
        {
            if (mListener != null)
            {
                mListener.onMediationSwitch(oldMediationConfig, newMediationConfig);     
            }
        }
       
    }
}
