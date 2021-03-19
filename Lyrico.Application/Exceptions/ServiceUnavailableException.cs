using System;

namespace Lyrico.Application.Exceptions
{
    public class ServiceUnavailableException : ApplicationException
    {
        /// <summary>
        /// Exception to be used when a service returns a non-success status code
        /// </summary>
        /// <param name="serviceName"></param>
        public ServiceUnavailableException(string serviceName) : base($"An error occurred contacting the service. Service: {serviceName}")
        { }
    }
}
