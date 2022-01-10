//
//  OFMMediationConfigExtention.h
//  OfmSDK
//
//  Created by stephen on 12/1/2021.
//  Copyright © 2021 AnyThink. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "OFMMediationConfig.h"

typedef NS_ENUM(NSInteger, OFMInitSDKConfigType) {
    OFMInitSDKServerConfigType = 1,
    OFMInitSDKLocalConfigType = 2,
    OFMInitSDKDefaultConfigType = 3,
    OFMInitSDKNewConfigType = 4,
};



@interface OFMMediationConfigMode:OFMMediationConfig

// logger tk_sw
@property(nonatomic, assign) BOOL trackingEnabled;
// logger da_sw
@property(nonatomic, assign) BOOL agentEventEnabled;
// logger tk_ilrd_sw
@property(nonatomic, assign) BOOL trackingILRDEnabled;

// st_vt
@property(nonatomic, assign) NSInteger configValidDuration;


@property(nonatomic, strong) NSDate *updateDate;

@property(nonatomic, strong) NSDictionary *currentCustomData;


@property(nonatomic, assign) OFMInitSDKConfigType initSDKConfigType;

- (instancetype)initWithDictionary:(NSDictionary *) dictionary initSDKConfigType:(OFMInitSDKConfigType)initSDKConfigType;

- (BOOL)expired;

- (NSDictionary*)infoForOFMAppID:(NSString *) appID;
- (NSDictionary*)infoForOFMPlacementID:(NSString *) placementID;

- (NSInteger)getUniqueUnitID:(NSString *)placemenID;



@end
