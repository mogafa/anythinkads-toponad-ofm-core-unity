using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Api;

namespace OfmSDK.Common
{
    public interface IOfmSDKAPIClient
    {
        void initSDK(string appId, string appKey, string defaultConfig);
        void initSDK(string appId, string appKey, string defaultConfig, OfmSDKInitListener listener);
        void setLogDebug(bool isDebug);
        void initCustomMap(string cutomMap);
        void setPersonalizedAdState(int personalizedAdState);

        void setHasUserConsent(bool hasUserConsent);

        void setIsAgeRestrictedUser(bool isAgeRestrictedUser);

        void setDoNotSell(bool doNotSell);

        void setTimeoutForWaitingSetting(int millisecond);

        int getMediationId();

        string getMediationConfig();

        void setMediationSwitchListener(OfmMediationSwitchListener listener);
    }
}
