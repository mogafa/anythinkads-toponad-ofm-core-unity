//
//  OFMCheckLoadModel.h
//  OFMSDK
//
//  Created by GUO PENG on 2021/11/19.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface OFMCheckLoadModel : NSObject

@property(nonatomic) BOOL isReady;
@property(nonatomic, strong) NSDictionary *adOfferInfo;


- (instancetype)initWithDictionary:(NSDictionary *)infoDic;

@end

NS_ASSUME_NONNULL_END
