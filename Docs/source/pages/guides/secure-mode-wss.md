# Secure mode (WSS)

## Secure Mode (WSS) in AltTester® Unity SDK

AltTester® Unity SDK can communicate with AltTester® Server using a **secure WebSocket
connection** (``wss://``). Secure mode encrypts all data exchanged between the
instrumented application and the server.

To successfully establish a secure connection, the Unity SDK configuration,
AltTester® Server configuration, and client environment must match.

### Enabling Secure Mode

Secure mode is enabled in the instrumented application by activating the
**Secure Mode (WSS)** option, either by enabling the toggle in the AltTester®
Editor or by selecting the secure protocol in the AltTester® PopUp (green pop-up).

When enabled:
- The Unity application connects to the server using ``wss://``
- The AltTester® Server must be running in secure mode
- A valid TLS certificate must be configured on the server

If secure mode is enabled in the SDK but the server is not configured for secure
connections, the connection will fail.

Likewise, if the server runs in secure mode but secure mode is disabled in the SDK,
the connection will fail.

```eval_rst
.. note::
   The Secure Mode (WSS) setting must always match the server configuration.
```

### Secure Mode in WebGL Builds

For **WebGL instrumented builds**, secure mode has additional browser-specific
requirements.

When using secure mode (``wss://``) in WebGL:
- The server URL must use HTTPS  
  (for example: ``https://127.0.0.1:13000``)
- The HTTPS endpoint must be **trusted by the browser**

If the certificate is self-signed or not trusted by default, the browser will block
the connection.

To allow the connection:
1. Open a new browser tab
2. Navigate to ``https://<host>:<port>``  
   (for example: ``https://127.0.0.1:13000``)
3. Proceed through the browser security warning
4. Confirm that you want to continue to the unsafe site

Once the URL is trusted, reload the WebGL application and retry the connection.

```eval_rst
.. important::
   This step is required only once per browser session. Without explicitly trusting
   the HTTPS endpoint, secure WebSocket connections from WebGL builds will now work.
```
