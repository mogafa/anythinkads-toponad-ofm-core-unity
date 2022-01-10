//
//  OFMUnityManager.m
//  UnityContainer
//
//  Created by Martin Lau on 08/08/2018.
//  Copyright © 2018 Martin Lau. All rights reserved.
//

#import "OFMUnityManager.h"
#import "OFMUnityUtilities.h"
#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <CoreTelephony/CTCarrier.h>
#import "OFMBannerAdWrapper.h"
#import "OFMInterstitialAdWrapper.h"
#import "OFMRewardedVideoWrapper.h"
#import <OFMSDK/OFMSDK.h>

/*
 *class:
 *selector:
 *arguments:
 */
bool message_from_unity(const char *msg, void(*callback)(const char*, const char *)) {
    NSString *msgStr = [NSString stringWithUTF8String:msg];
    NSDictionary *msgDict = [NSJSONSerialization JSONObjectWithData:[msgStr dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
    Class class = NSClassFromString(msgDict[@"class"]);

    bool ret = false;
    ret = [[[class sharedInstance] selWrapperClassWithDict:msgDict callback:callback != NULL ? callback : nil] boolValue];
    
    return ret;
}

int get_message_for_unity(const char *msg, void(*callback)(const char*, const char *)) {
    NSString *msgStr = [NSString stringWithUTF8String:msg];
    NSDictionary *msgDict = [NSJSONSerialization JSONObjectWithData:[msgStr dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
    
    Class class = NSClassFromString(msgDict[@"class"]);
    
    int ret = 0;
    ret = [[[class sharedInstance] selWrapperClassWithDict:msgDict callback:callback != NULL ? callback : nil] intValue];
    
    return ret;
}

char * get_string_message_for_unity(const char *msg, void(*callback)(const char*, const char *)) {
    NSString *msgStr = [NSString stringWithUTF8String:msg];
    NSDictionary *msgDict = [NSJSONSerialization JSONObjectWithData:[msgStr dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];

    Class class = NSClassFromString(msgDict[@"class"]);
    
    NSString *ret = @"";
    ret = [[class sharedInstance] selWrapperClassWithDict:msgDict callback:callback != NULL ? callback : nil];
    
    if ([ret UTF8String] == NULL)
        return NULL;

    char* res = (char*)malloc(strlen([ret UTF8String]) + 1);
    strcpy(res, [ret UTF8String]);

    return res;
}

@interface OFMUnityManager()
@property(nonatomic, readonly) NSMutableDictionary *consentInfo;
@end
@implementation OFMUnityManager
+(instancetype)sharedInstance {
    static OFMUnityManager *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[OFMUnityManager alloc] init];
    });
    return sharedInstance;
}

-(instancetype) init {
    self = [super init];
    if (self != nil) {
        _consentInfo = [NSMutableDictionary dictionary];
    }
    return self;
}

- (id)selWrapperClassWithDict:(NSDictionary *)dict callback:(void(*)(const char*))callback {
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
    
    if ([selector isEqualToString:@"startSDKWithAppID:appKey:defaultConfig:"]) {
        return [NSNumber numberWithBool:[self startSDKWithAppID:firstObject appKey:secondObject defaultConfig:lastObject]];
    }else if ([selector isEqualToString:@"setCustomData:"]) {
        [self setCustomData:firstObject];
    }else if ([selector isEqualToString:@"setDebugLog:"]) {
        [self setDebugLog:firstObject];
    }else if ([selector isEqualToString:@"setPersonalizedAdState:"]) {
        [self setPersonalizedAdState:[NSNumber numberWithInt:firstObject.intValue]];
    }else if ([selector isEqualToString:@"setHasUserConsent:"]) {
        [self setHasUserConsent:firstObject];
    }else if ([selector isEqualToString:@"setIsAgeRestrictedUser:"]) {
        [self setIsAgeRestrictedUser:firstObject];
    }else if ([selector isEqualToString:@"setDoNotSell:"]) {
        [self setDoNotSell:firstObject];
    }else if ([selector isEqualToString:@"setTimeoutForWaitingSetting:"]) {
        [self setTimeoutForWaitingSetting:firstObject];
    }else if ([selector isEqualToString:@"getMediationId"]) {
        return [NSNumber numberWithInteger:[self getMediationId]];
    }else if ([selector isEqualToString:@"getMediationConfig"]) {
        return [self getMediationConfig];
    }else if ([selector isEqualToString:@"setMediationSwitchListener:"]) {
        [self setMediationSwitchListener:callback];
    }
    return nil;
}

-(BOOL) startSDKWithAppID:(NSString*)appID appKey:(NSString*)appKey defaultConfig:(NSString *)defaultConfig{
    [[OFMAPI sharedInstance] setLogDebug:YES];
    NSLog(@"OFMUnityManager::startSDKWithAppID:(%@) appKey:(%@) defaultConfig:(%@)",appID,appKey,defaultConfig);
    [[OFMAPI sharedInstance] initWithAppId:appID appKey:appKey defaultConfig:defaultConfig completion:^(OFMMediationConfig * mediationConfig, NSError * error) {
    }];
    return YES;
}

-(void) setCustomData:(NSString*)customDataStr {
    if ([customDataStr isKindOfClass:[NSString class]] && [customDataStr length] > 0) {
        NSDictionary *customData = [NSJSONSerialization JSONObjectWithData:[customDataStr dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        [[OFMAPI sharedInstance] setCustomRule:customData];
    }
}

-(void) setDebugLog:(NSString*)flagStr {
    [[OFMAPI sharedInstance] setLogDebug:[flagStr boolValue]];
}

-(void) setPersonalizedAdState:(NSNumber*)personalizedAdState {
    [[OFMAPI sharedInstance] setPersonalizedAdState:[personalizedAdState intValue]];
}

-(void) setHasUserConsent:(NSString*)hasUserConsent {
    [[OFMAPI sharedInstance] setUserDataConsent:[hasUserConsent boolValue]];
}

-(void) setIsAgeRestrictedUser:(NSString*)isAgeRestrictedUser {
    [[OFMAPI sharedInstance] setIsAgeRestrictedUser:[isAgeRestrictedUser boolValue]];
}

-(void) setDoNotSell:(NSString*)doNotSell {
    [[OFMAPI sharedInstance] setDoNotSell:[doNotSell boolValue]];
}

-(void) setTimeoutForWaitingSetting:(NSString*)millisecond {
    [OFMAPI sharedInstance].timeoutForUpdateSetting = millisecond.integerValue;
}

-(NSInteger) getMediationId {
    return [[OFMAPI sharedInstance] getCurrentMediationID];
}

-(NSString *) getMediationConfig {
    return ([[OFMAPI sharedInstance] getMediationCofig]).jsonString;
}

-(void) setMediationSwitchListener:(void(*)(const char*))callback {
    [[OFMAPI sharedInstance] switchMediationConfigCompletion:^(OFMMediationConfig *mediationConfig, OFMMediationConfig *previousConfig) {
        if (callback != NULL) {
            NSMutableDictionary *oldMediationConfig = [NSMutableDictionary dictionary];
            oldMediationConfig[@"mediation_id"] = @(previousConfig.mediationSystem);
            oldMediationConfig[@"tid"] = @(previousConfig.mediationTrafficId);
            oldMediationConfig[@"sid"] = @(previousConfig.mediationSid);
            oldMediationConfig[@"app_info"] = previousConfig.mediationAppInfo != nil ? previousConfig.mediationAppInfo : @{};
            oldMediationConfig[@"pl_info"] = previousConfig.mediationPlacementInfo != nil ? previousConfig.mediationPlacementInfo : @{};
            
            NSMutableDictionary *newMediationConfig = [NSMutableDictionary dictionary];
            newMediationConfig[@"mediation_id"] = @(mediationConfig.mediationSystem);
            newMediationConfig[@"tid"] = @(mediationConfig.mediationTrafficId);
            newMediationConfig[@"sid"] = @(mediationConfig.mediationSid);
            newMediationConfig[@"app_info"] = mediationConfig.mediationAppInfo != nil ? mediationConfig.mediationAppInfo : @{};
            newMediationConfig[@"pl_info"] = mediationConfig.mediationPlacementInfo != nil ? mediationConfig.mediationPlacementInfo : @{};
            
            NSMutableDictionary *mediationInfo = [NSMutableDictionary dictionary];
            mediationInfo[@"oldConfig"] = oldMediationConfig.jsonString;
            mediationInfo[@"newConfig"] = newMediationConfig.jsonString;
            
            callback(mediationInfo.jsonString.UTF8String);
        }
    }];
}


@end

