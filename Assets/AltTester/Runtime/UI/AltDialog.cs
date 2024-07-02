/*
    Copyright(C) 2024 Altom Consulting

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
using AltTester.AltTesterUnitySDK.Commands;
using AltTester.AltTesterUnitySDK.Communication;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.InputModule;
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

        private static readonly Color primarySuccessColor = new Color32(0, 165, 36, 255);
        private static readonly Color secondarySuccessColor = new Color32(0, 115, 25, 255);
        private static readonly Color primaryErrorColor = new Color32(191, 71, 85, 255);
        private static readonly Color secondaryErrorColor = new Color32(136, 47, 58, 255);

        private static readonly Tuple<Color, Color> successColor = new Tuple<Color, Color>(primarySuccessColor, secondarySuccessColor);
        private static readonly Tuple<Color, Color> errorColor = new Tuple<Color, Color>(primaryErrorColor, secondaryErrorColor);
        private static readonly Color warningColor = new Color32(255, 255, 95, 255);

        private const string HOST = "AltTesterHost";
        private const string PORT = "AltTesterPort";
        private const string APP_NAME = "AltTesterAppName";
        private const string UID = "UID";

        private int responseCode = 0;

        [SerializeField]
        public GameObject Dialog = null;

        [SerializeField]
        public UnityEngine.UI.Text TitleText = null;

        [SerializeField]
        public UnityEngine.UI.Text SubtitleText = null;

        [SerializeField]
        public GameObject InfoArea = null;

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
        int connectedDrivers = 0;

        private bool isDataValid = false;
        private bool wasConnected = false;
        private float timeSinceLastScreenshotWasSent;
        private string appId, platform, platformVersion, deviceInstanceId, currentHost, currentName, currentPort; //Connection parameters and tags

        private bool stopClientsCalled = false;
        private bool beginCommunicationCalled = false;
        private bool isEditing = false;
        private bool isError = false;
        private bool isCommunicationConnected;
        private bool isLiveUpdateConnected;
        private bool isDriverConnected;

        private UnityEngine.UI.Image dialogImage;
        private UnityEngine.UI.Image infoArea;
        private UnityEngine.UI.Image restartButton;

        protected void Awake()
        {
            dialogImage = Dialog.GetComponent<UnityEngine.UI.Image>();
            infoArea = InfoArea.GetComponent<UnityEngine.UI.Image>();
            restartButton = RestartButton.GetComponent<UnityEngine.UI.Image>();
        }

        protected void Start()
        {
            resetConnectionDataBasedOnUID();

            setTitle("AltTester® v." + AltRunner.VERSION);
            setSubtitle("AltTester® Server");
            setUpCloseButton();
            setUpIcon();
            setUpAppNameInputField();
            setUpHostInputField();
            setUpPortInputField();

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
            InvokeRepeating(nameof(CheckAlive), 5, 5);
        }

        protected void CheckAlive() // This method is just to see if sending a ping will keep client from disconnecting .
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
            setIntractabilityForRestartButton(isEditing);
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
            if (InputMisc.IsResetConnectionShortcutPressed())
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
            if (Application.isBatchMode)
                yield return null;
            else
                yield return new WaitForEndOfFrame();
            this.liveUpdateClient.SendScreenshot();
        }

        protected void OnApplicationQuit()
        {
            isEditing = true; //I set it true here to stop starting the communication in stopClients()
            stopClients();
        }

        private void setMessage(string message, Tuple<Color, Color> color, bool visible = true)
        {
            var primaryColor = color.Item1;
            var secondaryColor = color.Item2;

            Dialog.SetActive(visible);
            dialogImage.color = primaryColor;
            restartButton.color = secondaryColor;
            infoArea.color = secondaryColor;
            MessageText.text = message;
        }

        private void setTitle(string title) => TitleText.text = title;

        private void setSubtitle(string subtitle) => SubtitleText.text = subtitle;

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
            isEditing = true;
            string message = createMessage();
            setMessage(message, color: successColor, visible: Dialog.activeSelf);
        }

        private void onRestartButtonPress()
        {
            appId = null;

            responseCode = 0;
            isError = false;
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
                setMessage("The host should be a valid host.", color: errorColor, visible: true);
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
                setMessage("The port number should be between 1 and 65535.", color: errorColor, visible: true);
                return;
            }

            if (!string.IsNullOrEmpty(AppNameInputField.text))
            {
                currentName = AppNameInputField.text;
                InstrumentationSettings.AppName = currentName;
            }
            else
            {
                setMessage("App name should not be empty.", color: errorColor, visible: true);
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
            InputMisc.ActivateCustomInput(value);
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

        private void setIntractabilityForRestartButton(bool isInteractable)
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

                setMessage("An unexpected error occurred while starting the AltTester(R) client.", color: errorColor, true);
                logger.Error(ex, "An unexpected error occurred while starting the AltTester(R) client.");
                stopClient(communicationHandler);
                communicationHandler.waitingToConnect = false;
            }
        }

        private void stopClients()
        {
            if (stopClientsCalled) // Stop clients was already called
                return;

            stopClientsCalled = true;
            try
            {
                connectedDrivers = 0;
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

                if (!isEditing && isDataValid) //If is not editing the input field try reconnecting
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
            {
                isError = true;
                updateQueue.ScheduleResponse(() => setMessage(reason, errorColor, true));
            }
            updateQueue.ScheduleResponse(() => stopClients());
        }

        private void onStart()
        {
            string message = createMessage();
            setMessage(message, color: successColor, visible: Dialog.activeSelf);
        }

        private string createMessage()
        {
            if (isEditing)
            {
                var aux = $"Editing app name, host or port.";
                return aux + $"{Environment.NewLine}Press the <b>Restart</b> button to start connection with the new values.";
            }

            string message = wasConnected ? "Connected to " : "Waiting to connect to ";
            message += $"<b>AltTester® Server</b> on <b>{currentHost}:{currentPort}</b> with: {Environment.NewLine}";

            message += $"{Environment.NewLine}<b>App Name</b>{Environment.NewLine}{currentName}" +
                       $"{Environment.NewLine}<b>Platform</b>{Environment.NewLine}{platform}" +
                       $"{Environment.NewLine}<b>Platform Version</b>{Environment.NewLine}{platformVersion}" +
                       $"{Environment.NewLine}<b>Device Instance ID</b>{Environment.NewLine}{deviceInstanceId}" +
                       $"{Environment.NewLine}<b>App ID</b>{Environment.NewLine}{(string.IsNullOrEmpty(appId) ? "unknown" : appId)}";

            if (wasConnected)
                message += isDriverConnected ? $"{Environment.NewLine}{Environment.NewLine}Driver connected." : $"{Environment.NewLine}{Environment.NewLine}Waiting for Driver to connect.";

            return message;
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
            if (!isDriverConnected && !isError)
            {
                string message = createMessage();
                setMessage(message, color: successColor, visible: true);
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
            string message = createMessage();

            connectedDrivers++;

            if (connectedDrivers == 1)
            {
                updateQueue.ScheduleResponse(() =>
                {
                    PlayerPrefs.SetString(HOST, currentHost);
                    PlayerPrefs.SetString(PORT, currentPort);
                    PlayerPrefs.SetString(APP_NAME, currentName);
                    ToggleCustomInput(true);
                    setMessage(message, color: successColor, visible: false);
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
            connectedDrivers--;
            if (connectedDrivers == 0)
            {
                isDriverConnected = false;
                string message = createMessage();

                updateQueue.ScheduleResponse(() =>
                {
                    ToggleCustomInput(false);
                    setMessage(message, color: successColor, visible: true);
                });
            }
        }
    }
}
