using System;
using System.Collections;
using System.Collections.Generic;
using AltTester;
using AltTester.AltTesterUnitySDK.Communication;
using AltTester.AltTesterUnitySDK.Logging;
using AltWebSocketSharp;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.UI
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
        public UnityEngine.UI.Text InfoLabel = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.InputField HostInputField = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.InputField PortInputField = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.InputField AppNameInputField = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.Button RestartButton = null;

        [UnityEngine.SerializeField]
        public UnityEngine.UI.Toggle CustomInputToggle = null;

        public AltInstrumentationSettings InstrumentationSettings { get { return AltRunner._altRunner.InstrumentationSettings; } }

        private RuntimeCommunicationHandler _communication;
        private LiveUpdateCommunicationHandler _liveUpdateCommunication;

        private readonly AltResponseQueue _updateQueue = new AltResponseQueue();

        HashSet<string> _connectedDrivers = new HashSet<string>();

        private float update;

        protected void Start()
        {
            Dialog.SetActive(InstrumentationSettings.ShowPopUp);

            SetTitle("AltTester v." + "Connection Issue Problems");
            SetUpCloseButton();
            SetUpIcon();
            SetUpHostInputField();
            SetUpPortInputField();
            SetUpAppNameInputField();
            SetUpRestartButton();
            SetUpCustomInputToggle();

            StartClient();
        }

        protected void Update()
        {
            _updateQueue.Cycle();
            if (_liveUpdateCommunication == null && _communication == null)
            {
                ToggleCustomInput(false);
                StartClient();
                Debug.Log("Started Client");
            }
            else
            if (_liveUpdateCommunication == null ^ _communication == null)
            {
                Debug.Log("StopClient from the if");
                StopClient();
            }

            if (this._liveUpdateCommunication == null || !this._liveUpdateCommunication.IsConnected)
            {
                return;
            }

            update += Time.unscaledDeltaTime;
            if (update > 1.0f / this._liveUpdateCommunication.FrameRate)
            {
                update = 0.0f;
                StartCoroutine(this.SendScreenshot());
            }
        }

        protected IEnumerator SendScreenshot()
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            this._liveUpdateCommunication.SendScreenshot();
        }

        protected void OnApplicationQuit()
        {
            StopClient();
        }

        private void SetMessage(string message, UnityEngine.Color color, bool visible = true)
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

        private void SetUpIcon()
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
            HostInputField.text = InstrumentationSettings.AltServerHost;
        }

        private void SetUpPortInputField()
        {
            PortInputField.text = InstrumentationSettings.AltServerPort.ToString();
            PortInputField.onValueChanged.AddListener(OnPortInputFieldValueChange);
            PortInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
        }

        private void SetUpAppNameInputField()
        {
            AppNameInputField.text = InstrumentationSettings.AppName;
        }

        private void OnRestartButtonPress()
        {
            logger.Debug("Restart the AltTester client.");
            StopClient();

            if (Uri.CheckHostName(HostInputField.text) != UriHostNameType.Unknown)
            {
                InstrumentationSettings.AltServerHost = HostInputField.text;
            }
            else
            {
                SetMessage("The host should be a valid host.", color: ERROR_COLOR, visible: true);
                return;
            }

            int port;
            if (Int32.TryParse(PortInputField.text, out port) && port > 0 && port <= 65535)
            {
                InstrumentationSettings.AltServerPort = port;
            }
            else
            {
                SetMessage("The port number should be between 1 and 65535.", color: ERROR_COLOR, visible: true);
                return;
            }

            if (!string.IsNullOrEmpty(AppNameInputField.text))
            {
                InstrumentationSettings.AppName = AppNameInputField.text;
            }
            else
            {
                SetMessage("App name should not be empty.", color: ERROR_COLOR, visible: true);
                return;
            }

            try
            {
                StartClient();
            }
            catch (Exception ex)
            {
                SetMessage("An unexpected error occurred while restarting the AltTester client.", color: ERROR_COLOR, visible: true);
                logger.Error("An unexpected error occurred while restarting the AltTester client.");
                logger.Error(ex.GetType().ToString(), ex.Message);
            }
        }

        public void SetUpRestartButton()
        {
            RestartButton.onClick.AddListener(OnRestartButtonPress);
        }

        public void SetUpCustomInputToggle()
        {
            CustomInputToggle.onValueChanged.AddListener(ToggleCustomInput);
            ToggleCustomInput(false);
        }

        public void ToggleCustomInput(bool value)
        {
            CustomInputToggle.isOn = value;
            Icon.color = value ? UnityEngine.Color.white : UnityEngine.Color.grey;

#if ALTTESTER
#if ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = value;
            UnityEngine.Debug.Log("AltTester input: " + Input.UseCustomInput);
#endif
#if ENABLE_INPUT_SYSTEM
            if (value)
            {
                NewInputSystem.DisableDefaultDevicesAndEnableAltDevices();
            }
            else
            {
                NewInputSystem.EnableDefaultDevicesAndDisableAltDevices();
            }
#endif
#endif
        }

        private void InitClient()
        {
            _communication = new RuntimeCommunicationHandler(InstrumentationSettings.AltServerHost, InstrumentationSettings.AltServerPort, InstrumentationSettings.AppName);
            _communication.OnConnect += OnConnect;
            _communication.OnDisconnect += OnDisconnect;
            _communication.OnError += OnError;

            _communication.CmdHandler.OnDriverConnect += OnDriverConnect;
            _communication.CmdHandler.OnDriverDisconnect += OnDriverDisconnect;

            _liveUpdateCommunication = new LiveUpdateCommunicationHandler(InstrumentationSettings.AltServerHost, InstrumentationSettings.AltServerPort, InstrumentationSettings.AppName);
            _liveUpdateCommunication.OnDisconnect += OnDisconnect;
            _liveUpdateCommunication.OnError += OnError;
        }

        private void StartClient()
        {
            OnStart();
            InitClient();

            try
            {
                _communication.Connect();
                _liveUpdateCommunication.Connect();
            }
            catch (RuntimeWebSocketClientException ex)
            {
                SetMessage("An unexpected error occurred while starting the AltTester client.", ERROR_COLOR, true);
                logger.Error(ex.InnerException, "An unexpected error occurred while starting the AltTester client.");
                StopClient();
            }
            catch (Exception ex)
            {
                SetMessage("An unexpected error occurred while starting the AltTester client.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occurred while starting the AltTester client.");
                StopClient();
            }
        }

        private void StopClient()
        {
            _updateQueue.Clear();
            _connectedDrivers.Clear();


            if (_communication != null)
            {
                // Remove the callbacks before stopping the client to prevent the OnDisconnect callback to be called when we stop or restart the client.
                _communication.OnConnect = null;
                _communication.OnDisconnect = null;
                _communication.OnError = null;

                _communication.Close();
                _communication = null;
            }

            if (_liveUpdateCommunication != null)
            {
                _liveUpdateCommunication.OnDisconnect = null;
                _liveUpdateCommunication.OnError = null;

                _liveUpdateCommunication.Close();
                _liveUpdateCommunication = null;
            }
        }
        private void OnDisconnect()
        {
            _updateQueue.ScheduleResponse(() =>
            {
                StopClient();
            });

        }

        private void OnStart()
        {
            string message = String.Format("Waiting to connect to AltServer on {0}:{1} with app name: '{2}'.", InstrumentationSettings.AltServerHost, InstrumentationSettings.AltServerPort, InstrumentationSettings.AppName);
            SetMessage(message, color: SUCCESS_COLOR, visible: Dialog.activeSelf);
        }

        private void OnConnect()
        {
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}'. Waiting for Driver to connect.", InstrumentationSettings.AltServerHost, InstrumentationSettings.AltServerPort, InstrumentationSettings.AppName);

            _updateQueue.ScheduleResponse(() =>
            {
                SetMessage(message, color: SUCCESS_COLOR, visible: true);
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

        private void OnDriverConnect(string driverId)
        {
            logger.Debug("Driver Connected: " + driverId);
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}'. Driver connected.", InstrumentationSettings.AltServerHost, InstrumentationSettings.AltServerPort, InstrumentationSettings.AppName);

            _connectedDrivers.Add(driverId);

            if (_connectedDrivers.Count == 1)
            {
                _updateQueue.ScheduleResponse(() =>
                {
                    ToggleCustomInput(true);
                    SetMessage(message, color: SUCCESS_COLOR, visible: false);
                });
            }
        }

        private void OnDriverDisconnect(string driverId)
        {
            logger.Debug("Driver Disconnect: " + driverId);
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}'. Waiting for Driver to connect.", InstrumentationSettings.AltServerHost, InstrumentationSettings.AltServerPort, InstrumentationSettings.AppName);

            _connectedDrivers.Remove(driverId);

            if (_connectedDrivers.Count == 0)
            {

                _updateQueue.ScheduleResponse(() =>
                {
                    ToggleCustomInput(false);
                    SetMessage(message, color: SUCCESS_COLOR, visible: true);
                });
            }
        }
    }
}
