using System;
using Altom.AltUnityTester;
using Altom.AltUnityTester.Logging;
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
        private bool wasConnectedBefore = false;

        protected void Start()
        {
            Dialog.SetActive(InstrumentationSettings.ShowPopUp);
            CloseButton.onClick.AddListener(ToggleDialog);
            Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ToggleDialog);
            TitleText.text = "AltUnity Tester v." + AltUnityRunner.VERSION;

            startCommProtocol();
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

        private void initCommProtocol()
        {
            var cmdHandler = new CommandHandler();


#if UNITY_WEBGL && !UNITY_EDITOR
            communication = new WebSocketWebGLCommunication(cmdHandler, InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort);
#else

            communication = new WebSocketClientCommunication(cmdHandler, InstrumentationSettings.ProxyHost, InstrumentationSettings.ProxyPort);
#endif
            communication.OnConnect += onConnect;
            communication.OnDisconnect += onDisconnect;
            communication.OnError += onError;

        }
        private void startCommProtocol()
        {
            initCommProtocol();

            try
            {
                if (communication == null || !communication.IsListening) // start only if it is not already listening
                    communication.Start();

                if (!communication.IsConnected) // display dialog onlyy if not connected 
                    onStart();
            }

            catch (UnhandledStartCommError ex)
            {
                setDialog("An unexpected error occured while starting the communication protocol.", ERROR_COLOR, true);
                logger.Error(ex.InnerException, "An unexpected error occured while starting the communication protocol.");
            }
            catch (Exception ex)
            {
                setDialog("An unexpected error occured while starting the communication protocol.", ERROR_COLOR, true);
                logger.Error(ex, "An unexpected error occured while starting the communication protocol.");
            }
        }
        private void onStart()
        {
            setDialog("Connecting to AltUnity Proxy on " + InstrumentationSettings.ProxyHost + ":" + InstrumentationSettings.ProxyPort, SUCCESS_COLOR, Dialog.activeSelf || wasConnectedBefore);
            wasConnectedBefore = false;
        }
        private void onConnect()
        {

            string message = "Connected to AltUnity Proxy on " + InstrumentationSettings.ProxyHost + ":" + InstrumentationSettings.ProxyPort;
            _updateQueue.ScheduleResponse(() =>
            {
                setDialog(message, SUCCESS_COLOR, false);
                wasConnectedBefore = true;
            });
        }

        private void onDisconnect()
        {
            _updateQueue.ScheduleResponse(startCommProtocol);
        }

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