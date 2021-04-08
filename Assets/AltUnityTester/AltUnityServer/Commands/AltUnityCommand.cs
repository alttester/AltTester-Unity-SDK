using System;
using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Newtonsoft.Json;
using NLog;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public abstract class AltUnityCommand
    {
        protected string[] Parameters;

        public string MessageId { get { return Parameters[0]; } }
        public string CommandName { get { return Parameters[1]; } }

        protected static bool LogEnabled = false;

        protected AltUnityCommand(string[] parameters, int expectedParametersCount)
        {
            validateParametersCount(parameters, expectedParametersCount);
            this.Parameters = parameters;
        }

        public void SendResponse(AltClientSocketHandler handler)
        {
            AltUnityRunner._responseQueue.ScheduleResponse(delegate
            {
                Exception exception = null;
                string response = null;
                try
                {
                    response = Execute();
                }
                catch (System.NullReferenceException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorNullRefferenceMessage;
                }
                catch (FailedToParseArgumentsException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorFailedToParseArguments;
                }
                catch (MethodWithGivenParametersNotFoundException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorMethodWithGivenParametersNotFound;
                }
                catch (InvalidParameterTypeException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorInvalidParameterType;
                }
                catch (JsonException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorCouldNotParseJsonString;
                }
                catch (ComponentNotFoundException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorComponentNotFoundMessage;
                }
                catch (MethodNotFoundException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorMethodNotFoundMessage;
                }
                catch (PropertyNotFoundException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorPropertyNotFoundMessage;
                }
                catch (AssemblyNotFoundException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorAssemblyNotFoundMessage;
                }
                catch (CouldNotPerformOperationException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorCouldNotPerformOperationMessage;
                }
                catch (InvalidParametersOnDriverCommandException e)
                {
                    exception = e;
                    response = AltUnityErrors.errorInvalidParametersOnDriverCommand;
                }
                catch (Exception e)
                {
                    exception = e;
                    response = AltUnityErrors.errorUnknownError + AltUnityRunner._altUnityRunner.requestSeparatorString + e;
                }
                finally
                {
                    AltUnityCommand command = this;
                    if (exception != null)
                        command = new AltUnityErrorCommand(response, exception, this.Parameters);

                    handler.SendResponse(command, response);
                }
            });
        }
        public virtual string GetLogs()
        {
            return string.Empty;
        }
        public abstract string Execute();

        private void validateParametersCount(string[] parameters, int expectedCount)
        {
            if (parameters.Length != expectedCount)
            {
                throw new InvalidParametersOnDriverCommandException("Expected " + expectedCount + " parameters, got " + parameters.Length);
            }
        }
    }
}