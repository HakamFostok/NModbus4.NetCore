namespace Modbus.Utility;

/// <summary>
///     Possible options for DiscriminatedUnion type.
/// </summary>
public enum DiscriminatedUnionOption
{
    /// <summary>
    ///     Option A.
    /// </summary>
    A,

    /// <summary>
    ///     Option B.
    /// </summary>
    B
}

/// <summary>
///     A data type that can store one of two possible strongly typed options.
/// </summary>
/// <typeparam name="TA">The type of option A.</typeparam>
/// <typeparam name="TB">The type of option B.</typeparam>
public class DiscriminatedUnion<TA, TB>
{
    private TA optionA;
    private TB optionB;

    /// <summary>
    ///     Gets the value of option A.
    /// </summary>
    public TA A
    {
        get
        {
            if (Option != DiscriminatedUnionOption.A)
            {
                string msg = $"{DiscriminatedUnionOption.A} is not a valid option for this discriminated union instance.";
                throw new InvalidOperationException(msg);
            }

            return optionA;
        }
    }

    /// <summary>
    ///     Gets the value of option B.
    /// </summary>
    public TB B
    {
        get
        {
            if (Option != DiscriminatedUnionOption.B)
            {
                string msg = $"{DiscriminatedUnionOption.B} is not a valid option for this discriminated union instance.";
                throw new InvalidOperationException(msg);
            }

            return optionB;
        }
    }

    /// <summary>
    ///     Gets the discriminated value option set for this instance.
    /// </summary>
    public DiscriminatedUnionOption Option { get; private set; }

    /// <summary>
    ///     Factory method for creating DiscriminatedUnion with option A set.
    /// </summary>
    public static DiscriminatedUnion<TA, TB> CreateA(TA a) =>
        new() { Option = DiscriminatedUnionOption.A, optionA = a };

    /// <summary>
    ///     Factory method for creating DiscriminatedUnion with option B set.
    /// </summary>
    public static DiscriminatedUnion<TA, TB> CreateB(TB b) =>
        new() { Option = DiscriminatedUnionOption.B, optionB = b };

    /// <summary>
    ///     Returns a <see cref="string" /> that represents the current <see cref="object" />.
    /// </summary>
    /// <returns>
    ///     A <see cref="string" /> that represents the current <see cref="object" />.
    /// </returns>
    public override string? ToString() =>
        Option switch
        {
            DiscriminatedUnionOption.A => A.ToString(),
            DiscriminatedUnionOption.B => B.ToString(),
            _ => null,
        };
}