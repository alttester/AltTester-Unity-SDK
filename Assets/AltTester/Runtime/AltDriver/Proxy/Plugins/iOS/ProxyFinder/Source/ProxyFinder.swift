import Foundation

@objc public class ProxyFinder: NSObject {
  @objc public static let shared = ProxyFinder()

  @objc public func _test() {

  }

  @objc public func _getProxy(destinationUrl: String, destinationHost: String) -> String {
    let systemProxySettings =
      CFNetworkCopySystemProxySettings()?.takeUnretainedValue() ?? [:] as CFDictionary

    let proxyDict = systemProxySettings as NSDictionary

    if proxyDict.value("ProxyAutoConfigEnable") == 1 {
      let pacUrl = proxyDict.value("ProxyAutoConfigURLString")
      let url = URL(string: pacUrl)!
      let task = URLSession.shared.dataTask(with: url) {
        data,
        response, error in
        if let data = data {
          let jsContent = data
          let jsEngine: JSContext = JSContext()
          jsEngine.evaluateScript(js)
          let fn = "FindProxyForURL(\"" + destinationUrl + "\",\"" + destinationHost + "\")"
          let proxy = jsEngine.evaluateScript(fn)

          return proxy
        } else if let error = error {
          // Handle Error
        }
      }
    }

    if proxyDict.value("HTTPSEnable") == 1 && destinationUrl.starts(with: "https") {
      let host = systemProxySettings.getValue("HTTPSProxy")
      let port = systemProxySettings.getValue("HTTPSPort")

      return "https://" + host + ":" + port
    }

    if proxyDict.value("HTTPEnable") == 1 {
      let host = systemProxySettings.getValue("HTTPProxy")
      let port = systemProxySettings.getValue("HTTPPort")

      return "http://" + host + ":" + port
    }
  }
}
