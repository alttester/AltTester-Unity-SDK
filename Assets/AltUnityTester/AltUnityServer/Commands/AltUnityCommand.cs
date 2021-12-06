using System;
using System.Globalization;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Newtonsoft.Json;

namespace Altom.AltUnityTester.Commands
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
                errorType = AltUnityErrors.errorNullReference;
            }
            catch (FailedToParseArgumentsException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorFailedToParseArguments;
            }
            catch (MethodWithGivenParametersNotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorMethodWithGivenParametersNotFound;
            }
            catch (InvalidParameterTypeException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorInvalidParameterType;
            }
            catch (JsonException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorCouldNotParseJsonString;
            }
            catch (ComponentNotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorComponentNotFound;
            }
            catch (MethodNotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorMethodNotFound;
            }
            catch (PropertyNotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorPropertyNotFound;
            }
            catch (AssemblyNotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorAssemblyNotFound;
            }
            catch (CouldNotPerformOperationException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorCouldNotPerformOperation;
            }
            catch (InvalidPathException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorInvalidPath;
            }
            catch (NotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorNotFound;
            }
            catch (SceneNotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorSceneNotFound;
            }
            catch (CameraNotFoundException e)
            {
                exception = e;
                errorType = AltUnityErrors.errorCameraNotFound;
            }
            catch (InvalidCommandException e)
            {
                exception = e.InnerException;
                errorType = AltUnityErrors.errorInvalidCommand;
            }
            catch (AltUnityInnerException e)
            {
                exception = e.InnerException;
                errorType = AltUnityErrors.errorUnknownError;
            }
            catch (Exception e)
            {
                exception = e;
                errorType = AltUnityErrors.errorUnknownError;
            }

            if (exception != null)
            {
                error = new CommandError();
                error.type = errorType;
                error.message = exception.Message;
                error.trace = exception.StackTrace;
            }

            var cmdResponse = new CommandResponse<T>();
            cmdResponse.commandName = CommandParams.commandName;
            cmdResponse.messageId = CommandParams.messageId;
            cmdResponse.data = response;
            cmdResponse.error = error;

            return cmdResponse;
        }

        public abstract TResult Execute();
    }
}
