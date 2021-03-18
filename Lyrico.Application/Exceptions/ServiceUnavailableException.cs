using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lyrico.Application.Exceptions
{
    public class ServiceUnavailableException : ApplicationException
    {
        public ServiceUnavailableException(string serviceName) : base($"An error occurred contacting the service. Service: {serviceName}")
        { }
    }
}
