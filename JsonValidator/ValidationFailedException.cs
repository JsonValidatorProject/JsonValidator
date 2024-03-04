using System;
using System.Collections.Generic;

namespace JsonValidator;

public class ValidationFailedException : Exception
{
    private const string DefaultMessage = "Json validation failed.";

    private readonly string? _message;

    /// <summary>
    /// Gets a message that describes the current exception.
    /// </summary>
    public override string Message
        => _message is null
            ? base.Message
            : $"{base.Message} Problem(s):{Environment.NewLine}{_message}";


    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationFailedException"/> class.
    /// </summary>
    public ValidationFailedException()
        : base(DefaultMessage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationFailedException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ValidationFailedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationFailedException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference.</param>
    public ValidationFailedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationFailedException"/> class.
    /// </summary>
    /// <param name="errors">The issues found during validation.</param>
    public ValidationFailedException(List<string> errors) : base(DefaultMessage) =>
        _message = string.Join(Environment.NewLine, errors);
}
