/*
    Copyright(C) 2023 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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
        private readonly string HOST = "AltTesterHost";
        private readonly string PORT = "AltTesterPort";
        private readonly string APP_NAME = "AltTesterAppName";

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
        private bool DisconnectCommunicationFlag = false;
        private bool DisconnectLiveUpdateFlag = false;

        protected void Start()
        {
            Dialog.SetActive(InstrumentationSettings.ShowPopUp);

            SetTitle("AltTester v." + AltRunner.VERSION);
            SetUpCloseButton();
            SetUpIcon();
            SetUpHostInputField();
            SetUpPortInputField();
            SetUpAppNameInputField();
            SetUpRestartButton();
            SetUpCustomInputToggle();
        }

        protected void Update()
        {
            _updateQueue.Cycle();

            if (_liveUpdateCommunication == null && _communication == null)
            {
                ToggleCustomInput(false);
                InitClient();
                StartClient();
            }
            else
            {
                if (_liveUpdateCommunication == null ^ _communication == null)
                {
                    StopClient();
                }
                else
                {
                    if (_liveUpdateCommunication.IsConnected ^ _communication.IsConnected)
                    {
                        if ((DisconnectLiveUpdateFlag && _communication.IsConnected) || (_liveUpdateCommunication.IsConnected && DisconnectCommunicationFlag))
                        {
                            StopClient();
                        }
                    }
                    else
                    {
                        if (!_liveUpdateCommunication.IsConnected && !_communication.IsConnected && DisconnectLiveUpdateFlag && DisconnectCommunicationFlag)
                        {
                            StartClient();
                        }

                    }
                }
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
            HostInputField.text = PlayerPrefs.GetString(HOST, InstrumentationSettings.AltServerHost);
        }

        private void SetUpPortInputField()
        {
            PortInputField.text = PlayerPrefs.GetString(PORT, InstrumentationSettings.AltServerPort.ToString());
            PortInputField.onValueChanged.AddListener(OnPortInputFieldValueChange);
            PortInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
        }

        private void SetUpAppNameInputField()
        {
            AppNameInputField.text = PlayerPrefs.GetString(APP_NAME, InstrumentationSettings.AppName);
        }

        private void OnRestartButtonPress()
        {
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
                InitClient();
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
            _communication = new RuntimeCommunicationHandler(HostInputField.text, int.Parse(PortInputField.text), AppNameInputField.text);
            _communication.OnConnect += OnConnect;
            _communication.OnDisconnect += OnDisconnectCommunication;
            _communication.OnError += OnError;

            _communication.CmdHandler.OnDriverConnect += OnDriverConnect;
            _communication.CmdHandler.OnDriverDisconnect += OnDriverDisconnect;
            _communication.Init();

            _liveUpdateCommunication = new LiveUpdateCommunicationHandler(HostInputField.text, int.Parse(PortInputField.text), AppNameInputField.text);
            _liveUpdateCommunication.OnDisconnect += OnDisconnectLiveUpdate;
            _liveUpdateCommunication.OnError += OnError;
            _liveUpdateCommunication.OnConnect += OnConnect;
            _liveUpdateCommunication.Init();

            DisconnectLiveUpdateFlag = true;
            DisconnectCommunicationFlag = true;

            OnStart();
        }

        private void StartClient()
        {
            OnStart();

            try
            {
                DisconnectLiveUpdateFlag = false;
                DisconnectCommunicationFlag = false;
                _communication.Connect();
                _liveUpdateCommunication.Connect();
            }
            catch (RuntimeWebSocketClientException ex)
            {
                SetMessage("An unexpected runtime error occurred while starting the AltTester client.", ERROR_COLOR, true);
                logger.Error(ex.InnerException, "An unexpected error occurred while starting the AltTester client.");
                StopClient();
            }
            catch (Exception ex)
            {
                SetMessage("An unexpected error occurred while starting the AltTester client.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occurred while starting the AltTester client.");
                StopClient();
            }
            logger.Debug("EndStartClient");
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

                if (_communication.IsConnected)
                    _communication.Close();
                _communication = null;
                DisconnectCommunicationFlag = true;
            }

            if (_liveUpdateCommunication != null)
            {
                _liveUpdateCommunication.OnDisconnect = null;
                _liveUpdateCommunication.OnError = null;
                _liveUpdateCommunication.OnConnect = null;

                if (_liveUpdateCommunication.IsConnected)
                    _liveUpdateCommunication.Close();
                _liveUpdateCommunication = null;
                DisconnectLiveUpdateFlag = true;
            }
            OnStart();
        }

        private void OnDisconnectCommunication(int code, string reason)
        {
            // All custom close codes must be between 4000 - 4999.
            if (code > 4000)
            {
                _updateQueue.ScheduleResponse(() =>
                {
                    SetMessage(reason, ERROR_COLOR, true);
                });
            }
            else
            {
                DisconnectCommunicationFlag = true;
            }
        }

        private void OnDisconnectLiveUpdate(int code, string reason)
        {
            // All custom close codes must be between 4000 - 4999.
            if (code > 4000)
            {
                _updateQueue.ScheduleResponse(() =>
                {
                    SetMessage(reason, ERROR_COLOR, true);
                });
            }
            else
            {
                DisconnectLiveUpdateFlag = true;
            }
        }

        private void OnStart()
        {
            string message = String.Format("Waiting to connect to AltServer on {0}:{1} with app name: '{2}'.", HostInputField.text, PortInputField.text, AppNameInputField.text);
            SetMessage(message, color: SUCCESS_COLOR, visible: Dialog.activeSelf);
        }

        private void OnConnect()
        {
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}'. Waiting for Driver to connect.", HostInputField.text, PortInputField.text, AppNameInputField.text);

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
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}'. Driver connected.", HostInputField.text, PortInputField.text, AppNameInputField.text);

            _connectedDrivers.Add(driverId);

            if (_connectedDrivers.Count == 1)
            {
                _updateQueue.ScheduleResponse(() =>
                {
                    PlayerPrefs.SetString(HOST, HostInputField.text);
                    PlayerPrefs.SetString(PORT, PortInputField.text);
                    PlayerPrefs.SetString(APP_NAME, AppNameInputField.text);
                    ToggleCustomInput(true);
                    SetMessage(message, color: SUCCESS_COLOR, visible: false);
                });
            }
        }

        private void OnDriverDisconnect(string driverId)
        {
            logger.Debug("Driver Disconnect: " + driverId);
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}'. Waiting for Driver to connect.", HostInputField.text, PortInputField.text, AppNameInputField.text);

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
