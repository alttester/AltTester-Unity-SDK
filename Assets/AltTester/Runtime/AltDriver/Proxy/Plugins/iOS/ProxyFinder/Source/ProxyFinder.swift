import Foundation
import JavaScriptCore

@objc public class ProxyFinder: NSObject {
    @objc public static let shared = ProxyFinder()

    @objc public func swiftGetProxy(_ destinationUrl: String, destinationHost: String) -> String {
        guard let systemProxySettings = CFNetworkCopySystemProxySettings()?.takeUnretainedValue() as? [String: Any] else {
            return ""
        }

        let proxyAutoConfigEnable = systemProxySettings["ProxyAutoConfigEnable"] as? Int ?? 0

        if proxyAutoConfigEnable == 1 {
            guard let pacUrl = systemProxySettings["ProxyAutoConfigURLString"] as? String,
                  let url = URL(string: pacUrl) else {
                return ""
            }

            var proxyUrl = ""

            let semaphore = DispatchSemaphore(value: 0)

            let task = URLSession.shared.dataTask(with: url) { data, response, error in
                defer {
                    semaphore.signal()
                }

                if let data = data {
                    if let pacContent = String(data: data, encoding: .utf8) {
                        let proxies = CFNetworkCopyProxiesForAutoConfigurationScript(pacContent as CFString, CFURLCreateWithString(kCFAllocatorDefault, destinationUrl as CFString, nil), nil)!.takeUnretainedValue() as? [[AnyHashable: Any]] ?? [];

                        if (proxies.count > 0) {
                            let proxy = proxies[0]

                            if(proxy[kCFProxyTypeKey] as! CFString == kCFProxyTypeHTTP || proxy[kCFProxyTypeKey] as! CFString == kCFProxyTypeHTTPS) {
                                let host = proxy[kCFProxyHostNameKey] as? String ?? "null"
                                let port = proxy[kCFProxyPortNumberKey] as? Int ?? 0

                                if (host == "null" || port == 0) {
                                    return
                                }

                                proxyUrl = "http://" + host + ":" + String(port)
                            }
                        }
                    }
                } else if let error = error {
                    // Handle Error
                }
            }

            task.resume()
            semaphore.wait()

            return proxyUrl
        }

        if let httpSEnable = systemProxySettings["HTTPSEnable"] as? Int, httpSEnable == 1,
           destinationUrl.starts(with: "https") {
            if let host = systemProxySettings["HTTPSProxy"] as? String,
               let port = systemProxySettings["HTTPSPort"] as? String {
                return "https://\(host):\(port)"
            }
        }

        if let httpEnable = systemProxySettings["HTTPEnable"] as? Int, httpEnable == 1 {
            if let host = systemProxySettings["HTTPProxy"] as? String,
               let port = systemProxySettings["HTTPPort"] as? String {
                return "http://\(host):\(port)"
            }
        }

        return ""
    }
}
