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
using AltTester.AltTesterUnitySDK.Communication;
using AltTester.AltTesterUnitySDK.Logging;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace AltTester.AltTesterUnitySDK.UI
{
    public class AltDialog : UnityEngine.MonoBehaviour
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        private readonly Color SUCCESS_COLOR = new Color32(0, 165, 36, 255);
        private readonly Color WARNING_COLOR = new Color32(255, 255, 95, 255);
        private readonly Color ERROR_COLOR = new Color32(191, 71, 85, 255);
        private const string HOST = "AltTesterHost";
        private const string PORT = "AltTesterPort";
        private const string APP_NAME = "AltTesterAppName";
        private const string UID = "UID";
        private readonly string EDITING_TEXT = $"Editing host, port or appName.{Environment.NewLine}Press the Restart button to start connection with the new values.";
        private int responseCode = 0;

        [SerializeField]
        public GameObject Dialog = null;

        [SerializeField]
        public UnityEngine.UI.Text TitleText = null;

        [SerializeField]
        public UnityEngine.UI.Text MessageText = null;

        [SerializeField]
        public UnityEngine.UI.Button CloseButton = null;

        [SerializeField]
        public UnityEngine.UI.Image Icon = null;

        [SerializeField]
        public UnityEngine.UI.Text InfoLabel = null;

        [SerializeField]
        public UnityEngine.UI.InputField HostInputField = null;

        [SerializeField]
        public UnityEngine.UI.InputField PortInputField = null;

        [SerializeField]
        public UnityEngine.UI.InputField AppNameInputField = null;

        [SerializeField]
        public UnityEngine.UI.Button RestartButton = null;

        [SerializeField]
        public UnityEngine.UI.Toggle CustomInputToggle = null;

        public AltInstrumentationSettings InstrumentationSettings { get { return AltRunner._altRunner.InstrumentationSettings; } }

        private RuntimeCommunicationHandler communicationClient;
        private LiveUpdateCommunicationHandler liveUpdateClient;
        private readonly AltResponseQueue updateQueue = new AltResponseQueue();
        HashSet<string> connectedDrivers = new HashSet<string>();

        private bool isDataValid = false;
        private bool wasConnected = false;
        private float timeSinceLastScreenshotWasSent;
        private string appId, platform, platformVersion, deviceInstanceId, currentHost, currentName, currentPort;//Connection parameters and tags

        private bool stopClientsCalled = false;
        private bool beginCommunicationCalled = false;
        private bool isEditing = false;
        private bool isCommunicationConnected;
        private bool isLiveUpdateConnected;
        private bool isDriverConnected;


        private UnityEngine.UI.Image dialogImage;
        protected void Awake()
        {
            dialogImage = Dialog.GetComponent<UnityEngine.UI.Image>();
        }
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

            this.platform = Application.platform.ToString();
            this.platformVersion = SystemInfo.operatingSystem;
            this.deviceInstanceId = SystemInfo.deviceUniqueIdentifier;
            validateFields();
            onStart();

            //Connection
            if (isDataValid)
                beginCommunication();
            InvokeRepeating("CheckAlive", 5, 5);//TODO 




        }
        protected void CheckAlive()// This method is just to see if sending a ping will keep client from disconnecting .
        {
            if (liveUpdateClient != null)
            {
                var test = liveUpdateClient.IsConnected;
            }
            if (communicationClient != null)
            {
                var test = communicationClient.IsConnected;
            }
        }

        protected void Update()
        {
            updateQueue.Cycle();
            checkIfPlayerPrefNeedsToBeDeleted();
            setInteractibilityForRestartButton(isEditing);
            if (this.liveUpdateClient == null || !this.liveUpdateClient.IsRunning || !this.liveUpdateClient.IsConnected)
                return;

            timeSinceLastScreenshotWasSent += Time.unscaledDeltaTime;
            if (timeSinceLastScreenshotWasSent > 1.0f / this.liveUpdateClient.FrameRate)
            {
                timeSinceLastScreenshotWasSent = 0.0f;
                StartCoroutine(this.SendScreenshot());
            }

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
            liveUpdateClient = new LiveUpdateCommunicationHandler(currentHost, int.Parse(currentPort), currentName, platform, platformVersion, deviceInstanceId, appId);
            liveUpdateClient.OnDisconnect += onDisconnect;
            liveUpdateClient.OnError += onError;
            liveUpdateClient.OnConnect += onLiveUpdateConnected;
            liveUpdateClient.Init();

        }

        private void beginCommunication()
        {
            if (beginCommunicationCalled)
            {
                return;
            }
            beginCommunicationCalled = true;

            ToggleCustomInput(false);
            initCommunicationClient();
            startClient(communicationClient);

            beginCommunicationCalled = false;
        }
        private void beginLiveUpdate()
        {
            initLiveUpdateClient();
            startClient(liveUpdateClient);
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
                yield return new WaitForEndOfFrame();
            this.liveUpdateClient.SendScreenshot();
        }

        protected void OnApplicationQuit()
        {
            isEditing = true;//I set it true here to stop starting the communication in stopClients()
            stopClients();
        }


        private void setMessage(string message, Color color, bool visible = true)
        {
            Dialog.SetActive(visible);
            dialogImage.color = color;
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
            onValueChanged();
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
            currentHost = PlayerPrefs.GetString(HOST, InstrumentationSettings.AltServerHost);
            currentPort = PlayerPrefs.GetString(PORT, InstrumentationSettings.AltServerPort.ToString());
            currentName = PlayerPrefs.GetString(APP_NAME, InstrumentationSettings.AppName);
            HostInputField.text = currentHost;
            PortInputField.text = currentPort;
            AppNameInputField.text = currentName;
        }
        private void setUpHostInputField()
        {
            currentHost = PlayerPrefs.GetString(HOST, InstrumentationSettings.AltServerHost);
            HostInputField.text = currentHost;
            HostInputField.onValueChanged.AddListener(onValueChanged);
        }

        private void setUpPortInputField()
        {
            currentPort = PlayerPrefs.GetString(PORT, InstrumentationSettings.AltServerPort.ToString());
            PortInputField.text = currentPort;
            PortInputField.onValueChanged.AddListener(onPortInputFieldValueChange);
            PortInputField.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Integer;
        }

        private void setUpAppNameInputField()
        {
            currentName = PlayerPrefs.GetString(APP_NAME, InstrumentationSettings.AppName);
            AppNameInputField.text = currentName;
            AppNameInputField.onValueChanged.AddListener(onValueChanged);
        }

        private void onValueChanged(string _ = "")
        {
            setMessage(EDITING_TEXT, color: SUCCESS_COLOR, visible: Dialog.activeSelf);
            isEditing = true;
        }

        private void onRestartButtonPress()
        {
            appId = null;

            responseCode = 0;
            validateFields();
            if (isDataValid)
                isEditing = false;
            stopClients();

        }
        private void validateFields()
        {
            isDataValid = false;

            if (Uri.CheckHostName(HostInputField.text) != UriHostNameType.Unknown)
            {
                currentHost = HostInputField.text;
                InstrumentationSettings.AltServerHost = currentHost;
            }
            else
            {
                setMessage("The host should be a valid host.", color: ERROR_COLOR, visible: true);
                return;
            }

            int port;
            if (Int32.TryParse(PortInputField.text, out port) && port > 0 && port <= 65535)
            {
                currentPort = port.ToString();
                InstrumentationSettings.AltServerPort = port;
            }
            else
            {
                setMessage("The port number should be between 1 and 65535.", color: ERROR_COLOR, visible: true);
                return;
            }

            if (!string.IsNullOrEmpty(AppNameInputField.text))
            {
                currentName = AppNameInputField.text;
                InstrumentationSettings.AppName = currentName;
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
            Icon.color = value ? Color.white : Color.grey;

#if ALTTESTER
#if ENABLE_LEGACY_INPUT_MANAGER
            Input.UseCustomInput = value;
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

        private void initCommunicationClient()
        {
            communicationClient = new RuntimeCommunicationHandler(currentHost, int.Parse(currentPort), currentName, platform, platformVersion, deviceInstanceId, appId == null ? "unknown" : appId);
            communicationClient.OnConnect += onCommunicationConnected;
            communicationClient.OnDisconnect += onDisconnect;
            communicationClient.OnError += onError;

            communicationClient.CmdHandler.OnDriverConnect += onDriverConnect;
            communicationClient.CmdHandler.OnDriverDisconnect += onDriverDisconnect;
            communicationClient.CmdHandler.OnAppConnect += onAppConnect;
            communicationClient.Init();

        }
        private void setInteractibilityForRestartButton(bool isInteractable)
        {
            RestartButton.interactable = isInteractable;
        }

        private void startClient(BaseCommunicationHandler communicationHandler)
        {
            try
            {
                communicationHandler.waitingToConnect = true;
                communicationHandler.Connect();
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError(e.Message);
                stopClient(communicationHandler);
                communicationHandler.waitingToConnect = false;
                if (communicationHandler.GetType().Equals(typeof(RuntimeCommunicationHandler)))
                {
                    initCommunicationClient();
                }
                else
                {
                    initLiveUpdateClient();
                }

            }
            catch (Exception ex)
            {

                setMessage("An unexpected error occurred while starting the AltTester® client.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occurred while starting the AltTester® client.");
                stopClient(communicationHandler);
                communicationHandler.waitingToConnect = false;
            }
        }

        private void stopClients()
        {
            if (stopClientsCalled)//Stop clients was already called
                return;
            stopClientsCalled = true;
            try
            {
                connectedDrivers.Clear();
                if (isCommunicationConnected)
                {
                    stopCommunicationClient();
                    isCommunicationConnected = false;
                }
                if (isLiveUpdateConnected)
                {
                    stopLiveUpdateClient();
                    isLiveUpdateConnected = false;
                }

                wasConnected = false;
                if (responseCode > 4000 && responseCode < 5000)
                {
                    isEditing = true;
                    stopClientsCalled = false;
                    return;
                }
                if (!isEditing && isDataValid)//If is not editing the input field try reconnecting
                {
                    updateQueue.Clear();
                    onStart();
                    beginCommunication();
                }
            }
            catch (Exception e)
            {
                updateQueue.ScheduleResponse(() => Debug.LogError(e));
            }
            isDriverConnected = false;
            stopClientsCalled = false;

        }
        private void stopCommunicationClient()
        {
            stopClient(communicationClient);
            communicationClient = null;

        }
        private void stopLiveUpdateClient()
        {
            stopClient(liveUpdateClient);
            liveUpdateClient = null;

        }

        private static void stopClient(BaseCommunicationHandler communicationHandler)
        {
            if (communicationHandler == null)
            {
                return;
            }
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
            if (code >= 4000 && code < 5000)
                updateQueue.ScheduleResponse(() => setMessage(reason, ERROR_COLOR, true));
            updateQueue.ScheduleResponse(() => stopClients());
        }

        private void onStart()
        {
            string message = $"Waiting to connect to AltTester® Server on {Environment.NewLine}host:port {currentHost}:{currentPort}with appName: '{currentName}',{Environment.NewLine}platform: '{platform}',{Environment.NewLine}platformVersion: '{platformVersion}',{Environment.NewLine}deviceInstanceId: '{deviceInstanceId}' {Environment.NewLine}and appId '{appId}'.";
            setMessage(message, color: SUCCESS_COLOR, visible: Dialog.activeSelf);
        }
        private void onCommunicationConnected()
        {
            isCommunicationConnected = true;
        }
        private void onLiveUpdateConnected()
        {

            isLiveUpdateConnected = true;
            updateQueue.ScheduleResponse(() => onConnect());
        }

        private void onConnect()
        {

            wasConnected = true;

            if (!isDriverConnected)
            {

                string message = $"Connected to AltTester® Server on {Environment.NewLine}host:port {currentHost}:{currentPort}{Environment.NewLine}with appName: '{currentName}'{Environment.NewLine}platform: '{platform}'{Environment.NewLine}platformVersion: '{platformVersion}'{Environment.NewLine}deviceInstanceId: '{deviceInstanceId}' {Environment.NewLine}appId '{appId}'.{Environment.NewLine}Waiting for Driver to connect.";
                setMessage(message, color: SUCCESS_COLOR, visible: true);
            }

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
            isDriverConnected = true;
            string message = String.Format("Connected to AltTester® Server on {0}host:port {1}:{2}with appName: '{3}',{4}platform: '{5}',{6}platformVersion: '{7}',{8}deviceInstanceId: '{9}' {10}and appId '{11}'.{12}Driver connected.", Environment.NewLine, currentHost, currentPort + Environment.NewLine, currentName, Environment.NewLine, this.platform, Environment.NewLine, this.platformVersion, Environment.NewLine, this.deviceInstanceId, Environment.NewLine, appId, Environment.NewLine);

            connectedDrivers.Add(driverId);

            if (connectedDrivers.Count == 1)
            {
                updateQueue.ScheduleResponse(() =>
                {
                    PlayerPrefs.SetString(HOST, currentHost);
                    PlayerPrefs.SetString(PORT, currentPort);
                    PlayerPrefs.SetString(APP_NAME, currentName);
                    ToggleCustomInput(true);
                    setMessage(message, color: SUCCESS_COLOR, visible: false);
                });
            }
        }
        private void onAppConnect(string appId)
        {
            this.appId = appId;
            updateQueue.ScheduleResponse(() => beginLiveUpdate());
        }


        private void onDriverDisconnect(string driverId)
        {
            string message = String.Format("Connected to AltTester® Server on {0}host:port {1}:{2}with appName: '{3}',{4}platform: '{5}',{6}platformVersion: '{7}',{8}deviceInstanceId: '{9}' {10}and appId '{11}'.{12}Waiting for Driver to connect.", Environment.NewLine, currentHost, currentPort + Environment.NewLine, currentName, Environment.NewLine, this.platform, Environment.NewLine, this.platformVersion, Environment.NewLine, this.deviceInstanceId, Environment.NewLine, appId, Environment.NewLine);

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
