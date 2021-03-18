using System;

namespace Lyrico.Application.Exceptions
{
    public class ServiceUnavailableException : ApplicationException
    {
        public ServiceUnavailableException(string serviceName) : base($"An error occurred contacting the service. Service: {serviceName}")
        { }
    }
}
