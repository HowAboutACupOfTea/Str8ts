namespace Str8tSolver.Lib;

/// <summary>
/// This class represents a str8t field.
/// </summary>
public class Str8tField
{
    /// <summary>
    /// The number of the field.
    /// </summary>
    private int number;

    /// <summary>
    /// Initializes a new instance of the <see cref="Str8tField"/> class.
    /// </summary>
    /// <param name="number">The number of the field.</param>
    /// <param name="fieldColor">The color of the field.</param>
    /// <param name="position">The position of the field.</param>
    public Str8tField(int number, FieldColor fieldColor, Position position)
    {
        this.Number = number;
        this.FieldColor = fieldColor;
        this.Position = position;
    }

    /// <summary>
    /// This event gets fired whenever a number in the field changed.
    /// Given that the event has at least one subscriber.
    /// </summary>
    public event EventHandler<NumberChangedArguments>? NumberChanged;

    /// <summary>
    /// Gets or sets the number of the field.
    /// </summary>
    public int Number
    {
        get
        {
            return this.number;
        }

        set
        {
            this.number = value;
            this.NumberChanged?.Invoke(this, new NumberChangedArguments(this.Number, this.Position));
        }
    }

    /// <summary>
    /// Gets the color of the field.
    /// </summary>
    public FieldColor FieldColor
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the position of the field.
    /// </summary>
    public Position Position
    {
        get;
        private set;
    }
}
