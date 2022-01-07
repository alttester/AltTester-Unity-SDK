using System;
using Altom.AltUnityTester.Logging;
using Altom.AltUnityTester;
using Altom.AltUnityTester.Communication;
using NLog;

namespace Altom.AltUnityTester.UI
{
    public class AltUnityDialog : UnityEngine.MonoBehaviour
    {
        private readonly UnityEngine.Color SUCCESS_COLOR = new UnityEngine.Color32(0, 165, 36, 255);
        private readonly UnityEngine.Color WARNING_COLOR = new UnityEngine.Color32(255, 255, 95, 255);
        private readonly UnityEngine.Color ERROR_COLOR = new UnityEngine.Color32(191, 71, 85, 255);
        private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        [UnityEngine.SerializeField]
        public UnityEngine.GameObject Dialog = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.Text TitleText = null;
        [UnityEngine.SerializeField]
        public UnityEngine.UI.Text MessageText = null;
        [UnityEngine.SerializeField]
        public UnityEngine.UI.Button CloseButton = null;
        [UnityEngine.SerializeField]
        public UnityEngine.UI.Image Icon = null;

        private ICommunication communication;

        public AltUnityInstrumentationSettings InstrumentationSettings { get { return AltUnityRunner._altUnityRunner.InstrumentationSettings; } }

        private readonly AltResponseQueue _updateQueue = new AltResponseQueue();
        private bool wasConnectedBeforeToProxy = false;

        protected void Start()
        {
            Dialog.SetActive(InstrumentationSettings.ShowPopUp);
            CloseButton.onClick.AddListener(ToggleDialog);
            Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ToggleDialog);
            TitleText.text = "AltUnity Tester v." + AltUnityRunner.VERSION;

            if (InstrumentationSettings.InstrumentationMode == AltUnityInstrumentationMode.Server)

                startServerCommProtocol();
            else
            {
                startProxyCommProtocol();
            }
        }
        protected void Update()
        {
            _updateQueue.Cycle();
        }

        protected void OnApplicationQuit()
        {
            cleanUp();
        }


        public void ToggleDialog()
        {
            Dialog.SetActive(!Dialog.activeSelf);
        }

        private void setDialog(string message, UnityEngine.Color color, bool visible)
        {
            Dialog.SetActive(visible);
            MessageText.text = message;
            Dialog.GetComponent<UnityEngine.UI.Image>().color = color;
        }

        private void startServerCommProtocol()
        {
            var cmdHandler = new CommandHandler();
            communication = new WebSocketServerCommunication(cmdHandler, "0.0.0.0", InstrumentationSettings.AltUnityTesterPort);
            communication.OnConnect += onClientConnected;
            communication.OnDisconnect += onClientDisconnected;
            communication.OnError += onError;
            setDialog("Starting AltUnity Tester on port: " + InstrumentationSettings.AltUnityTesterPort, WARNING_COLOR, true);

            try
            {
                communication.Start();
                setDialog("Waiting for connection on port: " + InstrumentationSettings.AltUnityTesterPort, SUCCESS_COLOR, true);
            }
            catch (AddressInUseCommError)
            {
                setDialog("Cannot start AltUnity Tester communication protocol. Another process is listening on port " + InstrumentationSettings.AltUnityTesterPort, ERROR_COLOR, true);
                logger.Error("Cannot start AltUnity Tester communication protocol. Another process is listening on port" + InstrumentationSettings.AltUnityTesterPort);
            }

            catch (UnhandledStartCommError ex)
            {
                setDialog("An unexpected error occured while starting the communication protocol.", ERROR_COLOR, true);
                logger.Error(ex.InnerException, ex.InnerException.Message);
            }
        }

        private void onClientConnected()
        {

            string message = "Client connected.";
            _updateQueue.ScheduleResponse(() =>
            {
                setDialog(message, SUCCESS_COLOR, false);
            });
        }

        private void onClientDisconnected()
        {
            if (!communication.IsConnected) // 
                _updateQueue.ScheduleResponse(() =>
                {
                    setDialog("Waiting for connections on port: " + InstrumentationSettings.AltUnityTesterPort, SUCCESS_COLOR, false);
                });
        }


        #region proxy mode comm protocol
        private void initProxyCommProtocol()
        {
            var cmdHandler = new CommandHandler();

#if UNITY_WEBGL && !UNITY_EDITOR
                    communication = new WebSocketWebGLCommunication(cmdHandler, InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort);
#else

            communication = new WebSocketClientCommunication(cmdHandler, InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort);
#endif
            communication.OnConnect += onProxyConnect;
            communication.OnDisconnect += onProxyDisconnect;
            communication.OnError += onError;

        }
        private void startProxyCommProtocol()
        {
            initProxyCommProtocol();

            try
            {
                if (communication == null || !communication.IsListening) // start only if it is not already listening
                    communication.Start();

                if (!communication.IsConnected) // display dialog only if not connected 
                    onStart();
            }

            catch (UnhandledStartCommError ex)
            {
                setDialog("An unexpected error occurred while starting the communication protocol.", ERROR_COLOR, true);
                logger.Error(ex.InnerException, "An unexpected error occurred while starting the communication protocol.");
            }
            catch (Exception ex)
            {
                setDialog("An unexpected error occurred while starting the communication protocol.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occurred while starting the communication protocol.");
            }
        }
        private void onStart()
        {
            setDialog("Connecting to AltUnity Proxy on " + InstrumentationSettings.ProxyHost + ":" + InstrumentationSettings.ProxyPort, SUCCESS_COLOR, Dialog.activeSelf || wasConnectedBeforeToProxy);
            wasConnectedBeforeToProxy = false;
        }
        private void onProxyConnect()
        {

            string message = "Connected to AltUnity Proxy on " + InstrumentationSettings.ProxyHost + ":" + InstrumentationSettings.ProxyPort;
            _updateQueue.ScheduleResponse(() =>
            {
                setDialog(message, SUCCESS_COLOR, false);
                wasConnectedBeforeToProxy = true;
            });
        }

        private void onProxyDisconnect()
        {
            _updateQueue.ScheduleResponse(startProxyCommProtocol);
        }

        #endregion

        private void onError(string message, Exception ex)
        {
            logger.Error(message);
            if (ex != null)
                logger.Error(ex);
        }
        private void cleanUp()
        {
            logger.Debug("Stopping communication protocol");
            if (communication != null)
                communication.Stop();
        }
    }
}