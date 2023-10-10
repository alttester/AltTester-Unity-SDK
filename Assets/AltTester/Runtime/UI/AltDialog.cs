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
using System.Threading;
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
        private bool isValidData = false;
        private float update;
        public static string AppId;
        private string platform;
        private string platformVersion;
        private string deviceInstanceId;

        protected void Start()
        {
            Dialog.SetActive(InstrumentationSettings.ShowPopUp);

            setTitle("AltTester v." + AltRunner.VERSION);
            setUpCloseButton();
            setUpIcon();
            setUpHostInputField();
            setUpPortInputField();
            setUpAppNameInputField();
            setUpRestartButton();
            setUpCustomInputToggle();

            this.platform = Application.platform.ToString();
            this.platformVersion = SystemInfo.operatingSystem;
            this.deviceInstanceId = SystemInfo.deviceUniqueIdentifier;
            validateFields();
        }

        protected void Update()
        {
            _updateQueue.Cycle();

            handleConnectionLogic();

            if (this._liveUpdateCommunication == null || !this._liveUpdateCommunication.IsConnected)
                return;

            update += Time.unscaledDeltaTime;
            if (update > 1.0f / this._liveUpdateCommunication.FrameRate)
            {
                update = 0.0f;
                StartCoroutine(this.SendScreenshot());
            }
        }

        private void handleConnectionLogic()
        {
            if (_liveUpdateCommunication == null && _communication == null)
            {
                //This is the initial state where no connection is established
                if (isValidData)
                    beginCommunication();
                return;
            }
            if (_liveUpdateCommunication != null && _communication == null)
            {
                //Communication somehow stopped so we stop also liveUpdate 
                stopClient(_liveUpdateCommunication);
                _liveUpdateCommunication = null;
                beginCommunication();
                return;
            }
            if (_communication != null && _communication.IsConnected && _liveUpdateCommunication == null)
            {
                //Communication is connected and we start LiveUpdate to connect
                initLiveUpdateClient();
                startClient(_liveUpdateCommunication);
                return;
            }
            if (_communication != null && !_communication.IsConnected)
            {
                return;
            }
            if (_communication.IsConnected == false || _liveUpdateCommunication.IsConnected == false)
            {
                //One of the connection or both are disconnected
                stopClients();
                beginCommunication();
                return;
            }
        }

        private void initLiveUpdateClient()
        {
            _liveUpdateCommunication = new LiveUpdateCommunicationHandler(HostInputField.text, int.Parse(PortInputField.text), AppNameInputField.text, platform, platformVersion, deviceInstanceId, AppId);
            _liveUpdateCommunication.OnDisconnect += onDisconnect;
            _liveUpdateCommunication.OnError += onError;
            _liveUpdateCommunication.OnConnect += onConnect;
            _liveUpdateCommunication.Init();
        }

        private void beginCommunication()
        {
            ToggleCustomInput(false);
            initRuntimeClient();
            startClient(_communication);
        }

        protected IEnumerator SendScreenshot()
        {
            yield return new UnityEngine.WaitForEndOfFrame();
            this._liveUpdateCommunication.SendScreenshot();
        }

        protected void OnApplicationQuit() => stopClients();

        private void setMessage(string message, UnityEngine.Color color, bool visible = true)
        {
            Dialog.SetActive(visible);
            Dialog.GetComponent<UnityEngine.UI.Image>().color = color;
            MessageText.text = message;
        }

        private void setTitle(string title) => TitleText.text = title;
        private void toggleDialog() => Dialog.SetActive(!Dialog.activeSelf);
        private void setUpCloseButton() => CloseButton.onClick.AddListener(toggleDialog);
        private void setUpIcon() => Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(toggleDialog);

        private void onPortInputFieldValueChange(string value)
        {
            // Allow only positive numbers.
            if (value == "-")
                PortInputField.text = "";
        }

        private void setUpHostInputField()
        {
            HostInputField.text = PlayerPrefs.GetString(HOST, InstrumentationSettings.AltServerHost);
        }

        private void setUpPortInputField()
        {
            PortInputField.text = PlayerPrefs.GetString(PORT, InstrumentationSettings.AltServerPort.ToString());
            PortInputField.onValueChanged.AddListener(onPortInputFieldValueChange);
            PortInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
        }

        private void setUpAppNameInputField()
        {
            AppNameInputField.text = PlayerPrefs.GetString(APP_NAME, InstrumentationSettings.AppName);
        }

        private void onRestartButtonPress()
        {
            stopClients();
            validateFields();
        }

        private void validateFields()
        {
            isValidData = false;

            if (Uri.CheckHostName(HostInputField.text) != UriHostNameType.Unknown)
            {
                InstrumentationSettings.AltServerHost = HostInputField.text;
            }
            else
            {
                setMessage("The host should be a valid host.", color: ERROR_COLOR, visible: true);
                return;
            }

            int port;
            if (Int32.TryParse(PortInputField.text, out port) && port > 0 && port <= 65535)
            {
                InstrumentationSettings.AltServerPort = port;
            }
            else
            {
                setMessage("The port number should be between 1 and 65535.", color: ERROR_COLOR, visible: true);
                return;
            }

            if (!string.IsNullOrEmpty(AppNameInputField.text))
            {
                InstrumentationSettings.AppName = AppNameInputField.text;
            }
            else
            {
                setMessage("App name should not be empty.", color: ERROR_COLOR, visible: true);
                return;
            }
            isValidData = true;
        }

        private void setUpRestartButton()
        {
            RestartButton.onClick.AddListener(onRestartButtonPress);
        }

        private void setUpCustomInputToggle()
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

        private void initRuntimeClient()
        {
            _communication = new RuntimeCommunicationHandler(HostInputField.text, int.Parse(PortInputField.text), AppNameInputField.text, platform, platformVersion, deviceInstanceId);
            _communication.OnConnect += onConnect;
            _communication.OnDisconnect += onDisconnect;
            _communication.OnError += onError;

            _communication.CmdHandler.OnDriverConnect += onDriverConnect;
            _communication.CmdHandler.OnDriverDisconnect += onDriverDisconnect;
            _communication.CmdHandler.OnAppConnect += onAppConnect;
            _communication.Init();


            onStart();
        }

        private void startClient(BaseCommunicationHandler communicationHandler)
        {
            try
            {
                communicationHandler.Connect();
            }
            catch (RuntimeWebSocketClientException ex)
            {
                setMessage("An unexpected runtime error occurred while starting the AltTester client.", ERROR_COLOR, true);
                logger.Error(ex.InnerException, "An unexpected error occurred while starting the AltTester client.");
                stopClient(communicationHandler);
            }
            catch (Exception ex)
            {
                setMessage("An unexpected error occurred while starting the AltTester client.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occurred while starting the AltTester client.");
                stopClient(communicationHandler);
            }
        }

        private void stopClients()
        {
            _updateQueue.Clear();
            _connectedDrivers.Clear();
            stopClient(_communication);
            _communication = null;
            stopClient(_liveUpdateCommunication);
            _liveUpdateCommunication = null;
            onStart();
        }

        private static void stopClient(BaseCommunicationHandler communicationHandler)
        {
            if (communicationHandler == null)
                return;
            // Remove the callbacks before stopping the client to prevent the OnDisconnect callback to be called when we stop or restart the client.
            communicationHandler.OnConnect = null;
            communicationHandler.OnDisconnect = null;
            communicationHandler.OnError = null;

            if (communicationHandler.IsConnected)
                communicationHandler.Close();
        }

        private void onDisconnect(int code, string reason)
        {
            // All custom close codes must be between 4000 - 4999.
            if (code > 4000)
            {
                _updateQueue.ScheduleResponse(() =>
                {
                    setMessage(reason, ERROR_COLOR, true);
                });
            }
        }

        private void onStart()
        {
            string message = String.Format("Waiting to connect to AltServer on {0}:{1} with app name: '{2}', '{3}', '{4}', '{5}' and app id '{6}'.", HostInputField.text, PortInputField.text, AppNameInputField.text, this.platform, this.platformVersion, this.deviceInstanceId, AppId);
            setMessage(message, color: SUCCESS_COLOR, visible: Dialog.activeSelf);
        }

        private void onConnect()
        {
            string message = String.Format("Connected to AltServer on \n host:port {0}:{1} \n with app name: '{2}', \n '{3}', '{4}', '{5}'and app id '{6}'.. Waiting for Driver to connect.", HostInputField.text, PortInputField.text, AppNameInputField.text, this.platform, this.platformVersion, this.deviceInstanceId, AppId);

            _updateQueue.ScheduleResponse(() =>
            {
                setMessage(message, color: SUCCESS_COLOR, visible: true);
            });
        }

        private void onError(string message, Exception ex)
        {
            logger.Error(message);
            if (ex != null)
            {
                logger.Error(ex);
            }
        }

        private void onDriverConnect(string driverId)
        {
            logger.Debug("Driver Connected: " + driverId);
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}', '{3}', '{4}' and '{5}'.. Driver connected.", HostInputField.text, PortInputField.text, AppNameInputField.text, this.platform, this.platformVersion, this.deviceInstanceId);

            _connectedDrivers.Add(driverId);

            if (_connectedDrivers.Count == 1)
            {
                _updateQueue.ScheduleResponse(() =>
                {
                    PlayerPrefs.SetString(HOST, HostInputField.text);
                    PlayerPrefs.SetString(PORT, PortInputField.text);
                    PlayerPrefs.SetString(APP_NAME, AppNameInputField.text);
                    ToggleCustomInput(true);
                    setMessage(message, color: SUCCESS_COLOR, visible: false);
                });
            }
        }

        private void onAppConnect(string appId)
        {
            AppId = appId;
        }

        private void onDriverDisconnect(string driverId)
        {
            logger.Debug("Driver Disconnect: " + driverId);
            string message = String.Format("Connected to AltServer on {0}:{1} with app name: '{2}', '{3}', '{4}' and '{5}'.. Waiting for Driver to connect.", HostInputField.text, PortInputField.text, AppNameInputField.text, this.platform, this.platformVersion, this.deviceInstanceId);

            _connectedDrivers.Remove(driverId);
            if (_connectedDrivers.Count == 0)
            {
                _updateQueue.ScheduleResponse(() =>
                {
                    ToggleCustomInput(false);
                    setMessage(message, color: SUCCESS_COLOR, visible: true);
                });
            }
        }
    }
}
