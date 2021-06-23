
using System;
using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver
{
    public class AltUnityErrors
    {
        public const string errorNotFoundMessage = "error:notFound";
        public const string errorPropertyNotFoundMessage = "error:propertyNotFound";
        public const string errorMethodNotFoundMessage = "error:methodNotFound";
        public const string errorComponentNotFoundMessage = "error:componentNotFound";
        public const string errorAssemblyNotFoundMessage = "error:assemblyNotFound";
        public const string errorCouldNotPerformOperationMessage = "error:couldNotPerformOperation";
        public const string errorCouldNotParseJsonString = "error:couldNotParseJsonString";
        public const string errorMethodWithGivenParametersNotFound = "error:methodWithGivenParametersNotFound";
        public const string errorInvalidParameterType = "error:invalidParameterType";
        public const string errorFailedToParseArguments = "error:failedToParseMethodArguments";
        public const string errorObjectWasNotFound = "error:objectNotFound";
        public const string errorPropertyNotSet = "error:propertyCannotBeSet";
        public const string errorNullRefferenceMessage = "error:nullReferenceException";
        public const string errorUnknownError = "error:unknownError";
        public const string errorFormatException = "error:formatException";
        public const string errorCameraNotFound = "error:cameraNotFound";
        public const string errorIndexOutOfRange = "error:indexOutOfRange";
        public const string errorInvalidParametersOnDriverCommand = "error:invalidParametersOnDriverCommand";
        public const string errorInvalidCommand = "error:invalidCommand";
        public const string errorInvalidPath = "error:invalidPath";
        public const string errorInputModule = "error:ALTUNITYTESTERNotAddedAsDefineVariable";
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

}
