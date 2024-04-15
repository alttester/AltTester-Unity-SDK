#import <UnityFramework/UnityFramework-Swift.h>

extern "C"
{
    char* cStringCopy(const char* string) {
        if (string == NULL){
          return NULL;
        }

        char* res = (char*) malloc(strlen(string) + 1);
        strcpy(res, string);

        return res;
    }

    char* _getProxy(const char* uri, const char* host)
    {
        NSString *returnString = [[AltProxyFinder shared] swiftGetProxy:[NSString stringWithUTF8String:uri] destinationHost:[NSString stringWithUTF8String:host]];
        return cStringCopy([returnString UTF8String]);
    }
}
