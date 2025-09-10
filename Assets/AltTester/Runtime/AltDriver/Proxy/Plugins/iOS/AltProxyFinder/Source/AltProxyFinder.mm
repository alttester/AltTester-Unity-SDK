// AltProxyFinder.m
#import "AltProxyFinder.h"
#import <CoreFoundation/CoreFoundation.h>
#import <CFNetwork/CFNetwork.h>

@implementation AltProxyFinder

+ (instancetype)shared {
    static AltProxyFinder *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[AltProxyFinder alloc] init];
    });
    return sharedInstance;
}

- (NSString *)getProxyForDestinationUrl:(NSString *)destinationUrl destinationHost:(NSString *)destinationHost {
    NSDictionary *proxySettings = (__bridge NSDictionary *)(CFNetworkCopySystemProxySettings());
    if (!proxySettings) {
        return @"";
    }
    
    NSNumber *pacEnabled = proxySettings[@"ProxyAutoConfigEnable"];
    if ([pacEnabled intValue] == 1) {
        NSString *pacUrlString = proxySettings[@"ProxyAutoConfigURLString"];
        if (pacUrlString) {
            NSURL *pacUrl = [NSURL URLWithString:pacUrlString];
            if (pacUrl) {
                __block NSString *proxyUrl = @"";
                dispatch_semaphore_t semaphore = dispatch_semaphore_create(0);
                
                [[[NSURLSession sharedSession] dataTaskWithURL:pacUrl completionHandler:^(NSData *data, NSURLResponse *response, NSError *error) {
                    if (data && !error) {
                        NSString *pacContent = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                        pacContent = [pacContent stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
                        if (![pacContent hasPrefix:@"<!DOCTYPE html>"]) {
                            CFArrayRef proxyArray = CFNetworkCopyProxiesForAutoConfigurationScript((__bridge CFStringRef)pacContent, (__bridge CFURLRef)[NSURL URLWithString:destinationUrl], NULL);
                            if (proxyArray && CFArrayGetCount(proxyArray) > 0) {
                                NSDictionary *proxy = (__bridge NSDictionary *)CFArrayGetValueAtIndex(proxyArray, 0);
                                NSString *type = proxy[(NSString *)kCFProxyTypeKey];
                                if ([type isEqualToString:(NSString *)kCFProxyTypeHTTP] || [type isEqualToString:(NSString *)kCFProxyTypeHTTPS]) {
                                    NSString *host = proxy[(NSString *)kCFProxyHostNameKey] ?: @"null";
                                    NSNumber *port = proxy[(NSString *)kCFProxyPortNumberKey] ?: @(0);
                                    if (![host isEqualToString:@"null"] && [port intValue] != 0) {
                                        proxyUrl = [NSString stringWithFormat:@"http://%@:%@", host, port];
                                    }
                                }
                            }
                            if (proxyArray) CFRelease(proxyArray);
                        }
                    }
                    dispatch_semaphore_signal(semaphore);
                }] resume];
                dispatch_semaphore_wait(semaphore, DISPATCH_TIME_FOREVER);
                return proxyUrl;
            }
        }
    }
    
    NSNumber *httpsEnable = proxySettings[@"HTTPSEnable"];
    if ([httpsEnable intValue] == 1 && [destinationUrl hasPrefix:@"https"]) {
        NSString *host = proxySettings[@"HTTPSProxy"];
        NSNumber *port = proxySettings[@"HTTPSPort"];
        if (host && port) {
            return [NSString stringWithFormat:@"https://%@:%@", host, port];
        }
    }
    
    NSNumber *httpEnable = proxySettings[@"HTTPEnable"];
    if ([httpEnable intValue] == 1) {
        NSString *host = proxySettings[@"HTTPProxy"];
        NSNumber *port = proxySettings[@"HTTPPort"];
        if (host && port) {
            return [NSString stringWithFormat:@"http://%@:%@", host, port];
        }
    }
    
    return @"";
}

@end
