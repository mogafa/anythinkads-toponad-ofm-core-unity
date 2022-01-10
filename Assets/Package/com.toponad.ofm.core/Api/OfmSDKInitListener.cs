using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OfmSDK.Api
{
    public interface OfmSDKInitListener
    {

        void initSuccess();
        void initFail(string message);
    }
}
