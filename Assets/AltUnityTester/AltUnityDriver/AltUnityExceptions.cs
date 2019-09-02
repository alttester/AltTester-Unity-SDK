
namespace Assets.AltUnityTester.AltUnityDriver
{
   public class AltUnityException: System.Exception
    {
        public AltUnityException()
        {

        }

        public AltUnityException(string message) : base(message)
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

    public class CouldNotPerformOperationException : AltUnityException
    {
        public CouldNotPerformOperationException()
        {
        }

        public CouldNotPerformOperationException(string message) : base(message)
        {
        }
    }

    public class IncorrectNumberOfParametersException : AltUnityException
    {
        public IncorrectNumberOfParametersException()
        {
        }

        public IncorrectNumberOfParametersException(string message) : base(message)
        {
        }
    }

    public class CouldNotParseJsonStringException : AltUnityException
    {
        public CouldNotParseJsonStringException()
        {
        }

        public CouldNotParseJsonStringException(string message) : base(message)
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

    public class PropertyCannotBeSetException : AltUnityException
    {
        public PropertyCannotBeSetException()
        {
        }

        public PropertyCannotBeSetException(string message) : base(message)
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

}
