import Foundation
import JavaScriptCore

@objc public class ProxyFinder: NSObject {
  @objc public static let shared = ProxyFinder()

  @objc public func swiftGetProxy(_ destinationUrl: String, destinationHost: String) -> String {
    let systemProxySettings =
      CFNetworkCopySystemProxySettings()?.takeUnretainedValue() ?? [:] as CFDictionary

    let proxyDict = systemProxySettings as NSDictionary

    if proxyDict["ProxyAutoConfigEnable"] as! Int == 1 {
      let pacUrl = proxyDict["ProxyAutoConfigURLString"] as! String
      let url = URL(string: pacUrl)!
      var proxyUrl = "";

      let task = URLSession.shared.dataTask(with: url) {
        data,
        response, error in
        if let data = data {
          let jsContent = data
          let js = String(data: jsContent, encoding: .utf8)
          let jsEngine: JSContext = JSContext()

          jsEngine.evaluateScript(js)

          let fn = "FindProxyForURL(\"" + destinationUrl + "\",\"" + destinationHost + "\")"
          proxyUrl = jsEngine.evaluateScript(fn).toString()
        } else if let error = error {
          // Handle Error
        }
      }
      task.resume();

      while (task.state != URLSessionTask.State.completed) {
        sleep(1);
      }

      return proxyUrl
    }

    if proxyDict["HTTPSEnable"] as! Int == 1 && destinationUrl.starts(with: "https") {
      let host = proxyDict["HTTPSProxy"] as! String
      let port = proxyDict["HTTPSPort"] as! String

      return "https://" + host + ":" + port
    }

    if proxyDict["HTTPEnable"] as! Int == 1 {
      let host = proxyDict["HTTPProxy"] as! String
      let port = proxyDict["HTTPPort"] as! String

      return "http://" + host + ":" + port
    }

    return "";
  }
}
