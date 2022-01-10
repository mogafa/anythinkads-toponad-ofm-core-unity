//
//  OFMBannerAdWrapper.m
//  UnityContainer
//
//  Created by Martin Lau on 2019/1/8.
//  Copyright © 2019 Martin Lau. All rights reserved.
//

#import "OFMBannerAdWrapper.h"
#import <OFMSDK/OFMSDK.h>
#import <AnyThinkBanner/AnyThinkBanner.h>
#import "OFMUnityUtilities.h"

@interface OFMBannerAdWrapper()<OFMBannerDelegate>
@property(nonatomic, readonly) NSMutableDictionary<NSString*, UIView*> *bannerViewStorage;
@property(nonatomic, readonly) BOOL interstitialOrRVBeingShown;
@end

static NSString *kOfmBannerSizeUsesPixelFlagKey = @"uses_pixel";
static NSString *kOfmBannerAdLoadingExtraInlineAdaptiveWidthKey = @"inline_adaptive_width";
static NSString *kOfmBannerAdLoadingExtraInlineAdaptiveOrientationKey = @"inline_adaptive_orientation";

@implementation OFMBannerAdWrapper
+(instancetype)sharedInstance {
    static OFMBannerAdWrapper *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[OFMBannerAdWrapper alloc] init];
    });
    return sharedInstance;
}

-(instancetype) init {
    self = [super init];
    if (self != nil) {
        _bannerViewStorage = [NSMutableDictionary<NSString*, UIView*> dictionary];
    }
    return self;
}

-(NSString*) scriptWrapperClass {
    return @"OFMBannerAdWrapper";
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
    
    if ([selector isEqualToString:@"loadBannerAdWithPlacementID:extraJSONString:customDataJSONString:callback:"]) {
        [self loadBannerAdWithPlacementID:firstObject extraJSONString:secondObject customDataJSONString:lastObject callback:callback];
    } else if ([selector isEqualToString:@"showBannerAdWithPlacementID:rect:"]) {
        [self showBannerAdWithPlacementID:firstObject rect:secondObject];
    } else if ([selector isEqualToString:@"removeBannerAdWithPlacementID:"]) {
        [self removeBannerAdWithPlacementID:firstObject];
    } else if ([selector isEqualToString:@"showBannerAdWithPlacementID:"]) {
        [self showBannerAdWithPlacementID:firstObject];
    } else if ([selector isEqualToString:@"hideBannerAdWithPlacementID:"]) {
        [self hideBannerAdWithPlacementID:firstObject];
    } else if ([selector isEqualToString:@"clearCache"]) {
        [self clearCache];
    } else if ([selector isEqualToString:@"checkAdStatus:"]) {
        return [self checkAdStatus:firstObject];
    }
    return nil;
}

-(void) loadBannerAdWithPlacementID:(NSString*)placementID extraJSONString:(NSString*)extraJSONString customDataJSONString:(NSString*)customDataJSONString callback:(void(*)(const char*, const char*))callback {
    [self setCallBack:callback forKey:placementID];
    NSMutableDictionary *extra = [NSMutableDictionary dictionary];
    if ([extraJSONString isKindOfClass:[NSString class]] && ![OFMUnityUtilities isEmpty:extraJSONString]) {
        NSDictionary *extraDict = [NSJSONSerialization JSONObjectWithData:[extraJSONString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        NSLog(@"extraDict = %@", extraDict);
        CGFloat scale = [extraDict[kOfmBannerSizeUsesPixelFlagKey] boolValue] ? [UIScreen mainScreen].nativeScale : 1.0f;
        if ([extraDict[kATAdLoadingExtraBannerAdSizeKey] isKindOfClass:[NSString class]] && [[extraDict[kATAdLoadingExtraBannerAdSizeKey] componentsSeparatedByString:@"x"] count] == 2) {
            NSArray<NSString*>* com = [extraDict[kATAdLoadingExtraBannerAdSizeKey] componentsSeparatedByString:@"x"];

            extra[kATAdLoadingExtraBannerAdSizeKey] = [NSValue valueWithCGSize:CGSizeMake([com[0] doubleValue] / scale, [com[1] doubleValue] / scale)];
            extra[OFMExtraAdSizeKey] = [NSString stringWithFormat:@"%lfx%lf",[com[0] doubleValue] / scale,[com[1] doubleValue] / scale];
        }
    }
    if (extra[kATAdLoadingExtraBannerAdSizeKey] == nil) {
        extra[kATAdLoadingExtraBannerAdSizeKey] = [NSValue valueWithCGSize:CGSizeMake(320.0f, 50.0f)];
        extra[OFMExtraAdSizeKey] = [NSString stringWithFormat:@"320x50"];
    }
    
    
    NSDictionary *customData = nil;
    if ([customDataJSONString isKindOfClass:[NSString class]] && ![OFMUnityUtilities isEmpty:customDataJSONString]) {
        NSDictionary *customDict = [NSJSONSerialization JSONObjectWithData:[customDataJSONString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
        customData = customDict;
    }
    NSLog(@"extraDict = %@，customDict = %@", extra,customData);
    
    [[OFMBannerAdManager sharedManager] loadOFMADWithPlacementID:placementID extra:extra customData:customData delegate:self];
}

UIEdgeInsets SafeAreaInsets_ATUnityBanner() {
    return ([[UIApplication sharedApplication].keyWindow respondsToSelector:@selector(safeAreaInsets)] ? [UIApplication sharedApplication].keyWindow.safeAreaInsets : UIEdgeInsetsZero);
}

-(void) showBannerAdWithPlacementID:(NSString*)placementID rect:(NSString*)rect {
    dispatch_async(dispatch_get_main_queue(), ^{
        if ([rect isKindOfClass:[NSString class]] && [rect dataUsingEncoding:NSUTF8StringEncoding] != nil) {
            NSDictionary *rectDict = [NSJSONSerialization JSONObjectWithData:[rect dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingAllowFragments error:nil];
            NSLog(@"rectDict = %@", rectDict);
            CGFloat scale = [rectDict[kOfmBannerSizeUsesPixelFlagKey] boolValue] ? [UIScreen mainScreen].nativeScale : 1.0f;
            UIView *bannerView = [[OFMBannerAdManager sharedManager] retrieveBannerViewForPlacementID:placementID viewController:[UIApplication sharedApplication].keyWindow.rootViewController delegate:self];
            UIButton *bannerCointainer = [UIButton buttonWithType:UIButtonTypeCustom];
            [bannerCointainer addTarget:self action:@selector(noop) forControlEvents:UIControlEventTouchUpInside];
            
            NSString *position = rectDict[@"position"];
            CGSize totalSize = [UIApplication sharedApplication].keyWindow.rootViewController.view.bounds.size;
            UIEdgeInsets safeAreaInsets = SafeAreaInsets_ATUnityBanner();
            if ([@"top" isEqualToString:position]) {
                bannerCointainer.frame = CGRectMake((totalSize.width - CGRectGetWidth(bannerView.bounds)) / 2.0f, safeAreaInsets.top , CGRectGetWidth(bannerView.bounds), CGRectGetHeight(bannerView.bounds));
            } else if ([@"bottom" isEqualToString:position]) {
                bannerCointainer.frame = CGRectMake((totalSize.width - CGRectGetWidth(bannerView.bounds)) / 2.0f, totalSize.height - safeAreaInsets.bottom - CGRectGetHeight(bannerView.bounds) , CGRectGetWidth(bannerView.bounds), CGRectGetHeight(bannerView.bounds));
            } else {
                bannerCointainer.frame = CGRectMake([rectDict[@"x"] doubleValue] / scale, [rectDict[@"y"] doubleValue] / scale, [rectDict[@"width"] doubleValue] / scale, [rectDict[@"height"] doubleValue] / scale);
            }
            
            bannerView.frame = bannerCointainer.bounds;
            [bannerCointainer addSubview:bannerView];
            
//            bannerCointainer.layer.borderColor = [UIColor redColor].CGColor;
//            bannerCointainer.layer.borderWidth = .5f;
            [[UIApplication sharedApplication].keyWindow.rootViewController.view addSubview:bannerCointainer];
            self->_bannerViewStorage[placementID] = bannerCointainer;
        }
    });
}

-(void) noop {
    
}

-(void) removeBannerAdWithPlacementID:(NSString*)placementID {
    dispatch_async(dispatch_get_main_queue(), ^{
        [self->_bannerViewStorage[placementID] removeFromSuperview];
        [self->_bannerViewStorage removeObjectForKey:placementID];
    });
}

-(void) showBannerAdWithPlacementID:(NSString*)placementID {
    dispatch_async(dispatch_get_main_queue(), ^{
        UIView *bannerView = self->_bannerViewStorage[placementID];
        if (bannerView.superview != nil && !_interstitialOrRVBeingShown) { bannerView.hidden = NO; }
    });
}

-(void) hideBannerAdWithPlacementID:(NSString*)placementID {
    dispatch_async(dispatch_get_main_queue(), ^{
        UIView *bannerView = self->_bannerViewStorage[placementID];
        if (bannerView.superview != nil) { bannerView.hidden = YES; }
    });
}

-(void) clearCache {
    // to do
}

-(NSString*) checkAdStatus:(NSString *)placementID {
    OFMCheckLoadModel *checkLoadModel = [[OFMBannerAdManager sharedManager] checkAdLoadStatusForPlacementID:placementID];
    NSMutableDictionary *statusDict = [NSMutableDictionary dictionary];
    statusDict[@"isReady"] = @(checkLoadModel.isReady);
    statusDict[@"adInfo"] = checkLoadModel.adOfferInfo;
    NSLog(@"OFMBannerAdWrapper::statusDict = %@", statusDict);
    return statusDict.jsonString;
}

#pragma mark - banner delegate method(s)
- (void)didFinishLoadingOFMADWithPlacementID:(NSString *)placementID extra:(NSDictionary *)extra {
    NSLog(@"OFMBannerAdWrapper::didFinishLoadingOFMADWithPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnBannerAdLoad" placementID:placementID error:nil extra:extra];
}

-(void) didFailToLoadOFMADWithPlacementID:(NSString*)placementID error:(NSError*)error {
    error = error != nil ? error : [NSError errorWithDomain:@"com.anythink.Unity3DPackage" code:100001 userInfo:@{NSLocalizedDescriptionKey:@"AT has failed to load ad", NSLocalizedFailureReasonErrorKey:@"AT has failed to load ad"}];
    NSLog(@"OFMBannerAdWrapper::didFailToLoadOFMADWithPlacementID:%@ error:%@", placementID, error);
    [self invokeCallback:@"OnBannerAdLoadFail" placementID:placementID error:error extra:nil];
}

-(void) bannerDidShowForPlacementID:(NSString*)placementID extra:(NSDictionary*)extra {
    NSLog(@"OFMBannerAdWrapper::bannerDidShowForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnBannerAdImpress" placementID:placementID error:nil extra:extra];
}

-(void) bannerDidCloseForPlacementID:(NSString*)placementID extra:(NSDictionary*)extra {
    NSLog(@"OFMBannerAdWrapper::bannerDidCloseForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnBannerAdClose" placementID:placementID error:nil extra:extra];
}

-(void) bannerDidClickForPlacementID:(NSString*)placementID extra:(NSDictionary*)extra {
    NSLog(@"OFMBannerAdWrapper::bannerDidClickForPlacementID:%@ extra:%@", placementID, extra);
    [self invokeCallback:@"OnBannerAdClick" placementID:placementID error:nil extra:extra];
}

- (void)bannerDeepLinkOrJumpForPlacementID:(NSString *)placementID extra:(NSDictionary *)extra result:(BOOL)success {
    NSLog(@"OFMBannerAdWrapper::bannerDeepLinkOrJumpForPlacementID:%@ extra:%@ result:%@", placementID, extra, success ? @"YES" : @"NO");
}


@end
