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
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = AltUnityRunner._altUnityRunner.errorNullRefferenceMessage;
            }
            catch (System.ArgumentException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = AltUnityRunner._altUnityRunner.errorFailedToParseArguments;
            }
            catch (System.Reflection.TargetParameterCountException)
            {
                response = AltUnityRunner._altUnityRunner.errorIncorrectNumberOfParameters;
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
            catch (Assets.AltUnityTester.AltUnityDriver.PropertyNotFoundException e)
            {
                UnityEngine.Debug.Log(e);
                response = AltUnityRunner._altUnityRunner.errorPropertyNotFoundMessage;
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
