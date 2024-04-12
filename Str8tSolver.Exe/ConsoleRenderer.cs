using Str8tSolver.Lib;

namespace Str8tSolver.Exe;

/// <summary>
/// This class implements the console renderer.
/// </summary>
internal class ConsoleRenderer
{
    /// <summary>
    /// This method visualizes the given str8t once on the console.
    /// </summary>
    /// <param name="str8tInput">The str8t to visualize.</param>
    internal void VisualizePuzzleOnce(Str8tField[,] str8tInput)
    {
        for (int i = 0; i < str8tInput.GetLength(0); i++)
        {
            for (int j = 0; j < str8tInput.GetLength(1); j++)
            {
                Console.SetCursorPosition(j, i);

                if (str8tInput[i, j].FieldColor == FieldColor.black)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;

                    if (str8tInput[i, j].Number == 0)
                    {
                        Console.Write(' ');
                        continue;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }

                Console.Write(str8tInput[i, j].Number.ToString().Last());
            }
        }

        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;
    }

    /// <summary>
    /// This method visualizes the numbers that change on the console.
    /// </summary>
    /// <param name="str8tInput">The str8t which numbers get visualized.</param>
    internal void VisualizeSteps(Str8tField[,] str8tInput)
    {
        for (int i = 0; i < str8tInput.GetLength(0); i++)
        {
            for (int j = 0; j < str8tInput.GetLength(1); j++)
            {
                str8tInput[i, j].NumberChanged += Str8tField_NumberChanged;
            }
        }
    }

    /// <summary>
    /// This method writes the number that changed to the console at the correct position.
    /// </summary>
    /// <param name="sender">The one who fired the event.</param>
    /// <param name="args">The event arguments containing the number and it's position.</param>
    private void Str8tField_NumberChanged(object? sender, NumberChangedArguments args)
    {
        if (args.Number == 0)
        {
            return;
        }

        Console.SetCursorPosition(args.Position.Y, args.Position.X);
        Console.Write(args.Number.ToString().Last());
    }
}
