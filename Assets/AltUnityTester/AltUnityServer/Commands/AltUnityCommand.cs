public abstract class AltUnityCommand
{
    public void SendResponse(AltClientSocketHandler handler)
    {
        AltUnityRunner._responseQueue.ScheduleResponse(delegate
        {
            string response = null;
            try
            {
                response = Execute();
            }
            catch (System.NullReferenceException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorNullRefferenceMessage;
            }
            catch (Assets.AltUnityTester.AltUnityDriver.FailedToParseArgumentsException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorFailedToParseArguments;
            }
            catch (Assets.AltUnityTester.AltUnityDriver.MethodWithGivenParametersNotFoundException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorMethodWithGivenParametersNotFound;
            }
            catch (Assets.AltUnityTester.AltUnityDriver.InvalidParameterTypeException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorInvalidParameterType;
            }
            catch (Newtonsoft.Json.JsonException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorCouldNotParseJsonString;
            }
            catch (Assets.AltUnityTester.AltUnityDriver.ComponentNotFoundException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorComponentNotFoundMessage;
            }
            catch (Assets.AltUnityTester.AltUnityDriver.MethodNotFoundException e)
            {
                AltUnityRunner._altUnityRunner.LogMessage(e.ToString());
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorMethodNotFoundMessage;
            }
            catch (Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
            }
            catch (Assets.AltUnityTester.AltUnityDriver.AssemblyNotFoundException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorAssemblyNotFoundMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = AltUnityRunner._altUnityRunner.errorUnknownError + AltUnityRunner._altUnityRunner.requestSeparatorString + exception;
            }

            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    public abstract string Execute();

}
