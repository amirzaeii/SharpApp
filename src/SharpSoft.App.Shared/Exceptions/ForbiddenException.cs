﻿using System.Net;

namespace SharpSoft.App.Shared.Exceptions;

public class ForbiddenException : RestException
{
    public ForbiddenException()
        : base(nameof(AppStrings.ForbiddenException))
    {
    }

    public ForbiddenException(string message)
        : base(message)
    {
    }

    public ForbiddenException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public ForbiddenException(LocalizedString message)
        : base(message)
    {
    }

    public ForbiddenException(LocalizedString message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
}
