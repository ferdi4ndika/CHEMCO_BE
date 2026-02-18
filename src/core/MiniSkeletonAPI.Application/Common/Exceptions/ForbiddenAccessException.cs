namespace MiniSkeletonAPI.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
}

public class NotFoundException : Exception
{
    public NotFoundException() : base() { }
}

//public class NotImplementedException : Exception
//{
//    public NotImplementedException() : base() { }
//}