using System;
using Altom.AltTester;
using Altom.AltTester.Communication;
using Altom.AltTester.Logging;

namespace Altom.AltTester.UI
{
    public class AltDialog : UnityEngine.MonoBehaviour
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

        [UnityEngine.SerializeField]
        public UnityEngine.UI.Toggle CustomInputToggle = null;

        private ICommunication communication;

        public AltInstrumentationSettings InstrumentationSettings { get { return AltRunner._altRunner.InstrumentationSettings; } }

        private readonly AltResponseQueue _updateQueue = new AltResponseQueue();
        private bool wasConnectedBeforeToProxy = false;

        public void Awake()
        {
            CustomInputToggle = UnityEngine.GameObject.Find("AltDialog/Dialog/Toggle").GetComponent<UnityEngine.UI.Toggle>();
        }

        protected void Start()
        {
            SetUpPortInputField();
            SetUpRestartButton();

            Dialog.SetActive(InstrumentationSettings.ShowPopUp);
            CloseButton.onClick.AddListener(ToggleDialog);
            Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ToggleDialog);
            TitleText.text = "AltTester v." + AltRunner.VERSION;
            CustomInputToggle.onValueChanged.AddListener(ToggleInput);
            CustomInputToggle.isOn=false;

            StartAltTester();
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
            PortInputField.text = InstrumentationSettings.AltTesterPort.ToString();
            PortInputField.onValueChanged.AddListener(OnPortInputFieldValueChange);
            PortInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
        }

        private void OnRestartButtonPress()
        {
            logger.Debug("Restart the AltTester.");

            int port;
            if (Int32.TryParse(PortInputField.text, out port) && port > 0 && port <= 65535)
            {
                InstrumentationSettings.AltTesterPort = port;
            }
            else
            {
                setDialog("The port number should be beteween 1 and 65535.", ERROR_COLOR, true);
                return;
            }

            try
            {
                RestartAltTester();
            }
            catch (Exception ex)
            {
                setDialog("An unexpected error occurred while restarting the AltTester.", ERROR_COLOR, true);
                logger.Error("An unexpected error occurred while restarting the AltTester.");
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

        public void ToggleInput(bool value)
        {
            Icon.color = value ? UnityEngine.Color.white : UnityEngine.Color.grey;
#if ALTTESTER
#if ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = value;
            UnityEngine.Debug.Log("Custom input: " + Input.UseCustomInput);
#endif
#if ENABLE_INPUT_SYSTEM
            if (value)
                NewInputSystem.DisableDefaultDevicesAndEnableAltDevices();
            else
                NewInputSystem.EnableDefaultDevicesAndDisableAltDevices();
#endif
#endif
        }

        private void setDialog(string message, UnityEngine.Color color, bool visible)
        {
            Dialog.SetActive(visible);
            MessageText.text = message;
            Dialog.GetComponent<UnityEngine.UI.Image>().color = color;
        }

        private void StartAltTester()
        {
            if (InstrumentationSettings.InstrumentationMode == AltInstrumentationMode.Server)
            {
                startServerCommProtocol();
            }
            else
            {
                startProxyCommProtocol();
            }
        }

        private void StopAltTester()
        {
            logger.Debug("Stopping AltTester.");
            if (communication != null)
            {
                communication.Stop();
            }
        }

        private void RestartAltTester()
        {
            StopAltTester();
            StartAltTester();
        }

        private void startServerCommProtocol()
        {
            var cmdHandler = new CommandHandler();
            communication = new WebSocketServerCommunication(cmdHandler, "0.0.0.0", InstrumentationSettings.AltTesterPort);
            communication.OnConnect += onClientConnected;
            communication.OnDisconnect += onClientDisconnected;
            communication.OnError += onError;

            setDialog("Starting AltTester on port: " + InstrumentationSettings.AltTesterPort, WARNING_COLOR, true);

            try
            {
                communication.Start();
                setDialog("Waiting for connection on port: " + InstrumentationSettings.AltTesterPort, SUCCESS_COLOR, true);
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

            _updateQueue.ScheduleResponse(() =>
            {
                CustomInputToggle.isOn = true;
                setDialog(message, SUCCESS_COLOR, false);
            });
        }

        private void onClientDisconnected()
        {

            if (!communication.IsConnected) //
                _updateQueue.ScheduleResponse(() =>
                {
                    CustomInputToggle.isOn = false;
                    setDialog("Waiting for connections on port: " + InstrumentationSettings.AltTesterPort, SUCCESS_COLOR, false);
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
            setDialog("Connecting to AltProxy on " + InstrumentationSettings.ProxyHost + ":" + InstrumentationSettings.ProxyPort, SUCCESS_COLOR, Dialog.activeSelf || wasConnectedBeforeToProxy);
            wasConnectedBeforeToProxy = false;
        }

        private void onProxyConnect()
        {
            string message = "Connected to AltProxy on " + InstrumentationSettings.ProxyHost + ":" + InstrumentationSettings.ProxyPort;
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