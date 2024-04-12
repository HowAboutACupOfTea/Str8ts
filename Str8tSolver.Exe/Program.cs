using Str8tSolver.Exe;
using Str8tSolver.Lib;

Str8tField[,] str8tInput = Logic.ExtractStr8tInput(args);

if (!Logic.IsStr8tInputValid(str8tInput))
{
    throw new ArgumentException("The given str8t input is not valid.");
}

ConsoleRenderer consoleRenderer = new ConsoleRenderer();
consoleRenderer.VisualizePuzzleOnce(str8tInput);
consoleRenderer.VisualizeSteps(str8tInput);

Str8tField[,] solvedStr8t = Logic.SolveStr8t(str8tInput);
