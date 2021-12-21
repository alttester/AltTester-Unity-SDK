
using System;
using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver
{
    public class AltUnityErrors
    {
        public const string errorNotFound = "notFound";
        public const string errorSceneNotFound = "sceneNotFound";
        public const string errorPropertyNotFound = "propertyNotFound";
        public const string errorMethodNotFound = "methodNotFound";
        public const string errorComponentNotFound = "componentNotFound";
        public const string errorAssemblyNotFound = "assemblyNotFound";
        public const string errorCouldNotPerformOperation = "couldNotPerformOperation";
        public const string errorCouldNotParseJsonString = "couldNotParseJsonString";
        public const string errorMethodWithGivenParametersNotFound = "methodWithGivenParametersNotFound";
        public const string errorInvalidParameterType = "invalidParameterType";
        public const string errorFailedToParseArguments = "failedToParseMethodArguments";
        public const string errorObjectWasNotFound = "objectNotFound";
        public const string errorPropertyNotSet = "propertyCannotBeSet";
        public const string errorNullReference = "nullReferenceException";
        public const string errorUnknownError = "unknownError";
        public const string errorFormatException = "formatException";
        public const string errorCameraNotFound = "cameraNotFound";
        public const string errorIndexOutOfRange = "indexOutOfRange";
        public const string errorInvalidCommand = "invalidCommand";
        public const string errorInvalidPath = "invalidPath";
        public const string errorInputModule = "ALTUNITYTESTERNotAddedAsDefineVariable";
    }

    public class AltUnityException : Exception
    {
        public AltUnityException()
        {

        }

        public AltUnityException(string message) : base(message)
        {

        }
        public AltUnityException(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public class NotFoundException : AltUnityException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class CameraNotFoundException : AltUnityException
    {
        public CameraNotFoundException()
        {
        }

        public CameraNotFoundException(string message) : base(message)
        {
        }
    }

    public class PropertyNotFoundException : AltUnityException
    {
        public PropertyNotFoundException()
        {
        }

        public PropertyNotFoundException(string message) : base(message)
        {
        }
    }

    public class MethodNotFoundException : AltUnityException
    {
        public MethodNotFoundException()
        {
        }

        public MethodNotFoundException(string message) : base(message)
        {
        }
    }

    public class ComponentNotFoundException : AltUnityException
    {
        public ComponentNotFoundException()
        {
        }

        public ComponentNotFoundException(string message) : base(message)
        {
        }
    }

    public class AssemblyNotFoundException : AltUnityException
    {
        public AssemblyNotFoundException()
        {
        }

        public AssemblyNotFoundException(string message) : base(message)
        {
        }
    }

    public class CouldNotPerformOperationException : AltUnityException
    {
        public CouldNotPerformOperationException()
        {
        }

        public CouldNotPerformOperationException(string message) : base(message)
        {
        }
    }

    public class InvalidParameterTypeException : AltUnityException
    {
        public InvalidParameterTypeException()
        {
        }

        public InvalidParameterTypeException(string message) : base(message)
        {
        }
    }

    public class MethodWithGivenParametersNotFoundException : AltUnityException
    {
        public MethodWithGivenParametersNotFoundException()
        {
        }

        public MethodWithGivenParametersNotFoundException(string message) : base(message)
        {
        }
    }

    public class FailedToParseArgumentsException : AltUnityException
    {
        public FailedToParseArgumentsException()
        {
        }

        public FailedToParseArgumentsException(string message) : base(message)
        {
        }
    }

    public class ObjectWasNotFoundException : AltUnityException
    {
        public ObjectWasNotFoundException()
        {
        }

        public ObjectWasNotFoundException(string message) : base(message)
        {
        }
    }

    public class NullReferenceException : AltUnityException
    {
        public NullReferenceException()
        {
        }

        public NullReferenceException(string message) : base(message)
        {
        }
    }

    public class UnknownErrorException : AltUnityException
    {
        public UnknownErrorException()
        {
        }

        public UnknownErrorException(string message) : base(message)
        {
        }
    }

    public class FormatException : AltUnityException
    {
        public FormatException()
        {
        }

        public FormatException(string message) : base(message)
        {
        }
    }

    public class WaitTimeOutException : AltUnityException
    {
        public WaitTimeOutException()
        {
        }

        public WaitTimeOutException(string message) : base(message)
        {
        }
    }

    public class CommandResponseTimeoutException : AltUnityException
    {
        public CommandResponseTimeoutException()
        {
        }

        public CommandResponseTimeoutException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Raised when the client connection timesout.
    /// </summary>
    public class ConnectionTimeoutException : AltUnityException
    {
        public ConnectionTimeoutException()
        {

        }
        public ConnectionTimeoutException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// Raised when the client can not connect to the server
    /// </summary>
    public class ConnectionException : AltUnityException
    {
        public ConnectionException()
        {

        }
        public ConnectionException(string message) : base(message)
        {

        }
    }

    public class CommandNotFoundException : AltUnityException
    {
        public CommandNotFoundException()
        {
        }

        public CommandNotFoundException(string message) : base(message)
        {
        }
    }

    public class InvalidCommandException : AltUnityException
    {
        public InvalidCommandException(Exception innerException) : base(AltUnityErrors.errorInvalidCommand, innerException)
        {
        }

        public InvalidCommandException(string message) : base(message)
        {
        }
    }

    public class AltUnityRecvallException : AltUnityException
    {
        public AltUnityRecvallException()
        {

        }

        public AltUnityRecvallException(string message) : base(message)
        {

        }
    }

    public class AltUnityRecvallMessageIdException : AltUnityRecvallException
    {
        public AltUnityRecvallMessageIdException()
        {

        }

        public AltUnityRecvallMessageIdException(string message) : base(message)
        {

        }
    }

    public class AltUnityRecvallMessageFormatException : AltUnityRecvallException
    {
        public AltUnityRecvallMessageFormatException()
        {

        }

        public AltUnityRecvallMessageFormatException(string message) : base(message)
        {
        }
    }

    public class PortForwardingException : AltUnityException
    {
        public PortForwardingException()
        {
        }

        public PortForwardingException(string message) : base(message)
        {
        }

        public PortForwardingException(string message, Exception inner) : base(message, inner)
        {
        }
    }


    public class AltUnityInvalidServerResponse : AltUnityException
    {
        public AltUnityInvalidServerResponse(string expected, string received) : base(string.Format("Expected to get response '{0}'; Got  '{1}'", expected, received))
        {
        }
    }

    public class InvalidPathException : AltUnityException
    {
        public InvalidPathException()
        {
        }

        public InvalidPathException(string message) : base(message)
        {
        }
    }

    public class AltUnityInputModuleException : AltUnityException
    {

        public AltUnityInputModuleException()
        {
        }

        public AltUnityInputModuleException(string message) : base(message)
        {
        }
    }

    public class AltUnityInnerException : AltUnityException
    {

        public AltUnityInnerException(Exception inner) : base(AltUnityErrors.errorUnknownError, inner)
        {
        }
    }

    public class AltUnityCameraNotFoundException : AltUnityException
    {
        public AltUnityCameraNotFoundException()
        {
        }

        public AltUnityCameraNotFoundException(string message) : base(message)
        {
        }
    }

    public class AltUnityPathNotFoundException : AltUnityException
    {
        public AltUnityPathNotFoundException()
        {
        }

        public AltUnityPathNotFoundException(string message) : base(message)
        {
        }
    }

    public class SceneNotFoundException : AltUnityException
    {
        public SceneNotFoundException()
        {
        }

        public SceneNotFoundException(string message) : base(message)
        {
        }
    }

    public class ResponseFormatException : AltUnityException
    {
        public ResponseFormatException() { }
        public ResponseFormatException(Type t, string data) : base("Could not deserialize response data: `" + data + "` into " + t.FullName)
        {

        }
    }
}
