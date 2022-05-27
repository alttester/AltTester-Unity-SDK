using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Commands;
using Altom.AltUnityTester.Logging;
using Newtonsoft.Json;

namespace Altom.AltUnityTester.Communication
{
    public class CommandHandler : ICommandHandler
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

        public CommandHandler()
        {
        }

        public SendMessageHandler OnSendMessage { get; set; }

        public void Send(string data)
        {
            if (this.OnSendMessage != null)
            {
                this.OnSendMessage.Invoke(data);
                logger.Debug("response sent: " + trimLog(data));
            }
        }

        public void OnMessage(string data)
        {
            logger.Debug("command received: " + trimLog(data));

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture
            };
            Func<string> executeAndSerialize = null;
            CommandParams cmdParams = null;

            try
            {
                cmdParams = JsonConvert.DeserializeObject<CommandParams>(data, jsonSerializerSettings);
                var type = getCommandType((string)cmdParams.commandName);
                var commandParams = JsonConvert.DeserializeObject(data, type, jsonSerializerSettings) as CommandParams;
                executeAndSerialize = createCommand(commandParams);
            }
            catch (JsonException ex)
            {
                executeAndSerialize = new AltUnityInvalidCommand(cmdParams, ex).ExecuteAndSerialize;
            }
            catch (CommandNotFoundException ex)
            {
                executeAndSerialize = new AltUnityInvalidCommand(cmdParams, ex).ExecuteAndSerialize;
            }

            AltUnityRunner._responseQueue.ScheduleResponse(delegate
                {

                    var response = executeAndSerialize();
                    //TODO: remove this
                    if (!cmdParams.commandName.Equals("endTouch")) //Temporary solution to ignore first "Ok"
                    {
                        //Do not remove the send only the if
                        this.Send(response);
                    }
                });
        }

        private string trimLog(string log, int maxLogLength = 1000)
        {
            if (string.IsNullOrEmpty(log)) return log;
            if (log.Length <= maxLogLength) return log;
            return log.Substring(0, maxLogLength) + "[...]";
        }

        private Func<string> createCommand(CommandParams cmdParams)
        {
            if (cmdParams is AltUnityGetServerVersionParams)
            {
                return new AltUnityGetServerVersionCommand((AltUnityGetServerVersionParams)cmdParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityTapElementParams)
            {
                return new AltUnityTapElementCommand(this, cmdParams as AltUnityTapElementParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityClickElementParams)
            {
                return new AltUnityClickElementCommand(this, cmdParams as AltUnityClickElementParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityTapCoordinatesParams)
            {
                return new AltUnityTapCoordinatesCommand(this, cmdParams as AltUnityTapCoordinatesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityClickCoordinatesParams)
            {
                return new AltUnityClickCoordinatesCommand(this, cmdParams as AltUnityClickCoordinatesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityKeyDownParams)
            {
                return new AltUnityKeyDownCommand(cmdParams as AltUnityKeyDownParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityKeysDownParams)
            {
                return new AltUnityKeysDownCommand(cmdParams as AltUnityKeysDownParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityKeyUpParams)
            {
                return new AltUnityKeyUpCommand(cmdParams as AltUnityKeyUpParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityKeysUpParams)
            {
                return new AltUnityKeysUpCommand(cmdParams as AltUnityKeysUpParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityBeginTouchParams)
            {
                return new AltUnityBeginTouchCommand(cmdParams as AltUnityBeginTouchParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityMoveTouchParams)
            {
                return new AltUnityMoveTouchCommand(cmdParams as AltUnityMoveTouchParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityEndTouchParams)
            {
                return new AltUnityEndTouchCommand(this, cmdParams as AltUnityEndTouchParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetCurrentSceneParams)
            {
                return new AltUnityGetCurrentSceneCommand((AltUnityGetCurrentSceneParams)cmdParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetObjectComponentPropertyParams)
            {
                return new AltUnityGetComponentPropertyCommand(cmdParams as AltUnityGetObjectComponentPropertyParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnitySetObjectComponentPropertyParams)
            {
                return new AltUnitySetComponentPropertyCommand(cmdParams as AltUnitySetObjectComponentPropertyParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityCallComponentMethodForObjectParams)
            {
                return new AltUnityCallComponentMethodForObjectCommand(cmdParams as AltUnityCallComponentMethodForObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityDragObjectParams)
            {
                return new AltUnityDragObjectCommand(cmdParams as AltUnityDragObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityPointerUpFromObjectParams)
            {
                return new AltUnityPointerUpFromObjectCommand(cmdParams as AltUnityPointerUpFromObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityPointerDownFromObjectParams)
            {
                return new AltUnityPointerDownFromObjectCommand(cmdParams as AltUnityPointerDownFromObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityPointerEnterObjectParams)
            {
                return new AltUnityPointerEnterObjectCommand(cmdParams as AltUnityPointerEnterObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityPointerExitObjectParams)
            {
                return new AltUnityPointerExitObjectCommand(cmdParams as AltUnityPointerExitObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityTiltParams)
            {
                return new AltUnityTiltCommand(this, cmdParams as AltUnityTiltParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnitySwipeParams)
            {
                return new AltUnitySwipeCommand(this, cmdParams as AltUnitySwipeParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityMultipointSwipeParams)
            {
                return new AltUnityMultipointSwipeCommand(this, cmdParams as AltUnityMultipointSwipeParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityLoadSceneParams)
            {
                return new AltUnityLoadSceneCommand(this, (AltUnityLoadSceneParams)cmdParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityUnloadSceneParams)
            {
                return new AltUnityUnloadSceneCommand(this, cmdParams as AltUnityUnloadSceneParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnitySetTimeScaleParams)
            {
                return new AltUnitySetTimeScaleCommand(cmdParams as AltUnitySetTimeScaleParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetTimeScaleParams)
            {
                return new AltUnityGetTimeScaleCommand(cmdParams as AltUnityGetTimeScaleParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityDeletePlayerPrefParams)
            {
                return new AltUnityDeletePlayerPrefCommand(cmdParams as AltUnityDeletePlayerPrefParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityDeleteKeyPlayerPrefParams)
            {
                return new AltUnityDeleteKeyPlayerPrefCommand(cmdParams as AltUnityDeleteKeyPlayerPrefParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnitySetKeyPlayerPrefParams)
            {
                return new AltUnitySetKeyPlayerPrefCommand(cmdParams as AltUnitySetKeyPlayerPrefParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetKeyPlayerPrefParams)
            {
                var getKeyPlayerPrefParams = cmdParams as AltUnityGetKeyPlayerPrefParams;
                switch (getKeyPlayerPrefParams.keyType)
                {
                    case PlayerPrefKeyType.Int: return new AltUnityGetIntKeyPlayerPrefCommand(cmdParams as AltUnityGetKeyPlayerPrefParams).ExecuteAndSerialize;
                    case PlayerPrefKeyType.String: return new AltUnityGetStringKeyPlayerPrefCommand(cmdParams as AltUnityGetKeyPlayerPrefParams).ExecuteAndSerialize;
                    case PlayerPrefKeyType.Float: return new AltUnityGetFloatKeyPlayerPrefCommand(cmdParams as AltUnityGetKeyPlayerPrefParams).ExecuteAndSerialize;
                    default:
                        return new AltUnityInvalidCommand(cmdParams, new InvalidParameterTypeException(string.Format("PlayerPrefKeyType {0} not handled", getKeyPlayerPrefParams.keyType))).ExecuteAndSerialize;
                }
            }

            if (cmdParams is AltUnityGetAllComponentsParams)
            {
                return new AltUnityGetAllComponentsCommand(cmdParams as AltUnityGetAllComponentsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllFieldsParams)
            {
                return new AltUnityGetAllFieldsCommand(cmdParams as AltUnityGetAllFieldsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllPropertiesParams)
            {
                return new AltUnityGetAllPropertiesCommand(cmdParams as AltUnityGetAllPropertiesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllMethodsParams)
            {
                return new AltUnityGetAllMethodsCommand(cmdParams as AltUnityGetAllMethodsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllScenesParams)
            {
                return new AltUnityGetAllScenesCommand(cmdParams as AltUnityGetAllScenesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllCamerasParams)
            {
                return new AltUnityGetAllCamerasCommand(cmdParams as AltUnityGetAllCamerasParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllActiveCamerasParams)
            {
                return new AltUnityGetAllCamerasCommand(cmdParams as AltUnityGetAllActiveCamerasParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllLoadedScenesParams)
            {
                return new AltUnityGetAllLoadedScenesCommand(cmdParams as AltUnityGetAllLoadedScenesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetAllLoadedScenesAndObjectsParams)
            {
                return new AltUnityGetAllLoadedScenesAndObjectsCommand(cmdParams as AltUnityGetAllLoadedScenesAndObjectsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetScreenshotParams)
            {
                return new AltUnityGetScreenshotCommand(this, cmdParams as AltUnityGetScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityHightlightObjectScreenshotParams)
            {
                return new AltUnityHighlightSelectedObjectCommand(this, cmdParams as AltUnityHightlightObjectScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityHightlightObjectFromCoordinatesScreenshotParams)
            {
                return new AltUnityHightlightObjectFromCoordinatesCommand(this, cmdParams as AltUnityHightlightObjectFromCoordinatesScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityPressKeyboardKeyParams)
            {
                return new AltUnityPressKeyboardKeyCommand(this, cmdParams as AltUnityPressKeyboardKeyParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityPressKeyboardKeysParams)
            {
                return new AltUnityPressKeyboardKeysCommand(this, cmdParams as AltUnityPressKeyboardKeysParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityMoveMouseParams)
            {
                return new AltUnityMoveMouseCommand(this, cmdParams as AltUnityMoveMouseParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityScrollParams)
            {
                return new AltUnityScrollCommand(this, cmdParams as AltUnityScrollParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityFindObjectParams)
            {
                return new AltUnityFindObjectCommand(cmdParams as AltUnityFindObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityFindObjectsParams)
            {
                return new AltUnityFindObjectsCommand(cmdParams as AltUnityFindObjectsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityFindObjectsLightParams)
            {
                return new AltUnityFindObjectsLightCommand(cmdParams as AltUnityFindObjectsLightParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetTextParams)
            {
                return new AltUnityGetTextCommand(cmdParams as AltUnityGetTextParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnitySetTextParams)
            {
                return new AltUnitySetTextCommand(cmdParams as AltUnitySetTextParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetPNGScreenshotParams)
            {
                return new AltUnityGetScreenshotPNGCommand(this, cmdParams as AltUnityGetPNGScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityGetServerVersionParams)
            {
                return new AltUnityGetServerVersionCommand(cmdParams as AltUnityGetServerVersionParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnitySetServerLoggingParams)
            {
                return new AltUnitySetServerLoggingCommand(cmdParams as AltUnitySetServerLoggingParams).ExecuteAndSerialize;
            }
            if (cmdParams is ActivateNotification)
            {
                return new AltUnityActivateNotificationCommand(this, cmdParams as ActivateNotification).ExecuteAndSerialize;
            }
            if (cmdParams is DeactivateNotification)
            {
                return new AltUnityDeactivateNotificationCommand(this, cmdParams as DeactivateNotification).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnityFindObjectAtCoordinatesParams)
            {
                return new AltUnityFindObjectAtCoordinatesCommand(cmdParams as AltUnityFindObjectAtCoordinatesParams).ExecuteAndSerialize;
            }

            return new AltUnityInvalidCommand(cmdParams, new CommandNotFoundException(string.Format("Command {0} not handled", cmdParams.commandName))).ExecuteAndSerialize;
        }

        private Type getCommandType(string commandName)
        {
            var assembly = Assembly.GetAssembly(typeof(CommandParams));

            var derivedType = typeof(CommandParams);
            var type = assembly.GetTypes().FirstOrDefault(t =>
               {
                   if (derivedType.IsAssignableFrom(t)) // if type derrives from CommandParams
                   {
                       CommandAttribute cmdAttribute = (CommandAttribute)Attribute.GetCustomAttribute(t, typeof(CommandAttribute));
                       return cmdAttribute != null && cmdAttribute.Name == commandName;
                   }
                   return false;
               });

            if (type == null) { throw new CommandNotFoundException(string.Format("Command `{0}` not found", commandName)); }
            return type;
        }
    }
}
