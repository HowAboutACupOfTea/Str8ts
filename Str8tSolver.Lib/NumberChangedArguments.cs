namespace Str8tSolver.Lib;

/// <summary>
/// This class represents the event thrown when a number in the str8t field changes.
/// </summary>
public class NumberChangedArguments : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NumberChangedArguments"/> class.
    /// </summary>
    /// <param name="number">The new number.</param>
    /// <param name="position">The position of the changed number.</param>
    public NumberChangedArguments(int number, Position position)
    {
        this.Number = number;
        this.Position = position;
    }

    /// <summary>
    /// Gets the new number.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Gets the position of the changed number.
    /// </summary>
    public Position Position { get; }
}