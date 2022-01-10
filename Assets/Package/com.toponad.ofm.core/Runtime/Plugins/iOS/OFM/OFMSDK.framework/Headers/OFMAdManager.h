//
//  OFMAdManager.h
//  OfmSDK
//
//  Created by stephen on 12/1/2021.
//  Copyright © 2021 AnyThink. All rights reserved.
//


#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "OFMCheckLoadModel.h"

//extern NSString *const kOFMCustomDataUserIDKey;

@protocol OFMAdLoadingDelegate;
@class OFMPlacementModel;
@interface OFMAdManager : NSObject
@property(nonatomic, strong) NSDictionary *extra;

+ (instancetype)sharedManager;

- (void)loadOFMADWithPlacementID:(NSString*)placementID extra:(NSDictionary*)extra customData:(NSDictionary*) customData delegate:(id<OFMAdLoadingDelegate>)delegate;

- (OFMPlacementModel *)placementModelForPlacementID:(NSString *)placementID;

- (BOOL)adReadyForPlacementID:(NSString*)placementID;

- (OFMCheckLoadModel *)checkAdLoadStatusForPlacementID:(NSString*)placementID;

- (id)getShowDelegateForPlacementID:(NSString*)placementID;

- (void)saveShowDelegateForPlacementID:(NSString*)placementID delegate:(id)delegate;


@end
