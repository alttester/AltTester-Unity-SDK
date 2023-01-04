using System;
using AltTester;
using AltTester.Communication;
using AltTester.Logging;

namespace AltTester.UI
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
        public UnityEngine.UI.InputField HostInputField = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.InputField PortInputField = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.InputField GameNameInputField = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.Button RestartButton = null;

        public AltInstrumentationSettings InstrumentationSettings { get { return AltRunner._altRunner.InstrumentationSettings; } }

        private ICommunication _communication;
        private readonly AltResponseQueue _updateQueue = new AltResponseQueue();
        private bool _wasConnectedBefore = false;

        protected void Start()
        {
            Dialog.SetActive(InstrumentationSettings.ShowPopUp);

            SetTitle("AltTester v." + AltRunner.VERSION);
            SetUpCloseButton();
            setUpIcon();
            SetUpHostInputField();
            SetUpPortInputField();
            SetUpGameNameInputField();
            SetUpRestartButton();

            StartClient();
        }

        protected void Update()
        {
            _updateQueue.Cycle();
        }

        protected void OnApplicationQuit()
        {
            StopClient();
        }

        private void SetMessage(string message, UnityEngine.Color color, bool visible)
        {
            Dialog.SetActive(visible);
            Dialog.GetComponent<UnityEngine.UI.Image>().color = color;
            MessageText.text = message;
        }

        private void SetTitle(string title)
        {
            TitleText.text = title;
        }

        private void ToggleDialog()
        {
            Dialog.SetActive(!Dialog.activeSelf);
        }

        private void SetUpCloseButton()
        {
            CloseButton.onClick.AddListener(ToggleDialog);
        }

        private void setUpIcon()
        {
            Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ToggleDialog);
        }

        private void OnPortInputFieldValueChange(string value)
        {
            // Allow only positive numbers.
            if (value == "-")
            {
                PortInputField.text = "";
            }
        }

        private void SetUpHostInputField()
        {
            HostInputField.text = InstrumentationSettings.ProxyHost;
        }

        private void SetUpPortInputField()
        {
            PortInputField.text = InstrumentationSettings.ProxyPort.ToString();
            PortInputField.onValueChanged.AddListener(OnPortInputFieldValueChange);
            PortInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
        }

        private void SetUpGameNameInputField()
        {
            GameNameInputField.text = InstrumentationSettings.GameName;
        }

        private void OnRestartButtonPress()
        {
            logger.Debug("Restart the AltTester client.");
            StopClient();

            if (Uri.CheckHostName(HostInputField.text) != UriHostNameType.Unknown)
            {
                InstrumentationSettings.ProxyHost = HostInputField.text;
            }
            else
            {
                SetMessage("The host should be a valid host.", ERROR_COLOR, true);
                return;
            }

            int port;
            if (Int32.TryParse(PortInputField.text, out port) && port > 0 && port <= 65535)
            {
                InstrumentationSettings.ProxyPort = port;
            }
            else
            {
                SetMessage("The port number should be beteween 1 and 65535.", ERROR_COLOR, true);
                return;
            }

            if (!string.IsNullOrEmpty(GameNameInputField.text))
            {
                InstrumentationSettings.GameName = GameNameInputField.text;
            }
            else
            {
                SetMessage("Game name should not be empty.", ERROR_COLOR, true);
                return;
            }

            try
            {
                StartClient();
            }
            catch (Exception ex)
            {
                SetMessage("An unexpected error occurred while restarting the AltTester client.", ERROR_COLOR, true);
                logger.Error("An unexpected error occurred while restarting the AltTester client.");
                logger.Error(ex.GetType().ToString(), ex.Message);
            }
        }

        public void SetUpRestartButton()
        {
            RestartButton.onClick.AddListener(OnRestartButtonPress);
        }

        private void InitClient()
        {
            var cmdHandler = new CommandHandler();

#if UNITY_WEBGL && !UNITY_EDITOR
            _communication = new WebSocketWebGLCommunication(cmdHandler, InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort);
#else
            _communication = new WebSocketClientCommunication(cmdHandler, InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort, InstrumentationSettings.GameName);
#endif

            _communication.OnConnect += OnConnect;
            _communication.OnDisconnect += OnDisconnect;
            _communication.OnError += OnError;
        }

        private void StartClient()
        {
            InitClient();

            try
            {
                if (_communication == null || !_communication.IsListening) // start only if it is not already listening
                {
                    _communication.Start();
                }
                if (!_communication.IsConnected) // display dialog only if not connected
                {
                    OnStart();
                }
            }
            catch (UnhandledStartCommError ex)
            {
                SetMessage("An unexpected error occurred while starting the communication protocol.", ERROR_COLOR, true);
                logger.Error(ex.InnerException, "An unexpected error occurred while starting the communication protocol.");
            }
            catch (Exception ex)
            {
                SetMessage("An unexpected error occurred while starting the communication protocol.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occurred while starting the communication protocol.");
            }
        }

        private void StopClient()
        {
            _updateQueue.Clear();

            if (_communication != null)
            {
                // Remove the callbacks before stopping the client to prevent the OnDisconnect callback to be called when we stop or restart the client.
                _communication.OnConnect = null;
                _communication.OnDisconnect = null;
                _communication.OnError = null;

                _communication.Stop();
                _communication = null;
            }
        }

        private void OnStart()
        {
            string message = String.Format("Waiting to connect to AltProxy on {0}:{1} with game name: '{2}'.", InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort, InstrumentationSettings.GameName);
            SetMessage(message, SUCCESS_COLOR, Dialog.activeSelf || _wasConnectedBefore);
            _wasConnectedBefore = false;
        }

        private void OnConnect()
        {
            string message = String.Format("Connected to AltProxy on {0}:{1} with game name: '{2}'.", InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort, InstrumentationSettings.GameName);

#if ALTTESTER && ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = true;
            UnityEngine.Debug.Log("Custom input: " + Input.UseCustomInput);
#endif

            _updateQueue.ScheduleResponse(() =>
            {
#if ALTTESTER && ENABLE_INPUT_SYSTEM
                NewInputSystem.DisableDefaultDevicesAndEnableAltDevices();
#endif

                SetMessage(message, SUCCESS_COLOR, false);
                _wasConnectedBefore = true;
            });
        }

        private void OnDisconnect()
        {
#if ALTTESTER && ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = false;
            UnityEngine.Debug.Log("Custom input: " + Input.UseCustomInput);
#endif

            _updateQueue.ScheduleResponse(() =>
            {
#if ALTTESTER && ENABLE_INPUT_SYSTEM
                NewInputSystem.EnableDefaultDevicesAndDisableAltDevices();
#endif

                StartClient();
            });
        }

        private void OnError(string message, Exception ex)
        {
            logger.Error(message);

            if (ex != null)
            {
                logger.Error(ex);
            }
        }
    }
}
