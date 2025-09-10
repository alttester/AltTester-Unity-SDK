// AltProxyFinder.h
#import <Foundation/Foundation.h>

@interface AltProxyFinder : NSObject

+ (instancetype)shared;
- (NSString *)getProxyForDestinationUrl:(NSString *)destinationUrl destinationHost:(NSString *)destinationHost;

@end

