using System.Diagnostics.CodeAnalysis;
using Modbus.Message;

namespace Modbus;

/// <summary>
///     Represents slave errors that occur during communication.
/// </summary>
public class SlaveException : Exception
{
    private const string SlaveAddressPropertyName = "SlaveAdress";
    private const string FunctionCodePropertyName = "FunctionCode";
    private const string SlaveExceptionCodePropertyName = "SlaveExceptionCode";

    private readonly SlaveExceptionResponse _slaveExceptionResponse;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SlaveException" /> class.
    /// </summary>
    public SlaveException()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SlaveException" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public SlaveException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SlaveException" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public SlaveException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    internal SlaveException(SlaveExceptionResponse slaveExceptionResponse)
    {
        _slaveExceptionResponse = slaveExceptionResponse;
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by test code.")]
    internal SlaveException(string message, SlaveExceptionResponse slaveExceptionResponse)
        : base(message)
    {
        _slaveExceptionResponse = slaveExceptionResponse;
    }

    /// <summary>
    ///     Gets a message that describes the current exception.
    /// </summary>
    /// <value>
    ///     The error message that explains the reason for the exception, or an empty string.
    /// </value>
    public override string Message
    {
        get
        {
            string responseString;
            responseString = _slaveExceptionResponse != null ? string.Concat(Environment.NewLine, _slaveExceptionResponse) : string.Empty;
            return string.Concat(base.Message, responseString);
        }
    }

    /// <summary>
    ///     Gets the response function code that caused the exception to occur, or 0.
    /// </summary>
    /// <value>The function code.</value>
    public byte FunctionCode =>
        _slaveExceptionResponse != null ? _slaveExceptionResponse.FunctionCode : (byte)0;

    /// <summary>
    ///     Gets the slave exception code, or 0.
    /// </summary>
    /// <value>The slave exception code.</value>
    public byte SlaveExceptionCode =>
        _slaveExceptionResponse != null ? _slaveExceptionResponse.SlaveExceptionCode : (byte)0;

    /// <summary>
    ///     Gets the slave address, or 0.
    /// </summary>
    /// <value>The slave address.</value>
    public byte SlaveAddress =>
        _slaveExceptionResponse != null ? _slaveExceptionResponse.SlaveAddress : (byte)0;
}
