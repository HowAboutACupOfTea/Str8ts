namespace Str8tSolver.Lib;

/// <summary>
/// This class contains the logic for solving str8ts of various sizes.
/// </summary>
public static class Logic
{
    /// <summary>
    /// This method calculates the answer to the given str8t.
    /// </summary>
    /// <param name="str8tInput">The str8t that gets solved.</param>
    /// <returns>The solved str8t.</returns>
    /// <exception cref="ArgumentNullException">This exception gets thrown when the given str8t input is null.</exception>
    /// <exception cref="ArgumentException">This exception gets thrown when the given str8t input is not valid.</exception>
    public static Str8tField[,] SolveStr8t(Str8tField[,] str8tInput)
    {
        if (str8tInput == null)
        {
            throw new ArgumentNullException(nameof(str8tInput));
        }

        if (str8tInput.Length <= 0 || !IsStr8tInputValid(str8tInput))
        {
            throw new ArgumentException("The given str8t input is not valid.");
        }

        Position startPosition = GetStartPosition(str8tInput);
        List<Position> fixedPositions = GetFixedPositions(str8tInput, startPosition);

        for (int i = startPosition.X; i < str8tInput.GetLength(0); i++)
        {
            for (int j = 0; j < str8tInput.GetLength(1); j++)
            {
                if (str8tInput[i, j].FieldColor == FieldColor.black || fixedPositions.Exists(pos => pos.X == i && pos.Y == j))
                {
                    continue;
                }

                int newNumber = GetValidNumber(str8tInput, i, j);

                if (newNumber != -1)
                {
                    str8tInput[i, j].Number = newNumber;
                    continue;
                }

                str8tInput[i, j].Number = 0;
                Position previousPosition = GetPreviousPosition(str8tInput, i, j, startPosition, fixedPositions);
                i = previousPosition.X;
                j = previousPosition.Y - 1;
            }
        }

        return str8tInput;
    }

    /// <summary>
    /// This method checks if the given str8t is valid.
    /// </summary>
    /// <param name="str8tInput">The str8t that gets checked.</param>
    /// <returns>True if the str8t is valid, otherwise false.</returns>
    public static bool IsStr8tInputValid(Str8tField[,] str8tInput)
    {
        if (str8tInput == null || str8tInput.Length <= 0)
        {
            return false;
        }

        for (int i = 0; i < str8tInput.GetLength(0); i++)
        {
            for (int j = 0; j < str8tInput.GetLength(1); j++)
            {
                if (str8tInput[i, j].Number == 0)
                {
                    continue;
                }

                if (str8tInput[i, j].FieldColor == FieldColor.black)
                {
                    if (str8tInput[i, j].Number < 0 || str8tInput[i, j].Number > Math.Sqrt(str8tInput.Length))
                    {
                        return false;
                    }

                    if (DoesColumnContainDuplicates(str8tInput, i, j, str8tInput[i, j].Number) || DoesRowContainDuplicates(str8tInput, i, j, str8tInput[i, j].Number))
                    {
                        return false;
                    }

                    continue;
                }

                if (!IsNumberValid(str8tInput, i, j, str8tInput[i, j].Number))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// This method extracts the string representation of a str8t from the given arguments and converts it into the proprietary str8t class.
    /// String format example for a 3x3 str8t: w0,w1,b0;b2,w0,w0,w0;b0,b0,w0
    /// W for white and b for black fields, followed by the number.
    /// Semicolons seperate rows and commas seperate color/number pairs.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The extracted and converted str8t.</returns>
    /// <exception cref="ArgumentException">This exception gets thrown when the given str8t input is not valid.</exception>
    /// <exception cref="ArgumentNullException">This exception gets thrown when the given str8t input is null.</exception>
    public static Str8tField[,] ExtractStr8tInput(string[] args)
    {
        if (args == null)
        {
            throw new ArgumentNullException(nameof(args), "The given value must not be null.");
        }

        if (args.Length != 1 || args[0] == null)
        {
            throw new ArgumentException("The given str8t input is not valid.");
        }

        string[] rows = args[0].Split(';');

        if (rows.Length <= 1)
        {
            throw new ArgumentException("The given str8t input is not valid.");
        }

        Str8tField[,] str8TFields = new Str8tField[rows.Length, rows.Length];

        for (int i = 0; i < rows.Length; i++)
        {
            string[] row = rows[i].Split(',');

            for (int j = 0; j < row.Length; j++)
            {
                bool isInt = int.TryParse(row[j][1].ToString(), out int number);

                if (!isInt)
                {
                    throw new ArgumentException("The given str8t input is not valid.");
                }

                switch (row[j][0])
                {
                    case 'w':
                        str8TFields[i, j] = new Str8tField(number, FieldColor.white, new Position(i, j));
                        break;

                    case 'b':
                        str8TFields[i, j] = new Str8tField(number, FieldColor.black, new Position(i, j));
                        break;

                    default:
                        throw new ArgumentException("The given str8t input is not valid.");
                }
            }
        }

        return str8TFields;
    }

    /// <summary>
    /// This method gets the positions of the starting numbers of the str8t.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="startPosition">The position of the first white field with the number 0.</param>
    /// <returns>The positions of the starting numbers, which should not get changed.</returns>
    private static List<Position> GetFixedPositions(Str8tField[,] str8tInput, Position startPosition)
    {
        List<Position> fixedPositions = new();

        for (int i = startPosition.X; i < str8tInput.GetLength(0); i++)
        {
            for (int j = 0; j < str8tInput.GetLength(1); j++)
            {
                if (str8tInput[i, j].FieldColor == FieldColor.black)
                {
                    continue;
                }

                if (str8tInput[i, j].Number != 0)
                {
                    fixedPositions.Add(new Position(i, j));
                }
            }
        }

        return fixedPositions;
    }

    /// <summary>
    /// This method gets a valid number for the position at the given coordinates.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>A valid number for the given position or -1 if there is not valid number.</returns>
    private static int GetValidNumber(Str8tField[,] str8tInput, int x, int y)
    {
        int number = str8tInput[x, y].Number;

        while (number <= Math.Sqrt(str8tInput.Length))
        {
            number++;

            if (IsNumberValid(str8tInput, x, y, number))
            {
                return number;
            }
        }

        return -1;
    }

    /// <summary>
    /// This method gets the previous position of the field that gets checked.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="x">The x coordinate of the field that gets checked.</param>
    /// <param name="y">The y coordinate of the field that gets checked.</param>
    /// <param name="startPosition">The position of the first white field.</param>
    /// <param name="fixedPositions">The positions of the numbers that were included in the str8t input.</param>
    /// <returns>The position of the field before the field that got checked.</returns>
    /// <exception cref="ArgumentException">This exception gets thrown when the start position has been reached and it can't contain a valid number.</exception>
    private static Position GetPreviousPosition(Str8tField[,] str8tInput, int x, int y, Position startPosition, List<Position> fixedPositions)
    {
        do
        {
            y--;

            if (y < 0)
            {
                x--;
                y = str8tInput.GetLength(1) - 1;
            }

            if (y >= str8tInput.GetLength(1))
            {
                x--;
                y = str8tInput.GetLength(1) - 1;
            }

            if (x == startPosition.X && y == startPosition.Y && str8tInput[x, y].Number == Math.Sqrt(str8tInput.Length))
            {
                throw new ArgumentException("The given str8t is unsolvable.");
            }
        }
        while (str8tInput[x, y].FieldColor == FieldColor.black || fixedPositions.Exists(pos => pos.X == x && pos.Y == y));

        return new Position(x, y);
    }

    /// <summary>
    /// This method gets the position of the first white field that does not contain a number (except 0).
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <returns>The x and y coordinates of the start position.</returns>
    /// <exception cref="ArgumentException">This exception gets thrown when the given str8t does not contain a white field with the number 0.</exception>
    private static Position GetStartPosition(Str8tField[,] str8tInput)
    {
        for (int i = 0; i < str8tInput.GetLength(0); i++)
        {
            for (int j = 0; j < str8tInput.GetLength(1); j++)
            {
                if (str8tInput[i, j].FieldColor == FieldColor.white && str8tInput[i, j].Number == 0)
                {
                    return new Position(i, j);
                }
            }
        }

        throw new ArgumentException("The given str8t has no white fields");
    }

    /// <summary>
    /// This method checks if the given number is valid.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="x">The x coordinate of the given number.</param>
    /// <param name="y">The y coordinate of the given number.</param>
    /// <param name="number">The number to check.</param>
    /// <returns>True if the number is valid, otherwise false.</returns>
    private static bool IsNumberValid(Str8tField[,] str8tInput, int x, int y, int number)
    {
        if (number <= 0 || number > Math.Sqrt(str8tInput.Length))
        {
            return false;
        }

        if (!IsRowValid(str8tInput, x, y, number))
        {
            return false;
        }

        if (!IsColumnValid(str8tInput, x, y, number))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// This method checks if the column containing the number is valid.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="x">The x coordinate of the given number.</param>
    /// <param name="y">The y coordinate of the given number.</param>
    /// <param name="number">The number to check.</param>
    /// <returns>True if the number is valid, otherwise false.</returns>
    private static bool IsColumnValid(Str8tField[,] str8tInput, int x, int y, int number)
    {
        if (DoesColumnContainDuplicates(str8tInput, x, y, number))
        {
            return false;
        }

        //The compartment definition begins.
        List<int> columnCompartmentXCoordinates = new();
        List<int> columnCompartmentNumbers = new();

        for (int i = x; i >= 0; i--)
        {
            if (str8tInput[i, y].FieldColor != FieldColor.white)
            {
                columnCompartmentXCoordinates.Add(++i);
                columnCompartmentNumbers.Add(str8tInput[i, y].Number);
                break;
            }
        }

        if (columnCompartmentXCoordinates.Count == 0)
        {
            columnCompartmentXCoordinates.Add(0);
            columnCompartmentNumbers.Add(str8tInput[0, y].Number);
        }

        for (int i = columnCompartmentXCoordinates.First() + 1; i < str8tInput.GetLength(1); i++)
        {
            if (str8tInput[i, y].FieldColor != FieldColor.white)
            {
                break;
            }

            columnCompartmentXCoordinates.Add(i);
            columnCompartmentNumbers.Add(str8tInput[i, y].Number);
        }

        columnCompartmentNumbers.Remove(str8tInput[x, y].Number);
        columnCompartmentNumbers.Add(number);
        columnCompartmentNumbers.RemoveAll(numb => numb == 0);
        int span = columnCompartmentNumbers.Max() - columnCompartmentNumbers.Min();

        List<int> numbersOutsideColumnCompartment = new();

        for (int i = 0; i < str8tInput.GetLength(1); i++)
        {
            if (columnCompartmentXCoordinates.Contains(i))
            {
                continue;
            }

            numbersOutsideColumnCompartment.Add(str8tInput[i, y].Number);
        }

        //The compartment check begins.
        if (span + 1 > columnCompartmentXCoordinates.Count)
        {
            return false;
        }

        if (IsNumberOutsideCompartmentRange(columnCompartmentXCoordinates.Count - 1, number))
        {
            return false;
        }

        foreach (var numb in numbersOutsideColumnCompartment)
        {
            if (numb == 0 || columnCompartmentXCoordinates.Count == 1)
            {
                continue;
            }

            if (numb < columnCompartmentXCoordinates.Count && number <= numb)
            {
                return false;
            }

            int rangeLength = str8tInput.GetLength(1) + 1 - numb;

            if (rangeLength <= columnCompartmentXCoordinates.Count)
            {
                if (numb <= number)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// This method checks if the column contains duplicates.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="x">The x coordinate of the given number.</param>
    /// <param name="y">The y coordinate of the given number.</param>
    /// <param name="number">The number to check.</param>
    /// <returns>True if there are duplicates, otherwise false.</returns>
    private static bool DoesColumnContainDuplicates(Str8tField[,] str8tInput, int x, int y, int number)
    {
        for (int i = 0; i < str8tInput.GetLength(1); i++)
        {
            if (i == x)
            {
                continue;
            }

            if (str8tInput[i, y].Number == number)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// This method checks if the row containing the number is valid.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="x">The x coordinate of the given number.</param>
    /// <param name="y">The y coordinate of the given number.</param>
    /// <param name="number">The number to check.</param>
    /// <returns>True if the number is valid, otherwise false.</returns>
    private static bool IsRowValid(Str8tField[,] str8tInput, int x, int y, int number)
    {
        if (DoesRowContainDuplicates(str8tInput, x, y, number))
        {
            return false;
        }

        //The compartment definition begins.
        List<int> rowCompartmentYCoordinates = new();
        List<int> rowCompartmentNumbers = new();

        for (int i = y; i >= 0; i--)
        {
            if (str8tInput[x, i].FieldColor != FieldColor.white)
            {
                rowCompartmentYCoordinates.Add(++i);
                rowCompartmentNumbers.Add(str8tInput[x, i].Number);
                break;
            }
        }

        if (rowCompartmentYCoordinates.Count == 0)
        {
            rowCompartmentYCoordinates.Add(0);
            rowCompartmentNumbers.Add(str8tInput[x, 0].Number);
        }

        for (int i = rowCompartmentYCoordinates.First() + 1; i < str8tInput.GetLength(0); i++)
        {
            if (str8tInput[x, i].FieldColor != FieldColor.white)
            {
                break;
            }

            rowCompartmentYCoordinates.Add(i);
            rowCompartmentNumbers.Add(str8tInput[x, i].Number);
        }

        rowCompartmentNumbers.Remove(str8tInput[x, y].Number);
        rowCompartmentNumbers.Add(number);
        rowCompartmentNumbers.RemoveAll(numb => numb == 0);
        int span = rowCompartmentNumbers.Max() - rowCompartmentNumbers.Min();

        List<int> numbersOutsideRowCompartment = new();

        for (int i = 0; i < str8tInput.GetLength(0); i++)
        {
            if (rowCompartmentYCoordinates.Contains(i))
            {
                continue;
            }

            numbersOutsideRowCompartment.Add(str8tInput[x, i].Number);
        }

        //The compartment check begins.
        if (span + 1 > rowCompartmentYCoordinates.Count)
        {
            return false;
        }

        if (IsNumberOutsideCompartmentRange(rowCompartmentYCoordinates.Count - 1, number))
        {
            return false;
        }

        foreach (var numb in numbersOutsideRowCompartment)
        {
            if (numb == 0 || rowCompartmentYCoordinates.Count == 1)
            {
                continue;
            }


            if (numb < rowCompartmentYCoordinates.Count && number <= numb)
            {
                return false;
            }

            int rangeLength = str8tInput.GetLength(0) + 1 - numb;

            if (rangeLength <= rowCompartmentYCoordinates.Count)
            {
                if (numb <= number)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// This method checks if the row contains duplicates.
    /// </summary>
    /// <param name="str8tInput">The current str8t board.</param>
    /// <param name="x">The x coordinate of the given number.</param>
    /// <param name="y">The y coordinate of the given number.</param>
    /// <param name="number">The number to check.</param>
    /// <returns>True if there are duplicates, otherwise false.</returns>
    private static bool DoesRowContainDuplicates(Str8tField[,] str8tInput, int x, int y, int number)
    {
        for (int i = 0; i < str8tInput.GetLength(0); i++)
        {
            if (i == y)
            {
                continue;
            }

            if (str8tInput[x, i].Number == number)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// This method checks if the given number is outside the valid range of the compartment it is in.
    /// </summary>
    /// <param name="compartmentLength">The length of the compartment that contains the number.</param>
    /// <param name="number">The number to check.</param>
    /// <returns>True if the number is outside the valid range, otherwise false.</returns>
    private static bool IsNumberOutsideCompartmentRange(int compartmentLength, int number)
    {
        int upperLimit = number + compartmentLength;
        int lowerLimit = number - compartmentLength;

        if (number < lowerLimit || number > upperLimit)
        {
            return true;
        }

        return false;
    }
}
