using System;
using Altom.AltUnityTester;
using Altom.AltUnityTester.Communication;
using Altom.AltUnityTester.Logging;

namespace Altom.AltUnityTester.UI
{
    public class AltUnityDialog : UnityEngine.MonoBehaviour
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        private readonly UnityEngine.Color SUCCESS_COLOR = new UnityEngine.Color32(0, 165, 36, 255);
        private readonly UnityEngine.Color WARNING_COLOR = new UnityEngine.Color32(255, 255, 95, 255);
        private readonly UnityEngine.Color ERROR_COLOR = new UnityEngine.Color32(191, 71, 85, 255);


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

        [UnityEngine.SerializeField]
        public UnityEngine.UI.Text PortLabel = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.InputField PortInputField = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.Button RestartButton = null;

        private ICommunication communication;

        public AltUnityInstrumentationSettings InstrumentationSettings { get { return AltUnityRunner._altUnityRunner.InstrumentationSettings; } }

        private readonly AltResponseQueue _updateQueue = new AltResponseQueue();
        private bool wasConnectedBeforeToProxy = false;

        protected void Start()
        {
            SetUpPortInputField();
            SetUpRestartButton();

            Dialog.SetActive(InstrumentationSettings.ShowPopUp);
            CloseButton.onClick.AddListener(ToggleDialog);
            Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ToggleDialog);
            TitleText.text = "AltUnity Tester v." + AltUnityRunner.VERSION;

            StartAltUnityTester();
        }

        protected void Update()
        {
            _updateQueue.Cycle();
        }

        protected void OnApplicationQuit()
        {
            cleanUp();
        }

        public void OnPortInputFieldValueChange(string value)
        {
            // Allow only positive numbers.
            if (value == "-")
            {
                PortInputField.text = "";
            }
        }

        public void SetUpPortInputField()
        {
            PortInputField.text = InstrumentationSettings.AltUnityTesterPort.ToString();
            PortInputField.onValueChanged.AddListener(OnPortInputFieldValueChange);
            PortInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
        }

        private void OnRestartButtonPress()
        {
            logger.Debug("Restart the AltUnity Tester.");

            int port;
            if (Int32.TryParse(PortInputField.text, out port) && port > 0 && port <= 65535)
            {
                InstrumentationSettings.AltUnityTesterPort = port;
            }
            else
            {
                setDialog("The port number should be beteween 1 and 65535.", ERROR_COLOR, true);
                return;
            }

            try
            {
                RestartAltUnityTester();
            }
            catch (Exception ex)
            {
                setDialog("An unexpected error occurred while restarting the AltUnity Tester.", ERROR_COLOR, true);
                logger.Error("An unexpected error occurred while restarting the AltUnity Tester.");
                logger.Error(ex.GetType().ToString(), ex.Message);
            }
        }

        public void SetUpRestartButton()
        {
            RestartButton.onClick.AddListener(OnRestartButtonPress);
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

        private void StartAltUnityTester()
        {
            if (InstrumentationSettings.InstrumentationMode == AltUnityInstrumentationMode.Server)
            {
                startServerCommProtocol();
            }
            else
            {
                startProxyCommProtocol();
            }
        }

        private void StopAltUnityTester()
        {
            logger.Debug("Stopping AltUnity Tester.");
            if (communication != null)
            {
                communication.Stop();
            }
        }

        private void RestartAltUnityTester()
        {
            StopAltUnityTester();
            StartAltUnityTester();
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
            catch (AddressInUseCommError ex)
            {
                setDialog(ex.Message, ERROR_COLOR, true);
                logger.Error(ex.Message);
            }
            catch (UnhandledStartCommError ex)
            {
                setDialog(ex.Message, ERROR_COLOR, true);
                logger.Error(ex.InnerException, ex.InnerException.Message);
            }
        }

        private void onClientConnected()
        {
            string message = "Client connected.";
#if ALTUNITYTESTER && ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = true;
            UnityEngine.Debug.Log("Custom input: " + Input.UseCustomInput);
#endif


            _updateQueue.ScheduleResponse(() =>
            {
#if ALTUNITYTESTER && ENABLE_INPUT_SYSTEM
                NewInputSystem.DisableDefaultDevicesAndEnableAltUnityDevices();
#endif
                setDialog(message, SUCCESS_COLOR, false);
            });
        }

        private void onClientDisconnected()
        {
#if ALTUNITYTESTER && ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = false;
            UnityEngine.Debug.Log("Custom input: " + Input.UseCustomInput);
#endif

            if (!communication.IsConnected) //
                _updateQueue.ScheduleResponse(() =>
                {

#if ALTUNITYTESTER && ENABLE_INPUT_SYSTEM
                    NewInputSystem.EnableDefaultDevicesAndDisableAltUnityDevices();

#endif
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
            {
                logger.Error(ex);
            }
        }

        private void cleanUp()
        {
            logger.Debug("Stopping communication protocol");
            if (communication != null)
            {
                communication.Stop();
            }
        }
    }
}