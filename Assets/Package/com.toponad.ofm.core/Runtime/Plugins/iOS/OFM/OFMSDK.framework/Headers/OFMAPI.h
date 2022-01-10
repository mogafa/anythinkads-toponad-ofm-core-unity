//
//  OFMAPI.h
//  OfmSDK
//
//  Created by stephen on 12/1/2021.
//  Copyright © 2021 AnyThink. All rights reserved.
//


#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "OFMMediationConfigMode.h"

typedef NS_ENUM(NSInteger, OFMAdFormat) {
    OFMAdFormatNative        = 0,
    OFMAdFormatRewardedVideo = 1,
    OFMAdFormatBanner        = 2,
    OFMAdFormatInterstitial  = 3,
    OFMAdFormatSplash        = 4
};


extern NSString *const kOFMDeviceDataInfoUserAgentKey;

extern NSString *const kOFMADUserAreaKey;

//extern NSInteger const kOFMMediationSystemTopOn;
//extern NSInteger const kOFMMediationSystemMobrain;

extern NSString *const kOFMMediationNameTopOn;
extern NSString *const kOFMMediationNameMobrain;
extern NSString *const kOFMMediationNameMax;
extern NSString *const kOFMMediationNameIronSource;

extern NSString *const kOFMADLoadingErrorDomain;
extern NSString *const kOFMADSDKFailedToLoadADMsg;
extern NSString *const kOFMSDKImportIssueErrorReason;
extern NSInteger const kOFMADLoadingErrorCodeADOfferLoadingFailed;
extern NSInteger const kOFMADLoadingErrorCodeAdapterClassNotFound;
extern NSInteger const kOFMADLoadingErrorCodeSDKNotInitalizedProperly;
extern NSInteger const kOFMADLoadingErrorCodeThirdPartySDKNotImportedProperly;



extern NSString *const kOFMDeviceDataInfoOSVersionNameKey;
extern NSString *const kOFMDeviceDataInfoOSVersionCodeKey;
extern NSString *const kOFMDeviceDataInfoPackageNameKey;
extern NSString *const kOFMDeviceDataInfoAppVersionNameKey;
extern NSString *const kOFMDeviceDataInfoAppVersionCodeKey;
extern NSString *const kOFMDeviceDataInfoBrandKey;
extern NSString *const kOFMDeviceDataInfoModelKey;
extern NSString *const kOFMDeviceDataInfoScreenKey;
extern NSString *const kOFMDeviceDataInfoNetworkTypeKey;
extern NSString *const kOFMDeviceDataInfoMNCKey;
extern NSString *const kOFMDeviceDataInfoMCCKey;
extern NSString *const kOFMDeviceDataInfoLanguageKey;
extern NSString *const kOFMDeviceDataInfoTimeZoneKey;
extern NSString *const kOFMDeviceDataInfoUserAgentKey;
extern NSString *const kOFMDeviceDataInfoOrientKey;
extern NSString *const kOFMDeviceDataInfoIDFAKey;
extern NSString *const kOFMDeviceDataInfoIDFVKey;


extern NSString *const kOFMCustomDataUserIDKey;//string
extern NSString *const kOFMCustomDataAgeKey;//Integer
extern NSString *const kOFMCustomDataGenderKey;//Integer
extern NSString *const kOFMCustomDataNumberOfIAPKey;//Integer
extern NSString *const kOFMCustomDataIAPAmountKey;//Double
extern NSString *const kOFMCustomDataIAPCurrencyKey;//string
extern NSString *const kOFMCustomDataSegmentIDKey;//int
extern NSString *const kOFMCustomDataChannelKey;//string
extern NSString *const kOFMCustomDataSubchannelKey;//string


extern NSString *const OFMSDKInitErrorDomain;
extern NSInteger const OFMSDKInitErrorCodeDataConsentNotSet;
extern NSInteger const OFMSDKInitErrorCodeDataConsentForbidden;

extern NSString *const OFMExtraViewControllerKey;
extern NSString *const OFMExtraAdSizeKey;

typedef NS_ENUM(NSUInteger, OFMAreaCode) {
    OFMAreaCodeGlobal = 1,
    OFMAreaCodeChinese_mainland,
};

typedef NS_ENUM(NSInteger, OFMDataConsentSet) {
    //Let it default to forbidden if not set
    OFMDataConsentSetUnknown = 0,
    OFMDataConsentSetPersonalized = 1,
    OFMDataConsentSetNonpersonalized = 2
};

typedef NS_ENUM(NSInteger, OFMPersonalizedAdState) {
    OFMPersonalizedAdStateType = 1,
    OFMNonpersonalizedAdStateType = 2
};


typedef NS_ENUM(NSInteger, OFMMediationSystem) {
    //Let it default to forbidden if not set
    OFMMediationSystemTopOn = 1,
    OFMMediationSystemMobrain = 3,
    OFMMediationSystemMax = 4,
    OFMMediationSystemIronSource = 5,
    OFMMediationSystemAdmob = 6,
};


typedef void(^SwitchMediationConfigBlock)(OFMMediationConfig * mediationConfig, OFMMediationConfig * previousConfig);

@interface OFMAPI : NSObject

+(instancetype)sharedInstance;


@property(nonatomic, strong) NSString *appID;
@property(nonatomic, strong) NSString *appKey;
@property(nonatomic, strong) NSString *version;
@property(nonatomic, strong) NSDate *initiationTime;

@property(nonatomic, assign) OFMDataConsentSet dataConsentSet;

@property(nonatomic, strong) OFMMediationConfigMode *currentMediationConfig;

@property(nonatomic, strong) NSDictionary *customRule;

@property(nonatomic) NSInteger timeoutForUpdateSetting; // millisecond


- (void)initWithAppId:(NSString*)appID appKey:(NSString*)appKey defaultConfig:(NSString *)defaultConfig completion:(void (^)(OFMMediationConfig * mediationConfig, NSError * error)) completion;


- (NSDictionary *)getMediationCofig;
- (NSInteger)getCurrentMediationID;
- (void)switchMediationConfigCompletion:(SwitchMediationConfigBlock)completion;

- (void)setLogDebug:(BOOL)debug;
- (void)setCustomRule:(NSDictionary*) customData;

- (NSArray*)deniedUploadInfoArray;
- (void)setDeniedUploadInfoArray:(NSArray *)uploadInfoArray;
- (BOOL)isContainsForDeniedUploadInfoArray:(NSString *)key;

- (void)setPersonalizedAdState:(OFMPersonalizedAdState)state;
- (void)setUserDataConsent:(OFMDataConsentSet)dataConsentSet;
- (void)setIsAgeRestrictedUser:(BOOL)set;
- (void)setDoNotSell:(BOOL)set;

- (OFMPersonalizedAdState)getPersonalizedAdState;



@end
