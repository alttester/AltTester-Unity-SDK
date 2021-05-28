using System;
using System.Globalization;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.Server.Logging;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public abstract class AltUnityCommand<TParam, TResult> where TParam : CommandParams
    {
        public TParam CommandParams { get; private set; }

        protected AltUnityCommand(TParam commandParams)
        {
            CommandParams = commandParams;
        }

        public string ExecuteAndSerialize<T>(Func<T> action)
        {
            var result = ExecuteHandleErrors(action);
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = CultureInfo.InvariantCulture
            });
        }
        public string ExecuteAndSerialize()
        {
            return ExecuteAndSerialize(Execute);
        }

        protected CommandResponse<T> ExecuteHandleErrors<T>(Func<T> action)
        {
            Exception exception = null;
            T response = default(T);
            string error = null;

            try
            {
                response = action();
            }
            catch (System.NullReferenceException e)
            {
                exception = e;
                error = AltUnityErrors.errorNullRefferenceMessage;
            }
            catch (FailedToParseArgumentsException e)
            {
                exception = e;
                error = AltUnityErrors.errorFailedToParseArguments;
            }
            catch (MethodWithGivenParametersNotFoundException e)
            {
                exception = e;
                error = AltUnityErrors.errorMethodWithGivenParametersNotFound;
            }
            catch (InvalidParameterTypeException e)
            {
                exception = e;
                error = AltUnityErrors.errorInvalidParameterType;
            }
            catch (JsonException e)
            {
                exception = e;
                error = AltUnityErrors.errorCouldNotParseJsonString;
            }
            catch (ComponentNotFoundException e)
            {
                exception = e;
                error = AltUnityErrors.errorComponentNotFoundMessage;
            }
            catch (MethodNotFoundException e)
            {
                exception = e;
                error = AltUnityErrors.errorMethodNotFoundMessage;
            }
            catch (PropertyNotFoundException e)
            {
                exception = e;
                error = AltUnityErrors.errorPropertyNotFoundMessage;
            }
            catch (AssemblyNotFoundException e)
            {
                exception = e;
                error = AltUnityErrors.errorAssemblyNotFoundMessage;
            }
            catch (CouldNotPerformOperationException e)
            {
                exception = e;
                error = AltUnityErrors.errorCouldNotPerformOperationMessage;
            }
            catch (InvalidPathException e)
            {
                exception = e;
                error = AltUnityErrors.errorInvalidPath;
            }
            catch (NotFoundException e)
            {
                exception = e;
                error = AltUnityErrors.errorNotFoundMessage;
            }
            catch (CameraNotFoundException e)
            {
                exception = e;
                error = AltUnityErrors.errorCameraNotFound;
            }
            catch (InvalidCommandException e)
            {
                exception = e.InnerException;
                error = AltUnityErrors.errorInvalidCommand;
            }
            catch (Exception e)
            {
                exception = e;
                error = AltUnityErrors.errorUnknownError;
            }

            string logs = string.Empty;
            if (exception != null)
                logs = exception.Message + "\n" + exception.StackTrace;


            var cmdResponse = new CommandResponse<T>();
            cmdResponse.commandName = CommandParams.commandName;
            cmdResponse.messageId = CommandParams.messageId;
            cmdResponse.data = response;
            cmdResponse.error = error;
            cmdResponse.logs = logs;

            return cmdResponse;
        }
        public abstract TResult Execute();
    }
}