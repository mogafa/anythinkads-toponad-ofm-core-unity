//
//  OFMMediation.h
//  OfmSDK
//
//  Created by stephen on 12/1/2021.
//  Copyright © 2021 AnyThink. All rights reserved.
//


#import <UIKit/UIKit.h>

@interface OFMMediationConfig: NSObject
// system
@property(nonatomic, assign) NSInteger mediationSystem;
//tid
@property(nonatomic, assign) NSInteger mediationTrafficId;
// adapter_class
@property(nonatomic, strong) NSDictionary *mediationAdapterClassInfo;
// app_info
@property(nonatomic, strong) NSDictionary *mediationAppInfo;
// pl_info
@property(nonatomic, strong) NSDictionary *mediationPlacementInfo;
// st_addr
@property(nonatomic, strong) NSString *mediationRequestAddress;

// asid
@property(nonatomic, strong) NSString *mediationAsid;

// sid
@property(nonatomic, assign) NSInteger mediationSid;

@end
