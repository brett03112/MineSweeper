using System;
using System.Linq;
using static System.Console;

namespace MineSweeper.Game;

public class Game
{
    public string Player { get; }
    public int[,] Board { get; }

    private const int BoardSize = 10;
    private const int BombValue = 9;
    private const int BombProbability = 15;

    public Game(string player, int[,] board)
    {
        Player = player;
        Board = board;
    }

    /// <summary>
    /// Creates the reference board which has the location of the bombs
    /// </summary>
    /// <returns>int[,] board with bombs placed</returns>
    public static int[,] CreateReferenceBoard()
    {
        var rand = new Random();
        return To2DArray(Enumerable.Range(0, BoardSize)
            .Select(_ => Enumerable.Range(0, BoardSize)
                .Select(_ => rand.Next(1, 101) <= BombProbability ? BombValue : 0)
                .ToArray())
            .ToArray());
    }

    /// <summary>
    /// Converts a jagged array to a 2D array
    /// </summary>
    /// <typeparam name="T">Type of the elements in the array</typeparam>
    /// <param name="source">Jagged array to convert</param>
    /// <returns>2D array</returns>
    public static T[,] To2DArray<T>(T[][] source)
    {
        int FirstDim = source.Length;
        int SecondDim = source.GroupBy(row => row.Length).Single().Key;

        var result = new T[FirstDim, SecondDim];
        for (int i = 0; i < FirstDim; i++)
            for (int j = 0; j < SecondDim; j++)
                result[i, j] = source[i][j];

        return result;
    }
    /// <summary>
    /// Takes the game board and encodes the numbers around the bombs
    /// </summary>
    /// <param name="board"></param>
    /// <returns>The game board with the numbers encoded</returns>
    public static int[,] EncodeNumbers(int[,] board)
    {
        var encodedBoard = (int[,])board.Clone();
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                if (encodedBoard[i, j] != BombValue)
                {
                    encodedBoard[i, j] = CountAdjacentBombs(board, i, j);
                }
            }
        }
        return encodedBoard;
    }

        /// <summary>
        /// Counts the number of adjacent bombs to the cell at the given row and column
        /// </summary>
        /// <param name="board">The game board</param>
        /// <param name="row">Row of the cell in question</param>
        /// <param name="col">Column of the cell in question</param>
        /// <returns>The number of adjacent bombs</returns>
    private static int CountAdjacentBombs(int[,] board, int row, int col)
    {
        int count = 0;
        for (int i = Math.Max(0, row - 1); i <= Math.Min(BoardSize - 1, row + 1); i++)
        {
            for (int j = Math.Max(0, col - 1); j <= Math.Min(BoardSize - 1, col + 1); j++)
            {
                if (board[i, j] == BombValue)
                {
                    count++;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// Creates the board that the player will see and updated as the game progresses
    /// </summary>
    /// <returns>char[10,10] playBoard that will be displayed and updated</returns>
    public static char[,] PlayBoard() =>
        To2DArray(Enumerable.Range(0, BoardSize)
            .Select(_ => Enumerable.Repeat('*', BoardSize).ToArray())
            .ToArray());

    /// <summary>
    /// Counts the number of bombs on the reference board
    /// </summary>
    /// <param name="board"></param>
    /// <returns>int numOfBombs</returns>
    public static int Bombs(int[,] board) =>
        board.Cast<int>().Count(cell => cell == BombValue);

    /// <summary>
    /// Logic for the game
    /// </summary>
    /// <param name="referenceBoard"></param>
    /// <param name="playBoard"></param>
    /// <param name="player"></param>
    public static void PlayGame(int[,] referenceBoard, char[,] playBoard, string player)
    {
        int numOfBombs = Bombs(referenceBoard);
        int markedBombs = 0;
        WriteLine($"Welcome, {player}!");

        while (true)
        {
            WriteLine();
            DisplayBoard(playBoard);
            var action = GetPlayerAction();

            if (action.IsBombTag)
            {
                var (row, col) = GetValidCoordinates();
                playBoard[row, col] = 'B';
                markedBombs++;

                if (markedBombs == numOfBombs)
                {
                    EndGame(CheckWinner(playBoard, referenceBoard), playBoard, referenceBoard);
                    return;
                }
            }
            else
            {
                var (row, col) = GetValidCoordinates();

                if (referenceBoard[row, col] == BombValue)
                {
                    WriteLine("You hit a bomb! Now we are all dead!!");
                    DisplayWinningBoard(playBoard, referenceBoard);
                    WriteLine("Press any key to exit");
                    ReadKey(intercept: true);
                    return;
                }
                else if (referenceBoard[row, col] == 0)
                {
                    playBoard[row, col] = ' ';
                }
                else
                {
                    playBoard[row, col] = (char)(referenceBoard[row, col] + '0');
                }
            }
        }
    }

    /// <summary>
    /// Gets the player's action from the console.
    /// If the player presses the letter 'B' or 'b', then the player is tagging a bomb.
    /// The function returns a tuple where the first element is true if the player is tagging a bomb, and false otherwise.
    /// The second element is true if the input is valid, and false otherwise.
    /// </summary>
    /// <returns>A tuple containing whether the player is tagging a bomb and whether the input is valid.</returns>
    private static (bool IsBombTag, bool IsValid) GetPlayerAction()
    {
        WriteLine("If you would like to tag a bomb, press the letter 'B' else press any other letter key.");
        char key = ReadKey().KeyChar;
        WriteLine();
        return (key == 'B' || key == 'b', true);
    }

    /// <summary>
    /// Gets valid coordinates from the player.
    /// The function asks the player to enter a row and column.
    /// The function keeps asking until a valid row and column is entered.
    /// The function then returns a tuple containing the row and column.
    /// </summary>
    /// <returns>A tuple containing the row and column.</returns>
    private static (int Row, int Col) GetValidCoordinates()
    {
        int row = GetValidInput("Enter a row from 1-10: ", 1, BoardSize) - 1;
        int col = GetValidInput("Enter a column from 1-10: ", 1, BoardSize) - 1;
        return (row, col);
    }

        /// <summary>
        /// Gets valid integer input from the player.
        /// The function asks the player to enter an integer value.
        /// The function keeps asking until a valid integer value is entered that is between the given minimum and maximum values.
        /// </summary>
        /// <param name="prompt">The prompt to display to the player.</param>
        /// <param name="min">The minimum value that is allowed.</param>
        /// <param name="max">The maximum value that is allowed.</param>
        /// <returns>The valid integer value that was entered.</returns>
    private static int GetValidInput(string prompt, int min, int max)
    {
        while (true)
        {
            WriteLine(prompt);
            if (int.TryParse(ReadLine(), out int result) && result >= min && result <= max)
            {
                return result;
            }
            WriteLine($"Please enter a number between {min} and {max}.");
        }
    }

        /// <summary>
        /// Ends the game by displaying a win or lose message and the final state of the board.
        /// </summary>
        /// <param name="isWinner">True if the player has won, false otherwise.</param>
        /// <param name="playBoard">The current state of the play board.</param>
        /// <param name="referenceBoard">The reference board.</param> 
    private static void EndGame(bool isWinner, char[,] playBoard, int[,] referenceBoard)
    {
        if (isWinner)
        {
            WriteLine("YOU WIN!!!");
            DisplayBoard(playBoard);
        }
        else
        {
            WriteLine("YOU LOSE!!!");
            DisplayWinningBoard(playBoard, referenceBoard);
        }
        WriteLine("Thanks for playing!");
        WriteLine("Press any key to exit");
        ReadKey(intercept: true);
    }

    #region Display the play board with updates
    /// <summary>
    /// Displays the current board condition to the player
    /// </summary>
    /// <param name="board"></param>
    public static void DisplayBoard(char[,] board)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (board[i, j] != 'B')
                {
                    if (char.IsDigit(board[i, j]))
                    {
                        ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Green;
                    }
                }
                else
                {
                    ForegroundColor = ConsoleColor.White;
                }
                Write($"{board[i, j]} ");
                ResetColor();
            }
            WriteLine();
        }
        WriteLine();
    }
    #endregion

    #region Display the winning board
    /// <summary>
    /// Displays the winning board to the player
    /// </summary>
    /// <param name="board"></param>
    /// <param name="referenceBoard"></param> <summary>
    public static void DisplayWinningBoard(char[,] board, int[,] referenceBoard)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (referenceBoard[i, j] == 9)
                {
                    board[i, j] = 'B';
                }
                else if (referenceBoard[i, j] == 0)
                {
                    board[i, j] = ' ';
                }
                else
                {
                    board[i, j] = (char)(referenceBoard[i, j] + 48);
                }
            }
        }
        DisplayBoard(board);
    }
    #endregion

    #region Check for winner
    /// <summary>
    /// Checks to see if the player has won by checking if all the bombs have been marked
    /// </summary>
    /// <param name="board"></param>
    /// <param name="referenceBoard"></param>
    /// <returns>true if all bombs are marked correctly else false</returns>
    public static bool CheckWinner(char[,] board, int[,] referenceBoard)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (board[i, j] == 'B' && referenceBoard[i, j] != 9)
                {
                    return false;
                }
            }
        }
        return true;
    }
    #endregion

}

