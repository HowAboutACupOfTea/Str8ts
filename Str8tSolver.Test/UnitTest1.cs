using NUnit.Framework;
using Str8tSolver.Lib;
using static Str8tSolver.Lib.Logic;

namespace Str8tSolver.Test;

/// <summary>
/// This class implements the unit tests for the <see cref="Logic"/> class.
/// </summary>
public class Tests
{
    /// <summary>
    /// This method tests the <see cref="SolveStr8t"/> method.
    /// </summary>
    [Test]
    public void Solve_Str8t()
    {
        //Arrange
        Str8tField[,] expectedStr8t = GetCompletedStr8t();
        Str8tField[,] initialStr8t = GetInitialStr8tBoard();

        //Act
        Str8tField[,] actualStr8t = SolveStr8t(initialStr8t);

        //Assert
        for (int i = 0; i < expectedStr8t.GetLength(0); i++)
        {
            for (int j = 0; j < expectedStr8t.GetLength(1); j++)
            {
                Str8tField excpectedField = expectedStr8t[i, j];
                Str8tField actualField = actualStr8t[i, j];

                Assert.AreEqual(excpectedField.Number, actualField.Number);
            }
        }
    }

    /// <summary>
    /// This method tests the <see cref="SolveStr8t"/> method with null.
    /// The method should throw an <see cref="ArgumentNullException"/>.
    /// </summary>
    [Test]
    public void Solve_Str8t_With_Null()
    {
        //Arrange
        Str8tField[,] expectedStr8t = GetCompletedStr8t();
        Str8tField[,] initialStr8t = null;

        //Act
        void testDelegate() { SolveStr8t(initialStr8t); }

        //Assert
        Assert.Throws<ArgumentNullException>(testDelegate);
    }

    /// <summary>
    /// This method tests the <see cref="SolveStr8t"/> method with an unsolvable str8t.
    /// The method should throw an <see cref="ArgumentException"/> with a certain exception message.
    /// </summary>
    [Test]
    public void Solve_Str8t_With_Unsolvable_Str8t()
    {
        //Arrange
        Str8tField[,] initialStr8t = GetUnsolvableInitialStr8tBoard();

        //Act
        void testDelegate() { SolveStr8t(initialStr8t); }

        //Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(testDelegate);
        Assert.That(exception.Message, Is.EqualTo("The given str8t is unsolvable."));
    }

    /// <summary>
    /// This method tests the <see cref="SolveStr8t"/> method with an invalid str8t.
    /// The method should throw an <see cref="ArgumentException"/>.
    /// </summary>
    [Test]
    public void Solve_Str8t_With_Invalid_Str8t()
    {
        //Arrange
        Str8tField[,] invalidStr8t = GetInvalidInitialStr8tBoard();

        //Act
        void testDelegate() { SolveStr8t(invalidStr8t); }

        //Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(testDelegate);
    }

    /// <summary>
    /// This method tests the <see cref="IsStr8tInputValid"/> method.
    /// </summary>
    [Test]
    public void Is_Str8t_Input_Valid()
    {
        //Arrange
        Str8tField[,] correctStr8t = GetInitialStr8tBoard();

        //Act
        bool isValid = IsStr8tInputValid(correctStr8t);

        //Assert
        Assert.IsTrue(isValid);
    }

    /// <summary>
    /// This method tests the <see cref="IsStr8tInputValid"/> method with an empty str8t.
    /// The method should return false.
    /// </summary>
    [Test]
    public void Is_Str8t_Input_Valid_With_Empty_Str8t()
    {
        //Arrange
        Str8tField[,] testStr8tBoard = new Str8tField[0, 0];

        //Act
        bool isValid = IsStr8tInputValid(testStr8tBoard);

        //Assert
        Assert.IsFalse(isValid);
    }

    /// <summary>
    /// This method tests the <see cref="IsStr8tInputValid"/> method with null.
    /// The method should return false.
    /// </summary>
    [Test]
    public void Is_Str8t_Input_Valid_With_Null()
    {
        //Arrange
        Str8tField[,] testStr8tBoard = null;

        //Act
        bool isValid = IsStr8tInputValid(testStr8tBoard);

        //Assert
        Assert.IsFalse(isValid);
    }

    /// <summary>
    /// This method tests the <see cref="IsStr8tInputValid"/> method with an invalid str8t.
    /// The method should return false.
    /// </summary>
    [Test]
    public void Is_Str8t_Input_Valid_With_Invalid_Str8t()
    {
        //Arrange
        Str8tField[,] invalidStr8t = GetInvalidInitialStr8tBoard();

        //Act
        bool isValid = IsStr8tInputValid(invalidStr8t);

        //Assert
        Assert.IsFalse(isValid);
    }

    /// <summary>
    /// This method tests the <see cref="ExtractStr8tInput"/> method.
    /// </summary>
    [Test]
    public void Extract_Str8t_Input()
    {
        //Arrange
        Str8tField[,] expectedInitialStr8tBoard = GetInitialStr8tBoard();
        string str8tString = "b0,b0,w0,w0,w5,w0,w0,b3,b0;w0,w6,w0,w0,b0,b0,w1,w0,w0;w0,w0,w0,b0,b8,w0,w0,w0,w0;w9,w0,w0,b4,w0,w0,w0,b0,b5;b0,w0,w0,w0,w0,w3,w0,w0,b0;b0,b0,w0,w0,w0,b9,w0,w4,w0;w4,w0,w3,w0,b0,b0,w0,w6,w0;w0,w0,w1,b0,b0,w0,w0,w0,w0;b0,b0,w8,w0,w0,w0,w0,b0,b2";
        string[] fakeConsoleArgs = new string[] { str8tString };

        //Act
        Str8tField[,] actualStr8t = ExtractStr8tInput(fakeConsoleArgs);

        //Assert
        for (int i = 0; i < expectedInitialStr8tBoard.GetLength(0); i++)
        {
            for (int j = 0; j < expectedInitialStr8tBoard.GetLength(1); j++)
            {
                var correctTemp = expectedInitialStr8tBoard[i, j];
                var temp = actualStr8t[i, j];

                Assert.AreEqual(correctTemp.Number, temp.Number);
                Assert.AreEqual(correctTemp.FieldColor, temp.FieldColor);
                Assert.AreEqual(correctTemp.Position.X, temp.Position.X);
                Assert.AreEqual(correctTemp.Position.Y, temp.Position.Y);
            }
        }
    }

    /// <summary>
    /// This method tests the <see cref="ExtractStr8tInput"/> method with null as an input.
    /// The method should throw an <see cref="ArgumentNullException"/>.
    /// </summary>
    [Test]
    public void Extract_Str8t_Input_With_Null()
    {
        //Arrange
        Str8tField[,] expectedInitialStr8tBoard = GetInitialStr8tBoard();
        string[] fakeConsoleArgs = null;

        //Act
        void testDelegate() { ExtractStr8tInput(fakeConsoleArgs); }

        //Assert
        Assert.Throws<ArgumentNullException>(testDelegate);
    }

    /// <summary>
    /// This method tests the <see cref="ExtractStr8tInput"/> method with a null string in the string array.
    /// The method should throw an <see cref="ArgumentException"/>.
    /// </summary>
    [Test]
    public void Extract_Str8t_Input_With_Empty_String()
    {
        //Arrange
        Str8tField[,] expectedInitialStr8tBoard = GetInitialStr8tBoard();
        string[] fakeConsoleArgs = new string[] { null };

        //Act
        void testDelegate() { ExtractStr8tInput(fakeConsoleArgs); }

        //Assert
        Assert.Throws<ArgumentException>(testDelegate);
    }

    /// <summary>
    /// This method tests the <see cref="ExtractStr8tInput"/> method with several strings in the string array.
    /// The method should throw an <see cref="ArgumentException"/>.
    /// </summary>
    [Test]
    public void Extract_Str8t_Input_With_Several_Strings()
    {
        //Arrange
        Str8tField[,] expectedInitialStr8tBoard = GetInitialStr8tBoard();
        string[] fakeConsoleArgs = new string[] { "", "", "" };

        //Act
        void testDelegate() { ExtractStr8tInput(fakeConsoleArgs); }

        //Assert
        Assert.Throws<ArgumentException>(testDelegate);
    }

    /// <summary>
    /// This method tests the <see cref="ExtractStr8tInput"/> method with a string that is in the wrong format.
    /// The method should throw an <see cref="ArgumentException"/>.
    /// </summary>
    [Test]
    public void Extract_Str8t_Input_With_Wrong_Format()
    {
        //Arrange
        Str8tField[,] expectedInitialStr8tBoard = GetInitialStr8tBoard();
        string[] fakeConsoleArgs = new string[] { "b0,b0,w0,w0,w5,w0,w0,b3,b0,w0,w6,w0,w0,b0,b0,w1,w0,w0" };

        //Act
        void testDelegate() { ExtractStr8tInput(fakeConsoleArgs); }

        //Assert
        Assert.Throws<ArgumentException>(testDelegate);
    }

    /// <summary>
    /// This method tests the <see cref="ExtractStr8tInput"/> method with a string that is in the wrong format.
    /// The method should throw an <see cref="ArgumentException"/>.
    /// </summary>
    [Test]
    public void Extract_Str8t_Input_With_Wrong_String()
    {
        //Arrange
        Str8tField[,] expectedInitialStr8tBoard = GetInitialStr8tBoard();
        string[] fakeConsoleArgs = new string[] { "Hello there, please give me a str8t." };

        //Act
        void testDelegate() { ExtractStr8tInput(fakeConsoleArgs); }

        //Assert
        Assert.Throws<ArgumentException>(testDelegate);
    }

    /// <summary>
    /// This method creates the completed str8t board from moodle.
    /// </summary>
    /// <returns>The correctly completed str8t board.</returns>
    private Str8tField[,] GetCompletedStr8t()
    {
        Str8tField[,] completedStr8t = new Str8tField[9, 9];

        completedStr8t[0, 0] = new Str8tField(0, FieldColor.black, new Position(0, 0));
        completedStr8t[0, 1] = new Str8tField(0, FieldColor.black, new Position(0, 1));
        completedStr8t[0, 2] = new Str8tField(9, FieldColor.white, new Position(0, 2));
        completedStr8t[0, 3] = new Str8tField(6, FieldColor.white, new Position(0, 3));
        completedStr8t[0, 4] = new Str8tField(5, FieldColor.white, new Position(0, 4));
        completedStr8t[0, 5] = new Str8tField(8, FieldColor.white, new Position(0, 5));
        completedStr8t[0, 6] = new Str8tField(7, FieldColor.white, new Position(0, 6));
        completedStr8t[0, 7] = new Str8tField(3, FieldColor.black, new Position(0, 7));
        completedStr8t[0, 8] = new Str8tField(0, FieldColor.black, new Position(0, 8));
        completedStr8t[1, 0] = new Str8tField(8, FieldColor.white, new Position(1, 0));
        completedStr8t[1, 1] = new Str8tField(6, FieldColor.white, new Position(1, 1));
        completedStr8t[1, 2] = new Str8tField(5, FieldColor.white, new Position(1, 2));
        completedStr8t[1, 3] = new Str8tField(7, FieldColor.white, new Position(1, 3));
        completedStr8t[1, 4] = new Str8tField(0, FieldColor.black, new Position(1, 4));
        completedStr8t[1, 5] = new Str8tField(0, FieldColor.black, new Position(1, 5));
        completedStr8t[1, 6] = new Str8tField(1, FieldColor.white, new Position(1, 6));
        completedStr8t[1, 7] = new Str8tField(2, FieldColor.white, new Position(1, 7));
        completedStr8t[1, 8] = new Str8tField(3, FieldColor.white, new Position(1, 8));
        completedStr8t[2, 0] = new Str8tField(7, FieldColor.white, new Position(2, 0));
        completedStr8t[2, 1] = new Str8tField(5, FieldColor.white, new Position(2, 1));
        completedStr8t[2, 2] = new Str8tField(6, FieldColor.white, new Position(2, 2));
        completedStr8t[2, 3] = new Str8tField(0, FieldColor.black, new Position(2, 3));
        completedStr8t[2, 4] = new Str8tField(8, FieldColor.black, new Position(2, 4));
        completedStr8t[2, 5] = new Str8tField(2, FieldColor.white, new Position(2, 5));
        completedStr8t[2, 6] = new Str8tField(3, FieldColor.white, new Position(2, 6));
        completedStr8t[2, 7] = new Str8tField(1, FieldColor.white, new Position(2, 7));
        completedStr8t[2, 8] = new Str8tField(4, FieldColor.white, new Position(2, 8));
        completedStr8t[3, 0] = new Str8tField(9, FieldColor.white, new Position(3, 0));
        completedStr8t[3, 1] = new Str8tField(8, FieldColor.white, new Position(3, 1));
        completedStr8t[3, 2] = new Str8tField(7, FieldColor.white, new Position(3, 2));
        completedStr8t[3, 3] = new Str8tField(4, FieldColor.black, new Position(3, 3));
        completedStr8t[3, 4] = new Str8tField(3, FieldColor.white, new Position(3, 4));
        completedStr8t[3, 5] = new Str8tField(1, FieldColor.white, new Position(3, 5));
        completedStr8t[3, 6] = new Str8tField(2, FieldColor.white, new Position(3, 6));
        completedStr8t[3, 7] = new Str8tField(0, FieldColor.black, new Position(3, 7));
        completedStr8t[3, 8] = new Str8tField(5, FieldColor.black, new Position(3, 8));
        completedStr8t[4, 0] = new Str8tField(0, FieldColor.black, new Position(4, 0));
        completedStr8t[4, 1] = new Str8tField(7, FieldColor.white, new Position(4, 1));
        completedStr8t[4, 2] = new Str8tField(4, FieldColor.white, new Position(4, 2));
        completedStr8t[4, 3] = new Str8tField(1, FieldColor.white, new Position(4, 3));
        completedStr8t[4, 4] = new Str8tField(2, FieldColor.white, new Position(4, 4));
        completedStr8t[4, 5] = new Str8tField(3, FieldColor.white, new Position(4, 5));
        completedStr8t[4, 6] = new Str8tField(6, FieldColor.white, new Position(4, 6));
        completedStr8t[4, 7] = new Str8tField(5, FieldColor.white, new Position(4, 7));
        completedStr8t[4, 8] = new Str8tField(0, FieldColor.black, new Position(4, 8));
        completedStr8t[5, 0] = new Str8tField(0, FieldColor.black, new Position(5, 0));
        completedStr8t[5, 1] = new Str8tField(0, FieldColor.black, new Position(5, 1));
        completedStr8t[5, 2] = new Str8tField(2, FieldColor.white, new Position(5, 2));
        completedStr8t[5, 3] = new Str8tField(3, FieldColor.white, new Position(5, 3));
        completedStr8t[5, 4] = new Str8tField(1, FieldColor.white, new Position(5, 4));
        completedStr8t[5, 5] = new Str8tField(9, FieldColor.black, new Position(5, 5));
        completedStr8t[5, 6] = new Str8tField(5, FieldColor.white, new Position(5, 6));
        completedStr8t[5, 7] = new Str8tField(4, FieldColor.white, new Position(5, 7));
        completedStr8t[5, 8] = new Str8tField(6, FieldColor.white, new Position(5, 8));
        completedStr8t[6, 0] = new Str8tField(4, FieldColor.white, new Position(6, 0));
        completedStr8t[6, 1] = new Str8tField(1, FieldColor.white, new Position(6, 1));
        completedStr8t[6, 2] = new Str8tField(3, FieldColor.white, new Position(6, 2));
        completedStr8t[6, 3] = new Str8tField(2, FieldColor.white, new Position(6, 3));
        completedStr8t[6, 4] = new Str8tField(0, FieldColor.black, new Position(6, 4));
        completedStr8t[6, 5] = new Str8tField(0, FieldColor.black, new Position(6, 5));
        completedStr8t[6, 6] = new Str8tField(8, FieldColor.white, new Position(6, 6));
        completedStr8t[6, 7] = new Str8tField(6, FieldColor.white, new Position(6, 7));
        completedStr8t[6, 8] = new Str8tField(7, FieldColor.white, new Position(6, 8));
        completedStr8t[7, 0] = new Str8tField(3, FieldColor.white, new Position(7, 0));
        completedStr8t[7, 1] = new Str8tField(2, FieldColor.white, new Position(7, 1));
        completedStr8t[7, 2] = new Str8tField(1, FieldColor.white, new Position(7, 2));
        completedStr8t[7, 3] = new Str8tField(0, FieldColor.black, new Position(7, 3));
        completedStr8t[7, 4] = new Str8tField(0, FieldColor.black, new Position(7, 4));
        completedStr8t[7, 5] = new Str8tField(6, FieldColor.white, new Position(7, 5));
        completedStr8t[7, 6] = new Str8tField(9, FieldColor.white, new Position(7, 6));
        completedStr8t[7, 7] = new Str8tField(7, FieldColor.white, new Position(7, 7));
        completedStr8t[7, 8] = new Str8tField(8, FieldColor.white, new Position(7, 8));
        completedStr8t[8, 0] = new Str8tField(0, FieldColor.black, new Position(8, 0));
        completedStr8t[8, 1] = new Str8tField(0, FieldColor.black, new Position(8, 1));
        completedStr8t[8, 2] = new Str8tField(8, FieldColor.white, new Position(8, 2));
        completedStr8t[8, 3] = new Str8tField(5, FieldColor.white, new Position(8, 3));
        completedStr8t[8, 4] = new Str8tField(6, FieldColor.white, new Position(8, 4));
        completedStr8t[8, 5] = new Str8tField(7, FieldColor.white, new Position(8, 5));
        completedStr8t[8, 6] = new Str8tField(4, FieldColor.white, new Position(8, 6));
        completedStr8t[8, 7] = new Str8tField(0, FieldColor.black, new Position(8, 7));
        completedStr8t[8, 8] = new Str8tField(2, FieldColor.black, new Position(8, 8));

        return completedStr8t;
    }

    /// <summary>
    /// This method creates the str8t board from moodle.
    /// </summary>
    /// <returns>The correctly converted str8t board.</returns>
    private Str8tField[,] GetInitialStr8tBoard()
    {
        Str8tField[,] initalStr8t = new Str8tField[9, 9];

        for (int i = 0; i < initalStr8t.GetLength(0); i++)
        {
            for (int j = 0; j < initalStr8t.GetLength(1); j++)
            {
                initalStr8t[i, j] = new Str8tField(0, FieldColor.white, new Position(i, j));
            }
        }

        initalStr8t[0, 0] = new Str8tField(0, FieldColor.black, new Position(0, 0));
        initalStr8t[0, 1] = new Str8tField(0, FieldColor.black, new Position(0, 1));
        initalStr8t[0, 4] = new Str8tField(5, FieldColor.white, new Position(0, 4));
        initalStr8t[0, 7] = new Str8tField(3, FieldColor.black, new Position(0, 7));
        initalStr8t[0, 8] = new Str8tField(0, FieldColor.black, new Position(0, 8));
        initalStr8t[1, 1] = new Str8tField(6, FieldColor.white, new Position(1, 1));
        initalStr8t[1, 4] = new Str8tField(0, FieldColor.black, new Position(1, 4));
        initalStr8t[1, 5] = new Str8tField(0, FieldColor.black, new Position(1, 5));
        initalStr8t[1, 6] = new Str8tField(1, FieldColor.white, new Position(1, 6));
        initalStr8t[2, 3] = new Str8tField(0, FieldColor.black, new Position(2, 3));
        initalStr8t[2, 4] = new Str8tField(8, FieldColor.black, new Position(2, 4));
        initalStr8t[3, 0] = new Str8tField(9, FieldColor.white, new Position(3, 0));
        initalStr8t[3, 3] = new Str8tField(4, FieldColor.black, new Position(3, 3));
        initalStr8t[3, 7] = new Str8tField(0, FieldColor.black, new Position(3, 7));
        initalStr8t[3, 8] = new Str8tField(5, FieldColor.black, new Position(3, 8));
        initalStr8t[4, 0] = new Str8tField(0, FieldColor.black, new Position(4, 0));
        initalStr8t[4, 5] = new Str8tField(3, FieldColor.white, new Position(4, 5));
        initalStr8t[4, 8] = new Str8tField(0, FieldColor.black, new Position(4, 8));
        initalStr8t[5, 0] = new Str8tField(0, FieldColor.black, new Position(5, 0));
        initalStr8t[5, 1] = new Str8tField(0, FieldColor.black, new Position(5, 1));
        initalStr8t[5, 5] = new Str8tField(9, FieldColor.black, new Position(5, 5));
        initalStr8t[5, 7] = new Str8tField(4, FieldColor.white, new Position(5, 7));
        initalStr8t[6, 0] = new Str8tField(4, FieldColor.white, new Position(6, 0));
        initalStr8t[6, 2] = new Str8tField(3, FieldColor.white, new Position(6, 2));
        initalStr8t[6, 4] = new Str8tField(0, FieldColor.black, new Position(6, 4));
        initalStr8t[6, 5] = new Str8tField(0, FieldColor.black, new Position(6, 5));
        initalStr8t[6, 7] = new Str8tField(6, FieldColor.white, new Position(6, 7));
        initalStr8t[7, 2] = new Str8tField(1, FieldColor.white, new Position(7, 2));
        initalStr8t[7, 3] = new Str8tField(0, FieldColor.black, new Position(7, 3));
        initalStr8t[7, 4] = new Str8tField(0, FieldColor.black, new Position(7, 4));
        initalStr8t[8, 0] = new Str8tField(0, FieldColor.black, new Position(8, 0));
        initalStr8t[8, 1] = new Str8tField(0, FieldColor.black, new Position(8, 1));
        initalStr8t[8, 2] = new Str8tField(8, FieldColor.white, new Position(8, 2));
        initalStr8t[8, 7] = new Str8tField(0, FieldColor.black, new Position(8, 7));
        initalStr8t[8, 8] = new Str8tField(2, FieldColor.black, new Position(8, 8));

        return initalStr8t;
    }

    /// <summary>
    /// This method creates an invalid str8t board.
    /// </summary>
    /// <returns>The invalid str8t board.</returns>
    private Str8tField[,] GetInvalidInitialStr8tBoard()
    {
        Str8tField[,] initalStr8t = new Str8tField[9, 9];

        for (int i = 0; i < initalStr8t.GetLength(0); i++)
        {
            for (int j = 0; j < initalStr8t.GetLength(1); j++)
            {
                initalStr8t[i, j] = new Str8tField(0, FieldColor.white, new Position(i, j));
            }
        }

        initalStr8t[0, 0] = new Str8tField(1, FieldColor.black, new Position(0, 0));
        initalStr8t[0, 1] = new Str8tField(1, FieldColor.black, new Position(0, 1));

        return initalStr8t;
    }

    /// <summary>
    /// This method creates an unsolvable str8t board.
    /// </summary>
    /// <returns>The unsolvable str8t board.</returns>
    private Str8tField[,] GetUnsolvableInitialStr8tBoard()
    {
        Str8tField[,] initalStr8t = new Str8tField[3, 3];

        initalStr8t[0, 0] = new Str8tField(1, FieldColor.white, new Position(0, 0));
        initalStr8t[0, 1] = new Str8tField(0, FieldColor.white, new Position(0, 1));
        initalStr8t[0, 2] = new Str8tField(0, FieldColor.white, new Position(0, 2));
        initalStr8t[1, 0] = new Str8tField(0, FieldColor.white, new Position(1, 0));
        initalStr8t[1, 1] = new Str8tField(0, FieldColor.white, new Position(1, 1));
        initalStr8t[1, 2] = new Str8tField(0, FieldColor.white, new Position(1, 2));
        initalStr8t[2, 0] = new Str8tField(0, FieldColor.white, new Position(2, 0));
        initalStr8t[2, 1] = new Str8tField(2, FieldColor.white, new Position(2, 1));
        initalStr8t[2, 2] = new Str8tField(3, FieldColor.white, new Position(2, 2));


        return initalStr8t;
    }
}