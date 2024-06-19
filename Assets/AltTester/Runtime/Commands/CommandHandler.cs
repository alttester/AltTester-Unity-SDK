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
using System.Globalization;
using System.Linq;
using System.Reflection;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Driver.Communication;
using AltTester.AltTesterUnitySDK.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private static readonly NLog.Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();
        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            Culture = CultureInfo.InvariantCulture,
            Formatting = Formatting.Indented
        };

        public SendMessageHandler OnSendMessage { get; set; }

        public NotificationHandler OnDriverConnect { get; set; }
        public NotificationHandler OnDriverDisconnect { get; set; }
        public NotificationHandler OnAppConnect { get; set; }


        public CommandHandler()
        {
        }

        public void Send(string data)
        {
            if (this.OnSendMessage != null)
            {
                this.OnSendMessage.Invoke(data);
                logger.Debug(string.Format("response sent: {0}", Utils.TrimLog(data)));
            }
        }

        public void OnMessage(string data)
        {
            logger.Debug(string.Format("command received: {0}", Utils.TrimLog(data)));

            Func<string> executeAndSerialize = null;
            CommandParams cmdParams = null;
            try
            {
                cmdParams = JsonConvert.DeserializeObject<CommandParams>(data, jsonSerializerSettings);
                if (cmdParams.isNotification)
                {
                    handleNotifications(cmdParams);
                    return;
                }
                if (cmdParams.commandName == "AppId")
                {
                    handleAppId(cmdParams);
                    return;
                }

                var type = getCommandType((string)cmdParams.commandName);
                var commandParams = JsonConvert.DeserializeObject(data, type, jsonSerializerSettings) as CommandParams;
                executeAndSerialize = createCommand(commandParams);
            }
            catch (JsonException ex)
            {
                executeAndSerialize = new AltInvalidCommand(cmdParams, ex).ExecuteAndSerialize;
            }
            catch (CommandNotFoundException ex)
            {
                executeAndSerialize = new AltInvalidCommand(cmdParams, ex).ExecuteAndSerialize;
            }

            AltRunner._responseQueue.ScheduleResponse(delegate
                {
                    var response = executeAndSerialize();

                    // TODO: Remove this if statement
                    if (!cmdParams.commandName.Equals("endTouch")) // Temporary solution to ignore first "Ok"
                    {
                        // Do not remove the send only the if statement
                        this.Send(response);
                    }
                });
        }
        public void handleAppId(CommandParams cmdParams)
        {
            if (cmdParams.commandName == "AppId")
            {
                if (this.OnAppConnect != null)
                {
                    this.OnAppConnect.Invoke(cmdParams.driverId);
                }
            }
        }

        public void handleNotifications(CommandParams cmdParams)
        {
            if (cmdParams.commandName == "DriverConnectedNotification")
            {
                if (this.OnDriverConnect != null)
                {
                    this.OnDriverConnect.Invoke(cmdParams.driverId);
                }
            }
            else if (cmdParams.commandName == "DriverDisconnectedNotification")
            {
                if (this.OnDriverDisconnect != null)
                {
                    this.OnDriverDisconnect.Invoke(cmdParams.driverId);
                }
            }
        }

        private Func<string> createCommand(CommandParams cmdParams)
        {
            if (cmdParams is AltGetServerVersionParams)
            {
                return new AltGetServerVersionCommand((AltGetServerVersionParams)cmdParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltTapElementParams)
            {
                return new AltTapElementCommand(this, cmdParams as AltTapElementParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltClickElementParams)
            {
                return new AltClickElementCommand(this, cmdParams as AltClickElementParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltTapCoordinatesParams)
            {
                return new AltTapCoordinatesCommand(this, cmdParams as AltTapCoordinatesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltClickCoordinatesParams)
            {
                return new AltClickCoordinatesCommand(this, cmdParams as AltClickCoordinatesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltKeysDownParams)
            {
                return new AltKeysDownCommand(cmdParams as AltKeysDownParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltKeysUpParams)
            {
                return new AltKeysUpCommand(cmdParams as AltKeysUpParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltBeginTouchParams)
            {
                return new AltBeginTouchCommand(cmdParams as AltBeginTouchParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltMoveTouchParams)
            {
                return new AltMoveTouchCommand(cmdParams as AltMoveTouchParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltEndTouchParams)
            {
                return new AltEndTouchCommand(this, cmdParams as AltEndTouchParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetCurrentSceneParams)
            {
                return new AltGetCurrentSceneCommand((AltGetCurrentSceneParams)cmdParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetObjectComponentPropertyParams)
            {
                return new AltGetComponentPropertyCommand(cmdParams as AltGetObjectComponentPropertyParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltSetObjectComponentPropertyParams)
            {
                return new AltSetComponentPropertyCommand(cmdParams as AltSetObjectComponentPropertyParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltCallComponentMethodForObjectParams)
            {
                return new AltCallComponentMethodForObjectCommand(cmdParams as AltCallComponentMethodForObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltDragObjectParams)
            {
                return new AltDragObjectCommand(cmdParams as AltDragObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltPointerUpFromObjectParams)
            {
                return new AltPointerUpFromObjectCommand(cmdParams as AltPointerUpFromObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltPointerDownFromObjectParams)
            {
                return new AltPointerDownFromObjectCommand(cmdParams as AltPointerDownFromObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltPointerEnterObjectParams)
            {
                return new AltPointerEnterObjectCommand(cmdParams as AltPointerEnterObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltPointerExitObjectParams)
            {
                return new AltPointerExitObjectCommand(cmdParams as AltPointerExitObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltTiltParams)
            {
                return new AltTiltCommand(this, cmdParams as AltTiltParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltSwipeParams)
            {
                return new AltSwipeCommand(this, cmdParams as AltSwipeParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltMultipointSwipeParams)
            {
                return new AltMultipointSwipeCommand(this, cmdParams as AltMultipointSwipeParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltLoadSceneParams)
            {
                return new AltLoadSceneCommand(this, (AltLoadSceneParams)cmdParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltUnloadSceneParams)
            {
                return new AltUnloadSceneCommand(this, cmdParams as AltUnloadSceneParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltSetTimeScaleParams)
            {
                return new AltSetTimeScaleCommand(cmdParams as AltSetTimeScaleParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetTimeScaleParams)
            {
                return new AltGetTimeScaleCommand(cmdParams as AltGetTimeScaleParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltDeletePlayerPrefParams)
            {
                return new AltDeletePlayerPrefCommand(cmdParams as AltDeletePlayerPrefParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltDeleteKeyPlayerPrefParams)
            {
                return new AltDeleteKeyPlayerPrefCommand(cmdParams as AltDeleteKeyPlayerPrefParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltSetKeyPlayerPrefParams)
            {
                return new AltSetKeyPlayerPrefCommand(cmdParams as AltSetKeyPlayerPrefParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetKeyPlayerPrefParams)
            {
                var getKeyPlayerPrefParams = cmdParams as AltGetKeyPlayerPrefParams;
                switch (getKeyPlayerPrefParams.keyType)
                {
                    case PlayerPrefKeyType.Int: return new AltGetIntKeyPlayerPrefCommand(cmdParams as AltGetKeyPlayerPrefParams).ExecuteAndSerialize;
                    case PlayerPrefKeyType.String: return new AltGetStringKeyPlayerPrefCommand(cmdParams as AltGetKeyPlayerPrefParams).ExecuteAndSerialize;
                    case PlayerPrefKeyType.Float: return new AltGetFloatKeyPlayerPrefCommand(cmdParams as AltGetKeyPlayerPrefParams).ExecuteAndSerialize;
                    default:
                        return new AltInvalidCommand(cmdParams, new InvalidParameterTypeException(string.Format("PlayerPrefKeyType {0} not handled", getKeyPlayerPrefParams.keyType))).ExecuteAndSerialize;
                }
            }

            if (cmdParams is AltGetAllComponentsParams)
            {
                return new AltGetAllComponentsCommand(cmdParams as AltGetAllComponentsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetAllFieldsParams)
            {
                return new AltGetAllFieldsCommand(cmdParams as AltGetAllFieldsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetAllPropertiesParams)
            {
                return new AltGetAllPropertiesCommand(cmdParams as AltGetAllPropertiesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetAllMethodsParams)
            {
                return new AltGetAllMethodsCommand(cmdParams as AltGetAllMethodsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetAllScenesParams)
            {
                return new AltGetAllScenesCommand(cmdParams as AltGetAllScenesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetAllCamerasParams)
            {
                return new AltGetAllCamerasCommand(cmdParams as AltGetAllCamerasParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetAllActiveCamerasParams)
            {
                return new AltGetAllCamerasCommand(cmdParams as AltGetAllActiveCamerasParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetAllLoadedScenesParams)
            {
                return new AltGetAllLoadedScenesCommand(cmdParams as AltGetAllLoadedScenesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGameGetAllLoadedScenesAndObjectsParams)
            {
                return new AltGetAllLoadedScenesAndObjectsCommand(cmdParams as AltGameGetAllLoadedScenesAndObjectsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetScreenshotParams)
            {
                return new AltGetScreenshotCommand(this, cmdParams as AltGetScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltHighlightObjectScreenshotParams)
            {
                return new AltHighlightSelectedObjectCommand(this, cmdParams as AltHighlightObjectScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltHighlightObjectFromCoordinatesScreenshotParams)
            {
                return new AltHighlightObjectFromCoordinatesCommand(this, cmdParams as AltHighlightObjectFromCoordinatesScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltPressKeyboardKeysParams)
            {
                return new AltPressKeyboardKeysCommand(this, cmdParams as AltPressKeyboardKeysParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltMoveMouseParams)
            {
                return new AltMoveMouseCommand(this, cmdParams as AltMoveMouseParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltScrollParams)
            {
                return new AltScrollCommand(this, cmdParams as AltScrollParams).ExecuteAndSerialize;
            }
            //TODO remove this if when we will no longer support oldFindObject
            if (cmdParams is AltFindObjectParams)
            {
                return new AltOldFindObjectCommand(cmdParams as AltFindObjectParams).ExecuteAndSerialize;
            }
            //TODO remove this if when we will no longer support oldFindObject
            if (cmdParams is AltFindObjectsParams)
            {
                return new AltOldFindObjectsCommand(cmdParams as AltFindObjectsParams).ExecuteAndSerialize;
            }
            //TODO remove this if when we will no longer support oldFindObject
            if (cmdParams is AltFindObjectsLightParams)
            {
                return new AltOldFindObjectsLightCommand(cmdParams as AltFindObjectsLightParams).ExecuteAndSerialize;
            }
            //TODO remove this if when we will no longer support oldFindObject
            if (cmdParams is AltGetAllLoadedScenesAndObjectsParams)
            {
                return new AltOldGetAllLoadedScenesAndObjectsCommand(cmdParams as AltGetAllLoadedScenesAndObjectsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetTextParams)
            {
                return new AltGetTextCommand(cmdParams as AltGetTextParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltSetTextParams)
            {
                return new AltSetTextCommand(cmdParams as AltSetTextParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetPNGScreenshotParams)
            {
                return new AltGetScreenshotPNGCommand(this, cmdParams as AltGetPNGScreenshotParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGetServerVersionParams)
            {
                return new AltGetServerVersionCommand(cmdParams as AltGetServerVersionParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltSetServerLoggingParams)
            {
                return new AltSetServerLoggingCommand(cmdParams as AltSetServerLoggingParams).ExecuteAndSerialize;
            }
            if (cmdParams is ActivateNotification)
            {
                return new AltActivateNotificationCommand(this, cmdParams as ActivateNotification).ExecuteAndSerialize;
            }
            if (cmdParams is DeactivateNotification)
            {
                return new AltDeactivateNotificationCommand(this, cmdParams as DeactivateNotification).ExecuteAndSerialize;
            }
            if (cmdParams is AltFindObjectAtCoordinatesParams)
            {
                return new AltFindObjectAtCoordinatesCommand(cmdParams as AltFindObjectAtCoordinatesParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltResetInputParams)
            {
                return new AltResetInputCommand(cmdParams as AltResetInputParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGameFindObjectParams)
            {
                return new AltFindObjectCommand(cmdParams as AltGameFindObjectParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGameFindObjectsParams)
            {
                return new AltFindObjectsCommand(cmdParams as AltGameFindObjectsParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGameFindObjectsLightParams)
            {
                return new AltFindObjectsLightCommand(cmdParams as AltGameFindObjectsLightParams).ExecuteAndSerialize;
            }
            if (cmdParams is AltGameGetAllLoadedScenesAndObjectsParams)
            {
                return new AltGetAllLoadedScenesAndObjectsCommand(cmdParams as AltGameGetAllLoadedScenesAndObjectsParams).ExecuteAndSerialize;
            }

            return new AltInvalidCommand(cmdParams, new CommandNotFoundException(string.Format("Command {0} not handled", cmdParams.commandName))).ExecuteAndSerialize;
        }

        private Type getCommandType(string commandName)
        {
            //TODO Once we no longer suport the oldFindObject we will no longer need this if's
            if (commandName.Equals("gameFindObject"))
            {
                return typeof(AltGameFindObjectParams);
            }
            if (commandName.Equals("gameFindObjects"))
            {
                return typeof(AltGameFindObjectsParams);
            }
            if (commandName.Equals("gameFindObjectsLight"))
            {
                return typeof(AltGameFindObjectsLightParams);
            }
            if (commandName.Equals("gameGetAllLoadedScenesAndObjects"))
            {
                return typeof(AltGameGetAllLoadedScenesAndObjectsParams);
            }
            if (commandName.Equals("findObject"))
            {
                return typeof(AltFindObjectParams);
            }
            if (commandName.Equals("findObjects"))
            {
                return typeof(AltFindObjectsParams);
            }
            if (commandName.Equals("findObjectsLight"))
            {
                return typeof(AltFindObjectsLightParams);
            }
            if (commandName.Equals("getAllLoadedScenesAndObjects"))
            {
                return typeof(AltGetAllLoadedScenesAndObjectsParams);
            }


            var assembly = Assembly.GetAssembly(typeof(CommandParams));

            var derivedType = typeof(CommandParams);
            var type = assembly.GetTypes().FirstOrDefault(t =>
               {
                   if (derivedType.IsAssignableFrom(t)) // If type derrives from CommandParams
                   {
                       CommandAttribute cmdAttribute = (CommandAttribute)Attribute.GetCustomAttribute(t, typeof(CommandAttribute));
                       return cmdAttribute != null && cmdAttribute.Name == commandName;
                   }

                   return false;
               });

            if (type == null)
            {
                throw new CommandNotFoundException(string.Format("Command `{0}` not found.", commandName));
            }

            return type;
        }
    }
}
