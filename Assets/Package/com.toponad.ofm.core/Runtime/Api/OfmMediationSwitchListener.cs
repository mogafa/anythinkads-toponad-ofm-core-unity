using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OfmSDK.Api
{
    public interface OfmMediationSwitchListener
    {
        void onMediationSwitch(string oldMediationConfig, string newMediationConfig);
    }
}
