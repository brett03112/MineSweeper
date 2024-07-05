namespace MineSweeper.Game;

public class Game
{
    public string Player { get; } = "Player 1";
    public int[,] Board { get; }

    public Game(string player, int[,] board)
    {
        Player = player;
        Board = board;
    }
    #region Create bombs on the board
    /// <summary>
    /// Creates the reference board which has the location of the bombs
    /// </summary>
    /// <returns>int[,] board with bombs placed</returns>
    public static int[,] CreateReferenceBoard()
    {
        
        Random rand = new Random(); 

        int[ , ] board = new int[10, 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                int bombs = rand.Next(1, 101);// Used to randomize the number of bombs on the board
                if (bombs <= 15)  board[i, j] = 9;
                else board[i, j] = 0;
            }
        }
       
        return board;
         #endregion
    }
    #region Encode numbers
    /// <summary>
    /// Takes the game board and encodes the numbers around the bombs
    /// </summary>
    /// <param name="board"></param>
    /// <returns>The game board with the numbers encoded</returns>// // /// 
    public static int[,] EncodeNumbers(int[,] board)
    {
        int count = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (board[i, j] == 9)
                {
                    continue;
                }
                else if (i == 0)// Top row
                {
                    if (j == 0)// Top left
                    {
                        if(board[i, j + 1] == 9) count++;
                        if(board[i + 1, j] == 9) count++;
                        if(board[i + 1, j + 1] == 9) count++;
                        board[i, j] = count;
                        count = 0;
                    }
                    else if (j == 9)// Top right
                    {
                        if(board[i, j - 1] == 9) count++;
                        if(board[i + 1, j] == 9) count++;
                        if(board[i + 1, j - 1] == 9) count++;
                        board[i, j] = count;
                        count = 0;
                    }
                    else // Top middle
                    {
                        if(board[i, j - 1] == 9) count++;
                        if(board[i, j + 1] == 9) count++;
                        if(board[i + 1, j - 1] == 9) count++;
                        if(board[i + 1, j] == 9) count++;
                        if(board[i + 1, j + 1] == 9) count++;
                        board[i, j] = count;
                        count = 0;
                    }
                }
                else if (i == 9)// Bottom row
                {
                    if (j == 0)// Bottom left
                    {
                        if(board[i - 1, j] == 9) count++;
                        if(board[i - 1, j + 1] == 9) count++;
                        if(board[i, j + 1] == 9) count++;
                        board[i, j] = count;
                        count = 0;
                    }
                    else if (j == 9)// Bottom right
                    {
                        if(board[i - 1, j] == 9) count++;
                        if(board[i - 1, j - 1] == 9) count++;
                        if(board[i, j - 1] == 9) count++;
                        board[i, j] = count;
                        count = 0;
                    }
                    else // Bottom middle
                    {
                        if(board[i - 1, j - 1] == 9) count++;
                        if(board[i - 1, j] == 9) count++;
                        if(board[i - 1, j + 1] == 9) count++;
                        if(board[i, j - 1] == 9) count++;
                        if(board[i, j + 1] == 9) count++;
                        board[i, j] = count;
                        count = 0;
                    }
                }
                else if (j == 0)// Left column
                {
                    if(board[i - 1, j] == 9) count++;
                    if(board[i - 1, j + 1] == 9) count++;
                    if(board[i, j + 1] == 9) count++;
                    if(board[i + 1, j] == 9) count++;
                    if(board[i + 1, j + 1] == 9) count++;
                    board[i, j] = count;
                    count = 0;
                }
                else if (j == 9)// Right column
                {
                    if(board[i - 1, j] == 9) count++;
                    if(board[i - 1, j - 1] == 9) count++;
                    if(board[i, j - 1] == 9) count++;
                    if(board[i + 1, j] == 9) count++;
                    if(board[i + 1, j - 1] == 9) count++;
                    board[i, j] = count;
                    count = 0;
                }
                else // Middle
                {
                    if(board[i - 1, j - 1] == 9) count++;
                    if(board[i - 1, j] == 9) count++;
                    if(board[i - 1, j + 1] == 9) count++;
                    if(board[i, j - 1] == 9) count++;
                    if(board[i, j + 1] == 9) count++;
                    if(board[i + 1, j - 1] == 9) count++;
                    if(board[i + 1, j] == 9) count++;
                    if(board[i + 1, j + 1] == 9) count++;
                    board[i, j] = count;
                    count = 0;
                }
            }
        }
        return board;
    }
    #endregion

    #region Created the play board
    /// <summary>
    /// Creates the board that the player will see and updated as the game progresses
    /// </summary>
    /// <returns>char[10,10] playBoard that will be displayed and updated</returns>
    public static char[,] PlayBoard()
    {
        char[,] playBoard = new char[10, 10];
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                playBoard[i, j] = '*';
            }
        }
        return playBoard;
    }

    #endregion

    #region Get the number of bombs
    /// <summary>
    /// Counts the number of bombs on the reference board
    /// </summary>
    /// <param name="board"></param>
    /// <returns>int numOfBombs</returns>
    public static int Bombs(int[,] board)
    {
        int numOfBombs = 0;
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                if(board[i, j] == 9)
                {
                    numOfBombs++;
                }
            }
        }
        return numOfBombs;
    }
    #endregion

    #region Logic for the game
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
        int row = 0; 
        int column = 0;
        WriteLine($"Welcome, {player}!");
        bool gameOn = true;
        
        while(gameOn)
        {
            WriteLine();
            DisplayBoard(playBoard);
            WriteLine("If you would like to tag a bomb, press the letter 'B' else press any other letter key.");  
            char key = ReadKey().KeyChar;
            WriteLine();
            if(key == 'B')
            {
                int rowB = 0;
                int columnB = 0;
                WriteLine("Enter a row from 1-10: ");
                try
                {
                    rowB = int.Parse(ReadLine()!) - 1;
                }
                catch (FormatException) // catch a specific exception
                {
                    WriteLine("You did not enter a valid number.");
                    continue;
                }
                catch (Exception ex) // catch all exceptions
                {
                    WriteLine($"{ex.GetType()} says {ex.Message}");
                    continue;
                }
                
                if (rowB < 0 || rowB > 9)
                {
                    WriteLine("Row must be between 1 and 10");
                    continue;
                }

                WriteLine("Enter a column from 1-10: ");
                try
                {
                    columnB = int.Parse(ReadLine()!) - 1;
                }
                catch (FormatException) // catch a specific exception
                {
                    WriteLine("You did not enter a valid number.");
                    continue;
                }
                catch (Exception ex) // catch all exceptions
                {
                    WriteLine($"{ex.GetType()} says {ex.Message}");
                    continue;
                }
                
                
                if (columnB < 0 || columnB > 9)
                {
                    WriteLine("Column must be between 1 and 10");
                    continue;
                }
                playBoard[rowB, columnB] = 'B';
                markedBombs++;
                if(markedBombs == numOfBombs)
                {
                    bool winner = CheckWinner(playBoard, referenceBoard);
                    if(winner)
                    {
                        WriteLine("YOU WIN!!!");
                        DisplayBoard(playBoard);
                        gameOn = false;
                        return;
                    }
                    else
                    {
                        WriteLine("YOU LOSE!!!");
                        DisplayWinningBoard(playBoard, referenceBoard);
                        gameOn = false;
                        return;
                    }
                }
                DisplayBoard(playBoard);
                WriteLine();
                continue;
            }
            else
            {
                WriteLine("Enter a row from 1-10: ");
                try
                {
                    row = int.Parse(ReadLine()!) - 1;
                }
                catch (FormatException) // catch a specific exception
                {
                    WriteLine("You did not enter a valid number.");
                    continue;
                }
                catch (Exception ex) // catch all exceptions
                {
                    WriteLine($"{ex.GetType()} says {ex.Message}");
                    continue;
                }
                if(row < 0 || row > 9)
                {
                    WriteLine("Row must be between 1 and 10");
                    continue;
                }

                WriteLine("Enter a column from 1-10: ");
                try
                {
                    column = int.Parse(ReadLine()!) - 1;
                }
                catch (FormatException) // catch a specific exception
                {
                    WriteLine("You did not enter a valid number.");
                    continue;
                }
                catch (Exception ex) // catch all exceptions
                {
                    WriteLine($"{ex.GetType()} says {ex.Message}");
                    continue;
                }
                if(column < 0 || column > 9)
                {
                    WriteLine("Column must be between 1 and 10");
                    continue;
                }

                if(referenceBoard[row, column] == 9)
                {
                    WriteLine("You hit a bomb! Now we are all dead!!");
                    DisplayWinningBoard(playBoard, referenceBoard); // hit a bomb.  Game over
                    gameOn = false;
                    return;
                }
                else if (referenceBoard[row, column] == 0) // 0 is an empty space
                {
                    playBoard[row, column] = ' ';
                    continue;
                }
                else
                {
                    playBoard[row, column] = (char)(referenceBoard[row, column] + 48);// 48 is the ascii value of 0
                }
            }
        }
    }
    #endregion

    #region Display the play board with updates
    /// <summary>
    /// Displays the current board condition to the player
    /// </summary>
    /// <param name="board"></param>
    public static void DisplayBoard(char[,] board)
    {
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                Write($"{board[i, j]} ");
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
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                if(referenceBoard[i, j] == 9)
                {
                    board[i, j] = 'B';
                }
                else if(referenceBoard[i, j] == 0)
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

