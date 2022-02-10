//
//  OFMInterstitialAdWrapper.m
//  UnityContainer
//
//  Created by Martin Lau on 2019/1/8.
//  Copyright © 2019 Martin Lau. All rights reserved.
//

#import "OFMInterstitialAdWrapper.h"
#import "OFMUnityUtilities.h"
#import <OFMSDK/OFMSDK.h>

NSString *const kLoadUseRVAsInterstitialKey = @"UseRewardedVideoAsInterstitial";
NSString *const kInterstitialExtraUsesRewardedVideo = @"uses_rewarded_video_flag";

@interface OFMInterstitialAdWrapper()<OFMInterstitialDelegate>
@end
@implementation OFMInterstitialAdWrapper
+(instancetype)sharedInstance {
    static OFMInterstitialAdWrapper *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[OFMInterstitialAdWrapper alloc] init];
    });
    return sharedInstance;
}

-(NSString*) scriptWrapperClass {
    return @"OFMInterstitialAdWrapper";
}

- (id)selWrapperClassWithDict:(NSDictionary *)dict callback:(void(*)(const char*, const char*))callback {
    NSString *selector = dict[@"selector"];
    NSArray<NSString*>* arguments = dict[@"arguments"];
    NSString *firstObject = @"";
    NSString *secondObject = @"";
    NSString *lastObject = @"";
    if (![OFMUnityUtilities isEmpty:arguments]) {
        for (int i = 0; i < arguments.count; i++) {
            if (i == 0) { firstObject = arguments[i]; }
            else if (i == 1) { secondObject = arguments[i]; }
            else { lastObject = arguments[i]; }
        }
    }
    
    if ([selector isEqualToString:@"loadInterstitialAdWithPlacementID:extraJSONString:customDataJSONString:callback:"]) {
        [self loadInterstitialAdWithPlacementID:firstObject extraJSONString:secondObject customDataJSONString:lastObject callback:callback];
    } else if ([selector isEqualToString:@"interstitialAdReadyForPlacementID:"]) {
        return [NSNumber numberWithBool:[self interstitialAdReadyForPlacementID:firstObject]];
    } else if ([selector isEqualToString:@"showInterstitialAdWithPlacementID:extraJsonString:"]) {
        [self showInterstitialAdWithPlacementID:firstObject extraJsonString:secondObject];
    }
    else if ([selector isEqualToString:@"checkAdStatus:"]) {
        return [self checkAdStatus:firstObject];
    }
    else if ([selector isEqualToString:@"clearCache"]) {
        [self clearCache];
    }
    return nil;
}

-(void) loadInterstitialAdWithPlacementID:(NSString*)placementID extraJSONString:(NSString*)extraJSONString customDataJSONString:(NSString*)customDataJSONString callback:(void(*)(const char*, const char*))callback {
    
    [self setCallBack:callback forKey:placementID];
    NSDictionary *extra = nil;
    if ([extraJSONString isKindOfClass:[NSString class]] && ![OFMUnityUtilities isEmpty:extraJSONString]) {
        NSDictionary *extraDict = [NSJSONSerialization JSONObjectWithData:[extraJSONString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        if (extraDict[kLoadUseRVAsInterstitialKey] != nil) { extra = @{kInterstitialExtraUsesRewardedVideo:@([extraDict[kLoadUseRVAsInterstitialKey] boolValue])};
        }
    }
    
    NSDictionary *customData = nil;
    if ([customDataJSONString isKindOfClass:[NSString class]] && ![OFMUnityUtilities isEmpty:customDataJSONString]) {
        NSDictionary *customDict = [NSJSONSerialization JSONObjectWithData:[customDataJSONString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        customData = customDict;
    }
    NSLog(@"extraDict = %@，customDict = %@", extra,customData);
    
    [[OFMInterstitialAdManager sharedManager] loadOFMADWithPlacementID:placementID extra:extra != nil ? extra : nil customData:customData delegate:self];
}

-(BOOL) interstitialAdReadyForPlacementID:(NSString*)placementID {
    return [[OFMInterstitialAdManager sharedManager] adReadyForPlacementID:placementID];
}

-(void) showInterstitialAdWithPlacementID:(NSString*)placementID extraJsonString:(NSString*)extraJsonString {
    NSDictionary *extraDict = ([extraJsonString isKindOfClass:[NSString class]] && [extraJsonString dataUsingEncoding:NSUTF8StringEncoding] != nil) ? [NSJSONSerialization JSONObjectWithData:[extraJsonString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil] : nil;
    [[OFMInterstitialAdManager sharedManager] showInterstitialWithPlacementID:placementID scene:extraDict[kOFMUnityUtilitiesAdShowingExtraScenarioKey] inViewController:[UIApplication sharedApplication].delegate.window.rootViewController delegate:self];
}

-(NSString*) checkAdStatus:(NSString *)placementID {
    OFMCheckLoadModel *checkLoadModel = [[OFMInterstitialAdManager sharedManager] checkAdLoadStatusForPlacementID:placementID];
    NSMutableDictionary *statusDict = [NSMutableDictionary dictionary];
    statusDict[@"isReady"] = @(checkLoadModel.isReady);
    statusDict[@"adInfo"] = checkLoadModel.adOfferInfo;
    NSLog(@"OFMInterstitialAdWrapper::statusDict = %@", statusDict);
    return statusDict.jsonString;
}

-(void) clearCache {
    // to do
}

#pragma mark - delegate method(s)
-(void) didFinishLoadingOFMADWithPlacementID:(NSString *)placementID extra:(NSDictionary *)extra{
    NSLog(@"OFMInterstitialAdWrapper::didFinishLoadingOFMADWithPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnInterstitialAdLoaded" placementID:placementID error:nil extra:extra];
}

-(void) didFailToLoadOFMADWithPlacementID:(NSString*)placementID error:(NSError*)error {
    error = error != nil ? error : [NSError errorWithDomain:@"com.anythink.Unity3DPackage" code:100001 userInfo:@{NSLocalizedDescriptionKey:@"AT has failed to load ad", NSLocalizedFailureReasonErrorKey:@"AT has failed to load ad"}];
    NSLog(@"OFMInterstitialAdWrapper::didFailToLoadOFMADWithPlacementID:%@ error:%@", placementID, error);
    [self invokeCallback:@"OnInterstitialAdLoadFailure" placementID:placementID error:error extra:nil];
}

-(void) interstitialDidShowForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMInterstitialAdWrapper::interstitialDidShowForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnInterstitialAdShow" placementID:placementID error:nil extra:extra];
    [[NSNotificationCenter defaultCenter] postNotificationName:kOFMUnityUtilitiesInterstitialImpressionNotification object:nil];
}

-(void) interstitialFailedToShowForPlacementID:(NSString*)placementID error:(NSError*)error extra:(NSDictionary *)extra {
    NSLog(@"OFMInterstitialAdWrapper::interstitialFailedToShowForPlacementID:%@ error:%@ extra:%@", placementID, error, extra);
    error = error != nil ? error : [NSError errorWithDomain:@"com.anythink.Unity3DPackage" code:100001 userInfo:@{NSLocalizedDescriptionKey:@"AT has failed to show ad", NSLocalizedFailureReasonErrorKey:@"AT has failed to show ad"}];
    [self invokeCallback:@"OnInterstitialAdFailedToShow" placementID:placementID error:error extra:nil];
}

-(void) interstitialDidStartPlayingVideoForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMInterstitialAdWrapper::interstitialDidStartPlayingVideoForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnInterstitialAdVideoPlayStart" placementID:placementID error:nil extra:extra];
}

-(void) interstitialDidEndPlayingVideoForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMInterstitialAdWrapper::interstitialDidEndPlayingVideoForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnInterstitialAdVideoPlayEnd" placementID:placementID error:nil extra:extra];
}

-(void) interstitialDidFailToPlayVideoForPlacementID:(NSString*)placementID error:(NSError*)error extra:(NSDictionary *)extra {
    NSLog(@"OFMInterstitialAdWrapper::interstitialDidFailToPlayVideoForPlacementID:%@ error:%@ extra:%@", placementID, error, extra);
    [self invokeCallback:@"OnInterstitialAdVideoPlayFailure" placementID:placementID error:error extra:extra];
}

-(void) interstitialDidCloseForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMInterstitialAdWrapper::interstitialDidCloseForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnInterstitialAdClose" placementID:placementID error:nil extra:extra];
    [[NSNotificationCenter defaultCenter] postNotificationName:kOFMUnityUtilitiesInterstitialCloseNotification object:nil];
}

-(void) interstitialDidClickForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMInterstitialAdWrapper::interstitialDidClickForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnInterstitialAdClick" placementID:placementID error:nil extra:extra];
}
@end
