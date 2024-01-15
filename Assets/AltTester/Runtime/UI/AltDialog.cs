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
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

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
        private readonly string UID = "UID";
        private int responseCode = 0;

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

        private RuntimeCommunicationHandler communication;
        private LiveUpdateCommunicationHandler liveUpdateCommunication;
        private readonly AltResponseQueue updateQueue = new AltResponseQueue();
        HashSet<string> connectedDrivers = new HashSet<string>();

        private bool isDataValid = false;
        private bool wasConnected = false;
        private float update;
        public static string AppId;
        private string platform;
        private string platformVersion;
        private string deviceInstanceId;
        private float currentTime;
        float retryTime = 0.3f;

        protected void Start()
        {
            Dialog.SetActive(InstrumentationSettings.ShowPopUp);
            resetConnectionDataBasedOnUID();

            setTitle("AltTester® v." + AltRunner.VERSION);
            setUpCloseButton();
            setUpIcon();
            setUpHostInputField();
            setUpPortInputField();
            setUpAppNameInputField();
            resetConnectionDataBasedOnUID();
            setUpRestartButton();
            setUpCustomInputToggle();
            setInteractibilityForRestartButton(false);

            this.platform = Application.platform.ToString();
            this.platformVersion = SystemInfo.operatingSystem;
            this.deviceInstanceId = SystemInfo.deviceUniqueIdentifier;
            validateFields();
            onStart();
        }

        protected void Update()
        {
            updateQueue.Cycle();
            checkIfPlayerPrefNeedsToBeDeleted();

            handleConnectionLogic();

            if (this.liveUpdateCommunication == null || !this.liveUpdateCommunication.IsRunning || !this.liveUpdateCommunication.IsConnected)
                return;

            update += Time.unscaledDeltaTime;
            if (update > 1.0f / this.liveUpdateCommunication.FrameRate)
            {
                update = 0.0f;
                StartCoroutine(this.SendScreenshot());
            }

        }

        private void handleConnectionLogic()
        {
            if (RestartButton.interactable) //to prevent auto connect
                return;
            if (currentTime <= retryTime)
            {
                currentTime += Time.unscaledDeltaTime;
                return;
            }
            currentTime = 0;

            if (responseCode > 4000 && responseCode < 5000)
            {
                setInteractibilityForRestartButton(true);
                return;
            }
            if (liveUpdateCommunication == null && communication == null)
            {
                //This is the initial state where no connection is established
                if (isDataValid)
                {
                    beginCommunication();
                    setInteractibilityForRestartButton(false);
                }
                return;
            }
            if (liveUpdateCommunication != null && communication == null)
            {

                //Communication somehow stopped so we stop liveUpdate as well
                stopClient(liveUpdateCommunication);
                liveUpdateCommunication = null;
                beginCommunication();
                return;
            }
            if (communication != null && communication.waitingToConnect)
            {
                if (communication.IsConnected)
                    communication.waitingToConnect = false;
                if (communication.WsClientReadyState == WebSocketState.Closed)
                {
                    beginCommunication();
                }
                return;
            }
            if (communication != null && communication.IsConnected && liveUpdateCommunication == null && AppId != null)
            {

                //Communication is connected and we start LiveUpdate to connect
                initLiveUpdateClient();
                startClient(liveUpdateCommunication);
                return;
            }
            if (communication != null && !communication.IsConnected && !wasConnected)
            {

                //Communication is initialized but there is no server to connect to yet
                startClient(communication);
                return;
            }
            if (liveUpdateCommunication != null && liveUpdateCommunication.waitingToConnect)
            {
                if (liveUpdateCommunication.IsConnected)
                    liveUpdateCommunication.waitingToConnect = false;
                return;
            }
            if (communication.IsConnected == false || (liveUpdateCommunication != null && liveUpdateCommunication.IsConnected == false))
            {

                //One of the connections or both are disconnected
                stopClients();
                beginCommunication();
                return;
            }
            setInteractibilityForRestartButton(true);

        }

        private void checkIfPlayerPrefNeedsToBeDeleted()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.L))
#else
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.leftShiftKey.isPressed && Keyboard.current.dKey.isPressed && Keyboard.current.lKey.isPressed)
#endif
#endif
            {
                PlayerPrefs.DeleteKey(UID);
                resetConnectionDataBasedOnUID();
            }
        }

        private void initLiveUpdateClient()
        {
            liveUpdateCommunication = new LiveUpdateCommunicationHandler(HostInputField.text, int.Parse(PortInputField.text), AppNameInputField.text, platform, platformVersion, deviceInstanceId, AppId);
            liveUpdateCommunication.OnDisconnect += onDisconnect;
            liveUpdateCommunication.OnError += onError;
            liveUpdateCommunication.OnConnect += onConnect;
            liveUpdateCommunication.Init();
        }

        private void beginCommunication()
        {
            ToggleCustomInput(false);
            initRuntimeClient();
            startClient(communication);
        }

        protected IEnumerator SendScreenshot()
        {
#if UNITY_EDITOR
            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
#endif
                yield return new UnityEngine.WaitForEndOfFrame();
            this.liveUpdateCommunication.SendScreenshot();
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
            {
                PortInputField.text = "";
            }
            setInteractibilityForRestartButton(true);

        }

        private void resetConnectionDataBasedOnUID()
        {
            if (InstrumentationSettings.ResetConnectionData && (InstrumentationSettings.UID != PlayerPrefs.GetString(UID, "")))
            {
                PlayerPrefs.SetString(HOST, InstrumentationSettings.AltServerHost);
                PlayerPrefs.SetInt(PORT, InstrumentationSettings.AltServerPort);
                PlayerPrefs.SetString(APP_NAME, InstrumentationSettings.AppName);
            }
            PlayerPrefs.SetString(UID, InstrumentationSettings.UID);
            HostInputField.text = PlayerPrefs.GetString(HOST, InstrumentationSettings.AltServerHost);
            PortInputField.text = PlayerPrefs.GetString(PORT, InstrumentationSettings.AltServerPort.ToString());
            AppNameInputField.text = PlayerPrefs.GetString(APP_NAME, InstrumentationSettings.AppName);

        }
        private void setUpHostInputField()
        {
            HostInputField.text = PlayerPrefs.GetString(HOST, InstrumentationSettings.AltServerHost);
            HostInputField.onValueChanged.AddListener(onHostValueChange);
        }

        private void onHostValueChange(string _)
        {
            setInteractibilityForRestartButton(true);
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
            AppNameInputField.onValueChanged.AddListener(onAppNameValueChanged);
        }

        private void onAppNameValueChanged(string _)
        {
            setInteractibilityForRestartButton(true);
        }

        private void onRestartButtonPress()
        {
            responseCode = 0;
            stopClients();
            validateFields();
            setInteractibilityForRestartButton(false);
        }
        private void validateFields()
        {
            isDataValid = false;

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
            isDataValid = true;
        }


        private void setUpRestartButton() => RestartButton.onClick.AddListener(onRestartButtonPress);

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
            communication = new RuntimeCommunicationHandler(HostInputField.text, int.Parse(PortInputField.text), AppNameInputField.text, platform, platformVersion, deviceInstanceId);
            communication.OnConnect += onConnect;
            communication.OnDisconnect += onDisconnect;
            communication.OnError += onError;

            communication.CmdHandler.OnDriverConnect += onDriverConnect;
            communication.CmdHandler.OnDriverDisconnect += onDriverDisconnect;
            communication.CmdHandler.OnAppConnect += onAppConnect;
            communication.Init();
        }
        private void setInteractibilityForRestartButton(bool isInteractable) => RestartButton.interactable = isInteractable;

        private void startClient(BaseCommunicationHandler communicationHandler)
        {
            try
            {
                communicationHandler.waitingToConnect = true;
                communicationHandler.Connect();
            }
            catch (InvalidOperationException)
            {
                stopClient(communicationHandler);
                communicationHandler.waitingToConnect = false;
                if (communicationHandler.GetType().Equals(typeof(RuntimeCommunicationHandler)))
                    initRuntimeClient();
                else
                    initLiveUpdateClient();

            }
            catch (Exception ex)
            {
                setMessage("An unexpected error occurred while starting the AltTester client.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occurred while starting the AltTester client.");
                stopClient(communicationHandler);
                communicationHandler.waitingToConnect = false;
            }
        }

        private void stopClients()
        {
            updateQueue.Clear();
            connectedDrivers.Clear();
            stopClient(communication);
            communication = null;
            stopClient(liveUpdateCommunication);
            liveUpdateCommunication = null;
            onStart();
            AppId = null;
            wasConnected = false;
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
            responseCode = code;
            // All custom close codes must be between 4000 - 4999.
            if (code > 4000)
            {
                updateQueue.ScheduleResponse(() =>
                {
                    setMessage(reason, ERROR_COLOR, true);
                });
            }
        }

        private void onStart()
        {
            string message = String.Format("Waiting to connect to AltServer on {0}host:port {1}:{2}with appName: '{3}',{4}platform: '{5}',{6}platformVersion: '{7}',{8}deviceId: '{9}' {10}and appId '{11}'.", Environment.NewLine, HostInputField.text, PortInputField.text + Environment.NewLine, AppNameInputField.text, Environment.NewLine, this.platform, Environment.NewLine, this.platformVersion, Environment.NewLine, this.deviceInstanceId, Environment.NewLine, AppId);
            setMessage(message, color: SUCCESS_COLOR, visible: Dialog.activeSelf);
        }

        private void onConnect()
        {
            wasConnected = true;
            string message = $"Connected to AltServer on {Environment.NewLine}host:port {HostInputField.text}:{PortInputField.text}{Environment.NewLine}with appName: '{AppNameInputField.text}'{Environment.NewLine}platform: '{platform}'{Environment.NewLine}platformVersion: '{platformVersion}'{Environment.NewLine}deviceId: '{deviceInstanceId}' {Environment.NewLine}appId '{AppId}'.{Environment.NewLine}Waiting for Driver to connect.";

            updateQueue.ScheduleResponse(() =>
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
            string message = String.Format("Connected to AltServer on {0}host:port {1}:{2}with appName: '{3}',{4}platform: '{5}',{6}platformVersion: '{7}',{8}deviceId: '{9}' {10}and appId '{11}'.{12}Driver connected.", Environment.NewLine, HostInputField.text, PortInputField.text + Environment.NewLine, AppNameInputField.text, Environment.NewLine, this.platform, Environment.NewLine, this.platformVersion, Environment.NewLine, this.deviceInstanceId, Environment.NewLine, AppId, Environment.NewLine);

            connectedDrivers.Add(driverId);

            if (connectedDrivers.Count == 1)
            {
                updateQueue.ScheduleResponse(() =>
                {
                    PlayerPrefs.SetString(HOST, HostInputField.text);
                    PlayerPrefs.SetString(PORT, PortInputField.text);
                    PlayerPrefs.SetString(APP_NAME, AppNameInputField.text);
                    ToggleCustomInput(true);
                    setMessage(message, color: SUCCESS_COLOR, visible: false);
                });
            }
        }
        private void onAppConnect(string appId) => AppId = appId;

        private void onDriverDisconnect(string driverId)
        {
            logger.Debug("Driver Disconnect: " + driverId);
            string message = String.Format("Connected to AltServer on {0}host:port {1}:{2}with appName: '{3}',{4}platform: '{5}',{6}platformVersion: '{7}',{8}deviceId: '{9}' {10}and appId '{11}'.{12}Waiting for Driver to connect.", Environment.NewLine, HostInputField.text, PortInputField.text + Environment.NewLine, AppNameInputField.text, Environment.NewLine, this.platform, Environment.NewLine, this.platformVersion, Environment.NewLine, this.deviceInstanceId, Environment.NewLine, AppId, Environment.NewLine);

            connectedDrivers.Remove(driverId);
            if (connectedDrivers.Count == 0)
            {
                updateQueue.ScheduleResponse(() =>
                {
                    ToggleCustomInput(false);
                    setMessage(message, color: SUCCESS_COLOR, visible: true);
                });
            }
        }
    }
}
