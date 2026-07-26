namespace Project.Core;

/// <summary>
/// Represents errors that occur during application execution
/// </summary>
public partial class ProjectException : Exception
{
    /// <summary>
    /// Initializes a new instance of the Exception class.
    /// </summary>
    public ProjectException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Exception class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProjectException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the Exception class with a specified error message.
    /// </summary>
    /// <param name="messageFormat">The exception message format.</param>
    /// <param name="args">The exception message arguments.</param>
    public ProjectException(string messageFormat, params object[] args)
        : base(string.Format(messageFormat, args))
    {
    }
        
    /// <summary>
    /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public ProjectException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}