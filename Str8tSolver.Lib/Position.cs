namespace Str8tSolver.Lib;

/// <summary>
/// This class represents the position of a field in the str8t field.
/// </summary>
public class Position
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> class.
    /// </summary>
    /// <param name="x">The x coordinate of the position.</param>
    /// <param name="y">The y coordinate of the position.</param>
    public Position(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Gets the x coordinate of the position.
    /// </summary>
    public int X
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the y coordinate of the position.
    /// </summary>
    public int Y
    {
        get;
        private set;
    }
}
