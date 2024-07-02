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
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public abstract class AltCommand<TParam, TResult> where TParam : CommandParams
    {
        private const int MAX_DEPTH_REPONSE_DATA_SERIALIZATION = 2;
        public TParam CommandParams { get; private set; }
        protected JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Culture = CultureInfo.InvariantCulture
        };
        protected AltCommand(TParam commandParams)
        {
            CommandParams = commandParams;
        }

        public string ExecuteAndSerialize<T>(Func<T> action)
        {
            var result = ExecuteHandleErrors(action);
            return JsonConvert.SerializeObject(result, JsonSerializerSettings);
        }

        public string ExecuteAndSerialize()
        {
            return ExecuteAndSerialize(Execute);
        }

        protected CommandResponse ExecuteHandleErrors<T>(Func<T> action)
        {
            T response = default(T);
            Exception exception = null;
            CommandError error = null;
            String errorType = null;

            try
            {
                response = action();
            }
            catch (System.NullReferenceException e)
            {
                exception = e;
                errorType = AltErrors.errorNullReference;
            }
            catch (FailedToParseArgumentsException e)
            {
                exception = e;
                errorType = AltErrors.errorFailedToParseArguments;
            }
            catch (MethodWithGivenParametersNotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorMethodWithGivenParametersNotFound;
            }
            catch (InvalidParameterTypeException e)
            {
                exception = e;
                errorType = AltErrors.errorInvalidParameterType;
            }
            catch (JsonException e)
            {
                exception = e;
                errorType = AltErrors.errorCouldNotParseJsonString;
            }
            catch (ComponentNotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorComponentNotFound;
            }
            catch (MethodNotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorMethodNotFound;
            }
            catch (PropertyNotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorPropertyNotFound;
            }
            catch (AssemblyNotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorAssemblyNotFound;
            }
            catch (CouldNotPerformOperationException e)
            {
                exception = e;
                errorType = AltErrors.errorCouldNotPerformOperation;
            }
            catch (InvalidPathException e)
            {
                exception = e;
                errorType = AltErrors.errorInvalidPath;
            }
            catch (NotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorNotFound;
            }
            catch (SceneNotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorSceneNotFound;
            }
            catch (CameraNotFoundException e)
            {
                exception = e;
                errorType = AltErrors.errorCameraNotFound;
            }
            catch (InvalidCommandException e)
            {
                exception = e.InnerException;
                errorType = AltErrors.errorInvalidCommand;
            }
            catch (AltInnerException e)
            {
                exception = e.InnerException;
                errorType = AltErrors.errorUnknownError;
            }
            catch (Exception e)
            {
                exception = e;
                errorType = AltErrors.errorUnknownError;
            }
            if (exception != null)
            {
                error = new CommandError();
                error.type = errorType;
                error.message = exception.Message;
                error.trace = exception.StackTrace;
            }

            var cmdResponse = new CommandResponse();
            if (CommandParams.commandName.Equals("gameFindObject"))
            {
                CommandParams.commandName = "findObject";
            }
            if (CommandParams.commandName.Equals("gameFindObjects"))
            {
                CommandParams.commandName = "findObjects";
            }
            if (CommandParams.commandName.Equals("gameFindObjectsLight"))
            {
                CommandParams.commandName = "findObjectsLight";
            }
            if (CommandParams.commandName.Equals("gameGetAllLoadedScenesAndObjects"))
            {
                CommandParams.commandName = "getAllLoadedScenesAndObjects";
            }
            cmdResponse.commandName = CommandParams.commandName;
            cmdResponse.messageId = CommandParams.messageId;
            cmdResponse.driverId = CommandParams.driverId;

            if (response != null)
            {
                int maxDepth = MAX_DEPTH_REPONSE_DATA_SERIALIZATION;

                if (CommandParams is AltGetObjectComponentPropertyParams)
                {
                    maxDepth = (CommandParams as AltGetObjectComponentPropertyParams).maxDepth;
                }
                try
                {
                    using (var strWriter = new System.IO.StringWriter())
                    {
                        using (var jsonWriter = new CustomJsonTextWriter(strWriter))
                        {
                            Func<bool> include = () => jsonWriter.CurrentDepth <= maxDepth;
                            var resolver = new AltContractResolver(include);
                            var serializer = new Newtonsoft.Json.JsonSerializer { ContractResolver = resolver, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore };
                            serializer.Serialize(jsonWriter, response);
                        }
                        cmdResponse.data = strWriter.ToString();
                    }
                }
                catch (Exception e)
                {
                    error = new CommandError();
                    error.type = AltErrors.errorUnknownError;
                    error.message = e.Message;
                    error.trace = e.StackTrace;
                }

            }

            cmdResponse.error = error;
            cmdResponse.isNotification = false;

            return cmdResponse;
        }

        public abstract TResult Execute();
    }
}
