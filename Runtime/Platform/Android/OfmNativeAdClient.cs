using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfmSDK.Common;
using OfmSDK.Api;

namespace OfmSDK.Android
{
    public class OfmNativeAdClient : AndroidJavaProxy, IOfmNativeAdClient
    {
		
        private Dictionary<string, AndroidJavaObject> nativeAdHelperMap = new Dictionary<string, AndroidJavaObject>();
        private OfmNativeAdListener mlistener;

        public OfmNativeAdClient(): base("com.ofm.unitybridge.nativead.NativeListener")
        {

        }

        public void loadNativeAd(string placementId, string mapJson, string customRulesJson)
        {
			Debug.Log ("loadNativeAd....jsonmap:"+mapJson);
            if(!nativeAdHelperMap.ContainsKey(placementId)){
                AndroidJavaObject nativeHelper = new AndroidJavaObject(
                    "com.ofm.unitybridge.nativead.NativeHelper", this);
                nativeHelper.Call("initNative", placementId);
                nativeAdHelperMap.Add(placementId, nativeHelper);
            }
			try{
                if (nativeAdHelperMap.ContainsKey(placementId)) {
                    nativeAdHelperMap[placementId].Call ("loadNative",mapJson,customRulesJson);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmNativeAdClient :  error."+e.Message);
			}
        }


        public bool hasAdReady(string placementId)
        {
			bool isready = false;
			Debug.Log ("hasAdReady....");
			try{
                if (nativeAdHelperMap.ContainsKey(placementId)) {
                    isready = nativeAdHelperMap[placementId].Call<bool> ("isAdReady");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmNativeAdClient :  error."+e.Message);
			}
			return isready;   
        }

        public void setListener(OfmNativeAdListener listener)
        {
            mlistener = listener;
        }

		public void renderAdToScene(string placementId, OfmNativeAdView anyThinkNativeAdView)
        {	
			string showconfig = anyThinkNativeAdView.toJSON ();
            //暂未实现 show
			Debug.Log ("renderAdToScene....showconfig >>>:"+showconfig);
			try{
                if (nativeAdHelperMap.ContainsKey(placementId)) {
                    nativeAdHelperMap[placementId].Call ("show",showconfig);
				}
			}catch(System.Exception e){
				Debug.Log ("OfmNativeAdClient :  error."+e.Message);
				System.Console.WriteLine("Exception caught: {0}", e);
			}
        }

        public void cleanAdView(string placementId, OfmNativeAdView anyThinkNativeAdView)
        {
           //
			Debug.Log ("cleanAdView.... ");
			try{

				if (nativeAdHelperMap.ContainsKey(placementId)) {
					nativeAdHelperMap[placementId].Call ("cleanView");
				}

			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmNativeAdClient :  error."+e.Message);
			}
        }

        public void onApplicationForces(string placementId, OfmNativeAdView anyThinkNativeAdView)
        {


			Debug.Log ("onApplicationForces.... ");
			try{

				if (nativeAdHelperMap.ContainsKey(placementId)) {
					nativeAdHelperMap[placementId].Call ("onResume");
				}

			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmNativeAdClient :  error."+e.Message);
			}
        }


        public void onApplicationPasue(string placementId, OfmNativeAdView anyThinkNativeAdView)
        {

			Debug.Log ("onApplicationPasue.... ");
			try{
				

				if (nativeAdHelperMap.ContainsKey(placementId)) {
					nativeAdHelperMap[placementId].Call ("onPause");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmNativeAdClient :  error."+e.Message);
			}
        }

        public void cleanCache(string placementId)
        {
			Debug.Log ("cleanCache....");
			try{
                if (nativeAdHelperMap.ContainsKey(placementId)) {
                    nativeAdHelperMap[placementId].Call ("clean");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("OfmNativeAdClient :  error."+e.Message);
			}
        }

        /**
     * 广告展示回调
     *
     * @param view
     */
        public void onAdImpressed(string placementId, string callbackJson)
        {
            Debug.Log("onAdImpressed...unity3d.");
            if(mlistener != null){
                mlistener.onAdImpressed(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        /**
     * 广告点击回调
     *
     * @param view
     */
        public void onAdClicked(string placementId, string callbackJson)
        {
            Debug.Log("onAdClicked...unity3d.");
            if (mlistener != null)
            {
                mlistener.onAdClicked(placementId, new OfmCallbackInfo(callbackJson));
            }
        }

        /**
     * 广告视频开始回调
     *
     * @param view
     */
        public void onAdVideoStart(string placementId)
        {
            Debug.Log("onAdVideoStart...unity3d.");
            if (mlistener != null)
            {
                mlistener.onAdVideoStart(placementId);
            }
        }

        /**
     * 广告视频结束回调
     *
     * @param view
     */
        public void onAdVideoEnd(string placementId)
        {
            Debug.Log("onAdVideoEnd...unity3d.");
            if (mlistener != null)
            {
                mlistener.onAdVideoEnd(placementId);
            }
        }

        /**
     * 广告视频进度回调
     *
     * @param view
     */
        public void onAdVideoProgress(string placementId,int progress)
        {
            Debug.Log("onAdVideoProgress...progress[" + progress + "]");
            if (mlistener != null)
            {
                mlistener.onAdVideoProgress(placementId, progress);
            }
        }

        /**
   * 广告视频进度回调
   *
   * @param view
   */
        public void onAdCloseButtonClicked(string placementId, string callbackJson)
        {
            Debug.Log("onAdCloseButtonClicked...unity3d");
            if (mlistener != null)
            {
                mlistener.onAdCloseButtonClicked(placementId, new OfmCallbackInfo(callbackJson));
            }
        }


        /**
     * 广告加载成功
     */
        public void onNativeAdLoaded(string placementId, string callbackJson)
        {
            Debug.Log("onNativeAdLoaded...unity3d.");
            if (mlistener != null)
            {
                mlistener.onAdLoaded(placementId, new OfmCallbackInfo(callbackJson));
            }

        }

        /**
     * 广告加载失败
     */
        public void onNativeAdLoadFail(string placementId,string code, string msg)
        {
            Debug.Log("onNativeAdLoadFail...unity3d. code:" + code + " msg:" + msg);
            if (mlistener != null)
            {
                mlistener.onAdLoadFail(placementId, code, msg);
            }
        }


    }
}
