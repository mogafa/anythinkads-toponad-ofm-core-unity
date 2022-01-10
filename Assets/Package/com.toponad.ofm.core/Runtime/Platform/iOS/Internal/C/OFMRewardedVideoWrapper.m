//
//  OFMRewardedVideoWrapper.m
//  UnityContainer
//
//  Created by Martin Lau on 08/08/2018.
//  Copyright © 2018 Martin Lau. All rights reserved.
//

#import "OFMRewardedVideoWrapper.h"
#import "OFMUnityUtilities.h"
#import <AnyThinkRewardedVideo/AnyThinkRewardedVideo.h>
#import <OfmSDK/OfmSDK.h>

NSString *const kLoadExtraUserIDKey = @"UserId";
NSString *const kLoadExtraMediaExtraKey = @"UserExtraData";
@interface OFMRewardedVideoWrapper()<OFMRewardedVideoDelegate>
@end
@implementation OFMRewardedVideoWrapper
+(instancetype)sharedInstance {
    static OFMRewardedVideoWrapper *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[OFMRewardedVideoWrapper alloc] init];
    });
    return sharedInstance;
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
    
    if ([selector isEqualToString:@"loadRewardedVideoWithPlacementID:extraJSONString:customDataJSONString:callback:"]) {
        [self loadRewardedVideoWithPlacementID:firstObject extraJSONString:secondObject customDataJSONString:lastObject callback:callback];
    } else if ([selector isEqualToString:@"rewardedVideoReadyForPlacementID:"]) {
        return [NSNumber numberWithBool:[self rewardedVideoReadyForPlacementID:firstObject]];
    } else if ([selector isEqualToString:@"showRewardedVideoWithPlacementID:extraJsonString:"]) {
        [self showRewardedVideoWithPlacementID:firstObject extraJsonString:secondObject];
    }
    else if ([selector isEqualToString:@"checkAdStatus:"]) {
        return [self checkAdStatus:firstObject];
    }
    else if ([selector isEqualToString:@"clearCache"]) {
        [self clearCache];
    } else if ([selector isEqualToString:@"setExtra:"]) {
        [self setExtra:firstObject];
    }
    return nil;
}

-(void) loadRewardedVideoWithPlacementID:(NSString*)placementID extraJSONString:(NSString*)extraJSONString customDataJSONString:(NSString*)customDataJSONString callback:(void(*)(const char*, const char*))callback {
    [self setCallBack:callback forKey:placementID];
    NSMutableDictionary *extra = [NSMutableDictionary dictionary];
    if ([extraJSONString isKindOfClass:[NSString class]] && ![OFMUnityUtilities isEmpty:extraJSONString]) {
        NSDictionary *extraDict = [NSJSONSerialization JSONObjectWithData:[extraJSONString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        
        if (extraDict[kLoadExtraUserIDKey] != nil) { extra[kATAdLoadingExtraUserIDKey] = extraDict[kLoadExtraUserIDKey]; }
        if (extraDict[kLoadExtraMediaExtraKey] != nil) { extra[kATAdLoadingExtraMediaExtraKey] = extraDict[kLoadExtraMediaExtraKey]; }
    }
    
    NSDictionary *customData = nil;
    if ([customDataJSONString isKindOfClass:[NSString class]] && ![OFMUnityUtilities isEmpty:customDataJSONString]) {
        NSDictionary *customDict = [NSJSONSerialization JSONObjectWithData:[customDataJSONString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        customData = customDict;
    }
    NSLog(@"extraDict = %@，customDict = %@", extra,customData);
    
    [[OFMRewardedVideoAdManager sharedManager] loadOFMADWithPlacementID:placementID extra:extra customData:customData delegate:self];
}

-(BOOL) rewardedVideoReadyForPlacementID:(NSString*)placementID {
    return [[OFMRewardedVideoAdManager sharedManager] adReadyForPlacementID:placementID];
}

-(NSString*) checkAdStatus:(NSString *)placementID {
    OFMCheckLoadModel *checkLoadModel = [[OFMRewardedVideoAdManager sharedManager] checkAdLoadStatusForPlacementID:placementID];
    NSMutableDictionary *statusDict = [NSMutableDictionary dictionary];
    statusDict[@"isReady"] = @(checkLoadModel.isReady);
    statusDict[@"adInfo"] = checkLoadModel.adOfferInfo;
    NSLog(@"OFMRewardedVideoWrapper::statusDict = %@", statusDict);
    return statusDict.jsonString;
}

-(void) showRewardedVideoWithPlacementID:(NSString*)placementID extraJsonString:(NSString*)extraJsonString {
    NSDictionary *extraDict = ([extraJsonString isKindOfClass:[NSString class]] && [extraJsonString dataUsingEncoding:NSUTF8StringEncoding] != nil) ? [NSJSONSerialization JSONObjectWithData:[extraJsonString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil] : nil;
    NSLog(@"OFMRewardedVideoWrapper::showRewardedVideoWithPlacementID = %@ extraJsonString = %@", placementID,extraJsonString);
    NSLog(@"OFMRewardedVideoWrapper::extraDict = %@", extraDict);
    [[OFMRewardedVideoAdManager sharedManager] showRewardedVideoWithPlacementID:placementID scene:extraDict[kOFMUnityUtilitiesAdShowingExtraScenarioKey] inViewController:[UIApplication sharedApplication].delegate.window.rootViewController delegate:self];
}

-(void) clearCache {
    // to do
}

-(void) setExtra:(NSString*)extra {
    if ([extra isKindOfClass:[NSString class]]) {
        NSDictionary *extraDict = [NSJSONSerialization JSONObjectWithData:[extra dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        if ([extraDict isKindOfClass:[NSDictionary class]]) [[ATAdManager sharedManager] setExtra:extraDict];
    }
}

-(NSString*) scriptWrapperClass {
    return @"OFMRewardedVideoWrapper";
}

#pragma mark - delegate
-(void) didFinishLoadingOFMADWithPlacementID:(NSString *)placementID extra:(NSDictionary *)extra{
    NSLog(@"OFMRewardedVideoWrapper::didFinishLoadingOFMADWithPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnRewardedVideoLoaded" placementID:placementID error:nil extra:extra];
}

-(void) didFailToLoadOFMADWithPlacementID:(NSString*)placementID error:(NSError*)error {
    NSLog(@"OFMRewardedVideoWrapper::didFailToLoadOFMADWithPlacementID:%@ error:%@", placementID, error);
    error = error != nil ? error : [NSError errorWithDomain:@"com.anythink.Unity3DPackage" code:100001 userInfo:@{NSLocalizedDescriptionKey:@"AT has failed to load ad", NSLocalizedFailureReasonErrorKey:@"AT has failed to load ad"}];
    [self invokeCallback:@"OnRewardedVideoLoadFailure" placementID:placementID error:error extra:nil];
}

-(void) rewardedVideoDidStartPlayingForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMRewardedVideoWrapper::rewardedVideoDidStartPlayingForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnRewardedVideoPlayStart" placementID:placementID error:nil extra:extra];
    [[NSNotificationCenter defaultCenter] postNotificationName:kOFMUnityUtilitiesRewardedVideoImpressionNotification object:nil];
}

-(void) rewardedVideoDidEndPlayingForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMRewardedVideoWrapper::rewardedVideoDidEndPlayingForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnRewardedVideoPlayEnd" placementID:placementID error:nil extra:extra];
}

-(void) rewardedVideoDidFailToPlayForPlacementID:(NSString*)placementID error:(NSError*)error extra:(NSDictionary *)extra {
    NSLog(@"OFMRewardedVideoWrapper::rewardedVideoDidFailToPlayForPlacementID:%@ error:%@ extra:%@", placementID, error, extra);
    error = error != nil ? error : [NSError errorWithDomain:@"com.anythink.Unity3DPackage" code:100001 userInfo:@{NSLocalizedDescriptionKey:@"AT has failed to play video", NSLocalizedFailureReasonErrorKey:@"AT has failed to play video"}];
    [self invokeCallback:@"OnRewardedVideoPlayFailure" placementID:placementID error:error extra:extra];
}

-(void) rewardedVideoDidCloseForPlacementID:(NSString*)placementID rewarded:(BOOL)rewarded extra:(NSDictionary *)extra {
    NSLog(@"OFMRewardedVideoWrapper::rewardedVideoDidCloseForPlacementID:%@ rewarded:%d extra:%@", placementID, rewarded, extra);
    [self invokeCallback:@"OnRewardedVideoClose" placementID:placementID error:nil extra:@{@"rewarded":@(rewarded), @"extra":extra != nil ? extra : @{}}];
    [[NSNotificationCenter defaultCenter] postNotificationName:kOFMUnityUtilitiesRewardedVideoCloseNotification object:nil];
}

-(void) rewardedVideoDidClickForPlacementID:(NSString*)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMRewardedVideoWrapper::rewardedVideoDidClickForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnRewardedVideoClick" placementID:placementID error:nil extra:extra];
}

-(void) rewardedVideoDidRewardSuccessForPlacemenID:(NSString*)placementID extra:(NSDictionary*)extra {
    NSLog(@"OFMRewardedVideoWrapper::rewardedVideoDidRewardSuccessForPlacemenID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnRewardedVideoReward" placementID:placementID error:nil extra:extra];
}
@end
