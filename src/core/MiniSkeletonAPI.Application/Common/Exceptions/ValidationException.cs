using FluentValidation.Results;

namespace MiniSkeletonAPI.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}

//public class NotFoundException : Exception
//{
//    /// <summary>
//    /// Initializes a new instance of the NotFoundException class with a specified name of the queried object and its key.
//    /// </summary>
//    /// <param name="objectName">Name of the queried object.</param>
//    /// <param name="key">The value by which the object is queried.</param>
//    public NotFoundException(string key, string objectName)
//        : base($"Queried object {objectName} was not found, Key: {key}")
//    {
//    }

//    /// <summary>
//    /// Initializes a new instance of the NotFoundException class with a specified name of the queried object, its key,
//    /// and the exception that is the cause of this exception.
//    /// </summary>
//    /// <param name="objectName">Name of the queried object.</param>
//    /// <param name="key">The value by which the object is queried.</param>
//    /// <param name="innerException">The exception that is the cause of the current exception.</param>
//    public NotFoundException(string key, string objectName, Exception innerException)
//        : base($"Queried object {objectName} was not found, Key: {key}", innerException)
//    {
//    }
//}