using System;

namespace Universal.UsersService.Api.Infrastructure.Gateways
{
    public class ExternalApiException : Exception
    {
        public ExternalApiException(string message) : base(message) { }
    }

    public class ExternalApiNotFoundException : ExternalApiException
    {
        public ExternalApiNotFoundException(string message) : base(message) { }
    }

    public class ExternalApiTimeoutException : ExternalApiException
    {
        public ExternalApiTimeoutException(string message) : base(message) { }
    }

    public class ExternalApiBadRequestException : ExternalApiException
    {
        public ExternalApiBadRequestException(string message) : base(message) { }
    }
}
